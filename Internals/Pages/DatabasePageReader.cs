namespace SqlInternals.AllocationInfo.Internals.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    using SqlInternals.AllocationInfo.Internals.Properties;

    /// <summary>
    /// Reads database pages direct from the database using DBCC PAGE
    /// </summary>
    public class DatabasePageReader : PageReader
    {
        private readonly Dictionary<string, string> headerData = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabasePageReader"/> class.
        /// </summary>
        /// <param name="pageAddress">The page address.</param>
        /// <param name="databaseId">The database id.</param>
        public DatabasePageReader(PageAddress pageAddress, int databaseId)
            : base(pageAddress, databaseId)
        {
        }

        /// <summary>
        /// Loads this page instance.
        /// </summary>
        public override void Load()
        {
            headerData.Clear();
            Data = LoadDatabasePage();
            LoadHeader();
        }

        /// <summary>
        /// Loads the page header.
        /// </summary>
        /// <returns>
        /// True if the header has been loaded
        /// </returns>
        public override bool LoadHeader()
        {
            var parsed = true;

            var header = new Header();

            parsed = DatabaseHeaderReader.LoadHeader(headerData, header);

            Header = header;

            return parsed;
        }

        /// <summary>
        /// Loads the database page.
        /// </summary>
        /// <returns>byte array containing the page information</returns>
        private byte[] LoadDatabasePage()
        {
            var pageCommand = string.Format(Resources.SQL_Page, DatabaseId, PageAddress.FileId, PageAddress.PageId, 2);
            var offset = 0;
            var data = new byte[8192];

            using (var conn = new SqlConnection(ServerConnection.CurrentConnection().ConnectionString))
            {
                var cmd = new SqlCommand(pageCommand, conn);
                cmd.CommandType = CommandType.Text;

                try
                {
                    conn.Open();

                    var reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader[0].ToString() == "DATA:" && reader[1].ToString().StartsWith("Memory Dump"))
                            {
                                var currentRow = reader[3].ToString();
                                var currentData = currentRow.Replace(" ", string.Empty)
                                                            .Substring(currentRow.IndexOf(":") + 1, 40);

                                for (var i = 0; i < 40; i += 2)
                                {
                                    var byteString = currentData.Substring(i, 2);

                                    if (!byteString.Contains("†") && !byteString.Contains(".") && offset < 8192)
                                    {
                                        if (byte.TryParse(
                                            byteString,
                                            NumberStyles.HexNumber,
                                            CultureInfo.InvariantCulture,
                                            out data[offset]))
                                        {
                                            offset++;
                                        }
                                    }
                                }
                            }
                            else if (reader[0].ToString() == "PAGE HEADER:")
                            {
                                headerData.Add(reader[2].ToString(), reader[3].ToString());
                            }
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.ToString());
                }

                cmd.Dispose();
            }

            return data;
        }
    }
}