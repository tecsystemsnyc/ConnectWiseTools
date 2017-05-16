using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECMiscWiring : TECCost, CostComponent
    {
        public double MaterialCost
        {
            get
            {
                return 0;
            }
        }

        public double LaborCost
        {
            get
            {
                return 0;
            }
        }

        public double ElectricalCost
        {
            get
            {
                return Cost * Quantity;
            }
        }

        public double ElectricalLabor
        {
            get
            {
                return Labor * Quantity;
            }
        }

        public TECMiscWiring(Guid guid) : base(guid) { }
        public TECMiscWiring() : this(Guid.NewGuid()) { }
        public TECMiscWiring(TECMiscWiring wiringSource) : this()
        {
            copyPropertiesFromCost(wiringSource);
        }

        public override object Copy()
        {
            var outCost = new TECMiscWiring();
            outCost.copyPropertiesFromCost(this);
            outCost._guid = this.Guid;
            return outCost;
        }

        public override object DragDropCopy()
        {
            return new TECMiscWiring(this);
        }
    }
}
