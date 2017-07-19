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

            allocationContainer.PageOver += new EventHandler<PageEventArgs>(this.AllocationContainer_PageOver);
            allocationContainer.MouseLeave += new EventHandler(this.AllocationContainer_MouseLeave);

            extentSizeToolStripComboBox.SelectedIndex = extentSizeToolStripComboBox.FindStringExact("Fit");

            if (Internals.ServerConnection.CurrentConnection().CurrentDatabase != null)
            {
                this.serverConnection = Internals.ServerConnection.CurrentConnection();

                this.RefreshConnection();

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
            this.RefreshServerDatabases();

            Internals.ServerConnection.CurrentConnection().PropertyChanged += this.AllocationControl_PropertyChanged;

            this.bufferPool = new BufferPool();

            this.LoadDatabase();
        }

        /// <summary>
        /// Displays the allocation information table.
        /// </summary>
        private void DisplayAllocationInformationTable()
        {
            this.AllocationInfo = this.serverConnection.CurrentDatabase.AllocationInfo(false);

            this.CancelWorkerAndWait(advancedInfoBackgroundWorker);

            advancedInfoBackgroundWorker.RunWorkerAsync();
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

                if (this.serverConnection.CurrentDatabase != null)
                {
                    allocationContainer.CreateAllocationMaps(this.serverConnection.CurrentDatabase.Files);
                }

                this.CancelWorkerAndWait(allocUnitBackgroundWorker);

                this.DisplayAllocationInformationTable();

                this.DisplayLayers();
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

            this.CancelWorkerAndWait(allocUnitBackgroundWorker);

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
        /// Gets or sets the allocation info data table
        /// </summary>
        /// <value>The allocation info data table.</value>
        public DataTable AllocationInfo
        {
            get
            {
                return this.allocationInfo;
            }

            set
            {
                this.allocationInfo = value;

                if (this.allocationInfo != null)
                {
                    this.AllocationInfo.PrimaryKey = new DataColumn[] { this.AllocationInfo.Columns["ObjectName"] };

                    this.allocationBindingSource.DataSource = this.AllocationInfo;
                    this.allocationBindingSource.Sort = "TotalMb DESC";

                    this.allocationDataGridView.ClearSelection();
                }
            }
        }

        /// <summary>
        /// Changes the key colours for each row
        /// </summary>
        /// <param name="layers">The layers.</param>
        private void ChangeRowKeyColours(List<AllocationLayer> layers)
        {
            foreach (AllocationLayer layer in layers)
            {
                allocationContainer.AddMapLayer(layer);

                DataRow r = (this.allocationBindingSource.DataSource as DataTable).Rows.Find(layer.Name);

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
            this.serverConnection.CurrentDatabase = (Database)databaseComboBox.SelectedItem;
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
            this.ChangeExtentSize();
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
                this.serverConnection = Internals.ServerConnection.CurrentConnection();

                this.RefreshConnection();

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
        /// Handles the Click event of the BufferPoolToolStripButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BufferPoolToolStripButton_Click(object sender, EventArgs e)
        {
            this.ShowBufferPool(bufferPoolToolStripButton.Checked);
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

            this.allocationContainer.Holding = false;
            this.allocationContainer.HoldingMessage = string.Empty;
            this.allocationContainer.ClearMapLayers();
            this.allocationContainer.IncludeIam = true;

            List<AllocationLayer> layers = (List<AllocationLayer>)e.Result;

            foreach (AllocationLayer layer in layers)
            {
                this.allocationContainer.AddMapLayer(layer);
            }

            this.ChangeRowKeyColours(layers);

            if (this.bufferPoolToolStripButton.Checked)
            {
                this.ShowBufferPool(true);
            }

            if (this.allocationContainer.Mode == MapMode.Full)
            {
                this.statusStrip.Visible = false;

                this.allocationContainer.ShowFittedMap();
            }
            else
            {
                this.statusStrip.Visible = true;
            }
        }

        /// <summary>
        /// Handles the ProgressChanged event of the AllocUnitbackgroundWorker control and updates the UI
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.ProgressChangedEventArgs"/> instance containing the event data.</param>
        private void AllocUnitbackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.allocUnitProgressBar.Value = e.ProgressPercentage;
            this.allocUnitToolStripStatusLabel.Text = (string)e.UserState;
        }

        private void AdvancedInfoBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = this.serverConnection.CurrentDatabase.AllocationInfo(true, this.advancedInfoBackgroundWorker);
        }

        private void AdvancedInfoBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void AdvancedInfoBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                this.AllocationInfo = (DataTable)e.Result;

                if (!this.allocationContainer.Holding)
                {
                    this.ChangeRowKeyColours(allocationContainer.MapLayers);
                }
            }
        }

        #endregion
    }
}
