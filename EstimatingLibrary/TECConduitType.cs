using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECConduitType : TECElectricalMaterial
    {
        public TECConduitType(Guid guid) : base(guid) { }
        public TECConduitType() : base() { }
    }
}
