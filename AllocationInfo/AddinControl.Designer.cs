namespace SqlInternals.AllocationInfo.Addin
{
    partial class AddinControl
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
            this.allocationControl1 = new SqlInternals.AllocationInfo.Internals.UI.AllocationControl();
            this.SuspendLayout();
            // 
            // allocationControl1
            // 
            this.allocationControl1.AllocationInfo = null;
            this.allocationControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationControl1.Location = new System.Drawing.Point(0, 0);
            this.allocationControl1.Name = "allocationControl1";
            this.allocationControl1.Size = new System.Drawing.Size(585, 455);
            this.allocationControl1.TabIndex = 0;
            // 
            // AddinControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.allocationControl1);
            this.Name = "AddinControl";
            this.Size = new System.Drawing.Size(585, 455);
            this.ResumeLayout(false);

        }

        #endregion

        private SqlInternals.AllocationInfo.Internals.UI.AllocationControl allocationControl1;
    }
}
