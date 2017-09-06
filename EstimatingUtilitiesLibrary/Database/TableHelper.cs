using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace EstimatingUtilitiesLibrary.Database
{
    internal class TableHelper
    {
        public static List<TableBase> GetTables(List<TECObject> items, string propertyName)
        {
            List<TableBase> tables = new List<TableBase>();
            if(items.Count > 2 || items.Count == 0)
            { throw new NotImplementedException(); }

            foreach (TableBase table in AllTables.Tables)
            {
                if(matchesAllTypes(items, table.Types) && matchesPropertyName(propertyName, table.PropertyNames))
                {
                    tables.Add(table);
                }
                
            }
            return tables;
        }
        public static List<TableBase> GetTables(TECObject item)
        {
            List<TableBase> tables = new List<TableBase>();

            foreach (TableBase table in AllTables.Tables)
            {
                if (matchesObjectType(item, table.Types))
                {
                    tables.Add(table);
                }
            }
            return tables;
        }
        public static TableField GetField(TableBase table, string propertyName)
        {
            foreach(TableField field in table.Fields)
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
            foreach (TableField field in fields)
            {
                if (field.Property.ReflectedType == typeof(HelperProperties))
                {
                    var dataString = objectToDBString(helperObject(field, item, child));
                    fieldData.Add(field.Name, dataString);
                }
            }

            return fieldData;
        }
        public static Dictionary<string, string> PrepareDataForEditObject(List<TableField> fields, TECObject item, string propertyName, object value)
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
        public static Tuple<string, string> PrimaryKeyData(TableBase table, TECObject item)
        {
            if(table.PrimaryKeys.Count != 1)
            {
                throw new Exception("Must have one primary key in table being updated.");
            }
            TableField field = table.PrimaryKeys[0];
            if (field.Property.DeclaringType.IsInstanceOfType(item))
            {
                Tuple<string, string> outData = new Tuple<string, string>(field.Name, objectToDBString(item.Guid));
                return outData;
            }
            else
            {
                throw new Exception("Item does not match type of key.");
            }
        }

        private static bool matchesAllTypes(List<TECObject> items, List<Type> tableTypes)
        {
            if (items.Count == tableTypes.Count)
            {
                for (int x = 0; x < items.Count; x++)
                {
                    bool isTableType = tableTypes[x].IsInstanceOfType(items[x]);
                    
                    if (!isTableType)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        private static bool matchesPropertyName(string propertyName, List<string> tablePropertyNames)
        {
            return (tablePropertyNames.Contains(propertyName));
        }
        private static bool matchesObjectType(TECObject item, List<Type> tableTypes)
        {
            if (tableTypes.Count != 1)
            {
                return false;
            }
            Type tableType= tableTypes[0];
            return (item.GetType() == tableType);
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
        private static object helperObject(TableField field, object item, object child)
        {
            if(field.Property.Name == "Quantity")
            {

                var childCollection = item.GetType().GetProperty(field.HelperContext).GetValue(item) as IEnumerable;
                int count = 0;
                foreach(object thing in childCollection)
                {
                    if(thing == child)
                    {
                        count++;
                    }
                }
                return count;
            } else if (field.Property.Name == "Index")
            {
                var childCollection = item.GetType().GetProperty(field.HelperContext).GetValue(item) as IEnumerable;
                int x = 0;
                foreach(object thing in childCollection)
                {
                    if(thing == item)
                    {
                        return x;
                    }
                    x++;
                }
                return 0;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
