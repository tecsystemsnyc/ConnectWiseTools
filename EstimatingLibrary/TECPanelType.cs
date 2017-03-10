using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECPanelType : TECCost
    {
        public TECPanelType(Guid guid) : base(guid) { }
        public TECPanelType() : this(Guid.NewGuid()) { }
    }
}
