using System;
using System.Collections.Generic;
using System.Text;

namespace SqlInternals.AllocationInfo.Internals.Pages
{
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

        public PageAddress PageAddress
        {
            get { return pageAddress; }
            set { pageAddress = value; }
        }

        public PageAddress NextPage
        {
            get { return nextPage; }
            set { nextPage = value; }
        }

        public PageAddress PreviousPage
        {
            get { return previousPage; }
            set { previousPage = value; }
        }

        public PageType PageType
        {
            get { return pageType; }
            set { pageType = value; }
        }

        public long AllocationUnitId
        {
            get { return allocationUnitId; }
            set { allocationUnitId = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public int IndexId
        {
            get { return indexId; }
            set { indexId = value; }
        }

        public int SlotCount
        {
            get { return slotCount; }
            set { slotCount = value; }
        }

        public int FreeCount
        {
            get { return freeCount; }
            set { freeCount = value; }
        }

        public int FreeData
        {
            get { return freeData; }
            set { freeData = value; }
        }

        public int MinLen
        {
            get { return minLen; }
            set { minLen = value; }
        }

        public int ReservedCount
        {
            get { return reservedCount; }
            set { reservedCount = value; }
        }

        public int XactReservedCount
        {
            get { return xactReservedCount; }
            set { xactReservedCount = value; }
        }

        public long TornBits
        {
            get { return tornBits; }
            set { tornBits = value; }
        }

        public string FlagBits
        {
            get { return flagBits; }
            set { flagBits = value; }
        }

        public long ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }

        public long PartitionId
        {
            get { return partitionId; }
            set { partitionId = value; }
        }

        public LogSequenceNumber Lsn
        {
            get { return lsn; }
            set { lsn = value; }
        }

        public string AllocationUnit
        {
            get { return allocationUnit; }
            set { allocationUnit = value; }
        }

        public string PageTypeName
        {
            get { return pageTypeName; }
            set { pageTypeName = value; }
        }
    }
}
