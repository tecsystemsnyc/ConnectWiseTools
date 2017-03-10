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
    }
}
