using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs
    {
        public virtual TECObject Sender { get; private set; }
        public virtual Object Value { get; private set; }
        public virtual Object OldValue { get; private set; }
        public virtual Change Change { get; private set; }

        public PropertyChangedExtendedEventArgs(Change change, string propertyName, TECObject sender,
            object newValue, object oldValue)
            : base(propertyName)
        {
            Change = change;
            Sender = sender;
            Value = newValue;
            OldValue = oldValue;
        }
        
    }
}
