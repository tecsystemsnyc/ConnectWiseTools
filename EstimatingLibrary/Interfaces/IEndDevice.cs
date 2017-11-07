using System;
using System.Collections.ObjectModel;

namespace EstimatingLibrary.Interfaces
{
    public interface IEndDevice
    {
        ObservableCollection<TECElectricalMaterial> ConnectionTypes { get; }
        Guid Guid { get; }
    }
}
