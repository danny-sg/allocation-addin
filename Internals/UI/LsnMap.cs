using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SqlInternals.AllocationInfo.Internals.Pages;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    public class LsnMap
    {
        private Dictionary<int, decimal> lsnList;
        private decimal minLsn;
        private decimal maxLsn;
        private int fileId;

        public LsnMap()
        {
            this.lsnList = new Dictionary<int, decimal>();
        }

        public static List<LsnMap> LoadLsnMaps(Database database, BackgroundWorker worker)
        {
            int totalPages = 0;
            int completePages = 0;

            List<LsnMap> lsnMaps = new List<LsnMap>();

            foreach (DatabaseFile file in database.Files)
            {
                totalPages += file.TotalPages;
            }
            foreach (DatabaseFile file in database.Files)
            {
                LsnMap map = LoadFileLsnMap(file, database.Gam[file.FileId], worker, 0, file.TotalPages, totalPages, completePages);

                if (null == map)
                {
                    return null;
                }
                {
                    lsnMaps.Add(map);
                    completePages += file.TotalPages;
                }
            }

            return lsnMaps;
        }

        public static LsnMap LoadFileLsnMap(DatabaseFile file,
                                            Allocation gam,
                                            BackgroundWorker worker,
                                            int startPage,
                                            int endPage,
                                            int totalPages,
                                            int completePages)
        {
            LsnMap lsnMap = new LsnMap();

            lsnMap.FileId = file.FileId;

            Header pageHeader;

            for (int i = startPage; i < endPage; i++)
            {

                if (worker.CancellationPending)
                {
                    return null;
                }

                if (!gam.Allocated(i / 8, file.FileId))
                {
                    pageHeader = DatabaseHeaderReader.LoadHeader(new PageAddress(file.FileId, i));

                    if (pageHeader.ObjectId > 0 && pageHeader.Lsn.ToDecimal() > 1)
                    {
                        decimal lsn = pageHeader.Lsn.ToDecimalFileOffsetOnly();

                        if (i == 0)
                        {
                            lsnMap.MinLsn = lsn;
                            lsnMap.MaxLsn = lsn;
                        }

                        if (lsn < lsnMap.MinLsn)
                        {
                            lsnMap.MinLsn = lsn;
                        }
                        else if (lsn > lsnMap.MaxLsn)
                        {
                            lsnMap.MaxLsn = lsn;
                        }

                        lsnMap.LsnList.Add(pageHeader.PageAddress.PageId, lsn);
                    }

                    worker.ReportProgress((int)((i + completePages) / (float)totalPages * 100), new PageAddress(file.FileId, i));
                }
            }

            return lsnMap;
        }

        public Dictionary<int, decimal> LsnList
        {
            get { return lsnList; }
            set { lsnList = value; }
        }

        public decimal MinLsn
        {
            get { return minLsn; }
            set { minLsn = value; }
        }

        public decimal MaxLsn
        {
            get { return maxLsn; }
            set { maxLsn = value; }
        }

        public int FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }
    }
}
