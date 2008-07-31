using System;
using System.Collections.Generic;
using System.Text;

namespace SqlInternals.AllocationInfo.Internals.Pages
{
    public class PageEventArgs : EventArgs
    {
        private bool openInNewWindow;
        private RowIdentifier rowId;

        public PageEventArgs(RowIdentifier address, bool openInNewWindow)
        {
            this.RowId = address;
            this.OpenInNewWindow = openInNewWindow;
        }

        public bool OpenInNewWindow
        {
            get { return this.openInNewWindow; }
            set { this.openInNewWindow = value; }
        }

        public RowIdentifier RowId
        {
            get { return this.rowId; }
            set { this.rowId = value; }
        }

        public PageAddress Address
        {
            get { return this.rowId.PageAddress; }
            set { this.rowId.PageAddress = value; }
        }
    }
}
