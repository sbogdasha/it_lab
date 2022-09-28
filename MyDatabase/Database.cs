using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyDatabase
{
    public class Database
    {

        public string DBName { get; set; }

        public string Address { get; set; }

        public List<Table> Tables { get; set; } = new List<Table>();

        public void SaveChanges()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Address));
            File.WriteAllText(Address, JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }));
        }

        public static Database OpenDB(string address)
        {
            var s = File.ReadAllText(address);
            var db = JsonConvert.DeserializeObject<Database>(s);

            foreach (var table in db.Tables)
                foreach (var rows in table.Rows)
                    rows.Table = table;

            return db;
        }

        public static Database CreateDB(string address, string name)
        {
            if (File.Exists(address))
                throw new Exception("File already exists");
            var db = new Database { Address = Path.GetFullPath(address), DBName = name};
            db.SaveChanges();
            return db;
        }

        public static void DeleteDB(string address)
        {
            File.Delete(address);
        }

        public Table CreateTable(Table table)
        {
            if (Tables.Any(t => t.TableName == table.TableName))
                throw new Exception("Table already exists");

            Tables.Add(table);
            return table;
        }

        public Table GetTable (string tableName)
        {
            return Tables.First(table => table.TableName == tableName);
        }

        public void DeleteTable(string name)
        {
            var table = GetTable(name);
            Tables.Remove(table);
        }

        public bool TablesCompatibility(Table firstTable, Table secondTable)
        {
            bool compatibility = true; 
            var firstTableColumnsCount = firstTable.Columns.Count;
            var secondTableColumnsCount = secondTable.Columns.Count;

            if (firstTableColumnsCount != secondTableColumnsCount)
                compatibility = false;
            else 
                for (var i = 0; i < firstTableColumnsCount; i++)
                {
                    if (firstTable.Columns[i].ColType != secondTable.Columns[i].ColType)
                    {
                        compatibility = false;
                        break;
                    }
                    else
                    {
                        continue; 
                    }
                }

            return compatibility;
        }

        public void TableExcept(Table firstTable, Table secondTable)
        {
            var firstTableRowsCount = firstTable.Rows.Count;
            var secondTableRowsCount = secondTable.Rows.Count;

            if (!TablesCompatibility(firstTable, secondTable))
            {
                throw new Exception("Error! Tables are not compatible");
            }
            else
            {
                for (var i = 0; i < firstTableRowsCount; i++)
                    for (var j = 0; j < secondTableRowsCount; j++)
                    {
                        if (firstTable.Rows[i] == secondTable.Rows[j])
                        {
                            firstTable.DeleteRow(firstTable.Rows[i]);
                        } 
                    }
            }
        }
    }
}

