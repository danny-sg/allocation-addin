using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SqlInternals.AllocationInfo.Internals.Compression;
using SqlInternals.AllocationInfo.Internals.Properties;

namespace SqlInternals.AllocationInfo.Internals.Pages
{
    /// <summary>
    /// Database Page
    /// </summary>
    public class Page
    {
        public const int MAX_SIZE = 8192;
        private readonly PageReader reader;
        private readonly Database database;
        private PageAddress pageAddress;
        private int databaseId;
        private string databaseName;
        private Header header;
        private List<int> offsetTable;
        private byte[] pageData;
        private CompressionType compressionType;

        /// <summary>
        /// Create a Page with a DatabasePageReader
        /// </summary>
        public Page(Database database, PageAddress pageAddress)
        {
            this.pageAddress = pageAddress;
            this.database = database;
            this.databaseId = database.DatabaseId;

            if (pageAddress.FileId == 0)
            {
                return;
            }

            this.reader = new DatabasePageReader(this.PageAddress, this.DatabaseId);

            this.LoadPage();
        }

        /// <summary>
        /// Create a Page with a supplied PageReader
        /// </summary>
        public Page(PageReader reader)
        {
            this.reader = reader;

            this.LoadPage();

            this.pageAddress = reader.Header.PageAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        public Page()
        {
        }

        /// <summary>
        /// Returns the description of a PageType
        /// </summary>
        public static string GetPageTypeName(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.Data: return "Data";
                case PageType.Index: return "Index";
                case PageType.Lob3:
                case PageType.Lob4: return "LOB (Text/Image)";
                case PageType.Sort: return "Sort";
                case PageType.Gam: return "GAM (Global Allocation Map)";
                case PageType.Sgam: return "SGAM (Shared Global Allocation Map)";
                case PageType.Iam: return "IAM (Index Allocation Map)";
                case PageType.Pfs: return "PFS (Page Free Space)";
                case PageType.Dcm: return "DCM (Differential Changed Map)";
                case PageType.Bcm: return "BCM (Bulk Changed Map)";
                case PageType.Boot: return "Boot Page";
                case PageType.FileHeader: return "File Header Page";
                default: return string.Empty;
            }
        }

        /// <summary>
        /// Refresh the Page
        /// </summary>
        public virtual void Refresh(bool suppressLoad)
        {
            if (this.PageAddress != PageAddress.Empty)
            {
                if (!suppressLoad)
                {
                    this.offsetTable.Clear();
                }

                this.LoadPage(suppressLoad);
            }
        }

        /// <summary>
        /// Refresh the Page
        /// </summary>
        public virtual void Refresh()
        {
            this.Refresh(false);
        }

        /// <summary>
        /// Get a specified byte of the page data
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public byte PageByte(int offset)
        {
            return this.pageData[offset];
        }

        /// <summary>
        /// Load a page
        /// </summary>
        /// <param name="suppressLoad">Suppress a Page refresh</param>
        protected void LoadPage(bool suppressLoad)
        {
            if (!suppressLoad)
            {
                this.reader.Load();
                this.PageData = this.reader.Data;

                this.reader.LoadHeader();
                this.Header = this.reader.Header;
            }

            if (this.Header.PageType != PageType.Gam ||
                this.Header.PageType != PageType.Sgam ||
                this.Header.PageType != PageType.Pfs)
            {
                this.databaseName = LookupDatabaseName(this.DatabaseId);
                this.Header.PageTypeName = GetPageTypeName(Header.PageType);
                this.Header.AllocationUnit = this.LookupAllocationUnit(Header.AllocationUnitId);

                if (ServerConnection.CurrentConnection().Version > 9)
                {
                    this.CompressionType = this.GetPageCompressionType();
                }
            }

            if (this.Header.SlotCount > 0 && this.Header.ObjectId > 0)
            {
                this.LoadOffsetTable(Header.SlotCount);
            }
        }
       
        private static string LookupDatabaseName(int databaseId)
        {
            string databaseName;

            string sqlCommand = Resources.SQL_Database;

            databaseName = (string)DataAccess.GetScalar("master",
                                                        sqlCommand,
                                                        CommandType.Text,
                                                        new SqlParameter[1] { new SqlParameter("database_id", databaseId) });

            return databaseName;
        }

        private CompressionType GetPageCompressionType()
        {
            string sqlCommand = Resources.SQL_Compression;

            if (Header != null)
            {
                return (CompressionType)(DataAccess.GetScalar(this.DatabaseName, 
                                                              Resources.SQL_Compression, 
                                                              CommandType.Text,
                                                              new SqlParameter[1]
                                                                 {
                                                                 new SqlParameter("partition_id",
                                                                                  this.Header.PartitionId)
                                                                 }) ?? CompressionType.None);
            }
            else
            {
                return CompressionType.None;
            }
        }

        /// <summary>
        /// Load the Page without a data refresh
        /// </summary>
        private void LoadPage()
        {
            this.LoadPage(false);
        }

        /// <summary>
        /// Load the offset table with a given slot count from the page data
        /// </summary>
        private void LoadOffsetTable(int slotCount)
        {
            this.offsetTable = new List<int>();

            for (int i = 2; i <= (slotCount * 2); i += 2)
            {
                this.offsetTable.Add(BitConverter.ToInt16(this.pageData, this.pageData.Length - i));
            }
        }

        /// <summary>
        /// Lookups the Allocation Unit name for the given id
        /// </summary>
        /// <param name="allocationUnitId">The Allocation Unit Id.</param>
        /// <returns></returns>
        private string LookupAllocationUnit(long allocationUnitId)
        {
            string allocationUnitName;

            string sqlCommand = Resources.SQL_Allocation_Unit;

            if (this.DatabaseName == null)
            {
                allocationUnitName = Header.AllocationUnit;
            }
            else
            {
                allocationUnitName = (string)DataAccess.GetScalar(this.DatabaseName,
                                                                   sqlCommand,
                                                                   CommandType.Text,
                                                                   new SqlParameter[1]
                                                                       {
                                                                           new SqlParameter("allocation_unit_id",
                                                                                            allocationUnitId)
                                                                       });
            }

            return allocationUnitName;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the page compression type (SQL Server 2008+)
        /// </summary>
        /// <value>The type of the compression.</value>
        public CompressionType CompressionType
        {
            get { return this.compressionType; }
            set { this.compressionType = value; }
        }

        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName
        {
            get { return this.databaseName; }
        }

        /// <summary>
        /// Gets the database object of the page.
        /// </summary>
        /// <value>The database object of the page.</value>
        public Database Database
        {
            get { return this.database; }
        }

        /// <summary>
        /// Gets or sets the page address.
        /// </summary>
        /// <value>The page address.</value>
        public PageAddress PageAddress
        {
            get { return this.pageAddress; }
            set { this.pageAddress = value; }
        }

        /// <summary>
        /// Gets or sets the database Id.
        /// </summary>
        /// <value>The database Id.</value>
        public int DatabaseId
        {
            get { return this.databaseId; }
            set { this.databaseId = value; }
        }

        /// <summary>
        /// Gets or sets the page data.
        /// </summary>
        /// <value>The page data array.</value>
        public byte[] PageData
        {
            get { return this.pageData; }
            set { this.pageData = value; }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public Header Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        /// <summary>
        /// Gets the row offset table.
        /// </summary>
        /// <value>The row offset table.</value>
        public List<int> OffsetTable
        {
            get { return this.offsetTable; }
        }

        #endregion
    }
}
