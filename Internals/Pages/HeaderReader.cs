using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SqlInternals.AllocationInfo.Internals.Properties;

namespace SqlInternals.AllocationInfo.Internals.Pages
{
    public class DatabaseHeaderReader
    {
        public static Header LoadHeader(PageAddress pageAddress)
        {
            Header header = new Header();

            LoadHeader(LoadPageHeaderOnly(pageAddress), header);

            return header;
        }

        public static bool LoadHeader(IDictionary<string, string> headerData, Header header)
        {
            bool parsed = true;

            int slotCount;
            int freeCount;
            int freeData;
            int pageType;
            int level;
            int minLen;
            int indexId;
            long allocationUnitId;
            long objectId;
            long partitionId;
            int reservedCount;
            int xactReservedCount;
            long tornBits;

            parsed &= int.TryParse(headerData["m_slotCnt"], out slotCount);
            parsed &= int.TryParse(headerData["m_freeCnt"], out freeCount);
            parsed &= int.TryParse(headerData["m_freeData"], out freeData);
            parsed &= int.TryParse(headerData["m_type"], out pageType);
            parsed &= int.TryParse(headerData["m_level"], out level);
            parsed &= int.TryParse(headerData["pminlen"], out minLen);
            parsed &= int.TryParse(headerData["Metadata: IndexId"], out indexId);
            parsed &= long.TryParse(headerData["Metadata: AllocUnitId"], out allocationUnitId);
            parsed &= long.TryParse(headerData["Metadata: ObjectId"], out objectId);
            parsed &= long.TryParse(headerData["Metadata: PartitionId"], out partitionId);
            parsed &= int.TryParse(headerData["m_reservedCnt"], out reservedCount);
            parsed &= int.TryParse(headerData["m_xactReserved"], out xactReservedCount);
            parsed &= long.TryParse(headerData["m_tornBits"], out tornBits);

            header.PageAddress = new PageAddress(headerData["m_pageId"]);
            header.PageType = (PageType)pageType;
            header.Lsn = new LogSequenceNumber(headerData["m_lsn"]);
            header.FlagBits = headerData["m_flagBits"];
            header.PreviousPage = new PageAddress(headerData["m_prevPage"]);
            header.NextPage = new PageAddress(headerData["m_nextPage"]);

            header.SlotCount = slotCount;
            header.FreeCount = freeCount;
            header.FreeData = freeData;
            header.Level = level;
            header.MinLen = minLen;
            header.IndexId = indexId;
            header.AllocationUnitId = allocationUnitId;
            header.ObjectId = objectId;
            header.PartitionId = partitionId;
            header.ReservedCount = reservedCount;
            header.XactReservedCount = xactReservedCount;
            header.TornBits = tornBits;

            return parsed;
        }

        private static Dictionary<string, string> LoadPageHeaderOnly(PageAddress pageAddress)
        {
            Dictionary<string, string> headerData = new Dictionary<string, string>();

            string pageCommand = string.Format(
                                               Resources.SQL_Page,
                                               ServerConnection.CurrentConnection().CurrentDatabase.DatabaseId,
                                               pageAddress.FileId,
                                               pageAddress.PageId,
                                               0);

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
                            if (reader[0].ToString() == "PAGE HEADER:")
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

            return headerData;
        }
    }
}
