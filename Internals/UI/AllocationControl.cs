using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SqlInternals.AllocationInfo.Internals.Pages;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    /// <summary>
    /// SSMS Allocation Add-in control
    /// </summary>
    public partial class AllocationControl : UserControl
    {
        private BufferPool bufferPool;
        private bool loading;
        private bool keyChanging;
        private ServerConnection serverConnection;
        private DataTable allocationInfo;

        protected delegate void LoadDatabaseDelegate();

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationControl"/> class.
        /// </summary>
        public AllocationControl()
        {
            InitializeComponent();

            allocationDataGridView.AutoGenerateColumns = false;

            allocationContainer.PageOver += new EventHandler<PageEventArgs>(AllocationContainer_PageOver);
            allocationContainer.MouseLeave += new EventHandler(AllocationContainer_MouseLeave);

            extentSizeToolStripComboBox.SelectedIndex = extentSizeToolStripComboBox.FindStringExact("Fit");

            if (Internals.ServerConnection.CurrentConnection().CurrentDatabase != null)
            {
                serverConnection = Internals.ServerConnection.CurrentConnection();

                RefreshConnection();

                databaseComboBox.Enabled = true;
            }
            else
            {
                databaseComboBox.Enabled = false;
            }

            // Set the ranges for the fragmentation column
            AvgFragColumn.ColourRanges.Add(new ColourRange(1, 10, Color.FromArgb(100, 255, 100)));
            AvgFragColumn.ColourRanges.Add(new ColourRange(11, 20, Color.FromArgb(255, 170, 85)));
            AvgFragColumn.ColourRanges.Add(new ColourRange(21, 100, Color.FromArgb(255, 100, 100)));
        }

        /// <summary>
        /// Refresh the connection.
        /// </summary>
        private void RefreshConnection()
        {
            RefreshServerDatabases();

            Internals.ServerConnection.CurrentConnection().PropertyChanged += AllocationControl_PropertyChanged;

            bufferPool = new BufferPool();

            LoadDatabase();
        }

        /// <summary>
        /// Displays the allocation information table.
        /// </summary>
        private void DisplayAllocationInformationTable()
        {
            AllocationInfo = serverConnection.CurrentDatabase.AllocationInfo(false);

            CancelWorkerAndWait(advancedInfoBackgroundWorker);

            advancedInfoBackgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Loads the database.
        /// </summary>
        private void LoadDatabase()
        {
            if (allocationContainer.InvokeRequired)
            {
                Invoke(new LoadDatabaseDelegate(LoadDatabase));
            }
            else
            {
                if (databaseComboBox.ComboBox.SelectedItem != serverConnection.CurrentDatabase)
                {
                    databaseComboBox.ComboBox.SelectedItem = serverConnection.CurrentDatabase;
                }

                if (serverConnection.CurrentDatabase != null)
                {
                    allocationContainer.CreateAllocationMaps(serverConnection.CurrentDatabase.Files);
                }

                CancelWorkerAndWait(allocUnitBackgroundWorker);

                DisplayAllocationInformationTable();

                DisplayLayers();
            }
        }

        /// <summary>
        /// Displays the layers.
        /// </summary>
        private void DisplayLayers()
        {
            allocationContainer.IncludeIam = false;
            allocationContainer.ClearMapLayers();

            var unallocated = new AllocationLayer();

            unallocated.Name = "Available - (Unused)";
            unallocated.Colour = Color.Gainsboro;
            unallocated.Visible = false;

            allocationContainer.AddMapLayer(unallocated);

            allocUnitProgressBar.Visible = true;
            allocUnitToolStripStatusLabel.Visible = true;

            CancelWorkerAndWait(allocUnitBackgroundWorker);

            allocationContainer.Holding = true;
            allocationContainer.HoldingMessage = "Scanning allocations...";

            statusStrip.Visible = true;

            allocUnitBackgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Shows the buffer pool.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
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

        /// <summary>
        /// Displays the buffer pool layer.
        /// </summary>
        private void DisplayBufferPoolLayer()
        {
            bufferPool.Refresh();

            var clean = new AllocationPage();

            clean.SinglePageSlots.AddRange(bufferPool.CleanPages);

            var bufferPoolLayer = new AllocationLayer("Buffer Pool", clean, Color.Black);
            bufferPoolLayer.SingleSlotsOnly = true;
            bufferPoolLayer.Transparency = 80;
            bufferPoolLayer.Transparent = true;
            bufferPoolLayer.BorderColour = Color.WhiteSmoke;
            bufferPoolLayer.UseBorderColour = true;
            bufferPoolLayer.UseDefaultSinglePageColour = false;

            var dirty = new AllocationPage();

            dirty.SinglePageSlots.AddRange(bufferPool.DirtyPages);

            var bufferPoolDirtyLayer = new AllocationLayer("Buffer Pool (Dirty)", dirty, Color.IndianRed);

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
        /// Refreshes the server databases.
        /// </summary>
        private void RefreshServerDatabases()
        {
            loading = true;

            databaseComboBox.ComboBox.DataSource =
                serverConnection.Databases.FindAll(delegate(Database db) { return db.Compatible; });

            databaseComboBox.ComboBox.DisplayMember = "name";
            databaseComboBox.ComboBox.ValueMember = "name";

            loading = false;
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
        /// Gets or sets the allocation info data table
        /// </summary>
        /// <value>The allocation info data table.</value>
        public DataTable AllocationInfo
        {
            get
            {
                return allocationInfo;
            }

            set
            {
                allocationInfo = value;

                if (allocationInfo != null)
                {
                    AllocationInfo.PrimaryKey = new[] { AllocationInfo.Columns["ObjectName"] };

                    allocationBindingSource.DataSource = AllocationInfo;
                    allocationBindingSource.Sort = "TotalMb DESC";

                    allocationDataGridView.ClearSelection();
                }
            }
        }

        /// <summary>
        /// Changes the key colours for each row
        /// </summary>
        /// <param name="layers">The layers.</param>
        private void ChangeRowKeyColours(List<AllocationLayer> layers)
        {
            foreach (var layer in layers)
            {
                allocationContainer.AddMapLayer(layer);

                var r = (allocationBindingSource.DataSource as DataTable).Rows.Find(layer.Name);

                if (r != null)
                {
                    r["KeyColour"] = layer.Colour.ToArgb();
                }

                allocationDataGridView.Columns["KeyColumn"].Visible = true;
            }
        }

        private void CancelWorkerAndWait(BackgroundWorker worker)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }

            while (worker.IsBusy)
            {
                Application.DoEvents();
            }
        }

        #region Events

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
        /// Handles the CheckStateChanged event of the MapToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void MapToolStripButton_CheckStateChanged(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = !mapToolStripButton.Checked;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ExtentSizeToolStripComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ExtentSizeToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeExtentSize();
        }

        /// <summary>
        /// Handles the Click event of the RefreshToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RefreshToolStripButton_Click(object sender, EventArgs e)
        {
            if (Internals.ServerConnection.CurrentConnection().CurrentDatabase != null)
            {
                serverConnection = Internals.ServerConnection.CurrentConnection();

                RefreshConnection();

                databaseComboBox.Enabled = true;
            }
            else
            {
                databaseComboBox.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the TableToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TableToolStripButton_Click(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = !tableToolStripButton.Checked;
        }

        /// <summary>
        /// Handles the MouseLeave event of the AllocationContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void AllocationContainer_MouseLeave(object sender, EventArgs e)
        {
            allocUnitToolStripStatusLabel.Text = string.Empty;
        }

        /// <summary>
        /// Handles the PageOver event of the AllocationContainer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SqlInternals.AllocationInfo.Internals.Pages.PageEventArgs"/> instance containing the event data.</param>
        private void AllocationContainer_PageOver(object sender, PageEventArgs e)
        {
            allocUnitToolStripStatusLabel.Text = string.Empty;

            // Check if the page is a PFS page
            if (e.Address.PageId % Database.PFS_INTERVAL == 0 || e.Address.PageId == 1)
            {
                allocUnitToolStripStatusLabel.Text = "PFS";
            }

            // Check if its a File Header/GAM/SGAM/DCM/BCM pages
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

            var layers = AllocationLayer.FindPage(e.Address, allocationContainer.MapLayers);

            foreach (var name in layers)
            {
                if (allocUnitToolStripStatusLabel.Text != string.Empty)
                {
                    allocUnitToolStripStatusLabel.Text += " | ";
                }

                allocUnitToolStripStatusLabel.Text += name;
            }
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
                RefreshServerDatabases();
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

            allocationContainer.Refresh();
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
                keyChanging = true;

                if (allocationDataGridView.SelectedRows.Count > 0)
                {
                    foreach (var map in allocationContainer.AllocationMaps.Values)
                    {
                        foreach (var layer in map.MapLayers)
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
                    foreach (var map in allocationContainer.AllocationMaps.Values)
                    {
                        foreach (var layer in map.MapLayers)
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
            if (!keyChanging)
            {
                if (allocationDataGridView.SelectedRows.Count > 0)
                {
                    allocationDataGridView.ClearSelection();
                }
            }

            keyChanging = false;
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

        #endregion

        #region BackgroundWorker Events

        /// <summary>
        /// Handles the DoWork event of the AllocUnitbackgroundWorker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private void AllocUnitbackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = AllocationUnitsLayer.GenerateLayers(serverConnection.CurrentDatabase, (BackgroundWorker)sender);
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

            var layers = (List<AllocationLayer>)e.Result;

            foreach (var layer in layers)
            {
                allocationContainer.AddMapLayer(layer);
            }

            ChangeRowKeyColours(layers);

            if (bufferPoolToolStripButton.Checked)
            {
                ShowBufferPool(true);
            }

            if (allocationContainer.Mode == MapMode.Full)
            {
                statusStrip.Visible = false;

                allocationContainer.ShowFittedMap();
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

        private void AdvancedInfoBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = serverConnection.CurrentDatabase.AllocationInfo(true, advancedInfoBackgroundWorker);
        }

        private void AdvancedInfoBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void AdvancedInfoBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                AllocationInfo = (DataTable)e.Result;

                if (!allocationContainer.Holding)
                {
                    ChangeRowKeyColours(allocationContainer.MapLayers);
                }
            }
        }

        #endregion
    }
}
