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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.allocationContainer = new SqlInternals.AllocationInfo.Internals.UI.AllocationContainer();
            this.allocationDataGridView = new System.Windows.Forms.DataGridView();
            this.KeyColumn = new SqlInternals.AllocationInfo.Internals.UI.KeyImageColumn();
            this.ObjectNameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RowsTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpaceUsedColumn = new SqlInternals.AllocationInfo.Internals.UI.BarImageColumn();
            this.TotalSpaceMbTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsedSpaceMbTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataSpaceMbTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPagesTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsedPagesTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataPagesTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FragmentCountTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AverageFragmentationTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AverageFragmentSizeTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.allocationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.keyImageColumn1 = new SqlInternals.AllocationInfo.Internals.UI.KeyImageColumn();
            this.flatMenuStrip = new SqlInternals.AllocationInfo.Internals.UI.FlatMenuStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.databaseComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.allocUnitProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.bufferPoolToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.extentSizeToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.allocUnitToolStripStatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.infoTableContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allocUnitbackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.refreshToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.allocationDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allocationBindingSource)).BeginInit();
            this.flatMenuStrip.SuspendLayout();
            this.infoTableContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.allocationContainer);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.allocationDataGridView);
            this.splitContainer.Size = new System.Drawing.Size(837, 592);
            this.splitContainer.SplitterDistance = 245;
            this.splitContainer.TabIndex = 4;
            // 
            // allocationContainer
            // 
            this.allocationContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationContainer.DrawBorder = true;
            this.allocationContainer.ExtentSize = new System.Drawing.Size(64, 8);
            this.allocationContainer.IncludeIam = false;
            this.allocationContainer.LayoutStyle = SqlInternals.AllocationInfo.Internals.UI.LayoutStyle.Horizontal;
            this.allocationContainer.Location = new System.Drawing.Point(0, 0);
            this.allocationContainer.Mode = SqlInternals.AllocationInfo.Internals.UI.MapMode.Standard;
            this.allocationContainer.Name = "allocationContainer";
            this.allocationContainer.ShowFileInformation = false;
            this.allocationContainer.Size = new System.Drawing.Size(837, 245);
            this.allocationContainer.TabIndex = 1;
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
            this.TotalPagesTextBoxColumn,
            this.UsedPagesTextBoxColumn,
            this.DataPagesTextBoxColumn,
            this.FragmentCountTextBoxColumn,
            this.AverageFragmentationTextBoxColumn,
            this.AverageFragmentSizeTextBoxColumn});
            this.allocationDataGridView.DataSource = this.allocationBindingSource;
            this.allocationDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationDataGridView.GridColor = System.Drawing.Color.White;
            this.allocationDataGridView.Location = new System.Drawing.Point(0, 0);
            this.allocationDataGridView.Name = "allocationDataGridView";
            this.allocationDataGridView.ReadOnly = true;
            this.allocationDataGridView.RowHeadersVisible = false;
            this.allocationDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.allocationDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.allocationDataGridView.Size = new System.Drawing.Size(837, 343);
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
            this.SpaceUsedColumn.HeaderText = "Space %";
            this.SpaceUsedColumn.Name = "SpaceUsedColumn";
            this.SpaceUsedColumn.ReadOnly = true;
            // 
            // TotalSpaceMbTextBoxColumn
            // 
            this.TotalSpaceMbTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.TotalSpaceMbTextBoxColumn.DataPropertyName = "TotalMb";
            dataGridViewCellStyle6.Format = "0.00";
            this.TotalSpaceMbTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.TotalSpaceMbTextBoxColumn.HeaderText = "Total (MB)";
            this.TotalSpaceMbTextBoxColumn.Name = "TotalSpaceMbTextBoxColumn";
            this.TotalSpaceMbTextBoxColumn.ReadOnly = true;
            this.TotalSpaceMbTextBoxColumn.Width = 81;
            // 
            // UsedSpaceMbTextBoxColumn
            // 
            this.UsedSpaceMbTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.UsedSpaceMbTextBoxColumn.DataPropertyName = "UsedMb";
            dataGridViewCellStyle7.Format = "0.00";
            this.UsedSpaceMbTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.UsedSpaceMbTextBoxColumn.HeaderText = "Used (MB)";
            this.UsedSpaceMbTextBoxColumn.Name = "UsedSpaceMbTextBoxColumn";
            this.UsedSpaceMbTextBoxColumn.ReadOnly = true;
            this.UsedSpaceMbTextBoxColumn.Width = 82;
            // 
            // DataSpaceMbTextBoxColumn
            // 
            this.DataSpaceMbTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.DataSpaceMbTextBoxColumn.DataPropertyName = "DataMb";
            dataGridViewCellStyle8.Format = "0.00";
            this.DataSpaceMbTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.DataSpaceMbTextBoxColumn.HeaderText = "Data (MB)";
            this.DataSpaceMbTextBoxColumn.Name = "DataSpaceMbTextBoxColumn";
            this.DataSpaceMbTextBoxColumn.ReadOnly = true;
            this.DataSpaceMbTextBoxColumn.Visible = false;
            // 
            // TotalPagesTextBoxColumn
            // 
            this.TotalPagesTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.TotalPagesTextBoxColumn.DataPropertyName = "TotalPages";
            this.TotalPagesTextBoxColumn.HeaderText = "Total Pages";
            this.TotalPagesTextBoxColumn.Name = "TotalPagesTextBoxColumn";
            this.TotalPagesTextBoxColumn.ReadOnly = true;
            this.TotalPagesTextBoxColumn.Visible = false;
            // 
            // UsedPagesTextBoxColumn
            // 
            this.UsedPagesTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.UsedPagesTextBoxColumn.DataPropertyName = "UsedPages";
            this.UsedPagesTextBoxColumn.HeaderText = "Used Pages";
            this.UsedPagesTextBoxColumn.Name = "UsedPagesTextBoxColumn";
            this.UsedPagesTextBoxColumn.ReadOnly = true;
            this.UsedPagesTextBoxColumn.Visible = false;
            // 
            // DataPagesTextBoxColumn
            // 
            this.DataPagesTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.DataPagesTextBoxColumn.DataPropertyName = "DataPages";
            this.DataPagesTextBoxColumn.HeaderText = "Data Pages";
            this.DataPagesTextBoxColumn.Name = "DataPagesTextBoxColumn";
            this.DataPagesTextBoxColumn.ReadOnly = true;
            this.DataPagesTextBoxColumn.Visible = false;
            // 
            // FragmentCountTextBoxColumn
            // 
            this.FragmentCountTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.FragmentCountTextBoxColumn.DataPropertyName = "FragmentCount";
            this.FragmentCountTextBoxColumn.HeaderText = "Frag. Count";
            this.FragmentCountTextBoxColumn.Name = "FragmentCountTextBoxColumn";
            this.FragmentCountTextBoxColumn.ReadOnly = true;
            this.FragmentCountTextBoxColumn.Visible = false;
            // 
            // AverageFragmentationTextBoxColumn
            // 
            this.AverageFragmentationTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.AverageFragmentationTextBoxColumn.DataPropertyName = "AverageFragmentation";
            dataGridViewCellStyle9.Format = "0.00";
            this.AverageFragmentationTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle9;
            this.AverageFragmentationTextBoxColumn.HeaderText = "Avg. Frag. (%)";
            this.AverageFragmentationTextBoxColumn.Name = "AverageFragmentationTextBoxColumn";
            this.AverageFragmentationTextBoxColumn.ReadOnly = true;
            this.AverageFragmentationTextBoxColumn.Visible = false;
            // 
            // AverageFragmentSizeTextBoxColumn
            // 
            this.AverageFragmentSizeTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.AverageFragmentSizeTextBoxColumn.DataPropertyName = "AverageFragmentSize";
            dataGridViewCellStyle10.Format = "0.00";
            this.AverageFragmentSizeTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle10;
            this.AverageFragmentSizeTextBoxColumn.HeaderText = "Avg. Frag. Size (KB)";
            this.AverageFragmentSizeTextBoxColumn.Name = "AverageFragmentSizeTextBoxColumn";
            this.AverageFragmentSizeTextBoxColumn.ReadOnly = true;
            this.AverageFragmentSizeTextBoxColumn.Visible = false;
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
            this.allocUnitProgressBar,
            this.refreshToolStripButton,
            this.toolStripSeparator2,
            this.bufferPoolToolStripButton,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.extentSizeToolStripComboBox,
            this.allocUnitToolStripStatusLabel});
            this.flatMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.flatMenuStrip.Name = "flatMenuStrip";
            this.flatMenuStrip.Size = new System.Drawing.Size(837, 25);
            this.flatMenuStrip.TabIndex = 0;
            this.flatMenuStrip.Text = "flatMenuStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(57, 22);
            this.toolStripLabel1.Text = "Database:";
            // 
            // databaseComboBox
            // 
            this.databaseComboBox.Name = "databaseComboBox";
            this.databaseComboBox.Size = new System.Drawing.Size(121, 25);
            this.databaseComboBox.SelectedIndexChanged += new System.EventHandler(this.DatabaseComboBox_SelectedIndexChanged);
            // 
            // allocUnitProgressBar
            // 
            this.allocUnitProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.allocUnitProgressBar.Name = "allocUnitProgressBar";
            this.allocUnitProgressBar.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.allocUnitProgressBar.Size = new System.Drawing.Size(100, 22);
            this.allocUnitProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.allocUnitProgressBar.Visible = false;
            // 
            // bufferPoolToolStripButton
            // 
            this.bufferPoolToolStripButton.CheckOnClick = true;
            this.bufferPoolToolStripButton.Image = global::SqlInternals.AllocationInfo.Internals.Properties.Resources.bufferpool11;
            this.bufferPoolToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bufferPoolToolStripButton.Name = "bufferPoolToolStripButton";
            this.bufferPoolToolStripButton.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.bufferPoolToolStripButton.Size = new System.Drawing.Size(84, 22);
            this.bufferPoolToolStripButton.Text = "Buffer Pool";
            this.bufferPoolToolStripButton.CheckStateChanged += new System.EventHandler(this.BufferPoolToolStripButton_Click);
            // 
            // extentSizeToolStripComboBox
            // 
            this.extentSizeToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.extentSizeToolStripComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.extentSizeToolStripComboBox.Items.AddRange(new object[] {
            "Small",
            "Medium",
            "Large"});
            this.extentSizeToolStripComboBox.Name = "extentSizeToolStripComboBox";
            this.extentSizeToolStripComboBox.Size = new System.Drawing.Size(100, 25);
            this.extentSizeToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.ExtentSizeToolStripComboBox_SelectedIndexChanged);
            // 
            // allocUnitToolStripStatusLabel
            // 
            this.allocUnitToolStripStatusLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.allocUnitToolStripStatusLabel.AutoSize = false;
            this.allocUnitToolStripStatusLabel.Name = "allocUnitToolStripStatusLabel";
            this.allocUnitToolStripStatusLabel.Size = new System.Drawing.Size(200, 25);
            this.allocUnitToolStripStatusLabel.Text = "...";
            this.allocUnitToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.allocUnitToolStripStatusLabel.Visible = false;
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
            this.allocUnitbackgroundWorker.WorkerReportsProgress = true;
            this.allocUnitbackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AllocUnitbackgroundWorker_DoWork);
            this.allocUnitbackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AllocUnitbackgroundWorker_RunWorkerCompleted);
            this.allocUnitbackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.AllocUnitbackgroundWorker_ProgressChanged);
            // 
            // refreshToolStripButton
            // 
            this.refreshToolStripButton.Image = global::SqlInternals.AllocationInfo.Internals.Properties.Resources.RefreshDocView;
            this.refreshToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refreshToolStripButton.Name = "refreshToolStripButton";
            this.refreshToolStripButton.Size = new System.Drawing.Size(65, 22);
            this.refreshToolStripButton.Text = "Refresh";
            this.refreshToolStripButton.Click += new System.EventHandler(this.refreshToolStripButton_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(64, 22);
            this.toolStripLabel2.Text = "Extent size:";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // AllocationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.flatMenuStrip);
            this.Name = "AllocationControl";
            this.Size = new System.Drawing.Size(837, 617);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripProgressBar allocUnitProgressBar;
        private System.Windows.Forms.ToolStripLabel allocUnitToolStripStatusLabel;
        private System.Windows.Forms.ToolStripButton bufferPoolToolStripButton;
        private System.Windows.Forms.ToolStripComboBox extentSizeToolStripComboBox;
        private System.Windows.Forms.SplitContainer splitContainer;
        private AllocationContainer allocationContainer;
        private System.Windows.Forms.DataGridView allocationDataGridView;
        private System.Windows.Forms.BindingSource allocationBindingSource;
        private KeyImageColumn keyImageColumn1;
        private System.Windows.Forms.ContextMenuStrip infoTableContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker allocUnitbackgroundWorker;
        private KeyImageColumn KeyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ObjectNameTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowsTextBoxColumn;
        private BarImageColumn SpaceUsedColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSpaceMbTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsedSpaceMbTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataSpaceMbTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalPagesTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsedPagesTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataPagesTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FragmentCountTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AverageFragmentationTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AverageFragmentSizeTextBoxColumn;
        private System.Windows.Forms.ToolStripButton refreshToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
    }
}
