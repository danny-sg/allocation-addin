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
            
            decimal widthHeightRatio = rect.Width / rect.Height;

            int height = (int)Math.Ceiling(Math.Sqrt((double)fileSize));
            int width = height;

            Bitmap finalImage = new Bitmap(width, height);
            Bitmap modifyImage = new Bitmap(finalImage);

            Graphics finalGraphics = Graphics.FromImage(finalImage);
            Graphics modifyGraphics = Graphics.FromImage(modifyImage);

            Bitmap returnBitmap = new Bitmap(width, height, PixelFormat.Format1bppIndexed);

            foreach (AllocationLayer layer in mapLayers)
            {
                RenderLayer(layer, returnBitmap, fileSize);

                modifyGraphics.DrawImageUnscaled(returnBitmap, 0, 0);

                modifyImage.MakeTransparent(Color.Black);

                finalGraphics.DrawImageUnscaled(modifyImage, 0, 0);
            }


            return finalImage;
        }

        public static void RenderLayer(AllocationLayer layer, Bitmap bitmap, int fileSize)
        {
            foreach (Allocation allocation in layer.Allocations)
            {

                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                bitmapData.Stride = bitmap.Height;

                IntPtr ptr = bitmapData.Scan0;

                int bytes = bitmapData.Stride * bitmap.Height;

                foreach (Page page in allocation.Pages)
                {
                    if (page.GetType() == typeof(IamAllocation))
                    {
                        Marshal.Copy(page.PageData, 0, ptr, fileSize / 8);
                    }
                    else
                    {
                        Marshal.Copy(page.PageData, 0, ptr, fileSize / 8);
                    }
                }

                bitmap.UnlockBits(bitmapData);
            }
        }

    }
}
