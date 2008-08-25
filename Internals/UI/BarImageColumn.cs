using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SqlInternals.AllocationInfo.Internals.UI
{
    public class BarImageColumn : DataGridViewImageColumn
    {
        private List<ColourRange> colourRanges;

        public BarImageColumn()
        {
            CellTemplate = new CustomImageCell();
            this.ColourRanges = new List<ColourRange>();
            ValueType = typeof(decimal);
        }

        internal List<ColourRange> ColourRanges
        {
            get { return colourRanges; }
            set { colourRanges = value; }
        }
    }

    public class CustomImageCell : DataGridViewImageCell
    {
        public CustomImageCell()
        {
            ValueType = typeof(decimal);
        }

        public override object DefaultNewRowValue
        {
            get { return 0; }
        }

        protected override object GetFormattedValue(object value,
                                                    int rowIndex,
                                                    ref DataGridViewCellStyle cellStyle,
                                                    TypeConverter valueTypeConverter,
                                                    TypeConverter formattedValueTypeConverter,
                                                    DataGridViewDataErrorContexts context)
        {
            return new Bitmap(1, 1);
        }

        protected override void Paint(Graphics graphics,
                                      Rectangle clipBounds,
                                      Rectangle cellBounds,
                                      int rowIndex,
                                      DataGridViewElementStates elementState,
                                      object value,
                                      object formattedValue,
                                      string errorText,
                                      DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, 
                       clipBounds, 
                       cellBounds, 
                       rowIndex, 
                       elementState, 
                       value, 
                       formattedValue, 
                       errorText, 
                       cellStyle, 
                       advancedBorderStyle, 
                       paintParts);

            Font font = new Font(this.DataGridView.DefaultCellStyle.Font, FontStyle.Regular);

            Color gradientColour;

            ColourRange r = (this.OwningColumn as BarImageColumn).ColourRanges.Find(delegate(ColourRange range) 
                                { 
                                        return range.From <= Convert.ToInt32(value ?? 0) 
                                               && range.To >= Convert.ToInt32(value ?? 0); 
                                });

            if (r != null)
            {
                gradientColour = r.Colour;
            }
            else
            {
                gradientColour = Color.DarkGray;
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(cellBounds, 
                                                                       gradientColour, 
                                                                       ExtentColour.LightBackgroundColour(gradientColour), 
                                                                       90F, 
                                                                       false))
            {
                graphics.FillRectangle(brush, 
                                       cellBounds.X + 2, 
                                       cellBounds.Y + 3, 
                                       (int)((cellBounds.Width - 6) * (Convert.ToDecimal(value) / 100)), 
                                       cellBounds.Height - 8);
            }

            graphics.DrawRectangle(Pens.Gray, cellBounds.X + 2, cellBounds.Y + 3, cellBounds.Width - 6, cellBounds.Height - 8);

            string cellText = string.Format("{0:0}%", Convert.ToDecimal(value));

            // Centre the text in the middle of the bar
            Point textPoint = new Point(cellBounds.X + cellBounds.Width / 2 - (TextRenderer.MeasureText(cellText, font).Width / 2), 
                                        cellBounds.Y + 4);

            TextRenderer.DrawText(graphics, cellText, font, textPoint, Color.Black);
        }
    }
}
