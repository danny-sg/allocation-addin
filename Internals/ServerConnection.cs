namespace SqlInternals.AllocationInfo.Internals
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;

    using SqlInternals.AllocationInfo.Internals.Properties;

    /// <summary>
    /// Class for managing the server connection for the addin
    /// </summary>
    public class ServerConnection : INotifyPropertyChanged
    {
        private static ServerConnection currentServer;

        private Database currentDatabase;

        private ServerConnection()
        {
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                Changed += value;
            }

            remove
            {
                Changed -= value;
            }
        }

        private event PropertyChangedEventHandler Changed;

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Gets or sets the current database.
        /// </summary>
        /// <value>The current database.</value>
        public Database CurrentDatabase
        {
            get
            {
                return currentDatabase;
            }

            set
            {
                currentDatabase = value;
                OnPropertyChanged("Database");
            }
        }

        /// <summary>
        /// Gets the server databases.
        /// </summary>
        /// <value>The server databases.</value>
        public List<Database> Databases { get; } = new List<Database>();

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets the server version.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; set; }

        /// <summary>
        /// Returns the ServerConnection
        /// </summary>
        /// <returns>The Current server connection / a new connection</returns>
        /// <remarks>This uses the singleton pattern</remarks>
        public static ServerConnection CurrentConnection()
        {
            if (currentServer == null)
            {
                currentServer = new ServerConnection();
            }

            return currentServer;
        }

        /// <summary>
        /// Sets the current database
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns>The database object</returns>
        public Database SetCurrentDatabase(string databaseName)
        {
            var database = Databases.Find(delegate (Database d) { return d.Name == databaseName; });

            if (database != null)
            {
                CurrentDatabase = database;

                return CurrentDatabase;
            }
            else
            {
                throw new Exception("Database not found");
            }
        }

        /// <summary>
        /// Sets the current server.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="integratedSecurity">if set to <c>true</c> uses [integrated security].</param>
        /// <param name="userName">Username for the connection (SQL Server Authentication)</param>
        /// <param name="password">Password for the connection (SQL Server Authentication)</param>
        public void SetCurrentServer(string serverName, bool integratedSecurity, string userName, string password)
        {
            // Default to master database
            var databaseName = "master";
            Version = 0;

            var builder = new SqlConnectionStringBuilder();

            builder["Data Source"] = serverName;
            builder.ApplicationName = "Allocation Info";
            builder["Initial Catalog"] = databaseName;
            builder.AsynchronousProcessing = true;

            if (!integratedSecurity)
            {
                builder["uid"] = userName;
                builder["password"] = password;
            }
            else
            {
                builder["integrated Security"] = true;
            }

            ConnectionString = builder.ConnectionString;

            var conn = new SqlConnection(builder.ConnectionString);

            try
            {
                conn.Open();

                CheckVersion(serverName, conn);

                CheckPermissions(conn);

                GetDatabases();

                currentDatabase = Databases.Find(delegate (Database d) { return d.Name == databaseName; });

                OnPropertyChanged("Server");
                OnPropertyChanged("Database");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        protected void OnPropertyChanged(string prop)
        {
            if (Changed != null)
            {
                Changed(this, new PropertyChangedEventArgs(prop));
            }
        }

        /// <summary>
        /// Checks the user has sysadmin permissions (necessary for certain DBCC commands and DMVs)
        /// </summary>
        /// <param name="conn">The SqlConnection to the target database</param>
        private static void CheckPermissions(SqlConnection conn)
        {
            var cmd = new SqlCommand(Resources.SQL_Sysadmin_Check, conn);

            var hasSysadmin = (bool)cmd.ExecuteScalar();

            if (!hasSysadmin)
            {
                throw new System.Security.SecurityException(Resources.Exception_NotSysadmin);
            }
        }

        /// <summary>
        /// Checks the version of the SQL Server is compatible
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="conn">The SqlConnection to the target database</param>
        private void CheckVersion(string serverName, SqlConnection conn)
        {
            var cmd = new SqlCommand(Resources.SQL_Version, conn);
            cmd.CommandType = CommandType.Text;

            var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

            if (reader.Read())
            {
                Version = int.Parse(reader[0].ToString().Split(".".ToCharArray())[0]);
            }

            reader.Close();

            ServerName = serverName;

            if (Version < 9)
            {
                throw new NotSupportedException("This application currently only supports SQL Server 2005 and 2008.");
            }
        }

        /// <summary>
        /// Gets the databases.
        /// </summary>
        private void GetDatabases()
        {
            var databasesDataTable =
                DataAccess.GetDataTable(Resources.SQL_Databases, "master", "Databases", CommandType.Text);
            Databases.Clear();

            foreach (DataRow r in databasesDataTable.Rows)
            {
                Databases.Add(
                    new Database(
                        (int)r["database_id"],
                        (string)r["name"],
                        (byte)r["state"],
                        (byte)r["compatibility_level"]));
            }
        }
    }
}