namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// A database file
    /// </summary>
    public class DatabaseFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseFile"/> class.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="database">The database.</param>
        public DatabaseFile(int fileId, Database database)
        {
            Database = database;
            FileId = fileId;
        }

        /// <summary>
        /// Gets or sets the database file file belongs to
        /// </summary>
        /// <value>The database of the file.</value>
        public Database Database { get; set; }

        /// <summary>
        /// Gets or sets the file group.
        /// </summary>
        /// <value>The file group.</value>
        public string FileGroup { get; set; }

        /// <summary>
        /// Gets or sets the file Id.
        /// </summary>
        /// <value>The file id.</value>
        public int FileId { get; set; }

        /// <summary>
        /// Gets the file name
        /// </summary>
        /// <value>The file name.</value>
        public string FileName => PhysicalName.Substring(PhysicalName.LastIndexOf(@"\") + 1);

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        /// <value>The file name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the physical name of the file
        /// </summary>
        /// <value>The name of the physical nmae of the file</value>
        public string PhysicalName { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the number total extents in the file
        /// </summary>
        /// <value>The total extents in the file</value>
        public int TotalExtents { get; set; }

        /// <summary>
        /// Gets the total size in MB of the file.
        /// </summary>
        /// <value>The total size in MB</value>
        public float TotalMb => (TotalExtents * 64) / 1024F;

        /// <summary>
        /// Gets the total number of pages in the file
        /// </summary>
        /// <value>The total number of pages.</value>
        public int TotalPages => TotalExtents * 8;

        /// <summary>
        /// Gets or sets the number of used extents.
        /// </summary>
        /// <value>The number of used extents.</value>
        public int UsedExtents { get; set; }

        /// <summary>
        /// Gets the used size in MB of the file.
        /// </summary>
        /// <value>The used size in MB</value>
        public float UsedMb => (UsedExtents * 64) / 1024F;

        /// <summary>
        /// Gets the used number of pages in the file
        /// </summary>
        /// <value>The number of used pages.</value>
        public int UsedPages => UsedExtents * 8;

        /// <summary>
        /// Refreshes the size.
        /// </summary>
        public void RefreshSize()
        {
            Size = Database.GetSize(this);
        }
    }
}