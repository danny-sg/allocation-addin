using System;
using System.Collections.Generic;
using System.Text;

namespace SqlInternals.AllocationInfo.Internals.Pages
{
    /// <summary>
    /// Page Header
    /// </summary>
    public class Header
    {
        private string allocationUnit;
        private long allocationUnitId;
        private string flagBits;
        private int freeCount;
        private int freeData;
        private int indexId;
        private int level;
        private LogSequenceNumber lsn;
        private int minLen;
        private PageAddress nextPage;
        private long objectId;
        private PageAddress pageAddress;
        private PageType pageType;
        private string pageTypeName;
        private long partitionId;
        private PageAddress previousPage;
        private int reservedCount;
        private int slotCount;
        private long tornBits;
        private int xactReservedCount;

        /// <summary>
        /// Gets or sets the page address.
        /// </summary>
        /// <value>The page address.</value>
        public PageAddress PageAddress
        {
            get { return pageAddress; }
            set { pageAddress = value; }
        }

        /// <summary>
        /// Gets or sets the next page.
        /// </summary>
        /// <value>The next page.</value>
        public PageAddress NextPage
        {
            get { return nextPage; }
            set { nextPage = value; }
        }

        /// <summary>
        /// Gets or sets the previous page.
        /// </summary>
        /// <value>The previous page.</value>
        public PageAddress PreviousPage
        {
            get { return previousPage; }
            set { previousPage = value; }
        }

        /// <summary>
        /// Gets or sets the type of the page.
        /// </summary>
        /// <value>The type of the page.</value>
        public PageType PageType
        {
            get { return pageType; }
            set { pageType = value; }
        }

        /// <summary>
        /// Gets or sets the allocation unit id.
        /// </summary>
        /// <value>The allocation unit id.</value>
        public long AllocationUnitId
        {
            get { return allocationUnitId; }
            set { allocationUnitId = value; }
        }

        /// <summary>
        /// Gets or sets the index level.
        /// </summary>
        /// <value>The index level.</value>
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// Gets or sets the index id.
        /// </summary>
        /// <value>The index id.</value>
        public int IndexId
        {
            get { return indexId; }
            set { indexId = value; }
        }

        /// <summary>
        /// Gets or sets the page slot count.
        /// </summary>
        /// <value>The page slot count.</value>
        public int SlotCount
        {
            get { return slotCount; }
            set { slotCount = value; }
        }

        /// <summary>
        /// Gets or sets the free count in bytes.
        /// </summary>
        /// <value>The free count in bytes.</value>
        public int FreeCount
        {
            get { return freeCount; }
            set { freeCount = value; }
        }

        /// <summary>
        /// Gets or sets the free data.
        /// </summary>
        /// <value>The free data.</value>
        public int FreeData
        {
            get { return freeData; }
            set { freeData = value; }
        }

        /// <summary>
        /// Gets or sets the min len.
        /// </summary>
        /// <value>The min len.</value>
        public int MinLen
        {
            get { return minLen; }
            set { minLen = value; }
        }

        /// <summary>
        /// Gets or sets the reserved count.
        /// </summary>
        /// <value>The reserved count.</value>
        public int ReservedCount
        {
            get { return reservedCount; }
            set { reservedCount = value; }
        }

        /// <summary>
        /// Gets or sets the xact reserved count value.
        /// </summary>
        /// <value>The xact reserved count value.</value>
        public int XactReservedCount
        {
            get { return xactReservedCount; }
            set { xactReservedCount = value; }
        }

        /// <summary>
        /// Gets or sets the torn bits value.
        /// </summary>
        /// <value>The torn bits value.</value>
        public long TornBits
        {
            get { return tornBits; }
            set { tornBits = value; }
        }

        /// <summary>
        /// Gets or sets the flag bits value.
        /// </summary>
        /// <value>The flag bits value.</value>
        public string FlagBits
        {
            get { return flagBits; }
            set { flagBits = value; }
        }

        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        /// <value>The object id.</value>
        public long ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }

        /// <summary>
        /// Gets or sets the partition id.
        /// </summary>
        /// <value>The partition id.</value>
        public long PartitionId
        {
            get { return partitionId; }
            set { partitionId = value; }
        }

        /// <summary>
        /// Gets or sets the page LSN.
        /// </summary>
        /// <value>The page LSN.</value>
        public LogSequenceNumber Lsn
        {
            get { return lsn; }
            set { lsn = value; }
        }

        /// <summary>
        /// Gets or sets the allocation unit name.
        /// </summary>
        /// <value>The allocation unit name.</value>
        public string AllocationUnit
        {
            get { return allocationUnit; }
            set { allocationUnit = value; }
        }

        /// <summary>
        /// Gets or sets the name of the page type.
        /// </summary>
        /// <value>The name of the page type.</value>
        public string PageTypeName
        {
            get { return pageTypeName; }
            set { pageTypeName = value; }
        }
    }
}
