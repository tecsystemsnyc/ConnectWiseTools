using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class PropertyChangedExtendedEventArgs<T> : PropertyChangedEventArgs
    {
        public virtual T OldValue { get; private set; }
        public virtual T NewValue { get; private set; }
        public virtual Type OldType { get; private set; }
        public virtual Type NewType { get; private set; }

        public PropertyChangedExtendedEventArgs(string propertyName, T oldValue, T newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
            if (OldValue != null) { OldType = OldValue.GetType(); }
            if (NewValue != null) { NewType = NewValue.GetType(); }

        }

        public PropertyChangedExtendedEventArgs(string propertyName, T oldValue, T newValue, Type oldType, Type newType)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
            OldType = oldType;
            NewType = newType;
        }
    }
}
