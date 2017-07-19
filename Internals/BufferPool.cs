namespace SqlInternals.AllocationInfo.Internals
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using SqlInternals.AllocationInfo.Internals.Pages;
    using SqlInternals.AllocationInfo.Internals.Properties;

    /// <summary>
    /// Set of pages in the server's buffer bool
    /// </summary>
    public class BufferPool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BufferPool"/> class.
        /// </summary>
        public BufferPool()
        {
            CleanPages = new List<PageAddress>();
            DirtyPages = new List<PageAddress>();

            Refresh();
        }

        /// <summary>
        /// Gets the clean page addresses.
        /// </summary>
        /// <value>The clean page addresses.</value>
        public List<PageAddress> CleanPages { get; }

        /// <summary>
        /// Gets the dirty page addresses.
        /// </summary>
        /// <value>The dirty page addresses.</value>
        public List<PageAddress> DirtyPages { get; }

        /// <summary>
        /// Requeries buffer pool information.
        /// </summary>
        public void Refresh()
        {
            CleanPages.Clear();
            DirtyPages.Clear();

            if (ServerConnection.CurrentConnection().CurrentDatabase == null)
            {
                return;
            }

            using (var conn = new SqlConnection(ServerConnection.CurrentConnection().ConnectionString))
            {
                var cmd = new SqlCommand(Resources.SQL_Buffer_Pool, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("database", ServerConnection.CurrentConnection().CurrentDatabase.Name);

                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.GetBoolean(2))
                    {
                        DirtyPages.Add(new PageAddress(reader.GetInt32(0), reader.GetInt32(1)));
                    }

                    CleanPages.Add(new PageAddress(reader.GetInt32(0), reader.GetInt32(1)));
                }

                conn.Close();
            }
        }
    }
}