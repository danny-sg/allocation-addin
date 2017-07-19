namespace SqlInternals.AllocationInfo.Internals
{
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    /// <summary>
    /// Class for standard data access
    /// </summary>
    public static class DataAccess
    {
        /// <summary>
        /// Executes a statement that does not return a resultset
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        public static int ExecuteNonQuery(string storedProcedureName, SqlParameter[] parameters)
        {
            var conn = new SqlConnection(ConnectionString());

            var returnParam = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            var cmd = new SqlCommand(storedProcedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }

            cmd.Parameters.Add(returnParam);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            cmd.Dispose();

            return (int)(returnParam.Value ?? -1);
        }

        /// <summary>
        /// Gets a data table for a given query
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="database">The database.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="commandType">Type of the command.</param>
        public static DataTable GetDataTable(string command, string database, string tableName, CommandType commandType)
        {
            var returnDataTable = new DataTable();
            returnDataTable.Locale = CultureInfo.InvariantCulture;

            using (var conn = new SqlConnection(ConnectionString()))
            {
                var cmd = new SqlCommand(command, conn);
                cmd.CommandTimeout = 600;
                cmd.CommandType = commandType;

                var da = new SqlDataAdapter(cmd);

                try
                {
                    conn.Open();

                    if (conn.Database != database)
                    {
                        conn.ChangeDatabase(database);
                    }

                    da.Fill(returnDataTable);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }

            returnDataTable.TableName = tableName;

            return returnDataTable;
        }

        /// <summary>
        /// Gets the data table asyncronously with a background worker (supports cancellation)
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="database">The database.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="worker">The worker.</param>
        public static DataTable GetDataTable(
            string command,
            string database,
            string tableName,
            CommandType commandType,
            BackgroundWorker worker)
        {
            using (var conn = new SqlConnection(ConnectionString()))
            {
                var cmd = new SqlCommand(command, conn);
                cmd.CommandTimeout = 600;

                conn.Open();

                if (conn.Database != database)
                {
                    conn.ChangeDatabase(database);
                }

                var result = cmd.BeginExecuteReader();

                while (!result.IsCompleted)
                {
                    if (worker.CancellationPending)
                    {
                        return null;
                    }

                    System.Threading.Thread.Sleep(100);
                }

                using (var reader = cmd.EndExecuteReader(result))
                {
                    return CreateDataTableFromReader(reader);
                }
            }
        }

        /// <summary>
        /// Gets the data table for a given query with parameters
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        public static DataTable GetDataTable(
            string command,
            string databaseName,
            string tableName,
            CommandType commandType,
            SqlParameter[] parameters)
        {
            var returnDataTable = new DataTable();
            returnDataTable.Locale = CultureInfo.InvariantCulture;

            var conn = new SqlConnection(ConnectionString());

            var cmd = new SqlCommand(command, conn);
            cmd.CommandType = commandType;

            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }

            var da = new SqlDataAdapter(cmd);

            try
            {
                conn.Open();
                conn.ChangeDatabase(databaseName);
                da.Fill(returnDataTable);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            returnDataTable.TableName = tableName;

            return returnDataTable;
        }

        /// <summary>
        /// Gets a scalar result for a given query
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="command">The command.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        public static object GetScalar(
            string database,
            string command,
            CommandType commandType,
            SqlParameter[] parameters)
        {
            object returnObject;

            var conn = new SqlConnection(ConnectionString());

            var cmd = new SqlCommand(command, conn);
            cmd.CommandType = commandType;

            foreach (var parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }

            try
            {
                conn.Open();

                if (conn.Database != database)
                {
                    conn.ChangeDatabase(database);
                }

                returnObject = cmd.ExecuteScalar();
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            cmd.Dispose();

            return returnObject;
        }

        /// <summary>
        /// The current connectionstring
        /// </summary>
        private static string ConnectionString()
        {
            var connectionString = ServerConnection.CurrentConnection().ConnectionString;

            return connectionString;
        }

        private static DataTable CreateDataTableFromReader(SqlDataReader reader)
        {
            var returnDataTable = new DataTable();

            returnDataTable.Load(reader);

            return returnDataTable;
        }
    }
}