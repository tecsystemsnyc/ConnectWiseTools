using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary
{
    public class TableHelper
    {
        public static List<TableBase> GetTables(List<TECObject> items)
        {
            List<TableBase> tables = new List<TableBase>();
            foreach(TableBase table in AllTables.Tables)
            {
                TableInfo info = new TableInfo(table);
                if(info.Types.Count == items.Count)
                {
                    for (int x = 0; x < items.Count; x++)
                    {
                        FlavoredType tableType = info.Types[x];
                        FlavoredType itemType = new FlavoredType(items[x].GetType(), items[x].Flavor);
                        if(tableType.Type == itemType.Type &&
                            tableType.Flavor == itemType.Flavor)
                        {
                            tables.Add(table);
                        }
                    }
                }
            }
            return tables;
        }

        public static TableField GetField(TableBase table, string propertyName)
        {
            TableInfo info = new TableInfo(table);
            foreach(TableField field in info.Fields)
            {
                if(field.Property.Name == propertyName)
                {
                    return field;
                }
            }
            return null;
        }
        
    }
}
