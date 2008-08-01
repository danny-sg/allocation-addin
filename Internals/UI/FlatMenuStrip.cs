using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    internal class FlatMenuStrip : ToolStrip
    {
        private ToolStripProfessionalRenderer pr;

        public FlatMenuStrip()
        {
            base.Dock = DockStyle.Top;
            GripStyle = ToolStripGripStyle.Hidden;
            base.AutoSize = false;
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

        protected override void OnRendererChanged(EventArgs e)
        {
            base.OnRendererChanged(e);

            SetRenderer();
        }
    }
}
