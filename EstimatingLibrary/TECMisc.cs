using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECMisc : TECCost, CostComponent
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

        public TECMisc(Guid guid) : base(guid) { }
        public TECMisc() : this(Guid.NewGuid()) { }
        public TECMisc(TECMisc miscSource) : this()
        {
            copyPropertiesFromCost(miscSource);
        }

        public override object Copy()
        {
            var outCost = new TECMisc();
            outCost.copyPropertiesFromCost(this);
            outCost._guid = this.Guid;
            return outCost;
        }

        public override object DragDropCopy()
        {
            return new TECMisc(this);
        }
    }
}
