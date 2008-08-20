using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    internal class FittedMap
    {
        public static Bitmap DrawFitMap(BackgroundWorker worker, List<AllocationLayer> mapLayers, Rectangle rect, int fileId, int fileSize)
        {
            Bitmap map = new Bitmap(rect.Width, rect.Height);

            Graphics g = Graphics.FromImage(map);

            float extentHeight;
            float extentWidth;

            double adjustedWidth = rect.Width / 8.0D;
            double databaseSize = fileSize / 8.0D;
            double initalExtentSize = Math.Sqrt((rect.Height * adjustedWidth) / databaseSize);

            int extentsPerLine = (int)Math.Floor(adjustedWidth / initalExtentSize) + 1;

            extentWidth = (float)(adjustedWidth / (float)extentsPerLine);
            extentHeight = (float)(rect.Height / Math.Ceiling(databaseSize / extentsPerLine));

            extentWidth *= 8;

            foreach (AllocationLayer layer in mapLayers)
            {
                LinearGradientBrush brush = new LinearGradientBrush(rect, layer.Colour, ExtentColour.LightBackgroundColour(layer.Colour), 0.45F);

                if (layer.Visible && !layer.SingleSlotsOnly)
                {
                    foreach (Allocation chain in layer.Allocations)
                    {
                        int colPos = 0;
                        float rowPos = 0.0F;

                        for (int i = 0; (i < fileSize / 8); i++)
                        {
                            if (worker.CancellationPending)
                            {
                                return map;
                            }

                            if (colPos >= extentsPerLine)
                            {
                                colPos = 0;
                                rowPos += extentHeight;
                            }

                            if (Allocation.CheckAllocationStatus(i, fileId, layer.Invert, chain))
                            {
                                g.FillRectangle(brush, colPos * extentWidth, rowPos, extentWidth, extentHeight);
                            }

                            colPos++;
                        }
                    }
                }

                worker.ReportProgress(0, map);
            }

            return map;
        }
    }
}
