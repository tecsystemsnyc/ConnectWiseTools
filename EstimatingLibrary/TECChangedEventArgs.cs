using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECChangedEventArgs : PropertyChangedEventArgs
    {
        public TECObject Sender { get; private set; }
        public Object Value { get; private set; }
        public Object OldValue { get; private set; }
        public Change Change { get; private set; }

        public TECChangedEventArgs(Change change, string propertyName, TECObject sender,
            object value, object oldValue)
            : base(propertyName)
        {
            Change = change;
            Sender = sender;
            Value = value;
            OldValue = oldValue;
        }
    }
}
