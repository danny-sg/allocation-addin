using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo.RegSvrEnum;
using Microsoft.SqlServer.Management.UI.VSIntegration;
using Microsoft.SqlServer.Management.UI.VSIntegration.ObjectExplorer;
using SqlInternals.AllocationInfo.Internals;
using SqlInternals.AllocationInfo.Internals.Pages;
using SqlInternals.AllocationInfo.Internals.UI;
using System.Data;

namespace SqlInternals.AllocationInfo.Addin
{
    public partial class AllocationControl : UserControl
    {
        private const int EM_GETEVENTMASK = (WM_USER + 59);
        private const int EM_SETEVENTMASK = (WM_USER + 69);
        private const int WM_SETREDRAW = 0x000B;
        private const int WM_USER = 0x400;
        private readonly BufferPool bufferPool;
        private bool loading;
        private bool keyChanging;
        private SqlInternals.AllocationInfo.Internals.ServerConnection serverConnection;
        private DataTable allocationInfo;


        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        delegate void LoadDatabaseDelegate();

        public AllocationControl()
        {
            InitializeComponent();

            Internals.ServerConnection.CurrentConnection().PropertyChanged += new PropertyChangedEventHandler(AllocationControl_PropertyChanged);

            GetConnection();

            bufferPool = new BufferPool();

            allocationDataGridView.AutoGenerateColumns = false;

            DisplayAllocationInformationTable();
        }

        private void DisplayAllocationInformationTable()
        {
            this.AllocationInfo = serverConnection.CurrentDatabase.AllocationInfo();
            this.AllocationInfo.PrimaryKey = new DataColumn[] { this.AllocationInfo.Columns["ObjectName"] };
            this.allocationDataGridView.DataSource = this.AllocationInfo;
        }

