using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SqlInternals.AllocationInfo.Internals.Pages
{
    /// <summary>
    /// Page used for internal SQL Server allocations
    /// </summary>
    /// <remarks>
    /// Includes bitmap for storing allocation information
    /// </remarks>
    public class AllocationPage : Page
    {
        public const int ALLOCATION_ARRAY_OFFSET = 194;
        public const int SINGLE_PAGE_SLOT_OFFSET = 142;
        public const int START_PAGE_OFFSET = 136;

        private readonly bool[] allocationMap = new bool[64000];
        private readonly List<PageAddress> singlePageSlots = new List<PageAddress>();
        private PageAddress startPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationPage"/> class.
        /// </summary>
        /// <param name="page">The page</param>
        public AllocationPage(Page page)
        {
            this.Header = page.Header;
            this.PageData = page.PageData;
            this.PageAddress = page.PageAddress;
            this.DatabaseId = page.DatabaseId;

            LoadPage(true);

            this.LoadAllocationMap();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationPage"/> class.
        /// </summary>
        /// <param name="database">The database connection.</param>
        /// <param name="address">The page address.</param>
        public AllocationPage(Database database, PageAddress address)
            : base(database, address)
        {
            PageAddress = address;
            this.LoadAllocationMap();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationPage"/> class.
        /// </summary>
        public AllocationPage()
        {
            Header = new Header();
            Header.PageType = PageType.None;
        }

        /// <summary>
        /// Refresh (reload) the Page
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.LoadAllocationMap();
        }

        public Bitmap ToBitmap(int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format1bppIndexed);

            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            IntPtr ptr = bitmapData.Scan0;

            int bytes = (bitmap.Width * bitmap.Height) / 8;

            byte[] values = new byte[bytes];

            Marshal.Copy(ptr, values, 0, bytes);

            Array.Copy(this.PageData, ALLOCATION_ARRAY_OFFSET, values, 0, values.Length);

            Marshal.Copy(values, 0, ptr, bytes);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// Loads the Allocation Map.
        /// </summary>
        private void LoadAllocationMap()
        {
            byte[] allocationData = new byte[8000];
            int allocationArrayOffset;

            switch (Header.PageType)
            {
                case PageType.Gam:
                case PageType.Sgam:
                case PageType.Dcm:
                case PageType.Bcm:

                    this.StartPage = new PageAddress(Header.PageAddress.FileId, 0);
                    allocationArrayOffset = ALLOCATION_ARRAY_OFFSET;
                    break;

                case PageType.Iam:

                    allocationArrayOffset = ALLOCATION_ARRAY_OFFSET;

                    this.LoadIamHeader();
                    this.LoadSinglePageSlots();
                    break;

                default:
                    return;
            }

            Array.Copy(PageData,
                       allocationArrayOffset,
                       allocationData,
                       0,
                       allocationData.Length - (Header.SlotCount * 2));

            BitArray bitArray = new BitArray(allocationData);

            bitArray.CopyTo(this.allocationMap, 0);
        }

        /// <summary>
        /// Loads the IAM header.
        /// </summary>
        private void LoadIamHeader()
        {
            byte[] pageAddress = new byte[6];

            Array.Copy(PageData, START_PAGE_OFFSET, pageAddress, 0, 6);

            this.startPage = new PageAddress(pageAddress);
        }

        /// <summary>
        /// Loads the single page slots (6 byte page addresses * 8)
        /// </summary>
        private void LoadSinglePageSlots()
        {
            int slotOffset = SINGLE_PAGE_SLOT_OFFSET;

            for (int i = 0; i < 8; i++)
            {
                byte[] pageAddress = new byte[6];

                Array.Copy(PageData, slotOffset, pageAddress, 0, 6);

                this.singlePageSlots.Add(new PageAddress(pageAddress));

                slotOffset += 6;
            }
        }

        #region Properties

        /// <summary>
        /// Gets the allocation map.
        /// </summary>
        /// <value>The allocation map.</value>
        public bool[] AllocationMap
        {
            get { return this.allocationMap; }
        }

        /// <summary>
        /// Gets the single page slots.
        /// </summary>
        /// <value>The single page slots.</value>
        public List<PageAddress> SinglePageSlots
        {
            get { return this.singlePageSlots; }
        }

        /// <summary>
        /// Gets or sets the start page.
        /// </summary>
        /// <value>The start page.</value>
        public PageAddress StartPage
        {
            get { return this.startPage; }
            set { this.startPage = value; }
        }

        #endregion
    }
}
