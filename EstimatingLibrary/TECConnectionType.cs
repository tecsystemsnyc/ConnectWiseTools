using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECConnectionType : TECElectricalMaterial
    {
        private double _plenumCost;
        private double _plenumLabor;

        public double PlenumCost
        {
            get { return _plenumCost; }
            set
            {
                var old = PlenumCost;
                _plenumCost = value;
                notifyCombinedChanged(Change.Edit, "PlenumCost", this, value, old);
                notifyCostChanged(new CostBatch(value - old, 0, Type));
            }
        }
        public double PlenumLabor
        {
            get { return _plenumLabor; }
            set
            {
                var old = PlenumLabor;
                _plenumLabor = value;
                notifyCombinedChanged(Change.Edit, "PlenumLabor", this, value, old);
                notifyCostChanged(new CostBatch(0, value - old, Type));
            }
        }

        public TECConnectionType(Guid guid) : base(guid)
        {
            PlenumCost = 0.0;
        }
        public TECConnectionType() : this(Guid.NewGuid()) { }
        public TECConnectionType(TECConnectionType typeSource) : base(typeSource)
        {
            PlenumCost = typeSource.PlenumCost;
        }

        public CostBatch GetCosts(double length, bool isPlenum)
        {
            CostBatch outCosts = base.GetCosts(length);
            if (isPlenum)
            {
                TECCost plenumCost = new TECCost(CostType.Electrical);
                plenumCost.Cost = length * PlenumCost;
                plenumCost.Labor = length * PlenumLabor;
                outCosts.AddCost(plenumCost);
            }
            return outCosts;
        } 

    }
}
