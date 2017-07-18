using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECPanelType : TECHardware
    {
        public TECPanelType(Guid guid, TECManufacturer manufacturer) : base(guid, manufacturer) {
            _type = CostType.TEC;
        }
        public TECPanelType(TECManufacturer manufacturer) : this(Guid.NewGuid(), manufacturer) { }
        public TECPanelType(TECPanelType typeSource) : this(typeSource.Manufacturer)
        {
            copyPropertiesFromCost(typeSource);
        }

        public override object Copy()
        {
            var outCost = new TECPanelType(Manufacturer);
            outCost.copyPropertiesFromHardware(this);
            outCost._guid = this.Guid;
            return outCost;
        }
    }
}
