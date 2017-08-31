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
            if(items.Count > 2 || items.Count == 0)
            { throw new NotImplementedException(); }
            bool areSystems = haveSameType(items) && items[0].GetType() == typeof(TECSystem);

            foreach (TableBase table in AllTables.Tables)
            {
                TableInfo info = new TableInfo(table);
                if(matchesAllTypes(items, info.Types))
                {
                    tables.Add(table);
                }
                else if (items.Count == 2 && matchesObjectType(items[1], info.Types) && (!haveSameType(items) || areSystems))
                {
                    tables.Add(table);
                }
                else if (info.Name == TypicalInstanceTable.TableName && haveSameType(items) && items[0].GetType() != typeof(TECSystem))
                {
                    tables.Add(table);
                }
                
            }
            return tables;
        }

        private static bool matchesAllTypes(List<TECObject> items, List<FlavoredType> tableTypes)
        {
            if (items.Count == tableTypes.Count)
            {
                for (int x = 0; x < items.Count; x++)
                {
                    bool isTableType;
                    if (tableTypes[x].Type.IsInterface)
                    {
                        isTableType = tableTypes[x].Type.IsInstanceOfType(items[x]);
                    } else
                    {
                        isTableType = tableTypes[x].Type == items[x].GetType();
                    }
                    bool isTableFlavor = items[x].Flavor == tableTypes[x].Flavor;
                    if (!isTableType || !isTableFlavor)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private static bool matchesObjectType(TECObject item, List<FlavoredType> tableTypes)
        {
            if(tableTypes.Count != 1)
            {
                return false;
            }
            FlavoredType ft = tableTypes[0];
            return (item.GetType() == ft.Type && item.Flavor == ft.Flavor);
        }

        private static bool haveSameType(List<TECObject> items)
        {
            Type inital = items[0].GetType();
            for(int x = 1; x < items.Count; x++)
            {
                if(inital != items[x].GetType())
                {
                    return false;
                }
            }
            return true;

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
                if(field.Property.DeclaringType.IsInstanceOfType(item))
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
            if (parentField.Property.DeclaringType.IsInstanceOfType(item))
            {
                var dataString = objectToDBString(parentField.Property.GetValue(item, null));
                fieldData.Add(parentField.Name, dataString);
            }
            if(childField.Property.DeclaringType.IsInstanceOfType(child))
            {
                var dataString = objectToDBString(childField.Property.GetValue(child, null));
                fieldData.Add(childField.Name, dataString);
            }
            return fieldData;
        }

        public static Dictionary<string, string> PrepareDataForEditObject(List<TableField> fields, object item, string propertyName, object value)
        {
            foreach(TableField field in fields)
            {
                if(field.Property.DeclaringType.IsInstanceOfType(item) && 
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
