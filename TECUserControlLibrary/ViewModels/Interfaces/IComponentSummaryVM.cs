using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels.Interfaces
{
    public interface IComponentSummaryVM
    {
        double TotalTECCost { get; }
        double TotalTECLabor { get; }
        double TotalElecCost { get; }
        double TotalElecLabor { get; }
    }
}
