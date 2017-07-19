namespace SqlInternals.AllocationInfo.Internals.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using SqlInternals.AllocationInfo.Internals.Compression;
    using SqlInternals.AllocationInfo.Internals.Properties;

    /// <summary>
    /// Database Page
    /// </summary>
    public class Page
    {
        public const int MaxSize = 8192;

        private readonly PageReader reader;

        /// <summary>
        /// Create a Page with a DatabasePageReader
        /// </summary>
        public Page(Database database, PageAddress pageAddress)
        {
            PageAddress = pageAddress;
            Database = database;
            DatabaseId = database.DatabaseId;

            if (pageAddress.FileId == 0)
            {
                return;
            }

            reader = new DatabasePageReader(PageAddress, DatabaseId);

            LoadPage();
        }

        /// <summary>
        /// Create a Page with a supplied PageReader
        /// </summary>
        public Page(PageReader reader)
        {
            this.reader = reader;

            LoadPage();

            PageAddress = reader.Header.PageAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        public Page()
        {
        }

        /// <summary>
        /// Gets or sets the page compression type (SQL Server 2008+)
        /// </summary>
        /// <value>The type of the compression.</value>
        public CompressionType CompressionType { get; set; }

        /// <summary>
        /// Gets the database object of the page.
        /// </summary>
        /// <value>The database object of the page.</value>
        public Database Database { get; }

        /// <summary>
        /// Gets or sets the database Id.
        /// </summary>
        /// <value>The database Id.</value>
        public int DatabaseId { get; set; }

        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName { get; private set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public Header Header { get; set; }

        /// <summary>
        /// Gets the row offset table.
        /// </summary>
        /// <value>The row offset table.</value>
        public List<int> OffsetTable { get; private set; }

        /// <summary>
        /// Gets or sets the page address.
        /// </summary>
        /// <value>The page address.</value>
        public PageAddress PageAddress { get; set; }

        /// <summary>
        /// Gets or sets the page data.
        /// </summary>
        /// <value>The page data array.</value>
        public byte[] PageData { get; set; }

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
        /// Get a specified byte of the page data
        /// </summary>
        /// <param name="offset">The offset.</param>
        public byte PageByte(int offset)
        {
            return PageData[offset];
        }

        /// <summary>
        /// Refresh the Page
        /// </summary>
        public virtual void Refresh(bool suppressLoad)
        {
            if (PageAddress != PageAddress.Empty)
            {
                if (!suppressLoad)
                {
                    OffsetTable.Clear();
                }

                LoadPage(suppressLoad);
            }
        }

        /// <summary>
        /// Refresh the Page
        /// </summary>
        public virtual void Refresh()
        {
            Refresh(false);
        }

        /// <summary>
        /// Load a page
        /// </summary>
        /// <param name="suppressLoad">Suppress a Page refresh</param>
        protected void LoadPage(bool suppressLoad)
        {
            if (!suppressLoad)
            {
                reader.Load();
                PageData = reader.Data;

                reader.LoadHeader();
                Header = reader.Header;
            }

            if (Header.PageType != PageType.Gam || Header.PageType != PageType.Sgam || Header.PageType != PageType.Pfs)
            {
                DatabaseName = LookupDatabaseName(DatabaseId);
                Header.PageTypeName = GetPageTypeName(Header.PageType);
                Header.AllocationUnit = LookupAllocationUnit(Header.AllocationUnitId);

                if (ServerConnection.CurrentConnection().Version > 9)
                {
                    CompressionType = GetPageCompressionType();
                }
            }

            if (Header.SlotCount > 0 && Header.ObjectId > 0)
            {
                LoadOffsetTable(Header.SlotCount);
            }
        }

        private static string LookupDatabaseName(int databaseId)
        {
            string databaseName;

            var sqlCommand = Resources.SQL_Database;

            databaseName = (string)DataAccess.GetScalar(
                "master",
                sqlCommand,
                CommandType.Text,
                new SqlParameter[1] { new SqlParameter("database_id", databaseId) });

            return databaseName;
        }

        private CompressionType GetPageCompressionType()
        {
            if (Header != null)
            {
                return (CompressionType)(DataAccess.GetScalar(
                                             DatabaseName,
                                             Resources.SQL_Compression,
                                             CommandType.Text,
                                             new[] { new SqlParameter("partition_id", Header.PartitionId) })
                                         ?? CompressionType.None);
            }

            return CompressionType.None;
        }

        /// <summary>
        /// Load the offset table with a given slot count from the page data
        /// </summary>
        private void LoadOffsetTable(int slotCount)
        {
            OffsetTable = new List<int>();

            for (var i = 2; i <= (slotCount * 2); i += 2)
            {
                OffsetTable.Add(BitConverter.ToInt16(PageData, PageData.Length - i));
            }
        }

        /// <summary>
        /// Load the Page without a data refresh
        /// </summary>
        private void LoadPage()
        {
            LoadPage(false);
        }

        /// <summary>
        /// Lookups the Allocation Unit name for the given id
        /// </summary>
        /// <param name="allocationUnitId">The Allocation Unit Id.</param>
        private string LookupAllocationUnit(long allocationUnitId)
        {
            string allocationUnitName;

            var sqlCommand = Resources.SQL_Allocation_Unit;

            if (DatabaseName == null)
            {
                allocationUnitName = Header.AllocationUnit;
            }
            else
            {
                allocationUnitName = (string)DataAccess.GetScalar(
                    DatabaseName,
                    sqlCommand,
                    CommandType.Text,
                    new[] { new SqlParameter("allocation_unit_id", allocationUnitId) });
            }

            return allocationUnitName;
        }
    }
}