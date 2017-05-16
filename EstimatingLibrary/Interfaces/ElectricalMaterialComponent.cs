using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface ElectricalMaterialComponent
    {
        event PropertyChangedEventHandler PropertyChanged;

        string Name { get; set; }
        double Cost { get; set; }
        double Labor { get; set; }
    }
}
