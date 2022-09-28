using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyDatabase
{
    public class Table 
    {
        public string TableName { get; set; }

        public List<Row> Rows { get; set; } = new List<Row>();

        public List<Column> Columns { get; set; } = new List<Column>();

        public Row AddRow()
        {
            var row = new Row { Table = this, Cells = new object[Columns.Count]};
            Rows.Add(row);
            return row;
        }

        public void DeleteRow (Row row)
        {
            Rows.Remove(row);
        }

        public void IncrementRowsCell()
        {
            object[] additionalCell = new object[1];
            for (var i=0; i<Rows.Count; i++)
            {
             Rows[i].Cells = Rows[i].Cells.Concat(additionalCell).ToArray();
            }
        }

        public Column AddColumn(string name, ColumnType type)
        {
            if(Rows.Count > 0)
            {
                IncrementRowsCell();
            } 
            var col = new Column { ColName = name, ColType = type };
            Columns.Add(col);
            return col;
                
        }

        public Column GetColumn(string name)
        {
            return Columns.First(col => col.ColName == name);
        }

        public void DeleteColumn(string columnName)
        {
            var col = GetColumn(columnName);
            Columns.Remove(col);
        }
    }
}
