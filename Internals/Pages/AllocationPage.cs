namespace SqlInternals.AllocationInfo.Internals.Pages
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Page used for internal SQL Server allocations
    /// </summary>
    /// <remarks>
    /// Includes bitmap for storing allocation information
    /// </remarks>
    public class AllocationPage : Page
    {
        public const int AllocationArrayOffset = 194;

        public const int SinglePageSlotOffset = 142;

        public const int StartPageOffset = 136;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationPage"/> class.
        /// </summary>
        /// <param name="page">The page</param>
        public AllocationPage(Page page)
        {
            Header = page.Header;
            PageData = page.PageData;
            PageAddress = page.PageAddress;
            DatabaseId = page.DatabaseId;

            LoadPage(true);

            LoadAllocationMap();
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
            LoadAllocationMap();
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
        /// Gets the allocation map.
        /// </summary>
        /// <value>The allocation map.</value>
        public bool[] AllocationMap { get; } = new bool[64000];

        /// <summary>
        /// Gets the single page slots.
        /// </summary>
        /// <value>The single page slots.</value>
        public List<PageAddress> SinglePageSlots { get; } = new List<PageAddress>();

        /// <summary>
        /// Gets or sets the start page.
        /// </summary>
        /// <value>The start page.</value>
        public PageAddress StartPage { get; set; }

        /// <summary>
        /// Refresh (reload) the Page
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            LoadAllocationMap();
        }

        public Bitmap ToBitmap(int width, int height)
        {
            var bitmap = new Bitmap(width, height, PixelFormat.Format1bppIndexed);

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                bitmap.PixelFormat);

            var ptr = bitmapData.Scan0;

            var bytes = (bitmap.Width * bitmap.Height) / 8;

            var values = new byte[bytes];

            Marshal.Copy(ptr, values, 0, bytes);

            Array.Copy(PageData, AllocationArrayOffset, values, 0, values.Length);

            Marshal.Copy(values, 0, ptr, bytes);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        /// <summary>
        /// Loads the Allocation Map.
        /// </summary>
        private void LoadAllocationMap()
        {
            var allocationData = new byte[8000];
            int allocationArrayOffset;

            switch (Header.PageType)
            {
                case PageType.Gam:
                case PageType.Sgam:
                case PageType.Dcm:
                case PageType.Bcm:

                    StartPage = new PageAddress(Header.PageAddress.FileId, 0);
                    allocationArrayOffset = AllocationArrayOffset;
                    break;

                case PageType.Iam:

                    allocationArrayOffset = AllocationArrayOffset;

                    LoadIamHeader();
                    LoadSinglePageSlots();
                    break;

                default: return;
            }

            Array.Copy(
                PageData,
                allocationArrayOffset,
                allocationData,
                0,
                allocationData.Length - (Header.SlotCount * 2));

            var bitArray = new BitArray(allocationData);

            bitArray.CopyTo(AllocationMap, 0);
        }

        /// <summary>
        /// Loads the IAM header.
        /// </summary>
        private void LoadIamHeader()
        {
            var pageAddress = new byte[6];

            Array.Copy(PageData, StartPageOffset, pageAddress, 0, 6);

            StartPage = new PageAddress(pageAddress);
        }

        /// <summary>
        /// Loads the single page slots (6 byte page addresses * 8)
        /// </summary>
        private void LoadSinglePageSlots()
        {
            var slotOffset = SinglePageSlotOffset;

            for (var i = 0; i < 8; i++)
            {
                var pageAddress = new byte[6];

                Array.Copy(PageData, slotOffset, pageAddress, 0, 6);

                SinglePageSlots.Add(new PageAddress(pageAddress));

                slotOffset += 6;
            }
        }
    }
}