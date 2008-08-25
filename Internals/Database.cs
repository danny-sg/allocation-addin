using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using SqlInternals.AllocationInfo.Internals.Pages;
using SqlInternals.AllocationInfo.Internals.Properties;

namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// A SQL Server Database
    /// </summary>
    public class Database
    {
        public const int ALLOCATION_INTERVAL = 511230;
        public const int PFS_INTERVAL = 8088;
        private readonly Dictionary<int, Allocation> bcm = new Dictionary<int, Allocation>();
        private readonly int compatibilityLevel;
        private readonly bool compatible;
        private readonly int databaseId;
        private readonly Dictionary<int, Allocation> dcm = new Dictionary<int, Allocation>();
        private readonly Dictionary<int, Allocation> gam = new Dictionary<int, Allocation>();
        private readonly string name;
        private readonly Dictionary<int, Allocation> sGam = new Dictionary<int, Allocation>();
        private List<DatabaseFile> files = new List<DatabaseFile>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// </summary>
        /// <param name="databaseId">The database id.</param>
        /// <param name="name">The name.</param>
        /// <param name="state">The state.</param>
        /// <param name="compatibilityLevel">The compatibility level.</param>
        public Database(int databaseId, string name, int state, byte compatibilityLevel)
        {
            this.databaseId = databaseId;
            this.name = name;
            this.compatibilityLevel = compatibilityLevel;

            this.compatible = (compatibilityLevel >= 90 && state == 0);
            
            this.LoadFiles();
        }

        /// <summary>
        /// Refreshes the allocations information.
        /// </summary>
        public void Refresh()
        {
            this.LoadAllocations();
        }

        /// <summary>
        /// Files the size for a particular file
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public int FileSize(int fileId)
        {
            return this.files.Find(delegate(DatabaseFile file) { return file.FileId == fileId; }).Size;
        }

        /// <summary>
        /// Returns a DataTable with the Allocation Units information
        /// </summary>
        /// <returns></returns>
        public DataTable AllocationUnits()
        {
            string sqlCommand = Resources.SQL_Allocation_Units;

            return DataAccess.GetDataTable(sqlCommand, this.Name, "Tables", CommandType.Text);
        }

        /// <summary>
        /// Returns a DataTable with the Table allocation information
        /// </summary>
        /// <returns></returns>
        public DataTable AllocationInfo(bool advanced)
        {
            DataTable allocationInfo = DataAccess.GetDataTable(advanced ? Resources.SQL_SpaceUsedAdvanced : Resources.SQL_SpaceUsed,
                                                               this.Name,
                                                               "Allocation Information",
                                                               CommandType.Text);

            allocationInfo.Columns.Add("KeyColour", typeof(int));
            
            return allocationInfo;                               
        }

        /// <summary>
        /// Returns a DataTable with the physical stats for an index
        /// </summary>
        /// <param name="objectId">The object id to return information on.</param>
        /// <param name="indexId">The index id.</param>
        /// <returns></returns>
        public DataTable IndexPhysicalStats(int objectId, int indexId)
        {
            return DataAccess.GetDataTable(Resources.SQL_Physical_Stats,
                                           this.Name,
                                           "Tables",
                                           CommandType.Text,
                                           new SqlParameter[3]
                                               {
                                                   new SqlParameter("database_name", this.Name),
                                                   new SqlParameter("object_id", objectId),
                                                   new SqlParameter("index_id", indexId)
                                               });
        }

        /// <summary>
        /// Gets the size of a database file
        /// </summary>
        /// <param name="databaseFile">The database file.</param>
        /// <returns></returns>
        internal int GetSize(DatabaseFile databaseFile)
        {
            return (int)DataAccess.GetScalar(this.Name, 
                                             Resources.SQL_File_Size, 
                                             CommandType.Text, 
                                             new SqlParameter[1] { new SqlParameter("file_id", databaseFile.FileId) });
        }

        /// <summary>
        /// Loads the database allocation pages (GAM, SGAM, DCM, and BCM)
        /// </summary>
        private void LoadAllocations()
        {
            foreach (DatabaseFile file in this.files)
            {
                this.gam.Add(file.FileId, new Allocation(this, new PageAddress(file.FileId, 2)));

                this.sGam.Add(file.FileId, new Allocation(this, new PageAddress(file.FileId, 3)));

                this.dcm.Add(file.FileId, new Allocation(this, new PageAddress(file.FileId, 6)));

                this.bcm.Add(file.FileId, new Allocation(this, new PageAddress(file.FileId, 7)));
            }
        }

        /// <summary>
        /// Loads information on the database files
        /// </summary>
        private void LoadFiles()
        {
            string sqlCommand = Resources.SQL_Files;

            DataTable filesDataTable = DataAccess.GetDataTable(sqlCommand, this.Name, "Files", CommandType.Text);

            foreach (DataRow r in filesDataTable.Rows)
            {
                DatabaseFile file = new DatabaseFile((int)r["file_id"], this);
                file.FileGroup = r["filegroup_name"].ToString();
                file.Name = r["name"].ToString();
                file.PhysicalName = r["physical_name"].ToString();
                file.Size = (int)r["size"];
                file.TotalExtents = (int)r["total_extents"];
                file.UsedExtents = (int)r["used_extents"];

                this.files.Add(file);
            }
        }

        #region Properties

        /// <summary>
        /// Gets the database id.
        /// </summary>
        /// <value>The database id.</value>
        public int DatabaseId
        {
            get { return this.databaseId; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the GAM page(s).
        /// </summary>
        /// <value>The GAM page(s).</value>
        public Dictionary<int, Allocation> Gam
        {
            get
            {
                if (this.gam.Count == 0)
                {
                    this.LoadAllocations();
                }

                return this.gam;
            }
        }

        /// <summary>
        /// Gets the SGAM page(s).
        /// </summary>
        /// <value>The SGAM page(s).</value>
        public Dictionary<int, Allocation> SGam
        {
            get
            {
                if (this.sGam.Count == 0)
                {
                    this.LoadAllocations();
                }

                return this.sGam;
            }
        }

        /// <summary>
        /// Gets the DCM page(s).
        /// </summary>
        /// <value>The DCM page(s).</value>
        public Dictionary<int, Allocation> Dcm
        {
            get
            {
                if (this.dcm.Count == 0)
                {
                    this.LoadAllocations();
                }

                return this.dcm;
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
                if (this.bcm.Count == 0)
                {
                    this.LoadAllocations();
                }

                return this.bcm;
            }
        }

        /// <summary>
        /// Gets or sets the database files collection.
        /// </summary>
        /// <value>The files.</value>
        public List<DatabaseFile> Files
        {
            get { return this.files; }
            set { this.files = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Database"/> is compatible with the allocation.
        /// </summary>
        /// <value><c>true</c> if compatible; otherwise, <c>false</c>.</value>
        public bool Compatible
        {
            get { return this.compatible; }
        }

        /// <summary>
        /// Gets the compatibility level.
        /// </summary>
        /// <value>The compatibility level.</value>
        public int CompatibilityLevel
        {
            get { return this.compatibilityLevel; }
        }

        #endregion
    }
}
