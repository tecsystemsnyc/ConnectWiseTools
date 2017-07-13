using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface ElectricalMaterialComponent
    {
        event PropertyChangedEventHandler PropertyChanged;

        Guid Guid { get; }
        string Name { get; set; }
        double Cost { get; set; }
        double Labor { get; set; }
        ObservableCollection<TECCost> RatedCosts { get; set; }
    }
}
