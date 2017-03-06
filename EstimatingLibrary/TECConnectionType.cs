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

        public override object Copy()
        {
            var outType = new TECConnectionType();
            outType._guid = this._guid;
            outType.copyPropertiesFromScope(this);
            outType._cost = this._cost;
            outType._labor = this._labor;

            return outType;
        }
    }
}
