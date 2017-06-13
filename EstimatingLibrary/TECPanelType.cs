using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECPanelType : TECCost
    {
        public TECPanelType(Guid guid) : base(guid) {
            _type = CostType.TEC;
        }
        public TECPanelType() : this(Guid.NewGuid()) { }
        public TECPanelType(TECPanelType typeSource) : this()
        {
            copyPropertiesFromCost(typeSource);
        }

        public override object Copy()
        {
            var outCost = new TECPanelType();
            outCost.copyPropertiesFromCost(this);
            outCost._guid = this.Guid;
            return outCost;
        }
    }
}
