using System;
using System.Collections.Generic;
using System.Text;
using SqlInternals.AllocationInfo.Internals.Pages;

namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// PFS (Page Free Space) allocation
    /// </summary>
    public class Pfs
    {
        private readonly List<PfsPage> pfsPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pfs"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="fileId">The file id.</param>
        public Pfs(Database database, int fileId)
        {
            pfsPages = new List<PfsPage>();

            // Use the interval to get the number of PFS pages in the file
            int pfsCount = (int)Math.Ceiling(database.FileSize(fileId) / (decimal)Database.PFS_INTERVAL);

            // Add the first PFS page
            pfsPages.Add(new PfsPage(database, new PageAddress(fileId, 1)));

            if (pfsCount > 1)
            {
                for (int i = 1; i < pfsCount; i++)
                {
                    // Add the remaining PFS pages
                    pfsPages.Add(new PfsPage(database, new PageAddress(fileId, i * Database.PFS_INTERVAL)));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pfs"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public Pfs(PfsPage page)
        {
            pfsPages = new List<PfsPage>();
            pfsPages.Add(page);
        }

        /// <summary>
        /// Gets a particular PFS byte for a page
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public PfsByte PagePfsByte(int page)
        {
            return pfsPages[page / Database.PFS_INTERVAL].PfsBytes[page % Database.PFS_INTERVAL];
        }
    }
}
