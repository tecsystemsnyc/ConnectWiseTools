using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECMiscCost : TECCost, CostComponent
    {
        public double MaterialCost
        {
            get
            {
                return Cost * Quantity;
            }
        }

        public double LaborCost
        {
            get
            {
                return Labor * Quantity;
            }
        }

        public double ElectricalCost
        {
            get
            {
                return 0;
            }
        }

        public double ElectricalLabor
        {
            get
            {
                return 0;
            }
        }

        public TECMiscCost(Guid guid) : base(guid) { }
        public TECMiscCost() : this(Guid.NewGuid()) { }
        public TECMiscCost(TECMiscCost costSource) : this()
        {
            copyPropertiesFromCost(costSource);
        }
        public override object Copy()
        {
            var outCost = new TECMiscCost();
            outCost.copyPropertiesFromCost(this);
            outCost._guid = this.Guid;
            return outCost;
        }

        public override object DragDropCopy()
        {
            return new TECMiscCost(this);
        }
    }
}
