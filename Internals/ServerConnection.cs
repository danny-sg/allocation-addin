﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using SqlInternals.AllocationInfo.Internals.Properties;

namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// Class for managing the server connection for the addin
    /// </summary>
    public class ServerConnection : INotifyPropertyChanged
    {
        private static ServerConnection currentServer;
        private readonly List<Database> databases = new List<Database>();
        private string connectionString;
        private Database currentDatabase;
        private string name;
        private int version;
        private event PropertyChangedEventHandler _changed;

        private ServerConnection()
        {
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _changed += new PropertyChangedEventHandler(value); }
            remove { _changed -= new PropertyChangedEventHandler(value); }
        }


        /// <summary>
        /// Returns the ServerConnection
        /// </summary>
        /// <returns>The Current server connection / a new connection</returns>
        /// <remarks>This uses the singleton pattern</remarks>
        public static ServerConnection CurrentConnection()
        {
            if (null == currentServer)
            {
                currentServer = new ServerConnection();
            }

            return currentServer;
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
            //Default to master database
            string databaseName = "master";
            version = 0;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder["Data Source"] = serverName;
            builder.ApplicationName = "Allocation Info";
            builder["Initial Catalog"] = databaseName;

            if (!integratedSecurity)
            {
                builder["uid"] = userName;
                builder["password"] = password;
            }
            else
            {
                builder["integrated Security"] = true;
            }

            connectionString = builder.ConnectionString;

            SqlConnection conn = new SqlConnection(builder.ConnectionString);

            try
            {
                conn.Open();

                CheckVersion(serverName, conn);

                CheckPermissions(conn);

                GetDatabases();

                currentDatabase = databases.Find(delegate(Database d) { return d.Name == databaseName; });

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

        /// <summary>
        /// Checks the version of the SQL Server is compatible
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="conn">The SqlConnection to the target database</param>
        private void CheckVersion(string serverName, SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand(Resources.SQL_Version, conn);
            cmd.CommandType = CommandType.Text;

            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

            if (reader.Read())
            {
                version = int.Parse(reader[0].ToString().Split(".".ToCharArray())[0]);
            }

            reader.Close();

            name = serverName;

            if (version < 9)
            {
                throw new NotSupportedException("This application currently only supports SQL Server 2005 and 2008.");
            }
        }

        /// <summary>
        /// Gets the databases.
        /// </summary>
        private void GetDatabases()
        {
            DataTable databasesDataTable = DataAccess.GetDataTable(Resources.SQL_Databases,
                                                                   "master",
                                                                   "Databases",
                                                                   CommandType.Text);
            databases.Clear();

            if (databasesDataTable.Rows.Count == 0)
            {

            }

            foreach (DataRow r in databasesDataTable.Rows)
            {
                databases.Add(new Database((int)r["database_id"],
                                           (string)r["name"],
                                           (byte)r["state"],
                                           (byte)r["compatibility_level"]));
            }
        }

        /// <summary>
        /// Checks the user has sysadmin permissions (necessary for certain DBCC commands and DMVs)
        /// </summary>
        /// <param name="conn">The SqlConnection to the target database</param>
        private static void CheckPermissions(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand(Resources.SQL_Sysadmin_Check, conn);

            bool hasSysadmin = (bool)cmd.ExecuteScalar();

            if (!hasSysadmin)
            {
                throw new System.Security.SecurityException(Resources.Exception_NotSysadmin);
            }
        }

        /// <summary>
        /// Sets the current database
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns>The database object</returns>
        public Database SetCurrentDatabase(string databaseName)
        {
            //PageHistory.GetPageHistory().Clear();

            Database database = databases.Find(delegate(Database d) { return d.Name == databaseName; });

            if (null != database)
            {
                CurrentDatabase = database;
                return currentDatabase;
            }
            else
            {
                throw new Exception("Database not found");
            }
        }

        protected void OnPropertyChanged(string prop)
        {
            if (null != _changed)
            {
                _changed(this, new PropertyChangedEventArgs(prop));
            }
        }

        #region Properties

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get { return connectionString; }
        }

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the current database.
        /// </summary>
        /// <value>The current database.</value>
        public Database CurrentDatabase
        {
            get { return currentDatabase; }
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
        public List<Database> Databases
        {
            get { return databases; }
        }

        /// <summary>
        /// Gets or sets the server version.
        /// </summary>
        /// <value>The version.</value>
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        #endregion
    }
}
