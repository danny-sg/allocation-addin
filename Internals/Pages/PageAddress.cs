using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using SqlInternals.AllocationInfo.Internals.Properties;

namespace SqlInternals.AllocationInfo.Internals.Pages
{
    [Serializable]
    public struct PageAddress : IEquatable<PageAddress>, IComparable<PageAddress>
    {
        public static readonly PageAddress Empty = new PageAddress();
        private int fileId;
        private int pageId;

        public PageAddress(string address)
        {
            try
            {
                PageAddress pageAddress = Parse(address);
                fileId = pageAddress.fileId;
                pageId = pageAddress.pageId;
            }
            catch
            {
                fileId = 0;
                pageId = 0;
            }
        }

        public PageAddress(int fileId, int pageId)
        {
            this.fileId = fileId;
            this.pageId = pageId;
        }

        public PageAddress(byte[] address)
        {
            pageId = BitConverter.ToInt32(address, 0);
            fileId = BitConverter.ToInt16(address, 4);
        }

        public int FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        public static PageAddress Parse(string address)
        {
            Regex bytePattern = new Regex(@"[0-9a-fA-F]{4}[\x3A][0-9a-fA-F]{8}$");

            if (bytePattern.IsMatch(address))
            {
                return ParseBytes(address);
            }

            int fileId;
            int pageId;

            bool parsed;

            StringBuilder sb = new StringBuilder(address);

            sb.Replace("(", string.Empty);
            sb.Replace(")", string.Empty);
            sb.Replace(",", ":");

            string[] splitAddress = sb.ToString().Split(@":".ToCharArray());

            if (splitAddress.Length != 2)
            {
                throw new ArgumentException(Resources.Exception_InvalidFormat);
            }

            parsed = true & int.TryParse(splitAddress[0], out fileId);
            parsed = parsed & int.TryParse(splitAddress[1], out pageId);

            if (parsed)
            {
                return new PageAddress(fileId, pageId);
            }
            else
            {
                throw new ArgumentException(Resources.Exception_InvalidFormat);
            }
        }

        private static PageAddress ParseBytes(string address)
        {
            int fileId;
            int pageId;

            string[] bytes = address.Split(new char[] { ':' });


            int.TryParse(bytes[0], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out fileId);
            int.TryParse(bytes[1], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out pageId);

            return new PageAddress(fileId, pageId);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "({0}:{1})", fileId, pageId);
        }

        public override bool Equals(object obj)
        {
            return obj is PageAddress && this == (PageAddress)obj;
        }

        public static bool operator ==(PageAddress address1, PageAddress address2)
        {
            return address1.PageId == address2.PageId && address1.FileId == address2.FileId;
        }

        public static bool operator !=(PageAddress x, PageAddress y)
        {
            return !(x == y);
        }

        public bool Equals(PageAddress pageAddress)
        {
            return fileId == pageAddress.fileId && pageId == pageAddress.pageId;
        }

        public override int GetHashCode()
        {
            return fileId + 29 * pageId;
        }

        public int CompareTo(PageAddress other)
        {
            return (this.FileId.CompareTo(other.FileId) * 9999999) + this.PageId.CompareTo(other.PageId);
        }
    }
}