        private bool GetConnection()
        {
            SqlConnectionInfo info = null;

            try
            {
                UIConnectionInfo connInfo = null;

                if (ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo != null)
                {
                    connInfo = ServiceCache.ScriptFactory.CurrentlyActiveWndConnectionInfo.UIConnectionInfo;
                }

                if (connInfo != null)
                {
                    Internals.ServerConnection.CurrentConnection().SetCurrentServer(connInfo.ServerName,
                                                                                    string.IsNullOrEmpty(connInfo.Password),
                                                                                    connInfo.UserName,
                                                                                    connInfo.Password);

                    return true;
                }
                else
                {
                    IObjectExplorerService objExplorer = ServiceCache.GetObjectExplorer();

                    int arraySize;
                    INodeInformation[] nodes;

                    objExplorer.GetSelectedNodes(out arraySize, out nodes);

                    if (nodes.Length > 0)
                    {
                        info = nodes[0].Connection as SqlConnectionInfo;

                        Internals.ServerConnection.CurrentConnection().SetCurrentServer(info.ServerName,
                                                                                        info.UseIntegratedSecurity,
                                                                                        info.UserName,
                                                                                        info.Password);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        private void LoadDatabase()
        {
            if (this.allocationContainer.InvokeRequired)
            {
                this.Invoke(new LoadDatabaseDelegate(LoadDatabase));
            }
            else
            {
                if (allocUnitbackgroundWorker.IsBusy)
                {
                    if (MessageBox.Show("Are you sure you want to cancel the Allocations scan?",
                                        "SQL Server Allocation Information",
                                        MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                if (databaseComboBox.ComboBox.SelectedItem != serverConnection.CurrentDatabase)
                {
                    databaseComboBox.ComboBox.SelectedItem = serverConnection.CurrentDatabase;
                }

                IntPtr eventMask = IntPtr.Zero;

                try
                {
                    SendMessage(allocationContainer.Handle, WM_SETREDRAW, 0, IntPtr.Zero);

                    eventMask = SendMessage(allocationContainer.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);

                    if (serverConnection.CurrentDatabase != null)
                    {
                        allocationContainer.CreateAllocationMaps(serverConnection.CurrentDatabase.Files);
                    }
                }
                finally
                {
                    SendMessage(allocationContainer.Handle, EM_SETEVENTMASK, 0, eventMask);
                    SendMessage(allocationContainer.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

                    allocationContainer.Refresh();
                }

                if (allocUnitbackgroundWorker.IsBusy)
                {
                    allocUnitbackgroundWorker.CancelAsync();
                }

                DisplayLayers();

                //bufferPool.Refresh();
            }
        }

        private void DisplayLayers()
        {
            allocationContainer.IncludeIam = false;
            allocationContainer.ClearMapLayers();

            AllocationLayer unallocated = new AllocationLayer();

            unallocated.Name = "Available - (Unused)";
            unallocated.Colour = Color.Gainsboro;
            unallocated.Visible = false;

            allocationContainer.AddMapLayer(unallocated);

            allocUnitProgressBar.Visible = true;
            allocUnitToolStripStatusLabel.Visible = true;

            allocUnitbackgroundWorker.RunWorkerAsync();
        }

        private void AllocUnitbackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = AllocationUnitsLayer.GenerateLayers(serverConnection.CurrentDatabase, (BackgroundWorker)sender);
        }

        private void ShowBufferPool(bool show)
        {
            if (show)
            {
                DisplayBufferPoolLayer();
            }
            else
            {
                allocationContainer.RemoveLayer("Buffer Pool");
                allocationContainer.RemoveLayer("Buffer Pool (Dirty)");
                allocationContainer.Invalidate();
            }
        }

        private void DisplayBufferPoolLayer()
        {
            bufferPool.Refresh();

            AllocationPage clean = new AllocationPage();

            clean.SinglePageSlots.AddRange(bufferPool.CleanPages);

            AllocationLayer bufferPoolLayer = new AllocationLayer("Buffer Pool", clean, Color.Black);
            bufferPoolLayer.SingleSlotsOnly = true;
            bufferPoolLayer.Transparency = 80;
            bufferPoolLayer.Transparent = true;
            bufferPoolLayer.BorderColour = Color.WhiteSmoke;
            bufferPoolLayer.UseBorderColour = true;
            bufferPoolLayer.UseDefaultSinglePageColour = false;

            AllocationPage dirty = new AllocationPage();

            dirty.SinglePageSlots.AddRange(bufferPool.DirtyPages);

            AllocationLayer bufferPoolDirtyLayer = new AllocationLayer("Buffer Pool (Dirty)", dirty, Color.IndianRed);
            bufferPoolDirtyLayer.SingleSlotsOnly = true;
            bufferPoolDirtyLayer.Transparent = false;

            bufferPoolDirtyLayer.BorderColour = Color.WhiteSmoke;
            bufferPoolDirtyLayer.UseBorderColour = true;
            bufferPoolDirtyLayer.UseDefaultSinglePageColour = false;
            bufferPoolDirtyLayer.LayerType = AllocationLayerType.TopLeftCorner;
            allocationContainer.AddMapLayer(bufferPoolLayer);
            allocationContainer.AddMapLayer(bufferPoolDirtyLayer);
        }

        /// <summary>
        /// Handles the PropertyChanged event of the AllocationControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void AllocationControl_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            serverConnection = Internals.ServerConnection.CurrentConnection();

            if (e.PropertyName == "Server" && null != serverConnection.ServerName)
            {
                loading = true;

                databaseComboBox.ComboBox.DataSource =
                    serverConnection.Databases.FindAll(delegate(Database db) { return db.Compatible; });

                databaseComboBox.ComboBox.DisplayMember = "name";
                databaseComboBox.ComboBox.ValueMember = "name";

                loading = false;
            }
            else if (e.PropertyName == "Database" && !loading)
            {
                LoadDatabase();
            }
            else if (e.PropertyName == "DatabaseRefresh")
            {
                DisplayLayers();

                bufferPool.Refresh();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the DatabaseComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DatabaseComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            serverConnection.CurrentDatabase = (Database)databaseComboBox.SelectedItem;
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the AllocUnitbackgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void AllocUnitbackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor = Cursors.Arrow;

            allocUnitProgressBar.Visible = false;
            allocUnitToolStripStatusLabel.Visible = false;
            allocUnitToolStripStatusLabel.Text = string.Empty;

            if (e.Result == null)
            {
                return;
            }

            allocationContainer.ClearMapLayers();
            allocationContainer.IncludeIam = true;

            List<AllocationLayer> layers = (List<AllocationLayer>)e.Result;

            foreach (AllocationLayer layer in layers)
            {
                allocationContainer.AddMapLayer(layer);
                DataRow r = allocationInfo.Rows.Find(layer.Name);

                if (r != null)
                {
                    r["KeyColour"] = layer.Colour.ToArgb();
                }
            }

            allocationContainer.Mode = MapMode.Standard;

            if (bufferPoolToolStripButton.Checked)
            {
                ShowBufferPool(true);
            }
        }

        /// <summary>
        /// Handles the ProgressChanged event of the AllocUnitbackgroundWorker control and updates the UI
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void AllocUnitbackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            allocUnitProgressBar.Value = e.ProgressPercentage;
            allocUnitToolStripStatusLabel.Text = (string)e.UserState;
        }

        /// <summary>
        /// Handles the Click event of the BufferPoolToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BufferPoolToolStripButton_Click(object sender, EventArgs e)
        {
            ShowBufferPool(bufferPoolToolStripButton.Checked);
        }

        public DataTable AllocationInfo
        {
            get { return allocationInfo; }
            set { allocationInfo = value; }
        }

        private void AllocationDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            keyChanging = true;

            if (this.allocationDataGridView.SelectedRows.Count > 0)
            {
                foreach (AllocationMap map in allocationContainer.AllocationMaps.Values)
                {
                    foreach (AllocationLayer layer in map.MapLayers)
                    {
                        if (layer.Name != ("Buffer Pool"))
                        {
                            layer.Transparent = layer.Name != allocationDataGridView.SelectedRows[0].Cells[1].Value.ToString();
                        }
                    }

                    map.Invalidate();
                }
            }
            else
            {
                foreach (AllocationMap map in allocationContainer.AllocationMaps.Values)
                {
                    foreach (AllocationLayer layer in map.MapLayers)
                    {
                        if (layer.Name != ("Buffer Pool"))
                        {
                            layer.Transparent = false;
                        }
                    }

                    map.Invalidate();
                }

            }

            allocationDataGridView.Invalidate();
        }

        private void AllocationDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!keyChanging)
            {
                if (allocationDataGridView.SelectedRows.Count > 0)
                {
                    allocationDataGridView.ClearSelection();
                }
            }

            keyChanging = false;
        }

        private void extentSizeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeExtentSize();
        }

        private void ChangeExtentSize()
        {
            switch (extentSizeToolStripComboBox.SelectedItem.ToString())
            {
                case "Small":

                    allocationContainer.ExtentSize = AllocationMap.Small;
                    break;
                case "Medium":
                    allocationContainer.ExtentSize = AllocationMap.Medium;
                    break;
                case "Large":
                    allocationContainer.ExtentSize = AllocationMap.Large;
                    break;
            }
        }
    }
}
