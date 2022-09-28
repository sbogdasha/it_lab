using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MyDatabase
{
    public class Row
    {
        public object[] Cells { get; set; }

        [JsonIgnore]

        public Table Table { get; set; }

        public void SetValue(string columnName, object value)
        {
            var col = Table.GetColumn(columnName);
            var index = Table.Columns.IndexOf(col);

            switch (col.ColType)
            {
                case ColumnType.Int:
                    Cells[index] = Convert.ToInt64(value);
                    break;
                case ColumnType.Real:
                    Cells[index] = Convert.ToDouble(value);
                    break;
                case ColumnType.Char:
                    Cells[index] = Convert.ToChar(value);
                    break;
                case ColumnType.String:
                    Cells[index] = Convert.ToString(value);
                    break;
                case ColumnType.Time:
                    Cells[index] = Convert.ToDateTime(value);
                    break;
                case ColumnType.TimeInvl:
                    var TimeInvl = Convert.ToDateTime(value);
                    if ((TimeInvl < col.TimeMin) || (TimeInvl > col.TimeMax))
                    {
                        throw new Exception("Input is not in range of interval");                            
                    }  
                    else
                    {
                        Cells[index] = TimeInvl;
                    }  
                    break;
                default:
                    throw new Exception("Bad type of input");
            }

        }

        public object GetValue (string columnName)
        {
            var col = Table.GetColumn(columnName);
            var index = Table.Columns.IndexOf(col);

            return Cells[index];
        }
    }
}
