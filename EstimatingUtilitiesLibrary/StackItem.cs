using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;

namespace EstimatingUtilitiesLibrary
{
    public class StackItem
    {
        public Change Change;
        public TECObject ReferenceObject;
        public TECObject TargetObject;
        public Type ReferenceType;
        public Type TargetType;

        public StackItem(Change change, PropertyChangedExtendedEventArgs<object> e)
        {
            Change = change;

            ReferenceObject = e.OldValue as TECObject;
            ReferenceType = e.OldType;

            TargetObject = e.NewValue as TECObject;
            TargetType = e.NewType;
        }
        public StackItem(Change change, object referenceObject, object targetObject)
        {
            Change = change;
            ReferenceObject = referenceObject as TECObject;
            if (referenceObject != null) { ReferenceType = referenceObject.GetType(); }

            TargetObject = targetObject as TECObject;
            if (TargetObject != null) { TargetType = targetObject.GetType(); }
        }
        public StackItem(Change change, object referenceObject, object targetObject, Type referenceType, Type targetType)
        {
            Change = change;
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
}
