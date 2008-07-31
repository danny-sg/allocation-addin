using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using SqlInternals.AllocationInfo.Internals.Pages;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    public class AllocationUnitsLayer
    {
        private static readonly int COLOUR_COUNT = 360;
        private static readonly int systemSaturation = 75;
        private static readonly int systemValue = 150;
        private static readonly int userSaturation = 150;
        private static readonly int userValue = 220;

        public static List<AllocationLayer> GenerateLayers(Database database, BackgroundWorker worker)
        {
            List<AllocationLayer> layers = new List<AllocationLayer>();
            AllocationLayer layer = null;
            int colourIndex = 0;
            int count = 0;
            int systemColourIndex = 0;
            string previousObjectName = string.Empty;

            DataTable allocationUnits = database.AllocationUnits();

            int userObjectCount = (int)allocationUnits.Compute("COUNT(table_name)",
                                                                "type=1 AND system=0 AND index_id < 2");

            int systemObjectCount = (int)allocationUnits.Compute("COUNT(table_name)",
                                                                  "type=1 AND system=1 AND index_id < 2");

            foreach (DataRow row in allocationUnits.Rows)
            {
                if (worker.CancellationPending)
                {
                    return null;
                }

                count++;

                string currentObjectName;

                if ((bool)row["system"])
                {
                    currentObjectName = "(System object)";
                }
                else
                {
                    currentObjectName = row["schema_name"] + "." + row["table_name"];
                }

                if (currentObjectName != previousObjectName)
                {
                    layer = new AllocationLayer();
                    layer.Name = currentObjectName;
                    layer.UseDefaultSinglePageColour = false;

                    if ((bool)row["system"])
                    {
                        if (layer.Name != previousObjectName)
                        {
                            systemColourIndex += (int)Math.Floor(COLOUR_COUNT / (double)systemObjectCount);

                            if (colourIndex >= COLOUR_COUNT)
                            {
                                colourIndex = 1;
                            }
                        }

                        if (true) //Settings.Default.Allocation_Map_Group_System)
                        {
                            layer.Colour = Color.FromArgb(255, 190, 190, 205);
                        }
                        else
                        {
                            layer.Colour = HsvColour.HsvToColor(systemColourIndex,
                                                                systemSaturation,
                                                                systemValue);
                        }
                    }
                    else
                    {
                        if (layer.Name != previousObjectName)
                        {
                            if (userObjectCount > COLOUR_COUNT)
                            {
                                colourIndex += 1;
                            }
                            else
                            {
                                colourIndex += (int)Math.Floor(COLOUR_COUNT / (double)userObjectCount);
                            }
                        }

                        layer.Colour = HsvColour.HsvToColor(colourIndex,
                                                            userSaturation,
                                                            userValue);
                    }

                    layers.Add(layer);
                }

                PageAddress address = new PageAddress((byte[])row["first_iam_page"]);

                if (address.PageId > 0)
                {
                    if (layer != null)
                    {
                        layer.Allocations.Add(new IamAllocation(database, address));
                    }
                }

                if (layer != null) previousObjectName = layer.Name;

                worker.ReportProgress((int)(count / (float)allocationUnits.Rows.Count * 100), layer.Name);
            }

            return layers;
        }
    }
}
