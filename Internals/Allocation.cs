using System;
using System.Collections.Generic;
using System.Text;
using SqlInternals.AllocationInfo.Internals.Pages;

namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// Collection of Allocation pages separated by an interval
    /// </summary>
    public class Allocation
    {
        private readonly List<AllocationPage> pages = new List<AllocationPage>();
        private int fileId;
        private int interval;
        private bool multiFile;
        private List<PageAddress> singlePageSlots = new List<PageAddress>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Allocation"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="pageAddress">The page address.</param>
        public Allocation(Database database, PageAddress pageAddress)
        {
            this.FileId = pageAddress.FileId;
            this.MultiFile = false;
            this.BuildChain(database, pageAddress);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Allocation"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public Allocation(AllocationPage page)
        {
            FileId = page.PageAddress.FileId;
            MultiFile = false;
            pages.Add(page);
            interval = PageInterval(page.Header.PageType);
            singlePageSlots.AddRange(page.SinglePageSlots);
        }

        /// <summary>
        /// Gets the allocation pages in the Allocation structure
        /// </summary>
        /// <value>The pages.</value>
        public List<AllocationPage> Pages
        {
            get { return pages; }
        }

        /// <summary>
        /// Gets or sets the single page slots in the allocation structure
        /// </summary>
        /// <value>The single page slots.</value>
        public List<PageAddress> SinglePageSlots
        {
            get { return singlePageSlots; }
            set { singlePageSlots = value; }
        }

        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        /// <value>The file id.</value>
        public int FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        /// <summary>
        /// Determines if the Allocation spans multiple files
        /// </summary>
        public bool MultiFile
        {
            get { return multiFile; }
            set { multiFile = value; }
        }

        /// <summary>
        /// Returns if a specific extent is allocated
        /// </summary>
        /// <param name="extent">The extent.</param>
        /// <param name="allocationFileId">The allocation file id.</param>
        /// <returns></returns>
        public virtual bool Allocated(int extent, int allocationFileId)
        {
            return pages[(extent * 8) / interval].AllocationMap[extent % ((interval / 8) + 1)];
        }

        /// <summary>
        /// Checks the allocation status of a particular extent
        /// </summary>
        /// <param name="targetExtent">The target extent.</param>
        /// <param name="fileId">The file id.</param>
        /// <param name="invert">if set to <c>true</c> [invert].</param>
        /// <param name="chain">The chain.</param>
        /// <returns></returns>
        public static bool CheckAllocationStatus(int targetExtent, int fileId, bool invert, Allocation chain)
        {
            return (!invert
                    && chain.Allocated(targetExtent, fileId)
                    && (fileId == chain.FileId || chain.MultiFile)
                   )
                   ||
                   (invert
                    && !chain.Allocated(targetExtent, fileId)
                    && (fileId == chain.FileId || chain.MultiFile)
                   );
        }

        /// <summary>
        /// Builds the allocation chain based on an interval
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="pageAddress">The page address.</param>
        protected virtual void BuildChain(Database database, PageAddress pageAddress)
        {
            // Add the first page in the chain
            AllocationPage page = new AllocationPage(database, pageAddress);

            if (page.Header.PageType == PageType.Iam)
            {
                throw new ArgumentException();
            }

            pages.Add(page);

            singlePageSlots.AddRange(page.SinglePageSlots);

            interval = PageInterval(page.Header.PageType);

            // Work out how many pages are in the file based on the size of the file and the interval
            int pageCount = (int)Math.Ceiling(database.FileSize(pageAddress.FileId) / (decimal)interval);

            if (pageCount > 1)
            {
                // Add pages at each interval
                for (int i = 1; i < pageCount; i++)
                {
                    pages.Add(new AllocationPage(database,
                                                 new PageAddress(pageAddress.FileId,
                                                                 pageAddress.PageId + (i * interval))));
                }
            }
        }

        /// <summary>
        /// Returns the interval between allocation pages
        /// </summary>
        /// <param name="pageType">Type of the page.</param>
        /// <returns></returns>
        private static int PageInterval(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Gam:
                case PageType.Sgam:
                case PageType.Dcm:
                case PageType.Bcm:
                case PageType.Iam:

                    return Database.ALLOCATION_INTERVAL;
                case PageType.None:
                    return Database.ALLOCATION_INTERVAL; //TODO: ?
                default:
                    throw new ArgumentException("Unknown Page type");
            }
        }

        /// <summary>
        /// Refreshes the allocation
        /// </summary>
        public void Refresh()
        {
            singlePageSlots.Clear();

            foreach (AllocationPage page in Pages)
            {
                page.Refresh();
                singlePageSlots.AddRange(page.SinglePageSlots);
            }
        }
    }
}
