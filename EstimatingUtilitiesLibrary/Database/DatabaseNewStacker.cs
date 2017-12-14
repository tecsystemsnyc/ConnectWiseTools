using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System.Collections.Generic;

namespace EstimatingUtilitiesLibrary.Database
{
    internal class DatabaseNewStacker
    {
        public static List<UpdateItem> NewStack(TECObject toSave)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            saveStack.AddRange(newStackForObject(toSave));
            TECTemplates templates = toSave as TECTemplates;
            if (toSave is IRelatable saveable)
            {
                saveStack.AddRange(DeltaStacker.ChildStack(Change.Add, saveable));
            }
            return saveStack;
        }
        
        private static List<UpdateItem> newStackForObject(TECObject toSave)
        {
            List<UpdateItem> outStack = new List<UpdateItem>();

            List<TableBase> tables = DatabaseHelper.GetTables(toSave);
            foreach (TableBase table in tables)
            {
                var fields = table.Fields;
                var data = DatabaseHelper.PrepareDataForObjectTable(fields, toSave);
                if (data != null)
                {
                    outStack.Add(new UpdateItem(Change.Add, table.NameString, data));
                }
            }
            return outStack;
        }
        
    }
}
