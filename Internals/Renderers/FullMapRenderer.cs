using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;
using SqlInternals.AllocationInfo.Internals.UI;
using System.Runtime.InteropServices;
using SqlInternals.AllocationInfo.Internals.Pages;

namespace SqlInternals.AllocationInfo.Internals.Renderers
{
    class FullMapRenderer
    {
        public static Bitmap RenderMapLayers(BackgroundWorker worker, List<AllocationLayer> mapLayers, Rectangle rect, int fileId, int fileSize)
        {
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

            bitmap.MakeTransparent(Color.Black);

            Bitmap returnBitmap = new Bitmap(rect.Width, rect.Height);

            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(bitmap, rect, fileRectange, GraphicsUnit.Pixel);
            }

            bitmap.Dispose();

            return returnBitmap;
        }

        private static Rectangle GetFileRectange(Rectangle rect, int fileSize)
        {
            double widthHeightRatio = rect.Width / (rect.Height * 8D);

            int height = (int)(Math.Ceiling(Math.Sqrt((double)fileSize) / widthHeightRatio));
            int width = (int)(Math.Ceiling(Math.Sqrt((double)fileSize) * widthHeightRatio));

            // Adjust so the stride won't have any padding bytes
            width = (width + 4) - (width % 4);

            return new Rectangle(0, 0, width, height);
        }

        private static void AddAllocationToBitmap(Bitmap bitmap, bool[] allocation, PageAddress startPage, int fileSize, Color colour)
        {
            int startExtent = startPage.PageId / 8;

            int length = Math.Min(fileSize, allocation.Length + startExtent);

            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            System.Diagnostics.Debug.Print("Width: {0}, Stride: {1}, Padding: {2}", bitmap.Width, bitmapData.Stride,bitmapData.Stride - ((bitmap.Width * 24) + 7) / 8);

            IntPtr ptr = bitmapData.Scan0;

            byte[] values = new byte[length * 3];

            Marshal.Copy(ptr, values, 0, length * 3);

            int extent = startExtent;

            for (int i = startExtent; i < values.Length; i += 3)
            {
                if (allocation[extent])
                {
                    values[i] = colour.B;
                    values[i + 1] = colour.G;
                    values[i + 2] = colour.R;
                }
                else
                {
                    values[i] = values[i];
                    values[i + 1] = values[i + 1];
                    values[i + 2] = values[i + 2];
                }

                extent++;
            }

            Marshal.Copy(values, 0, ptr, length * 3);

            bitmap.UnlockBits(bitmapData);
        }
    }
}
