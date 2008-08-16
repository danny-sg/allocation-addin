using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using SqlInternals.AllocationInfo.Internals.Properties;

namespace SqlInternals.AllocationInfo.Internals.Pages
{
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
            this.headerData.Clear();
            this.Data = this.LoadDatabasePage();
            this.LoadHeader();
        }

        /// <summary>
        /// Loads the page header.
        /// </summary>
        /// <returns></returns>
        public override bool LoadHeader()
        {
            bool parsed = true;

            Header header = new Header();

            parsed = DatabaseHeaderReader.LoadHeader(this.headerData, header);

            Header = header;

            return parsed;
        }

        /// <summary>
        /// Loads the database page.
        /// </summary>
        /// <returns>byte array containing the page information</returns>
        private byte[] LoadDatabasePage()
        {
            string pageCommand = string.Format(Resources.SQL_Page,
                                               DatabaseId,
                                               PageAddress.FileId,
                                               PageAddress.PageId,
                                               2);
            string currentData;
            string currentRow;
            int offset = 0;
            byte[] data = new byte[8192];

            using (SqlConnection conn = new SqlConnection(ServerConnection.CurrentConnection().ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(pageCommand, conn);
                cmd.CommandType = CommandType.Text;

                try
                {
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader[0].ToString() == "DATA:" && reader[1].ToString().StartsWith("Memory Dump"))
                            {
                                currentRow = reader[3].ToString();
                                currentData = currentRow.Replace(" ", "").Substring(currentRow.IndexOf(":") + 1, 40);

                                for (int i = 0; i < 40; i += 2)
                                {
                                    string byteString = currentData.Substring(i, 2);

                                    if (!byteString.Contains("†") && !byteString.Contains(".") && offset < 8192)
                                    {
                                        if (byte.TryParse(byteString,
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
                                this.headerData.Add(reader[2].ToString(), reader[3].ToString());
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
