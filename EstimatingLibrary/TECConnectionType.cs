using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    
    public class TECConnectionType : TECElectricalMaterial
    {
        public TECConnectionType(Guid guid) : base(guid) { }
        public TECConnectionType() : base() { }
    }
}
