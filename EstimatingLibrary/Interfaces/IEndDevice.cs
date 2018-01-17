using System;
using System.Collections.ObjectModel;

namespace EstimatingLibrary.Interfaces
{
    public interface IEndDevice : ITECObject
    {
        ObservableCollection<TECConnectionType> ConnectionTypes { get; }
        TECManufacturer Manufacturer { get; }
        String Name { get; }
        String Description { get; }
    }
}
