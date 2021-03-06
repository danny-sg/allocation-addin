﻿namespace SqlInternals.AllocationInfo.Internals
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using SqlInternals.AllocationInfo.Internals.Pages;
    using SqlInternals.AllocationInfo.Internals.Properties;

    /// <summary>
    /// A SQL Server Database
    /// </summary>
    public class Database
    {
        public const int AllocationInterval = 511230;

        public const int PfsInterval = 8088;

        private readonly Dictionary<int, Allocation> bcm = new Dictionary<int, Allocation>();

        private readonly Dictionary<int, Allocation> dcm = new Dictionary<int, Allocation>();

        private readonly Dictionary<int, Allocation> gam = new Dictionary<int, Allocation>();

        private readonly Dictionary<int, Allocation> sgam = new Dictionary<int, Allocation>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        /// <param name="databaseId">The database id.</param>
        /// <param name="name">The name.</param>
        /// <param name="state">The state.</param>
        /// <param name="compatibilityLevel">The compatibility level.</param>
        public Database(int databaseId, string name, int state, byte compatibilityLevel)
        {
            DatabaseId = databaseId;
            Name = name;
            CompatibilityLevel = compatibilityLevel;

            Compatible = compatibilityLevel >= 90 && state == 0;

            if (Compatible)
            {
                LoadFiles();
            }
        }

        /// <summary>
        /// Gets the BCM page(s).
        /// </summary>
        /// <value>The BCM page(s).</value>
        public Dictionary<int, Allocation> Bcm
        {
            get
            {
                if (bcm.Count == 0)
                {
                    LoadAllocations();
                }

                return bcm;
            }
        }

        /// <summary>
        /// Gets the compatibility level.
        /// </summary>
        /// <value>The compatibility level.</value>
        public int CompatibilityLevel { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Database"/> is compatible with the allocation.
        /// </summary>
        /// <value><c>true</c> if compatible; otherwise, <c>false</c>.</value>
        public bool Compatible { get; }

        /// <summary>
        /// Gets the database id.
        /// </summary>
        /// <value>The database id.</value>
        public int DatabaseId { get; }

        /// <summary>
        /// Gets the DCM page(s).
        /// </summary>
        /// <value>The DCM page(s).</value>
        public Dictionary<int, Allocation> Dcm
        {
            get
            {
                if (dcm.Count == 0)
                {
                    LoadAllocations();
                }

                return dcm;
            }
        }

        /// <summary>
        /// Gets or sets the database files collection.
        /// </summary>
        /// <value>The files.</value>
        public List<DatabaseFile> Files { get; set; } = new List<DatabaseFile>();

        /// <summary>
        /// Gets the GAM page(s).
        /// </summary>
        /// <value>The GAM page(s).</value>
        public Dictionary<int, Allocation> Gam
        {
            get
            {
                if (gam.Count == 0)
                {
                    LoadAllocations();
                }

                return gam;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the SGAM page(s).
        /// </summary>
        /// <value>The SGAM page(s).</value>
        public Dictionary<int, Allocation> Sgam
        {
            get
            {
                if (sgam.Count == 0)
                {
                    LoadAllocations();
                }

                return sgam;
            }
        }

        /// <summary>
        /// Returns a DataTable with the Table allocation information
        /// </summary>
        public DataTable AllocationInfo(bool advanced)
        {
            var allocationInfo = DataAccess.GetDataTable(
                advanced ? Resources.SQL_SpaceUsedAdvanced : Resources.SQL_SpaceUsed,
                Name,
                "Allocation Information",
                CommandType.Text);

            allocationInfo.Columns.Add("KeyColour", typeof(int));

            return allocationInfo;
        }

        /// <summary>
        /// Returns a DataTable with the Allocation Units information
        /// </summary>
        public DataTable AllocationUnits()
        {
            var sqlCommand = Resources.SQL_Allocation_Units;

            return DataAccess.GetDataTable(sqlCommand, Name, "Tables", CommandType.Text);
        }

        /// <summary>
        /// Files the size for a particular file
        /// </summary>
        /// <param name="fileId">The file id.</param>
        public int FileSize(int fileId)
        {
            return Files.Find(delegate (DatabaseFile file) { return file.FileId == fileId; }).Size;
        }

        /// <summary>
        /// Returns a DataTable with the physical stats for an index
        /// </summary>
        /// <param name="objectId">The object id to return information on.</param>
        /// <param name="indexId">The index id.</param>
        public DataTable IndexPhysicalStats(int objectId, int indexId)
        {
            return DataAccess.GetDataTable(
                Resources.SQL_Physical_Stats,
                Name,
                "Tables",
                CommandType.Text,
                new[]
                    {
                        new SqlParameter("database_name", Name),
                        new SqlParameter("object_id", objectId),
                        new SqlParameter("index_id", indexId)
                    });
        }

        /// <summary>
        /// Refreshes the allocations information.
        /// </summary>
        public void Refresh()
        {
            LoadAllocations();
        }

        internal DataTable AllocationInfo(bool advanced, System.ComponentModel.BackgroundWorker worker)
        {
            var allocationInfo = DataAccess.GetDataTable(
                advanced ? Resources.SQL_SpaceUsedAdvanced : Resources.SQL_SpaceUsed,
                Name,
                "Allocation Information",
                CommandType.Text,
                worker);

            allocationInfo?.Columns.Add("KeyColour", typeof(int));

            return allocationInfo;
        }

        /// <summary>
        /// Gets the size of a database file
        /// </summary>
        /// <param name="databaseFile">The database file.</param>
        internal int GetSize(DatabaseFile databaseFile)
        {
            return (int)DataAccess.GetScalar(
                Name,
                Resources.SQL_File_Size,
                CommandType.Text,
                new SqlParameter[1] { new SqlParameter("file_id", databaseFile.FileId) });
        }

        /// <summary>
        /// Loads the database allocation pages (GAM, SGAM, DCM, and BCM)
        /// </summary>
        private void LoadAllocations()
        {
            foreach (var file in Files)
            {
                gam.Add(file.FileId, new Allocation(this, new PageAddress(file.FileId, 2)));

                sgam.Add(file.FileId, new Allocation(this, new PageAddress(file.FileId, 3)));

                dcm.Add(file.FileId, new Allocation(this, new PageAddress(file.FileId, 6)));

                bcm.Add(file.FileId, new Allocation(this, new PageAddress(file.FileId, 7)));
            }
        }

        /// <summary>
        /// Loads information on the database files
        /// </summary>
        private void LoadFiles()
        {
            var sqlCommand = Resources.SQL_Files;

            var filesDataTable = DataAccess.GetDataTable(sqlCommand, Name, "Files", CommandType.Text);

            foreach (DataRow r in filesDataTable.Rows)
            {
                var file = new DatabaseFile((int)r["file_id"], this)
                               {
                                   FileGroup = r["filegroup_name"].ToString(),
                                   Name = r["name"].ToString(),
                                   PhysicalName = r["physical_name"].ToString(),
                                   Size = (int)r["size"],
                                   TotalExtents = (int)r["total_extents"],
                                   UsedExtents = (int)r["used_extents"]
                               };

                Files.Add(file);
            }
        }
    }
}