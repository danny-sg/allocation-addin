using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SqlInternals.AllocationInfo.Internals.UI;

namespace SqlInternals.AllocationInfo.Internals.Renderers
{
    internal class PageExtentRenderer : IDisposable
    {
        private Color pageBorderColour = Color.White;
        private readonly Color backgroundColour;
        private readonly Color unusedColour;
        private Pen borderPen;
        private LinearGradientBrush extentPageBrush;


        internal PageExtentRenderer(Color backgroundColour, Color unusedColour)
        {
            this.backgroundColour = backgroundColour;
            this.unusedColour = unusedColour;
        }

        internal void CreateBrushesAndPens(Size extentSize)
        {
            Rectangle gradientRect = new Rectangle(0, 0, extentSize.Width, extentSize.Height);

            extentPageBrush = new LinearGradientBrush(gradientRect,
                                                      backgroundColour,
                                                      unusedColour,
                                                      LinearGradientMode.Horizontal);

            borderPen = new Pen(pageBorderColour);
        }

        internal void DrawExtent(Graphics g, Rectangle rect)
        {
            g.FillRectangle(extentPageBrush, rect);
            g.DrawRectangle(borderPen, rect);

            for (int j = 1; j < 9; j++)
            {
                g.DrawLine(borderPen,
                           rect.X + (j * rect.Width / 8),
                           rect.Y,
                           (rect.X + j * rect.Width / 8),
                           rect.Y + rect.Height);
            }
        }

        internal void DrawPage(Graphics g, Rectangle rect, AllocationLayerType layerType)
        {
            switch (layerType)
            {
                case AllocationLayerType.Standard:

                    g.FillRectangle(extentPageBrush, rect);
                    break;

                case AllocationLayerType.TopLeftCorner:

                    GraphicsPath path = new GraphicsPath();

                    path.AddLine(rect.X, rect.Y, rect.X + rect.Width / 1.6F, rect.Y);
                    path.AddLine(rect.X + (rect.Width / 1.6F), rect.Y, rect.X, rect.Y + (rect.Height / 1.6F));
                    path.AddLine(rect.X, rect.Y + (rect.Height / 1.6F), rect.X, rect.Y);
                    path.CloseFigure();

                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    g.FillPath(extentPageBrush, path);
                    break;
            }

            g.DrawRectangle(borderPen, rect);
        }

        internal static void DrawLsnMapPage(Graphics g, Rectangle rect, decimal lsn, decimal maxLsn, decimal minLsn)
        {
            int h = 140;
            float step = 230F / ((float)(maxLsn - minLsn));

            int value = (int)(step / 1.175F * (float)lsn);

            Color pageColour = HsvColour.HsvToColor(h, (255 - value) % 255, value);

            g.FillRectangle(new SolidBrush(pageColour), rect);
            g.DrawRectangle(Pens.White, rect);
        }

        internal void DrawBackgroundExtents(PaintEventArgs e,
                                            Size extentSize,
                                            int extentsHorizontal,
                                            int extentsVertical,
                                            int extentsRemaining)
        {
            ResizeExtentBrush(extentSize);
            SetExtentBrushColour(backgroundColour, unusedColour);

            for (int i = 0; i < extentsHorizontal; i++)
            {
                Rectangle rect;

                if (i < extentsRemaining)
                {
                    rect = new Rectangle(i * extentSize.Width,
                                         0,
                                         extentSize.Width,
                                         (extentsVertical + 1) * extentSize.Height);
                }
                else
                {
                    rect = new Rectangle(i * extentSize.Width,
                                         0,
                                         extentSize.Width,
                                         extentsVertical * extentSize.Height);
                }

                e.Graphics.FillRectangle(extentPageBrush, rect);

                for (int j = 0; j <= 8; j++)
                {
                    if (i < extentsRemaining)
                    {
                        e.Graphics.DrawLine(borderPen,
                                            j * (extentSize.Width / 8) + (extentSize.Width * i),
                                            0,
                                            j * (extentSize.Width / 8) + (extentSize.Width * i),
                                            (extentsVertical + 1) * extentSize.Height);
                    }
                    else
                    {
                        e.Graphics.DrawLine(borderPen,
                                            j * (extentSize.Width / 8) + (extentSize.Width * i),
                                            0,
                                            j * (extentSize.Width / 8) + (extentSize.Width * i),
                                            extentsVertical * extentSize.Height);
                    }
                }

                for (int k = 0; k < extentsVertical + 1; k++)
                {
                    e.Graphics.DrawLine(borderPen,
                                        0,
                                        k * extentSize.Height,
                                        extentSize.Width * extentsHorizontal,
                                        k * extentSize.Height);
                }
            }
        }

        internal void DrawSelection(Graphics g, Rectangle rect)
        {
            g.FillRectangle(Brushes.LightGray, rect);
            g.DrawRectangle(borderPen, rect);

            for (int j = 1; j < 9; j++)
            {
                g.DrawLine(borderPen,
                           rect.X + (j * rect.Width / 8),
                           rect.Y,
                           (rect.X + j * rect.Width / 8),
                           rect.Y + rect.Height);
            }
        }

        internal void SetExtentBrushColour(Color foreColour, Color backColour)
        {
            if (extentPageBrush.LinearColors[0] != foreColour || extentPageBrush.LinearColors[1] != backColour)
            {
                extentPageBrush.LinearColors = new Color[2] { foreColour, backColour };
            }
        }

        internal void ResizeExtentBrush(Size extentSize)
        {
            extentPageBrush.ResetTransform();
            extentPageBrush.ScaleTransform(extentSize.Height / extentPageBrush.Rectangle.Height,
                                           extentSize.Width / extentPageBrush.Rectangle.Width,
                                           MatrixOrder.Append);

        }

        internal void ResizePageBrush(Size pageSize)
        {
            extentPageBrush.ResetTransform();
            extentPageBrush.ScaleTransform((pageSize.Width / extentPageBrush.Rectangle.Width) / 8,
                                           (pageSize.Width / extentPageBrush.Rectangle.Width) / 8,
                                           MatrixOrder.Append);
        }

        public Color PageBorderColour
        {
            get { return pageBorderColour; }
            set { pageBorderColour = value; }
        }

        public void Dispose()
        {
            extentPageBrush.Dispose();
        }

    }
}
