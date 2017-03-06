using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECConduitType : TECElectricalMaterial
    {
        public TECConduitType(Guid guid) : base(guid) { }
        public TECConduitType() : base() { }

        public override object Copy()
        {
            var outType = new TECConduitType();
            outType.copyPropertiesFromScope(this);
            outType._guid = this._guid;
            outType._cost = this._cost;
            outType._labor = this._labor;

            return outType;
        }
    }
}
