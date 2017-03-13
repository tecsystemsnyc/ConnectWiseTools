using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECMiscCost : TECCost
    {
        public TECMiscCost(Guid guid) : base(guid) { }
        public TECMiscCost() : this(Guid.NewGuid()) { }

        public override object Copy()
        {
            var outCost = new TECMiscCost();
            outCost.copyPropertiesFromCost(this);
            outCost._guid = this.Guid;
            return outCost;
        }
    }
}
