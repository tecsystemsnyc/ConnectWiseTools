using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using EstimatingLibrary;
using EstimatingLibrary.Interfaces;

namespace EstimatingUtilitiesLibrary.Database
{
    public class DeltaStacker
    {
        private List<UpdateItem> stack;

        public DeltaStacker(ChangeWatcher changeWatcher)
        {
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
        public static List<UpdateItem> ChildStack(Change change, ISaveable item)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            foreach (Tuple<string, TECObject> saveItem in item.SaveObjects.ChildList())
            {
                outStack.AddRange(addRemoveStack(change, saveItem.Item1, item as TECObject, saveItem.Item2));
            }
            if (item is TECSystem system)
            {
                outStack.AddRange(typicalInstanceStack(change, system));
            }

            return outStack;
        }
        
        private static List<UpdateItem> addRemoveStack(Change change, string propertyName, TECObject sender, TECObject item)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            List<TableBase> tables;
            if(sender is TECSystem system) { outStack.AddRange(typicalInstanceStack(change, system)); }
            if(sender is ISaveable parent && !parent.RelatedObjects.Contains(propertyName) && parent.SaveObjects.Contains(propertyName))
            {
                tables = DatabaseHelper.GetTables(new List<TECObject>() { item }, propertyName);
                outStack.AddRange(tableObjectStack(change, tables, item));
                if(item is ISaveable saveable)
                {
                    outStack.AddRange(ChildStack(change, saveable));
                }
            }
            tables = DatabaseHelper.GetTables(new List<TECObject>() { sender, item}, propertyName);
            outStack.AddRange(tableObjectStack(change, tables, sender, item));

            return outStack;
        }
        private static List<UpdateItem> editStack(TECObject sender, string propertyName, object value)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();

            List<TableBase> tables = DatabaseHelper.GetTables(sender);
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

        private static List<UpdateItem> tableObjectStack(Change change, List<TableBase> tables, TECObject item)
        {
            return tableObjectStack(change, tables, item, null);
        }
        private static List<UpdateItem> tableObjectStack(Change change, List<TableBase> tables, TECObject item, TECObject child)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            foreach (TableBase table in tables)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                if(table.Types.Count > 1)
                {
                    data = DatabaseHelper.PrepareDataForRelationTable(table.Fields, item, child);
                } else
                {
                    data = DatabaseHelper.PrepareDataForObjectTable(table.Fields, item);
                }
                outStack.Add(new UpdateItem(change, table.NameString, data));
                
            }
            return outStack;
        }
        private static List<UpdateItem> typicalInstanceStack(Change change, TECSystem system)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            foreach (KeyValuePair<TECObject, List<TECObject>> pair in system.TypicalInstanceDictionary.GetFullDictionary())
            {
                foreach(TECObject item in pair.Value)
                {
                    outStack.AddRange(addRemoveStack(change, "TypicalInstance", (TECObject)pair.Key, (TECObject)item));
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
                stack.AddRange(editStack(e.Sender as TECObject, e.PropertyName, e.Value));
            }
        }


    }
}
