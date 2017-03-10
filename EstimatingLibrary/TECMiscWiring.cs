using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECMiscWiring : TECCost
    {
        public TECMiscWiring(Guid guid) : base(guid) { }
        public TECMiscWiring() : this(Guid.NewGuid()) { }
    }
}
