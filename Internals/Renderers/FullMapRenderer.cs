using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;
using SqlInternals.AllocationInfo.Internals.UI;
using System.Runtime.InteropServices;
using SqlInternals.AllocationInfo.Internals.Pages;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace SqlInternals.AllocationInfo.Internals.Renderers
{
    class FullMapRenderer
    {
        /// <summary>
        /// Renders the map layers and returns a bitmap
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <param name="mapLayers">The map layers.</param>
        /// <param name="rect">The rect.</param>
        /// <param name="fileId">The file id.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <returns></returns>
        public static Bitmap RenderMapLayers(BackgroundWorker worker, List<AllocationLayer> mapLayers, Rectangle rect, int fileId, int fileSize)
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();

            fileSize = fileSize / 8;

            Rectangle fileRectange = GetFileRectange(rect, fileSize);

            Bitmap bitmap = new Bitmap(fileRectange.Width, fileRectange.Height, PixelFormat.Format24bppRgb);

            foreach (AllocationLayer layer in mapLayers)
            {
                foreach (Allocation allocation in layer.Allocations)
                {
                    foreach (AllocationPage page in allocation.Pages)
                    {
                        AddAllocationToBitmap(bitmap, page.AllocationMap, page.StartPage, fileSize, layer.Colour);
                    }
                }
            }

            stopWatch.Stop();

            System.Diagnostics.Debug.Print("Render time: {0}", stopWatch.Elapsed.TotalSeconds);

            bitmap.MakeTransparent(Color.Black);

            Bitmap returnBitmap = new Bitmap(rect.Width, rect.Height);

            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(bitmap, rect, fileRectange, GraphicsUnit.Pixel);
            }

            bitmap.Dispose();

            return returnBitmap;
        }

        /// <summary>
        /// Gets the file rectange for the file bitmap
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <returns></returns>
        private static Rectangle GetFileRectange(Rectangle rect, int fileSize)
        {
            double widthHeightRatio = rect.Width / (rect.Height * 8D);

            int height = (int)(Math.Ceiling(Math.Sqrt((double)fileSize) / widthHeightRatio));
            int width = (int)(Math.Ceiling(Math.Sqrt((double)fileSize) * widthHeightRatio));

            // Adjust so the stride won't have any padding bytes
            width = (width + 4) - (width % 4);

            System.Diagnostics.Debug.Print("Width: {0}, Height: {1}, File Size: {2}, (Width * Height): {3}", width, height, fileSize, width * height);

            return new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Writes the allocation bytes directly to the bitmap data
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="allocation">The allocation structure.</param>
        /// <param name="startPage">The start page.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <param name="colour">The layer colour.</param>
        private static void AddAllocationToBitmap(Bitmap bitmap, bool[] allocation, PageAddress startPage, int fileSize, Color colour)
        {
            int startExtent = startPage.PageId / 8;

            int bytesPerExtent = 3; // R,G,B

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                    ImageLockMode.ReadWrite,
                                                    bitmap.PixelFormat);

            IntPtr ptr = bitmapData.Scan0;

            byte[] values = new byte[fileSize * bytesPerExtent];

            Marshal.Copy(ptr, values, 0, fileSize * bytesPerExtent);

            int extent = startExtent;

            for (int i = startExtent * bytesPerExtent;
                 i < Math.Min(values.Length, (startExtent + allocation.Length) * bytesPerExtent);
                 i += bytesPerExtent)
            {
                if (allocation[extent - startExtent])
                {
                    values[i] = colour.B;
                    values[i + 1] = colour.G;
                    values[i + 2] = colour.R;
                }

                extent++;
            }

            Marshal.Copy(values, 0, ptr, fileSize * bytesPerExtent);

            bitmap.UnlockBits(bitmapData);
        }
    }
}
