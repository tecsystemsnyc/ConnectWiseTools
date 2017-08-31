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
        
        public static List<UpdateItem> AddStack(TECObject sender, TECObject item)
        {
            if (item == null)
            {
                throw new Exception("Add and Remove must have an item which is being added to sender.");
            }
            return addRemoveStack(Change.Add, sender, item);
        }

        public static List<UpdateItem> ChildStack(Change change, ISaveable item)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            foreach (TECObject saveItem in item.SaveObjects)
            {
                outStack.AddRange(addRemoveStack(change, item as TECObject, saveItem));
            }
            if (item is TECSystem system)
            {
                outStack.AddRange(typicalInstanceStack(change, system));
            }

            return outStack;
        }

        private void handleChange(TECChangedEventArgs e)
        {
            if(e.Change == Change.Add || e.Change == Change.Remove)
            {
                stack.AddRange(addRemoveStack(e.Change, e.Sender as TECObject, e.Value as TECObject));
            }
            else if (e.Change == Change.Edit)
            {
                stack.AddRange(editStack(e.Sender as TECObject, e.PropertyName, e.Value));
            }
        }

        private static List<UpdateItem> addRemoveStack(Change change, TECObject sender, TECObject item)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();

            List<TECObject> items = new List<TECObject>();
            items.Add(sender);
            items.Add(item);
            List<TableBase> tables = TableHelper.GetTables(items);
            foreach(TableBase table in tables)
            {
                var info = new TableInfo(table);
                var data = new Dictionary<string, string>();
                if (info.IsRelationTable)
                {
                    var fields = new List<TableField>();
                    fields.Add(info.Fields[0]);
                    fields.Add(info.Fields[1]);
                    data = TableHelper.PrepareDataForRelationTable(fields, sender, item);
                    outStack.Add(new UpdateItem(change, info.Name, data));
                }
                else if (!info.IsCatalogTable || (info.IsCatalogTable && sender is TECCatalogs))
                {
                    var fields = info.Fields;
                    data = TableHelper.PrepareDataForObjectTable(fields, item);
                    if(item is ISaveable saveable)
                    {
                        outStack.AddRange(ChildStack(change, saveable));
                    }
                    outStack.Add(new UpdateItem(change, info.Name, data));

                }
            }
            return outStack;
        }

        private static List<UpdateItem> editStack(TECObject sender, string propertyName, object value)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();

            List<TableBase> tables = TableHelper.GetTables(new List<TECObject> { sender });
            foreach(TableBase table in tables)
            {
                var info = new TableInfo(table);
                var fields = info.Fields;
                var data = TableHelper.PrepareDataForEditObject(fields, sender, propertyName, value);
                if(data != null)
                {
                    outStack.Add(new UpdateItem(Change.Edit, info.Name, data));
                }
            }
            return outStack;
        }

        public List<UpdateItem> CleansedStack()
        {
            return stack;
        }
        
        private static List<UpdateItem> typicalInstanceStack(Change change, TECSystem system)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();
            foreach (KeyValuePair<TECObject, List<TECObject>> pair in system.TypicalInstanceDictionary.GetFullDictionary())
            {
                foreach(TECObject item in pair.Value)
                {
                    outStack.AddRange(addRemoveStack(change, pair.Key, item));
                }
            }

            return outStack;
        }

    }
}
