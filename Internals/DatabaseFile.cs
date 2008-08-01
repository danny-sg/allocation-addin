using System;
using System.Collections.Generic;
using System.Text;

namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// A database file
    /// </summary>
    public class DatabaseFile
    {
        private string fileGroup;
        private int fileId;
        private string name;
        private string physicalName;
        private int size;
        private int totalExtents;
        private int usedExtents;
        private Database database;

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
        /// Refreshes the size.
        /// </summary>
        public void RefreshSize()
        {
            this.Size = database.GetSize(this);
        }

        /// <summary>
        /// Gets or sets the number total extents in the file
        /// </summary>
        /// <value>The total extents in the file</value>
        public int TotalExtents
        {
            get { return totalExtents; }
            set { totalExtents = value; }
        }

        /// <summary>
        /// Gets the total number of pages in the file
        /// </summary>
        /// <value>The total number of pages.</value>
        public int TotalPages
        {
            get { return totalExtents * 8; }
        }

        /// <summary>
        /// Gets the used number of pages in the file
        /// </summary>
        /// <value>The number of used pages.</value>
        public int UsedPages
        {
            get { return usedExtents * 8; }
        }

        /// <summary>
        /// Gets or sets the number of used extents.
        /// </summary>
        /// <value>The number of used extents.</value>
        public int UsedExtents
        {
            get { return usedExtents; }
            set { usedExtents = value; }
        }

        /// <summary>
        /// Gets the total size in MB of the file.
        /// </summary>
        /// <value>The total size in MB</value>
        public float TotalMb
        {
            get { return ((totalExtents * 64) / 1024F); }
        }

        /// <summary>
        /// Gets the used size in MB of the file.
        /// </summary>
        /// <value>The used size in MB</value>
        public float UsedMb
        {
            get { return ((usedExtents * 64) / 1024F); }
        }

        /// <summary>
        /// Gets or sets the file Id.
        /// </summary>
        /// <value>The file id.</value>
        public int FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        /// <summary>
        /// Gets or sets the file group.
        /// </summary>
        /// <value>The file group.</value>
        public string FileGroup
        {
            get { return fileGroup; }
            set { fileGroup = value; }
        }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        /// <value>The file name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the name of the physical name of the file
        /// </summary>
        /// <value>The name of the physical nmae of the file</value>
        public string PhysicalName
        {
            get { return physicalName; }
            set { physicalName = value; }
        }

        /// <summary>
        /// Gets the file name
        /// </summary>
        /// <value>The file name.</value>
        public string FileName
        {
            get { return physicalName.Substring(physicalName.LastIndexOf(@"\") + 1); }
        }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>The size.</value>
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// Gets or sets the database file file belongs to
        /// </summary>
        /// <value>The database of the file.</value>
        public Database Database
        {
            get { return database; }
            set { database = value; }
        }

    }
}
