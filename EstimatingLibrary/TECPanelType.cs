using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECPanelType : TECHardware
    {
        private const CostType COST_TYPE = CostType.TEC;

        public TECPanelType(Guid guid, TECManufacturer manufacturer) : base(guid, manufacturer, COST_TYPE) {
            _type = CostType.TEC;
        }
        public TECPanelType(TECManufacturer manufacturer) : this(Guid.NewGuid(), manufacturer) { }
        public TECPanelType(TECPanelType typeSource) : this(typeSource.Manufacturer)
        {
            copyPropertiesFromCost(typeSource);
        }
    }
}
