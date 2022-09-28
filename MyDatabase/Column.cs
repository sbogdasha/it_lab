using System;
using System.Collections.Generic;
using System.Text;

namespace MyDatabase
{
    public class Column
    {
        public string ColName { get; set; }

        public ColumnType ColType { get; set; }

        public DateTime TimeMin { get; set; }

        public DateTime TimeMax { get; set; }
    }
}
