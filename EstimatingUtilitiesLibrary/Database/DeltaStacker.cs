using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;

namespace EstimatingUtilitiesLibrary.Database
{
    public class DeltaStacker
    {
        private List<UpdateItem> stack;
        private static DBType dbType;
        private static TECTemplates templates;

        public DeltaStacker(ChangeWatcher changeWatcher, TECScopeManager manager = null)
        {
            dbType = DBType.Bid;
            if (manager != null && manager is TECTemplates)
            {
                dbType = DBType.Templates;
                templates = manager as TECTemplates;
            }
            changeWatcher.Changed += handleChange;
            stack = new List<UpdateItem>();
        }
        public List<UpdateItem> CleansedStack()
        {
            return stack;
        }

        public static List<UpdateItem> AddStack(string propertyName, TECObject sender, TECObject item)
        {
            if (item == null)
            {
                throw new Exception("Add and Remove must have an item which is being added to sender.");
            }
            return addRemoveStack(Change.Add, propertyName, sender, item);
        }
        public static List<UpdateItem> ChildStack(Change change, IRelatable item)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            foreach (Tuple<string, TECObject> saveItem in item.PropertyObjects.ChildList())
            {
                outStack.AddRange(addRemoveStack(change, saveItem.Item1, item as TECObject, saveItem.Item2));
            }
            if (item is TECTypical system)
            {
                outStack.AddRange(typicalInstanceStack(change, system));
            } else if(item is TECTemplates templates)
            {
                outStack.AddRange(templatesReferencesStack(change, templates));
            }

            return outStack;
        }

        private static IEnumerable<UpdateItem> templatesReferencesStack(Change change, TECTemplates templates)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            foreach (KeyValuePair<TECSubScope, List<TECSubScope>> pair in templates.SubScopeSynchronizer.GetFullDictionary())
            {
                foreach (TECSubScope item in pair.Value)
                {
                    outStack.AddRange(addRemoveStack(change, "TemplateRelationship", pair.Key, item));
                }
            }
            foreach (KeyValuePair<TECEquipment, List<TECEquipment>> pair in templates.EquipmentSynchronizer.GetFullDictionary())
            {
                foreach (TECEquipment item in pair.Value)
                {
                    outStack.AddRange(addRemoveStack(change, "TemplateRelationship", pair.Key, item));
                }
            }
            return outStack;
        }

        private static List<UpdateItem> addRemoveStack(Change change, string propertyName, TECObject sender, TECObject item)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            List<TableBase> tables;
            if(sender is IRelatable parent && !parent.LinkedObjects.Contains(propertyName) && parent.PropertyObjects.Contains(propertyName))
            {
                if(dbType != DBType.Templates || !templates.IsTemplateObject(item) || sender is TECTemplates)
                {
                    tables = DatabaseHelper.GetTables(new List<TECObject>() { item }, propertyName, dbType);
                    outStack.AddRange(tableObjectStack(change, tables, item));
                    if (item is IRelatable saveable)
                    {
                        outStack.AddRange(ChildStack(change, saveable));
                    }
                }
            }
            tables = DatabaseHelper.GetTables(new List<TECObject>() { sender, item }, propertyName);
            outStack.AddRange(tableObjectStack(change, tables, sender, item));

            return outStack;
        }
        private static List<UpdateItem> editStack(TECObject sender, string propertyName, 
            object value, object oldValue)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();

            if(!(value is TECObject) && !(oldValue is TECObject))
            {
                List<TableBase> tables = DatabaseHelper.GetTables(sender, dbType);
                foreach (TableBase table in tables)
                {
                    var fields = table.Fields;
                    var data = DatabaseHelper.PrepareDataForEditObject(fields, sender, propertyName, value);
                    var keyData = DatabaseHelper.PrimaryKeyData(table, sender);
                    if (data != null)
                    {
                        outStack.Add(new UpdateItem(Change.Edit, table.NameString, data, keyData));
                    }
                }
                return outStack;
            }
            else
            {
                if(value != null)
                {
                    List<TableBase> tables = DatabaseHelper.GetTables(new List<TECObject>() { sender, value as TECObject }, propertyName);
                    outStack.AddRange(tableObjectStack(Change.Add, tables, sender, value as TECObject));
                }
                if (oldValue != null)
                {
                    List<TableBase> tables = DatabaseHelper.GetTables(new List<TECObject>() { sender, oldValue as TECObject }, propertyName);
                    outStack.AddRange(tableObjectStack(Change.Remove, tables, sender, oldValue as TECObject));
                }
                return outStack;
            }
            
        }

        private static List<UpdateItem> tableObjectStack(Change change, List<TableBase> tables, TECObject item)
        {
            return tableObjectStack(change, tables, item, null);
        }
        private static List<UpdateItem> tableObjectStack(Change change, List<TableBase> tables, TECObject item, TECObject child)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            foreach (TableBase table in tables)
            {
                List<TableField> fields = new List<TableField>();
                if(change == Change.Remove)
                {
                    fields = table.PrimaryKeys;
                }
                else
                {
                    fields = table.Fields;
                }

                Dictionary<string, string> data = new Dictionary<string, string>();
                if(table.Types.Count > 1)
                {
                    data = DatabaseHelper.PrepareDataForRelationTable(fields, item, child);
                }
                else
                {
                    data = DatabaseHelper.PrepareDataForObjectTable(fields, item);
                }
                outStack.Add(new UpdateItem(change, table.NameString, data));
                
            }
            return outStack;
        }
        private static List<UpdateItem> typicalInstanceStack(Change change, TECTypical system)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            foreach (KeyValuePair<TECObject, List<TECObject>> pair in system.TypicalInstanceDictionary.GetFullDictionary())
            {
                foreach(TECObject item in pair.Value)
                {
                    outStack.AddRange(addRemoveStack(change, "TypicalInstanceDictionary", (TECObject)pair.Key, (TECObject)item));
                }
            }

            return outStack;
        }

        private void handleChange(TECChangedEventArgs e)
        {
            if (e.Change == Change.Add || e.Change == Change.Remove)
            {
                stack.AddRange(addRemoveStack(e.Change, e.PropertyName, e.Sender as TECObject, e.Value as TECObject));
            }
            else if (e.Change == Change.Edit)
            {
                stack.AddRange(editStack(e.Sender as TECObject, e.PropertyName, e.Value, e.OldValue));
            }
        }
    }
}
