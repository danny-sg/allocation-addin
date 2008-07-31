﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using SqlInternals.AllocationInfo.Internals.UI;

namespace SqlInternals.AllocationInfo.Addin
{
    public class KeyImageColumn : DataGridViewImageColumn
    {
        public KeyImageColumn()
        {
            CellTemplate = new KeyImageCell();
            ValueType = typeof(int);
        }
    }

    public class KeyImageCell : DataGridViewImageCell
    {
        public KeyImageCell()
        {
            ValueType = typeof(int);
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
            if (value != DBNull.Value)
            {
                return ExtentColour.KeyImage(Color.FromArgb((int)value));
            }
            else
            {
                return null;
            }
        }
    }
}
