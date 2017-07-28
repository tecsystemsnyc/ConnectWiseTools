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

        private void handleChange(object sender, PropertyChangedExtendedEventArgs e)
        {
            if(e.Change == Change.Add)
            {
                
            }
            else if (e.Change == Change.Remove)
            {

            }
            else if (e.Change == Change.Edit)
            {

            }
        }

        private void AddRemove(Change change, TECObject sender, TECObject item)
        {
            List<TECObject> items = new List<TECObject>();
            items.Add(sender);
            items.Add(item);
            List<TableBase> tables = TableHelper.GetTables(items);
            foreach(TableBase table in tables)
            {
                TableInfo info = new TableInfo(table);
                foreach(TableField field in info.Fields)
                {
                    if(field.)
                }
            }
        }
    }
}
