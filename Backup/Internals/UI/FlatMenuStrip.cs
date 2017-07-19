using System;
using System.Windows.Forms;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    internal class FlatMenuStrip : ToolStrip
    {
        private ToolStripProfessionalRenderer pr;

        public FlatMenuStrip()
        {
            this.Dock = DockStyle.Top;
            GripStyle = ToolStripGripStyle.Hidden;
            this.AutoSize = false;

            this.SetRenderer();
        }

        protected override void OnRendererChanged(EventArgs e)
        {
            base.OnRendererChanged(e);

            this.SetRenderer();
        }

        private void SetRenderer()
        {
            if ((Renderer is ToolStripProfessionalRenderer) && (Renderer != this.pr))
            {
                if (this.pr == null)
                {
                    this.pr = new ToolStripProfessionalRenderer();

                    this.pr.RoundedEdges = false;
                }

                Renderer = this.pr;
            }
        }
    }
}
