using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SqlInternals.AllocationInfo.Internals.Pages;
using SqlInternals.AllocationInfo.Internals.Properties;

namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// Collection of pages in the server's buffer bool
    /// </summary>
    public class BufferPool
    {
        private readonly List<PageAddress> cleanPages;
        private readonly List<PageAddress> dirtyPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferPool"/> class.
        /// </summary>
        public BufferPool()
        {
            cleanPages = new List<PageAddress>();
            dirtyPages = new List<PageAddress>();

            Refresh();
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            cleanPages.Clear();
            dirtyPages.Clear();

            if (ServerConnection.CurrentConnection().CurrentDatabase == null)
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(ServerConnection.CurrentConnection().ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(Resources.SQL_Buffer_Pool, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("database", ServerConnection.CurrentConnection().CurrentDatabase.Name);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetBoolean(2))
                    {
                        dirtyPages.Add(new PageAddress(reader.GetInt32(0), reader.GetInt32(1)));
                    }

                    cleanPages.Add(new PageAddress(reader.GetInt32(0), reader.GetInt32(1)));
                }

                conn.Close();
            }
        }

        /// <summary>
        /// Gets the clean page addresses.
        /// </summary>
        /// <value>The clean page addresses.</value>
        public List<PageAddress> CleanPages
        {
            get { return cleanPages; }
        }

        /// <summary>
        /// Gets the dirty page addresses.
        /// </summary>
        /// <value>The dirty page addresses.</value>
        public List<PageAddress> DirtyPages
        {
            get { return dirtyPages; }
        }
    }
}
