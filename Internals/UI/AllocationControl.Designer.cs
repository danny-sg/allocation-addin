namespace SqlInternals.AllocationInfo.Internals.UI
{
    partial class AllocationControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllocationControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.allocationContainer = new SqlInternals.AllocationInfo.Internals.UI.AllocationContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.allocUnitProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.allocUnitToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.allocationDataGridView = new System.Windows.Forms.DataGridView();
            this.KeyColumn = new SqlInternals.AllocationInfo.Internals.UI.KeyImageColumn();
            this.ObjectNameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RowsTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpaceUsedColumn = new SqlInternals.AllocationInfo.Internals.UI.BarImageColumn();
            this.TotalSpaceMbTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsedSpaceMbTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataSpaceMbTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvgFragColumn = new SqlInternals.AllocationInfo.Internals.UI.BarImageColumn();
            this.allocationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.keyImageColumn1 = new SqlInternals.AllocationInfo.Internals.UI.KeyImageColumn();
            this.flatMenuStrip = new SqlInternals.AllocationInfo.Internals.UI.FlatMenuStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.databaseComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.refreshToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bufferPoolToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.extentSizeToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.mapToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tableToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.infoTableContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allocUnitBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.advancedInfoBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.allocationDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allocationBindingSource)).BeginInit();
            this.flatMenuStrip.SuspendLayout();
            this.infoTableContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 32);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.allocationContainer);
            this.splitContainer.Panel1.Controls.Add(this.statusStrip);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.allocationDataGridView);
            this.splitContainer.Size = new System.Drawing.Size(790, 570);
            this.splitContainer.SplitterDistance = 234;
            this.splitContainer.TabIndex = 4;
            // 
            // allocationContainer
            // 
            this.allocationContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationContainer.DrawBorder = true;
            this.allocationContainer.ExtentSize = new System.Drawing.Size(64, 8);
            this.allocationContainer.Holding = true;
            this.allocationContainer.HoldingMessage = "";
            this.allocationContainer.IncludeIam = false;
            this.allocationContainer.LayoutStyle = SqlInternals.AllocationInfo.Internals.UI.LayoutStyle.Horizontal;
            this.allocationContainer.Location = new System.Drawing.Point(0, 0);
            this.allocationContainer.Mode = SqlInternals.AllocationInfo.Internals.UI.MapMode.Standard;
            this.allocationContainer.Name = "allocationContainer";
            this.allocationContainer.ShowFileInformation = false;
            this.allocationContainer.Size = new System.Drawing.Size(790, 212);
            this.allocationContainer.TabIndex = 3;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allocUnitProgressBar,
            this.allocUnitToolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 212);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(790, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // allocUnitProgressBar
            // 
            this.allocUnitProgressBar.Name = "allocUnitProgressBar";
            this.allocUnitProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // allocUnitToolStripStatusLabel
            // 
            this.allocUnitToolStripStatusLabel.Margin = new System.Windows.Forms.Padding(4, 3, 0, 2);
            this.allocUnitToolStripStatusLabel.Name = "allocUnitToolStripStatusLabel";
            this.allocUnitToolStripStatusLabel.Size = new System.Drawing.Size(19, 17);
            this.allocUnitToolStripStatusLabel.Text = "...";
            // 
            // allocationDataGridView
            // 
            this.allocationDataGridView.AllowUserToAddRows = false;
            this.allocationDataGridView.AllowUserToDeleteRows = false;
            this.allocationDataGridView.AllowUserToOrderColumns = true;
            this.allocationDataGridView.AllowUserToResizeRows = false;
            this.allocationDataGridView.AutoGenerateColumns = false;
            this.allocationDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.allocationDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.allocationDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.allocationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.allocationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KeyColumn,
            this.ObjectNameTextBoxColumn,
            this.RowsTextBoxColumn,
            this.SpaceUsedColumn,
            this.TotalSpaceMbTextBoxColumn,
            this.UsedSpaceMbTextBoxColumn,
            this.DataSpaceMbTextBoxColumn,
            this.AvgFragColumn});
            this.allocationDataGridView.DataSource = this.allocationBindingSource;
            this.allocationDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationDataGridView.GridColor = System.Drawing.Color.White;
            this.allocationDataGridView.Location = new System.Drawing.Point(0, 0);
            this.allocationDataGridView.MultiSelect = false;
            this.allocationDataGridView.Name = "allocationDataGridView";
            this.allocationDataGridView.ReadOnly = true;
            this.allocationDataGridView.RowHeadersVisible = false;
            this.allocationDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.allocationDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.allocationDataGridView.Size = new System.Drawing.Size(790, 332);
            this.allocationDataGridView.TabIndex = 0;
            this.allocationDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AllocationDataGridView_CellClick);
            this.allocationDataGridView.SelectionChanged += new System.EventHandler(this.AllocationDataGridView_SelectionChanged);
            // 
            // KeyColumn
            // 
            this.KeyColumn.DataPropertyName = "KeyColour";
            this.KeyColumn.Frozen = true;
            this.KeyColumn.HeaderText = "";
            this.KeyColumn.Name = "KeyColumn";
            this.KeyColumn.ReadOnly = true;
            this.KeyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.KeyColumn.Visible = false;
            this.KeyColumn.Width = 30;
            // 
            // ObjectNameTextBoxColumn
            // 
            this.ObjectNameTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ObjectNameTextBoxColumn.DataPropertyName = "ObjectName";
            this.ObjectNameTextBoxColumn.HeaderText = "Name";
            this.ObjectNameTextBoxColumn.Name = "ObjectNameTextBoxColumn";
            this.ObjectNameTextBoxColumn.ReadOnly = true;
            this.ObjectNameTextBoxColumn.Width = 60;
            // 
            // RowsTextBoxColumn
            // 
            this.RowsTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.RowsTextBoxColumn.DataPropertyName = "Rows";
            this.RowsTextBoxColumn.HeaderText = "Rows";
            this.RowsTextBoxColumn.Name = "RowsTextBoxColumn";
            this.RowsTextBoxColumn.ReadOnly = true;
            this.RowsTextBoxColumn.Width = 59;
            // 
            // SpaceUsedColumn
            // 
            this.SpaceUsedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SpaceUsedColumn.DataPropertyName = "UsedPercent";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle1.NullValue")));
            this.SpaceUsedColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.SpaceUsedColumn.HeaderText = "Space %";
            this.SpaceUsedColumn.Name = "SpaceUsedColumn";
            this.SpaceUsedColumn.ReadOnly = true;
            // 
            // TotalSpaceMbTextBoxColumn
            // 
            this.TotalSpaceMbTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.TotalSpaceMbTextBoxColumn.DataPropertyName = "TotalMb";
            dataGridViewCellStyle2.Format = "0.00";
            this.TotalSpaceMbTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.TotalSpaceMbTextBoxColumn.HeaderText = "Total MB";
            this.TotalSpaceMbTextBoxColumn.Name = "TotalSpaceMbTextBoxColumn";
            this.TotalSpaceMbTextBoxColumn.ReadOnly = true;
            this.TotalSpaceMbTextBoxColumn.Width = 75;
            // 
            // UsedSpaceMbTextBoxColumn
            // 
            this.UsedSpaceMbTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.UsedSpaceMbTextBoxColumn.DataPropertyName = "UsedMb";
            dataGridViewCellStyle3.Format = "0.00";
            this.UsedSpaceMbTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.UsedSpaceMbTextBoxColumn.HeaderText = "Used MB";
            this.UsedSpaceMbTextBoxColumn.Name = "UsedSpaceMbTextBoxColumn";
            this.UsedSpaceMbTextBoxColumn.ReadOnly = true;
            this.UsedSpaceMbTextBoxColumn.Width = 76;
            // 
            // DataSpaceMbTextBoxColumn
            // 
            this.DataSpaceMbTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.DataSpaceMbTextBoxColumn.DataPropertyName = "DataMb";
            dataGridViewCellStyle4.Format = "0.00";
            this.DataSpaceMbTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.DataSpaceMbTextBoxColumn.HeaderText = "Data MB";
            this.DataSpaceMbTextBoxColumn.Name = "DataSpaceMbTextBoxColumn";
            this.DataSpaceMbTextBoxColumn.ReadOnly = true;
            this.DataSpaceMbTextBoxColumn.Visible = false;
            // 
            // AvgFragColumn
            // 
            this.AvgFragColumn.DataPropertyName = "AverageFragmentation";
            this.AvgFragColumn.HeaderText = "Avg. Frag. %";
            this.AvgFragColumn.Name = "AvgFragColumn";
            this.AvgFragColumn.ReadOnly = true;
            // 
            // keyImageColumn1
            // 
            this.keyImageColumn1.DataPropertyName = "KeyColour";
            this.keyImageColumn1.HeaderText = "";
            this.keyImageColumn1.Name = "keyImageColumn1";
            this.keyImageColumn1.Width = 30;
            // 
            // flatMenuStrip
            // 
            this.flatMenuStrip.AutoSize = false;
            this.flatMenuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.flatMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.databaseComboBox,
            this.refreshToolStripButton,
            this.toolStripSeparator2,
            this.bufferPoolToolStripButton,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.extentSizeToolStripComboBox,
            this.mapToolStripButton,
            this.tableToolStripButton});
            this.flatMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.flatMenuStrip.Name = "flatMenuStrip";
            this.flatMenuStrip.Size = new System.Drawing.Size(790, 32);
            this.flatMenuStrip.TabIndex = 0;
            this.flatMenuStrip.Text = "flatMenuStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(57, 29);
            this.toolStripLabel1.Text = "Database:";
            // 
            // databaseComboBox
            // 
            this.databaseComboBox.Name = "databaseComboBox";
            this.databaseComboBox.Size = new System.Drawing.Size(140, 32);
            this.databaseComboBox.SelectedIndexChanged += new System.EventHandler(this.DatabaseComboBox_SelectedIndexChanged);
            // 
            // refreshToolStripButton
            // 
            this.refreshToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshToolStripButton.Image")));
            this.refreshToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshToolStripButton.Name = "refreshToolStripButton";
            this.refreshToolStripButton.Size = new System.Drawing.Size(23, 29);
            this.refreshToolStripButton.Text = "Refresh";
            this.refreshToolStripButton.Click += new System.EventHandler(this.RefreshToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // bufferPoolToolStripButton
            // 
            this.bufferPoolToolStripButton.CheckOnClick = true;
            this.bufferPoolToolStripButton.Enabled = false;
            this.bufferPoolToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("bufferPoolToolStripButton.Image")));
            this.bufferPoolToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bufferPoolToolStripButton.Name = "bufferPoolToolStripButton";
            this.bufferPoolToolStripButton.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.bufferPoolToolStripButton.Size = new System.Drawing.Size(84, 29);
            this.bufferPoolToolStripButton.Text = "Buffer Pool";
            this.bufferPoolToolStripButton.CheckStateChanged += new System.EventHandler(this.BufferPoolToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(64, 29);
            this.toolStripLabel2.Text = "Extent size:";
            // 
            // extentSizeToolStripComboBox
            // 
            this.extentSizeToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.extentSizeToolStripComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.extentSizeToolStripComboBox.Items.AddRange(new object[] {
            "Small",
            "Medium",
            "Large",
            "Fit"});
            this.extentSizeToolStripComboBox.Name = "extentSizeToolStripComboBox";
            this.extentSizeToolStripComboBox.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.extentSizeToolStripComboBox.Size = new System.Drawing.Size(75, 32);
            this.extentSizeToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.ExtentSizeToolStripComboBox_SelectedIndexChanged);
            // 
            // mapToolStripButton
            // 
            this.mapToolStripButton.AutoSize = false;
            this.mapToolStripButton.Checked = true;
            this.mapToolStripButton.CheckOnClick = true;
            this.mapToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mapToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mapToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("mapToolStripButton.Image")));
            this.mapToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mapToolStripButton.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this.mapToolStripButton.Name = "mapToolStripButton";
            this.mapToolStripButton.Size = new System.Drawing.Size(23, 26);
            this.mapToolStripButton.Text = "Show allocation map";
            this.mapToolStripButton.CheckStateChanged += new System.EventHandler(this.MapToolStripButton_CheckStateChanged);
            // 
            // tableToolStripButton
            // 
            this.tableToolStripButton.AutoSize = false;
            this.tableToolStripButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tableToolStripButton.Checked = true;
            this.tableToolStripButton.CheckOnClick = true;
            this.tableToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tableToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("tableToolStripButton.Image")));
            this.tableToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tableToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tableToolStripButton.Margin = new System.Windows.Forms.Padding(2, 1, 0, 2);
            this.tableToolStripButton.Name = "tableToolStripButton";
            this.tableToolStripButton.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.tableToolStripButton.Size = new System.Drawing.Size(23, 26);
            this.tableToolStripButton.Text = "Show allocation table";
            this.tableToolStripButton.Click += new System.EventHandler(this.TableToolStripButton_Click);
            // 
            // infoTableContextMenuStrip
            // 
            this.infoTableContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.advancedToolStripMenuItem});
            this.infoTableContextMenuStrip.Name = "infoTableContextMenuStrip";
            this.infoTableContextMenuStrip.Size = new System.Drawing.Size(123, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            this.toolStripMenuItem1.Text = "Basic";
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.advancedToolStripMenuItem.Text = "Advanced";
            // 
            // allocUnitbackgroundWorker
            // 
            this.allocUnitBackgroundWorker.WorkerReportsProgress = true;
            this.allocUnitBackgroundWorker.WorkerSupportsCancellation = true;
            this.allocUnitBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AllocUnitbackgroundWorker_DoWork);
            this.allocUnitBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AllocUnitbackgroundWorker_RunWorkerCompleted);
            this.allocUnitBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.AllocUnitbackgroundWorker_ProgressChanged);
            // 
            // advancedInfoBackgroundWorker
            // 
            this.advancedInfoBackgroundWorker.WorkerSupportsCancellation = true;
            this.advancedInfoBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AdvancedInfoBackgroundWorker_DoWork);
            this.advancedInfoBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AdvancedInfoBackgroundWorker_RunWorkerCompleted);
            this.advancedInfoBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.AdvancedInfoBackgroundWorker_ProgressChanged);
            // 
            // AllocationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.flatMenuStrip);
            this.Name = "AllocationControl";
            this.Size = new System.Drawing.Size(790, 602);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.allocationDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allocationBindingSource)).EndInit();
            this.flatMenuStrip.ResumeLayout(false);
            this.flatMenuStrip.PerformLayout();
            this.infoTableContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FlatMenuStrip flatMenuStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox databaseComboBox;
        private System.Windows.Forms.ToolStripButton bufferPoolToolStripButton;
        private System.Windows.Forms.ToolStripComboBox extentSizeToolStripComboBox;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView allocationDataGridView;
        private System.Windows.Forms.BindingSource allocationBindingSource;
        private KeyImageColumn keyImageColumn1;
        private System.Windows.Forms.ContextMenuStrip infoTableContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker allocUnitBackgroundWorker;
        private System.Windows.Forms.ToolStripButton refreshToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton mapToolStripButton;
        private System.Windows.Forms.ToolStripButton tableToolStripButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar allocUnitProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel allocUnitToolStripStatusLabel;
        private AllocationContainer allocationContainer;
        private System.ComponentModel.BackgroundWorker advancedInfoBackgroundWorker;
        private KeyImageColumn KeyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ObjectNameTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowsTextBoxColumn;
        private BarImageColumn SpaceUsedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSpaceMbTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsedSpaceMbTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataSpaceMbTextBoxColumn;
        private BarImageColumn AvgFragColumn;
    }
}
