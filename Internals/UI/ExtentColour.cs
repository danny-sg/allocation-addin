using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace SqlInternals.AllocationInfo.Internals.UI
{
    public class ExtentColour
    {
        public static Color BackgroundColour(Color color)
        {
            int red;
            int green;
            int blue;

            red = color.R + 32 > 255 ? 255 : color.R + 32;
            green = color.G + 32 > 255 ? 255 : color.G + 32;
            blue = color.B + 32 > 255 ? 255 : color.B + 32;

            return Color.FromArgb(color.A, red, green, blue);
        }

        public static Color LightBackgroundColour(Color color)
        {
            int red;
            int green;
            int blue;

            red = color.R + 48 > 255 ? 255 : color.R + 48;
            green = color.G + 48 > 255 ? 255 : color.G + 48;
            blue = color.B + 48 > 255 ? 255 : color.B + 48;

            return Color.FromArgb(color.A, red, green, blue);
        }

        public static Bitmap KeyImage(Color color)
        {
            Bitmap key = new Bitmap(16, 16);
            Rectangle keyRectange = new Rectangle(0, 0, key.Width - 1, key.Height - 1);
            Graphics g = Graphics.FromImage(key);

            LinearGradientBrush brush = new LinearGradientBrush(keyRectange,
                                                                color,
                                                                BackgroundColour(color),
                                                                LinearGradientMode.Horizontal);

            g.FillRectangle(brush, keyRectange);
            g.DrawRectangle(SystemPens.ControlDark, keyRectange);

            return key;
        }
    }
}
