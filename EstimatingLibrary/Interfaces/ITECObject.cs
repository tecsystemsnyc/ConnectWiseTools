using System;
using System.ComponentModel;

namespace EstimatingLibrary.Interfaces
{
    public interface ITECObject
    {
        Guid Guid { get; }

        event PropertyChangedEventHandler PropertyChanged;
        event Action<TECChangedEventArgs> TECChanged;
    }
}