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
        public BarImageColumn()
        {
            CellTemplate = new CustomImageCell();
            ValueType = typeof(decimal);
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
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            LinearGradientBrush brush = new LinearGradientBrush(cellBounds, Color.Gainsboro, Color.DarkGray, 90F, false);

            graphics.FillRectangle(brush, cellBounds.X + 2, cellBounds.Y + 3, (int)((cellBounds.Width - 6) * (Convert.ToDecimal(value) / 100)), cellBounds.Height - 8);

            graphics.DrawRectangle(Pens.Gray, cellBounds.X + 2, cellBounds.Y + 3, cellBounds.Width - 6, cellBounds.Height - 8);
        }
    }
}
