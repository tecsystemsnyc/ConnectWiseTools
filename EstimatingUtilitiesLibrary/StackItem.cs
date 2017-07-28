using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;

namespace EstimatingUtilitiesLibrary
{
    public class StackItem
    {
        public StackChange Change;
        public TECObject ReferenceObject;
        public TECObject TargetObject;
        public Type ReferenceType;
        public Type TargetType;

        public StackItem(StackChange change)
        {
            Change = change;
        }
        public StackItem(StackChange change, PropertyChangedExtendedEventArgs e) : this(change)
        {
            ReferenceObject = e.OldValue as TECObject;
            if(e.OldValue != null)
            {
                ReferenceType = e.OldValue.GetType();
            } else
            {
                ReferenceType = typeof(object);
            }
            

            TargetObject = e.Value as TECObject;
            TargetType = e.Value.GetType();
        }
        public StackItem(StackChange change, object referenceObject, object targetObject) : this(change)
        {
            if(targetObject == null || referenceObject == null)
            {
                throw new NotImplementedException();
            }
            ReferenceObject = referenceObject as TECObject;
            if (referenceObject != null) { ReferenceType = referenceObject.GetType(); }

            TargetObject = targetObject as TECObject;
            if (TargetObject != null) { TargetType = targetObject.GetType(); }
        }
        public StackItem(StackChange change, object referenceObject, object targetObject, Type referenceType, Type targetType) : this(change)
        {
            ReferenceObject = referenceObject as TECObject;
            ReferenceType = referenceType;

            TargetObject = targetObject as TECObject;
            TargetType = targetType;
        }

        public object[] Objects()
        {
            return new object[] { TargetObject, ReferenceObject };
        }
    }

    public class UpdateItem
    {
        public Change Change { get; private set; }
        public String Table { get; private set; }
        public Dictionary<String, String> FieldData { get; private set; }

        public UpdateItem(Change change, String tableName, Dictionary<String,String> fieldData)
        {
            Change = change;
            Table = tableName;
            FieldData = fieldData;
        }
    }
}
