using System;
using System.Collections.Generic;
using System.Text;

namespace SqlInternals.AllocationInfo.Internals.Pages
{
    public class PfsPage : Page
    {
        private const int PFS_OFFSET = 100;
        private const int PFS_SIZE = 8088;
        private List<PfsByte> pfsBytes;

        public PfsPage(Database database, PageAddress address)
            : base(database, address)
        {
            if (Header.PageType != PageType.Pfs)
            {
                throw new InvalidOperationException("Page type is not PFS");
            }

            this.pfsBytes = new List<PfsByte>();

            this.LoadPfsBytes();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <= this.pfsBytes.Count - 1; i++)
            {
                sb.AppendFormat("{0,-14}{1}", new PageAddress(1, i), this.pfsBytes[i]);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public List<PfsByte> PfsBytes
        {
            get { return this.pfsBytes; }
            set { this.pfsBytes = value; }
        }

        private void LoadPfsBytes()
        {
            byte[] pfsData = new byte[PFS_SIZE];

            Array.Copy(this.PageData, PFS_OFFSET, pfsData, 0, PFS_SIZE);

            foreach (byte pfsByte in pfsData)
            {
                this.pfsBytes.Add(new PfsByte(pfsByte));
            }
        }
    }
}
