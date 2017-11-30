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
                outCosts.AddCost(plenumCost);
            }
            return outCosts;
        } 

    }
}
