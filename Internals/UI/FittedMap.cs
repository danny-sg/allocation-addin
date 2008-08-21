using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    internal class FittedMap
    {
        public static Bitmap DrawFitMap(BackgroundWorker worker, List<AllocationLayer> mapLayers, Rectangle rect, int fileId, int fileSize)
        {
            Bitmap map = new Bitmap(rect.Width, rect.Height);

            Graphics g = Graphics.FromImage(map);

            double adjustedWidth = rect.Width / 8.0D;
            double databaseSize = fileSize / 8.0D;
            double initalExtentSize = Math.Sqrt((rect.Height * adjustedWidth) / databaseSize);

            int extentsPerLine = (int)Math.Floor(adjustedWidth / initalExtentSize) + 1;

            float extentWidth = (float)(adjustedWidth / (float)extentsPerLine);
            float extentHeight = (float)(rect.Height / Math.Ceiling(databaseSize / extentsPerLine));

            extentWidth *= 8;

            int colPos = 0;
            float rowPos = 0.0F;

            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.White, Color.White, 0.45F))
            {
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

                    foreach (AllocationLayer layer in mapLayers)
                    {
                        brush.LinearColors = new Color[] { layer.Colour, ExtentColour.LightBackgroundColour(layer.Colour) };

                        if (layer.Visible && !layer.SingleSlotsOnly)
                        {
                            foreach (Allocation chain in layer.Allocations)
                            {
                                if (Allocation.CheckAllocationStatus(i, fileId, layer.Invert, chain))
                                {
                                    g.FillRectangle(brush, colPos * extentWidth, rowPos, extentWidth, extentHeight);
                                }
                            }
                        }
                    }

                    colPos++;

                    // At some point add this back in
                    //worker.ReportProgress(0, map);
                }
            }

            return map;
        }
    }
}
