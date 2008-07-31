using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace SqlInternals.AllocationInfo.Internals
{
    /// <summary>
    /// Class for standard data access
    /// </summary>
    public static class DataAccess
    {
        /// <summary>
        /// Gets a data table for a given query
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="database">The database.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string command, string database, string tableName, CommandType commandType)
        {
            DataTable returnDataTable = new DataTable();
            returnDataTable.Locale = CultureInfo.InvariantCulture;

            using (SqlConnection conn = new SqlConnection(ConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(command, conn);
                cmd.CommandType = commandType;

                SqlDataAdapter da = new SqlDataAdapter(cmd);

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
        /// Gets the data table for a given query with parameters
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static DataTable GetDataTable(string command,
                                             string databaseName,
                                             string tableName,
                                             CommandType commandType,
                                             SqlParameter[] parameters)
        {
            DataTable returnDataTable = new DataTable();
            returnDataTable.Locale = CultureInfo.InvariantCulture;

            SqlConnection conn = new SqlConnection(ConnectionString());

            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = commandType;

            foreach (SqlParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);

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
        /// <returns></returns>
        public static object GetScalar(string database,
                                       string command,
                                       CommandType commandType,
                                       SqlParameter[] parameters)
        {
            object returnObject;

            SqlConnection conn = new SqlConnection(ConnectionString());

            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = commandType;

            foreach (SqlParameter parameter in parameters)
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
        /// Executes a statement that does not return a resultset
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string storedProcedureName, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(ConnectionString());

            SqlParameter returnParam = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            SqlCommand cmd = new SqlCommand(storedProcedureName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
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
        /// The current connectionstring
        /// </summary>
        /// <returns></returns>
        private static string ConnectionString()
        {
            string connectionString = ServerConnection.CurrentConnection().ConnectionString;

            return connectionString;
        }
    }
}
