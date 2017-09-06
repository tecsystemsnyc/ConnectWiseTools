using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;

namespace EstimatingUtilitiesLibrary
{ 
    public class UpdateItem
    {
        public Change Change { get; private set; }
        public String Table { get; private set; }
        public Dictionary<String, String> FieldData { get; private set; }
        public Tuple<string, string> PrimaryKey { get; private set; }

        public UpdateItem(Change change, String tableName, Dictionary<String,String> fieldData, Tuple<string, string> primaryKey = null)
        {
            Change = change;
            Table = tableName;
            FieldData = fieldData;
            PrimaryKey = primaryKey;
        }
    }
}
