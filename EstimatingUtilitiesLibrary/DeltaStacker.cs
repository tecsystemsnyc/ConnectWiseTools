using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using EstimatingLibrary;

namespace EstimatingUtilitiesLibrary
{
    public class DeltaStacker
    {
        private List<UpdateItem> stack;

        public DeltaStacker(ChangeWatcher changeWatcher)
        {
            changeWatcher.ExtendedChanged += handleChange;
            stack = new List<UpdateItem>();
        }

        private void handleChange(PropertyChangedExtendedEventArgs e)
        {
            if(e.Change == Change.Add || e.Change == Change.Remove)
            {
                addRemoveToStack(e.Change, e.Sender as TECObject, e.Value as TECObject);
            }
            else if (e.Change == Change.Edit)
            {
                editToStack(e.Sender as TECObject, e.PropertyName, e.Value);
            }
        }

        private void addRemoveToStack(Change change, TECObject sender, TECObject item)
        {
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
                }
                else if (!info.IsCatalogTable || (info.IsCatalogTable && sender is TECCatalogs))
                {
                    var fields = info.Fields;
                    data = TableHelper.PrepareDataForObjectTable(fields, sender);
                }
                stack.Add(new UpdateItem(change, info.Name, data));
            }
        }

        private void editToStack(TECObject sender, string propertyName, object value)
        {
            List<TableBase> tables = TableHelper.GetTables(new List<TECObject> { sender });
            foreach(TableBase table in tables)
            {
                var info = new TableInfo(table);
                var fields = info.Fields;
                var data = TableHelper.PrepareDataForEditObject(fields, sender, propertyName, value);
                if(data != null)
                {
                    stack.Add(new UpdateItem(Change.Edit, info.Name, data));
                }
            }
        }

        public List<UpdateItem> CleansedStack()
        {
            return stack;
        }
    }
}
