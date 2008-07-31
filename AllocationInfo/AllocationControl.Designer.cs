namespace SqlInternals.AllocationInfo.Addin
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.allocationContainer = new SqlInternals.AllocationInfo.Internals.UI.AllocationContainer();
            this.allocationDataGridView = new System.Windows.Forms.DataGridView();
            this.KeyColumn = new SqlInternals.AllocationInfo.Addin.KeyImageColumn();
            this.ObjectNameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpaceBarImageColumn = new SqlInternals.AllocationInfo.Addin.BarImageColumn();
            this.RowsTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalSpaceMbTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsedSpaceMbTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataSpaceMbTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPagesTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsedPagesTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataPagesTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FragmentCountTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AverageFragmentationTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AverageFragmentSizeTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.infoTableContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allocUnitbackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.keyImageColumn1 = new SqlInternals.AllocationInfo.Addin.KeyImageColumn();
            this.barImageColumn1 = new SqlInternals.AllocationInfo.Addin.BarImageColumn();
            this.flatMenuStrip = new SqlInternals.AllocationInfo.Addin.FlatMenuStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.databaseComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.allocUnitProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.allocUnitToolStripStatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.bufferPoolToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.extentSizeToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.allocationDataGridView)).BeginInit();
            this.infoTableContextMenuStrip.SuspendLayout();
            this.flatMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.allocationContainer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.allocationDataGridView);
            this.splitContainer1.Size = new System.Drawing.Size(635, 504);
            this.splitContainer1.SplitterDistance = 209;
            this.splitContainer1.TabIndex = 3;
            // 
            // allocationContainer
            // 
            this.allocationContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationContainer.ExtentSize = new System.Drawing.Size(64, 8);
            this.allocationContainer.IncludeIam = false;
            this.allocationContainer.LayoutStyle = SqlInternals.AllocationInfo.Internals.UI.LayoutStyle.Horizontal;
            this.allocationContainer.Location = new System.Drawing.Point(0, 0);
            this.allocationContainer.Mode = SqlInternals.AllocationInfo.Internals.UI.MapMode.Standard;
            this.allocationContainer.Name = "allocationContainer";
            this.allocationContainer.ShowFileInformation = false;
            this.allocationContainer.Size = new System.Drawing.Size(635, 209);
            this.allocationContainer.TabIndex = 1;
            // 
            // allocationDataGridView
            // 
            this.allocationDataGridView.AllowUserToAddRows = false;
            this.allocationDataGridView.AllowUserToDeleteRows = false;
            this.allocationDataGridView.AllowUserToOrderColumns = true;
            this.allocationDataGridView.AllowUserToResizeRows = false;
            this.allocationDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.allocationDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.allocationDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.allocationDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.allocationDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KeyColumn,
            this.ObjectNameTextBoxColumn,
            this.SpaceBarImageColumn,
            this.RowsTextBoxColumn,
            this.TotalSpaceMbTextBoxColumn,
            this.UsedSpaceMbTextBoxColumn,
            this.DataSpaceMbTextBoxColumn,
            this.TotalPagesTextBoxColumn,
            this.UsedPagesTextBoxColumn,
            this.DataPagesTextBoxColumn,
            this.FragmentCountTextBoxColumn,
            this.AverageFragmentationTextBoxColumn,
            this.AverageFragmentSizeTextBoxColumn});
            this.allocationDataGridView.ContextMenuStrip = this.infoTableContextMenuStrip;
            this.allocationDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationDataGridView.GridColor = System.Drawing.Color.DarkGray;
            this.allocationDataGridView.Location = new System.Drawing.Point(0, 0);
            this.allocationDataGridView.Name = "allocationDataGridView";
            this.allocationDataGridView.ReadOnly = true;
            this.allocationDataGridView.RowHeadersVisible = false;
            this.allocationDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.allocationDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.allocationDataGridView.Size = new System.Drawing.Size(635, 291);
            this.allocationDataGridView.TabIndex = 0;
            this.allocationDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AllocationDataGridView_CellClick);
            this.allocationDataGridView.SelectionChanged += new System.EventHandler(this.AllocationDataGridView_SelectionChanged);
            // 
            // KeyColumn
            // 
            this.KeyColumn.DataPropertyName = "KeyColour";
            this.KeyColumn.HeaderText = "";
            this.KeyColumn.Name = "KeyColumn";
            this.KeyColumn.ReadOnly = true;
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
            // SpaceBarImageColumn
            // 
            this.SpaceBarImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SpaceBarImageColumn.DataPropertyName = "UsedPercent";
            this.SpaceBarImageColumn.HeaderText = "Space";
            this.SpaceBarImageColumn.Name = "SpaceBarImageColumn";
            this.SpaceBarImageColumn.ReadOnly = true;
            // 
            // RowsTextBoxColumn
            // 
            this.RowsTextBoxColumn.DataPropertyName = "Rows";
            this.RowsTextBoxColumn.HeaderText = "Rows";
            this.RowsTextBoxColumn.Name = "RowsTextBoxColumn";
            this.RowsTextBoxColumn.ReadOnly = true;
            // 
            // TotalSpaceMbTextBoxColumn
            // 
            this.TotalSpaceMbTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.TotalSpaceMbTextBoxColumn.DataPropertyName = "TotalMb";
            dataGridViewCellStyle1.Format = "0.00";
            this.TotalSpaceMbTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.TotalSpaceMbTextBoxColumn.HeaderText = "Total (MB)";
            this.TotalSpaceMbTextBoxColumn.Name = "TotalSpaceMbTextBoxColumn";
            this.TotalSpaceMbTextBoxColumn.ReadOnly = true;
            this.TotalSpaceMbTextBoxColumn.Width = 81;
            // 
            // UsedSpaceMbTextBoxColumn
            // 
            this.UsedSpaceMbTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.UsedSpaceMbTextBoxColumn.DataPropertyName = "UsedMb";
            dataGridViewCellStyle2.Format = "0.00";
            this.UsedSpaceMbTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.UsedSpaceMbTextBoxColumn.HeaderText = "Used (MB)";
            this.UsedSpaceMbTextBoxColumn.Name = "UsedSpaceMbTextBoxColumn";
            this.UsedSpaceMbTextBoxColumn.ReadOnly = true;
            this.UsedSpaceMbTextBoxColumn.Width = 82;
            // 
            // DataSpaceMbTextBoxColumn
            // 
            this.DataSpaceMbTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.DataSpaceMbTextBoxColumn.DataPropertyName = "DataMb";
            dataGridViewCellStyle3.Format = "0.00";
            this.DataSpaceMbTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
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
            dataGridViewCellStyle4.Format = "0.00";
            this.AverageFragmentationTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.AverageFragmentationTextBoxColumn.HeaderText = "Avg. Frag. (%)";
            this.AverageFragmentationTextBoxColumn.Name = "AverageFragmentationTextBoxColumn";
            this.AverageFragmentationTextBoxColumn.ReadOnly = true;
            this.AverageFragmentationTextBoxColumn.Visible = false;
            // 
            // AverageFragmentSizeTextBoxColumn
            // 
            this.AverageFragmentSizeTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.AverageFragmentSizeTextBoxColumn.DataPropertyName = "AverageFragmentSize";
            dataGridViewCellStyle5.Format = "0.00";
            this.AverageFragmentSizeTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.AverageFragmentSizeTextBoxColumn.HeaderText = "Avg. Frag. Size (KB)";
            this.AverageFragmentSizeTextBoxColumn.Name = "AverageFragmentSizeTextBoxColumn";
            this.AverageFragmentSizeTextBoxColumn.ReadOnly = true;
            this.AverageFragmentSizeTextBoxColumn.Visible = false;
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
            // keyImageColumn1
            // 
            this.keyImageColumn1.HeaderText = "";
            this.keyImageColumn1.Name = "keyImageColumn1";
            this.keyImageColumn1.Width = 30;
            // 
            // barImageColumn1
            // 
            this.barImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.barImageColumn1.DataPropertyName = "UsedPercent";
            this.barImageColumn1.HeaderText = "Space";
            this.barImageColumn1.Name = "barImageColumn1";
            // 
            // flatMenuStrip
            // 
            this.flatMenuStrip.AutoSize = false;
            this.flatMenuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.flatMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.databaseComboBox,
            this.allocUnitProgressBar,
            this.allocUnitToolStripStatusLabel,
            this.bufferPoolToolStripButton,
            this.extentSizeToolStripComboBox});
            this.flatMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.flatMenuStrip.Name = "flatMenuStrip";
            this.flatMenuStrip.Size = new System.Drawing.Size(635, 28);
            this.flatMenuStrip.TabIndex = 2;
            this.flatMenuStrip.Text = "flatMenuStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(53, 25);
            this.toolStripLabel1.Text = "Database";
            // 
            // databaseComboBox
            // 
            this.databaseComboBox.Name = "databaseComboBox";
            this.databaseComboBox.Size = new System.Drawing.Size(121, 28);
            this.databaseComboBox.SelectedIndexChanged += new System.EventHandler(this.DatabaseComboBox_SelectedIndexChanged);
            // 
            // allocUnitProgressBar
            // 
            this.allocUnitProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.allocUnitProgressBar.Name = "allocUnitProgressBar";
            this.allocUnitProgressBar.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.allocUnitProgressBar.Size = new System.Drawing.Size(100, 25);
            this.allocUnitProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.allocUnitProgressBar.Visible = false;
            // 
            // allocUnitToolStripStatusLabel
            // 
            this.allocUnitToolStripStatusLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.allocUnitToolStripStatusLabel.AutoSize = false;
            this.allocUnitToolStripStatusLabel.Name = "allocUnitToolStripStatusLabel";
            this.allocUnitToolStripStatusLabel.Size = new System.Drawing.Size(140, 25);
            this.allocUnitToolStripStatusLabel.Text = "...";
            this.allocUnitToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.allocUnitToolStripStatusLabel.Visible = false;
            // 
            // bufferPoolToolStripButton
            // 
            this.bufferPoolToolStripButton.CheckOnClick = true;
            this.bufferPoolToolStripButton.Image = global::SqlInternals.AllocationInfo.Addin.CommandBar.bufferpool11;
            this.bufferPoolToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bufferPoolToolStripButton.Name = "bufferPoolToolStripButton";
            this.bufferPoolToolStripButton.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.bufferPoolToolStripButton.Size = new System.Drawing.Size(84, 25);
            this.bufferPoolToolStripButton.Text = "Buffer Pool";
            this.bufferPoolToolStripButton.Click += new System.EventHandler(this.BufferPoolToolStripButton_Click);
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
            this.extentSizeToolStripComboBox.Size = new System.Drawing.Size(100, 28);
            this.extentSizeToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.extentSizeToolStripComboBox_SelectedIndexChanged);
            // 
            // AllocationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.flatMenuStrip);
            this.Name = "AllocationControl";
            this.Size = new System.Drawing.Size(635, 532);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.allocationDataGridView)).EndInit();
            this.infoTableContextMenuStrip.ResumeLayout(false);
            this.flatMenuStrip.ResumeLayout(false);
            this.flatMenuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FlatMenuStrip flatMenuStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox databaseComboBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView allocationDataGridView;
        private SqlInternals.AllocationInfo.Internals.UI.AllocationContainer allocationContainer;
        private System.ComponentModel.BackgroundWorker allocUnitbackgroundWorker;
        private System.Windows.Forms.ToolStripProgressBar allocUnitProgressBar;
        private System.Windows.Forms.ToolStripLabel allocUnitToolStripStatusLabel;
        private System.Windows.Forms.ToolStripButton bufferPoolToolStripButton;
        private System.Windows.Forms.ContextMenuStrip infoTableContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem advancedToolStripMenuItem;
        private KeyImageColumn KeyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ObjectNameTextBoxColumn;
        private BarImageColumn SpaceBarImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RowsTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSpaceMbTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsedSpaceMbTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataSpaceMbTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalPagesTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsedPagesTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DataPagesTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FragmentCountTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AverageFragmentationTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AverageFragmentSizeTextBoxColumn;
        private KeyImageColumn keyImageColumn1;
        private BarImageColumn barImageColumn1;
        private System.Windows.Forms.ToolStripComboBox extentSizeToolStripComboBox;
    }
}
