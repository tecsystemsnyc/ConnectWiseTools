using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface INotifyCostChanged
    {
        event Action<List<TECCost>> CostChanged;
        List<TECCost> Costs { get; }
    }

    public static class CostHelper
    {
        public static List<TECCost> NegativeCosts(List<TECCost> costs)
        {
            List<TECCost> outCosts = new List<TECCost>();
            foreach(TECCost cost in costs)
            {
                var opposite = new TECCost(cost);
                opposite.Cost *= -1;
                outCosts.Add(opposite);
            }
            return outCosts;
        }
    }
}
