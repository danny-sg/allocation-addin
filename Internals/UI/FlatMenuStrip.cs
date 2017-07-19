using System;
using System.Windows.Forms;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    internal class FlatMenuStrip : ToolStrip
    {
        private ToolStripProfessionalRenderer pr;

        public FlatMenuStrip()
        {
            Dock = DockStyle.Top;
            GripStyle = ToolStripGripStyle.Hidden;
            AutoSize = false;

            SetRenderer();
        }

        protected override void OnRendererChanged(EventArgs e)
        {
            base.OnRendererChanged(e);

            SetRenderer();
        }

        private void SetRenderer()
        {
            if ((Renderer is ToolStripProfessionalRenderer) && (Renderer != pr))
            {
                if (pr == null)
                {
                    pr = new ToolStripProfessionalRenderer();

                    pr.RoundedEdges = false;
                }

                Renderer = pr;
            }
        }
    }
}
