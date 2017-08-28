using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.Database
{
    internal class TableHelper
    {
        public static List<TableBase> GetTables(List<TECObject> items)
        {
            List<TableBase> tables = new List<TableBase>();
            foreach(TableBase table in AllTables.Tables)
            {
                TableInfo info = new TableInfo(table);
                if(info.Types.Count == items.Count)
                {
                    bool allMatch = false;
                    for (int x = 0; x < items.Count; x++)
                    {
                        FlavoredType tableType = info.Types[x];
                        FlavoredType itemType = new FlavoredType(items[x].GetType(), items[x].Flavor);
                        if(tableType.Type == itemType.Type &&
                            tableType.Flavor == itemType.Flavor)
                        {
                            allMatch = true;
                        }
                        else
                        {
                            allMatch = false;
                            break;
                        }
                       
                    }
                    if (allMatch)
                    {
                        tables.Add(table);
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
        
        public static Dictionary<string, string> PrepareDataForObjectTable(List<TableField> fields, object item)
        {
            Dictionary<string, string> fieldData = new Dictionary<string, string>();
            foreach(TableField field in fields)
            {
                if(field.Property.ReflectedType == item.GetType())
                {
                    var dataString = objectToDBString(field.Property.GetValue(item, null));
                    fieldData.Add(field.Name, dataString);
                }
            }
            return fieldData;
        }

        public static Dictionary<string, string> PrepareDataForRelationTable(List<TableField> fields, object item, object child)
        {
            Dictionary<string, string> fieldData = new Dictionary<string, string>();
            var parentField = fields[0];
            var childField = fields[1];
            if (parentField.Property.ReflectedType == item.GetType())
            {
                var dataString = objectToDBString(parentField.Property.GetValue(item, null));
                fieldData.Add(parentField.Name, dataString);
            }
            if (childField.Property.ReflectedType == item.GetType())
            {
                var dataString = objectToDBString(childField.Property.GetValue(item, null));
                fieldData.Add(childField.Name, dataString);
            }
            return fieldData;
        }

        public static Dictionary<string, string> PrepareDataForEditObject(List<TableField> fields, object item, string propertyName, object value)
        {
            foreach(TableField field in fields)
            {
                if(field.Property.ReflectedType == item.GetType() && 
                    field.Property.Name == propertyName)
                {
                    Dictionary<string, string> outData = new Dictionary<string, string>();
                    outData[field.Name] = objectToDBString(value);
                    return outData;
                }
            }
            return null;
        }

        private static string objectToDBString(Object inObject)
        {
            string outstring = "";
            if (inObject is bool)
            {
                outstring = ((bool)inObject).ToInt().ToString();
            }
            else
            {
                outstring = inObject.ToString();
            }

            return outstring;
        }

    }
}
