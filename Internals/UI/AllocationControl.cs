using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SqlInternals.AllocationInfo.Internals.Pages;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    public partial class AllocationControl : UserControl
    {
        private const int EM_GETEVENTMASK = (WM_USER + 59);
        private const int EM_SETEVENTMASK = (WM_USER + 69);
        private const int WM_SETREDRAW = 0x000B;
        private const int WM_USER = 0x400;
        private BufferPool bufferPool;
        private bool loading;
        private bool keyChanging;
        private ServerConnection serverConnection;
        private DataTable allocationInfo;

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        protected delegate void LoadDatabaseDelegate();

        public AllocationControl()
        {
            InitializeComponent();

            allocationDataGridView.AutoGenerateColumns = false;
            allocationContainer.PageOver += new EventHandler<PageEventArgs>(this.AllocationContainer_PageOver);
            allocationContainer.MouseLeave += new EventHandler(this.AllocationContainer_MouseLeave);
            extentSizeToolStripComboBox.SelectedIndex = extentSizeToolStripComboBox.FindStringExact("Fit");

            if (Internals.ServerConnection.CurrentConnection().CurrentDatabase != null)
            {
                this.serverConnection = Internals.ServerConnection.CurrentConnection();

                this.RefreshConnection();

                databaseComboBox.Enabled = true;
                bufferPoolToolStripButton.Enabled = true;
            }
            else
            {
                databaseComboBox.Enabled = false;
                bufferPoolToolStripButton.Enabled = false;
            }
        }

        private void AllocationContainer_MouseLeave(object sender, EventArgs e)
        {
            allocUnitToolStripStatusLabel.Text = string.Empty;
        }

        private void AllocationContainer_PageOver(object sender, PageEventArgs e)
        {
            allocUnitToolStripStatusLabel.Text = string.Empty;

            if (e.Address.PageId % Database.PFS_INTERVAL == 0 || e.Address.PageId == 1)
            {
                allocUnitToolStripStatusLabel.Text = "PFS";
            }

            if (e.Address.PageId % Database.ALLOCATION_INTERVAL < 8)
            {
                switch (e.Address.PageId % Database.ALLOCATION_INTERVAL)
                {
                    case 0:
                        if (e.Address.PageId == 0)
                        {
                            allocUnitToolStripStatusLabel.Text = "File Header";
                        }

                        break;
                    case 2:
                        allocUnitToolStripStatusLabel.Text = "GAM";
                        break;
                    case 3:
                        allocUnitToolStripStatusLabel.Text = "SGAM";
                        break;
                    case 6:
                        allocUnitToolStripStatusLabel.Text = "DCM";
                        break;
                    case 7:
                        allocUnitToolStripStatusLabel.Text = "BCM";
                        break;
                }
            }

            List<string> layers = AllocationLayer.FindPage(e.Address, allocationContainer.MapLayers);

            foreach (string name in layers)
            {
                if (allocUnitToolStripStatusLabel.Text != string.Empty)
                {
                    allocUnitToolStripStatusLabel.Text += " | ";
                }

                allocUnitToolStripStatusLabel.Text += name;
            }
        }

        private void RefreshConnection()
        {
            this.RefreshServerDatabases();

            Internals.ServerConnection.CurrentConnection().PropertyChanged += new PropertyChangedEventHandler(this.AllocationControl_PropertyChanged);

            this.bufferPool = new BufferPool();

            this.LoadDatabase();
        }

        /// <summary>
        /// Displays the allocation information table.
        /// </summary>
        private void DisplayAllocationInformationTable()
        {
            this.AllocationInfo = this.serverConnection.CurrentDatabase.AllocationInfo().DefaultView.ToTable();
            this.AllocationInfo.PrimaryKey = new DataColumn[] { this.AllocationInfo.Columns["ObjectName"] };

            this.allocationBindingSource.DataSource = this.AllocationInfo;
            this.allocationBindingSource.Sort = "TotalMb DESC";

            allocationDataGridView.ClearSelection();
        }

        /// <summary>
        /// Loads the database.
        /// </summary>
        private void LoadDatabase()
        {
            if (this.allocationContainer.InvokeRequired)
            {
                this.Invoke(new LoadDatabaseDelegate(this.LoadDatabase));
            }
            else
            {
                if (databaseComboBox.ComboBox.SelectedItem != this.serverConnection.CurrentDatabase)
                {
                    databaseComboBox.ComboBox.SelectedItem = this.serverConnection.CurrentDatabase;
                }

                IntPtr eventMask = IntPtr.Zero;

                try
                {
                    SendMessage(allocationContainer.Handle, WM_SETREDRAW, 0, IntPtr.Zero);

                    eventMask = SendMessage(allocationContainer.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);

                    if (this.serverConnection.CurrentDatabase != null)
                    {
                        allocationContainer.CreateAllocationMaps(this.serverConnection.CurrentDatabase.Files);
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

                this.DisplayAllocationInformationTable();

                this.DisplayLayers();

                // bufferPool.Refresh();
            }
        }

        /// <summary>
        /// Displays the layers.
        /// </summary>
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

            if (allocUnitbackgroundWorker.IsBusy)
            {
                allocUnitbackgroundWorker.CancelAsync();
            }

            // Wait for the cancel to complete
            while (allocUnitbackgroundWorker.IsBusy)
            {
                Application.DoEvents();
            }

            allocationContainer.Holding = true;
            allocationContainer.HoldingMessage = "Scanning allocations...";

            statusStrip.Visible = true;

            allocUnitbackgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Shows the buffer pool.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        private void ShowBufferPool(bool show)
        {
            if (show)
            {
                this.DisplayBufferPoolLayer();
            }
            else
            {
                allocationContainer.RemoveLayer("Buffer Pool");
                allocationContainer.RemoveLayer("Buffer Pool (Dirty)");
                allocationContainer.Invalidate();
            }
        }

        /// <summary>
        /// Displays the buffer pool layer.
        /// </summary>
        private void DisplayBufferPoolLayer()
        {
            this.bufferPool.Refresh();

            AllocationPage clean = new AllocationPage();

            clean.SinglePageSlots.AddRange(this.bufferPool.CleanPages);

            AllocationLayer bufferPoolLayer = new AllocationLayer("Buffer Pool", clean, Color.Black);
            bufferPoolLayer.SingleSlotsOnly = true;
            bufferPoolLayer.Transparency = 80;
            bufferPoolLayer.Transparent = true;
            bufferPoolLayer.BorderColour = Color.WhiteSmoke;
            bufferPoolLayer.UseBorderColour = true;
            bufferPoolLayer.UseDefaultSinglePageColour = false;

            AllocationPage dirty = new AllocationPage();

            dirty.SinglePageSlots.AddRange(this.bufferPool.DirtyPages);

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
            this.serverConnection = Internals.ServerConnection.CurrentConnection();

            if (e.PropertyName == "Server" && null != this.serverConnection.ServerName)
            {
                this.RefreshServerDatabases();
            }
            else if (e.PropertyName == "Database" && !this.loading)
            {
                this.LoadDatabase();
            }
            else if (e.PropertyName == "DatabaseRefresh")
            {
                this.DisplayLayers();

                this.bufferPool.Refresh();
            }
        }

        /// <summary>
        /// Refreshes the server databases.
        /// </summary>
        private void RefreshServerDatabases()
        {
            this.loading = true;

            databaseComboBox.ComboBox.DataSource =
                this.serverConnection.Databases.FindAll(delegate(Database db) { return db.Compatible; });

            databaseComboBox.ComboBox.DisplayMember = "name";
            databaseComboBox.ComboBox.ValueMember = "name";

            this.loading = false;
        }

        /// <summary>
        /// Changes the size of the extent.
        /// </summary>
        private void ChangeExtentSize()
        {
            allocUnitToolStripStatusLabel.Text = string.Empty;

            switch (extentSizeToolStripComboBox.SelectedItem.ToString())
            {
                case "Small":

                    allocationContainer.Mode = MapMode.Standard;
                    allocationContainer.ExtentSize = AllocationMap.Small;
                    break;

                case "Medium":

                    allocationContainer.Mode = MapMode.Standard;
                    allocationContainer.ExtentSize = AllocationMap.Medium;
                    break;

                case "Large":

                    allocationContainer.Mode = MapMode.Standard;
                    allocationContainer.ExtentSize = AllocationMap.Large;
                    break;

                case "Fit":
                    allocationContainer.Mode = MapMode.Full;
                    allocationContainer.ShowFittedMap();
                    break;
            }

            bufferPoolToolStripButton.Enabled = allocationContainer.Mode != MapMode.Full;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the DatabaseComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void DatabaseComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.serverConnection.CurrentDatabase = (Database)databaseComboBox.SelectedItem;
        }

        /// <summary>
        /// Handles the Click event of the BufferPoolToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BufferPoolToolStripButton_Click(object sender, EventArgs e)
        {
            this.ShowBufferPool(bufferPoolToolStripButton.Checked);
        }

        /// <summary>
        /// Handles the SelectionChanged event of the AllocationDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AllocationDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (allocationContainer.Mode != MapMode.Full)
            {
                this.keyChanging = true;

                if (this.allocationDataGridView.SelectedRows.Count > 0)
                {
                    foreach (AllocationMap map in allocationContainer.AllocationMaps.Values)
                    {
                        foreach (AllocationLayer layer in map.MapLayers)
                        {
                            if (layer.Name != "Buffer Pool")
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
                            if (layer.Name != "Buffer Pool")
                            {
                                layer.Transparent = false;
                            }
                        }

                        map.Invalidate();
                    }
                }

                allocationDataGridView.Invalidate();
            }
        }

        /// <summary>
        /// Handles the CellClick event of the AllocationDataGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void AllocationDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.keyChanging)
            {
                if (allocationDataGridView.SelectedRows.Count > 0)
                {
                    allocationDataGridView.ClearSelection();
                }
            }

            this.keyChanging = false;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ExtentSizeToolStripComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ExtentSizeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeExtentSize();
        }

        /// <summary>
        /// Handles the DoWork event of the AllocUnitbackgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void AllocUnitbackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = AllocationUnitsLayer.GenerateLayers(this.serverConnection.CurrentDatabase, (BackgroundWorker)sender);
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
            allocUnitToolStripStatusLabel.Text = string.Empty;

            if (e.Result == null)
            {
                return;
            }

            allocationContainer.Holding = false;
            allocationContainer.HoldingMessage = string.Empty;

            allocationContainer.ClearMapLayers();
            allocationContainer.IncludeIam = true;

            List<AllocationLayer> layers = (List<AllocationLayer>)e.Result;

            foreach (AllocationLayer layer in layers)
            {
                allocationContainer.AddMapLayer(layer);

                DataRow r = (this.allocationBindingSource.DataSource as DataTable).Rows.Find(layer.Name);

                if (r != null)
                {
                    r["KeyColour"] = layer.Colour.ToArgb();
                }
            }

            allocationDataGridView.Columns["KeyColumn"].Visible = true;

            if (bufferPoolToolStripButton.Checked)
            {
                this.ShowBufferPool(true);
            }

            if (this.allocationContainer.Mode == MapMode.Full)
            {
                statusStrip.Visible = false;

                this.allocationContainer.ShowFittedMap();
            }
            else
            {
                statusStrip.Visible = true;
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

        private void RefreshToolStripButton_Click(object sender, EventArgs e)
        {
            if (Internals.ServerConnection.CurrentConnection().CurrentDatabase != null)
            {
                this.serverConnection = Internals.ServerConnection.CurrentConnection();

                this.RefreshConnection();

                databaseComboBox.Enabled = true;
                bufferPoolToolStripButton.Enabled = true;
            }
            else
            {
                databaseComboBox.Enabled = false;
                bufferPoolToolStripButton.Enabled = false;
            }
        }

        private void MapToolStripButton_CheckStateChanged(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = !mapToolStripButton.Checked;
        }

        private void TableToolStripButton_Click(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = !tableToolStripButton.Checked;
        }

        public DataTable AllocationInfo
        {
            get { return this.allocationInfo; }
            set { this.allocationInfo = value; }
        }
    }
}
