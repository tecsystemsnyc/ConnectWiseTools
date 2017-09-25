using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface IEndDevice
    {
        ObservableCollection<TECElectricalMaterial> ConnectionTypes { get; }
        Guid Guid { get; }
    }
}
