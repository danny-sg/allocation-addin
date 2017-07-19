namespace SqlInternals.AllocationInfo.Internals
{
    using System;
    using System.Collections.Generic;

    using SqlInternals.AllocationInfo.Internals.Pages;

    /// <summary>
    /// Collection of Allocation pages separated by an interval
    /// </summary>
    public class Allocation
    {
        private int interval;

        /// <summary>
        /// Initializes a new instance of the <see cref="Allocation"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="pageAddress">The page address.</param>
        public Allocation(Database database, PageAddress pageAddress)
        {
            FileId = pageAddress.FileId;
            MultiFile = false;
            BuildChain(database, pageAddress);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Allocation"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public Allocation(AllocationPage page)
        {
            FileId = page.PageAddress.FileId;
            MultiFile = false;
            Pages.Add(page);
            interval = PageInterval(page.Header.PageType);
            SinglePageSlots.AddRange(page.SinglePageSlots);
        }

        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        /// <value>The file id.</value>
        public int FileId { get; set; }

        /// <summary>
        /// Gets if the Allocation spans multiple files
        /// </summary>
        public bool MultiFile { get; set; }

        /// <summary>
        /// Gets the allocation pages in the Allocation structure
        /// </summary>
        /// <value>The pages.</value>
        public List<AllocationPage> Pages { get; } = new List<AllocationPage>();

        /// <summary>
        /// Gets or sets the single page slots in the allocation structure
        /// </summary>
        /// <value>The single page slots.</value>
        public List<PageAddress> SinglePageSlots { get; set; } = new List<PageAddress>();

        /// <summary>
        /// Checks the allocation status of a particular extent
        /// </summary>
        /// <param name="targetExtent">The target extent.</param>
        /// <param name="fileId">The file id.</param>
        /// <param name="invert">if set to <c>true</c> [invert].</param>
        /// <param name="chain">The chain.</param>
        public static bool CheckAllocationStatus(int targetExtent, int fileId, bool invert, Allocation chain)
        {
            return (!invert && chain.Allocated(targetExtent, fileId) && (fileId == chain.FileId || chain.MultiFile))
                   || (invert && !chain.Allocated(targetExtent, fileId) && (fileId == chain.FileId || chain.MultiFile));
        }

        /// <summary>
        /// Returns if a specific extent is allocated
        /// </summary>
        /// <param name="extent">The extent.</param>
        /// <param name="allocationFileId">The allocation file id.</param>
        public virtual bool Allocated(int extent, int allocationFileId)
        {
            return Pages[(extent * 8) / interval].AllocationMap[extent % ((interval / 8) + 1)];
        }

        /// <summary>
        /// Refreshes the allocation
        /// </summary>
        public void Refresh()
        {
            SinglePageSlots.Clear();

            foreach (var page in Pages)
            {
                page.Refresh();

                SinglePageSlots.AddRange(page.SinglePageSlots);
            }
        }

        /// <summary>
        /// Builds the allocation chain based on an interval
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="pageAddress">The page address.</param>
        protected virtual void BuildChain(Database database, PageAddress pageAddress)
        {
            // Add the first page in the chain
            var page = new AllocationPage(database, pageAddress);

            if (page.Header.PageType == PageType.Iam)
            {
                throw new ArgumentException();
            }

            Pages.Add(page);

            SinglePageSlots.AddRange(page.SinglePageSlots);

            interval = PageInterval(page.Header.PageType);

            // Work out how many pages are in the file based on the size of the file and the interval
            var pageCount = (int)Math.Ceiling(database.FileSize(pageAddress.FileId) / (decimal)interval);

            if (pageCount > 1)
            {
                // Add pages at each interval
                for (var i = 1; i < pageCount; i++)
                {
                    Pages.Add(
                        new AllocationPage(
                            database,
                            new PageAddress(pageAddress.FileId, pageAddress.PageId + (i * interval))));
                }
            }
        }

        /// <summary>
        /// Returns the interval between allocation pages
        /// </summary>
        /// <param name="pageType">Type of the page.</param>
        private static int PageInterval(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Gam:
                case PageType.Sgam:
                case PageType.Dcm:
                case PageType.Bcm:
                case PageType.Iam: return Database.AllocationInterval;
                case PageType.None: return Database.AllocationInterval;
                default: throw new ArgumentException("Unknown Page type");
            }
        }
    }
}