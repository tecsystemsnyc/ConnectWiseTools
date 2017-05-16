using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    interface CostComponent
    {
        double MaterialCost { get; }
        double LaborCost { get; }
        double ElectricalCost { get; }
        double ElectricalLabor { get; }
    }
}
