using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECConnectionType : TECScope, ElectricalMaterialComponent
    {
        #region Properties

        protected double _cost;
        public double Cost
        {
            get { return _cost; }
            set
            {
                var temp = Copy();
                _cost = value;
                NotifyPropertyChanged("Cost", temp, this);
            }
        }

        protected double _labor;
        public double Labor
        {
            get { return _labor; }
            set
            {
                var temp = Copy();
                _labor = value;
                NotifyPropertyChanged("Labor", temp, this);
            }
        }

        #endregion

        public TECConnectionType(Guid guid) : base(guid)
        {
            _cost = 0;
            _labor = 0;
        }
        public TECConnectionType() : this(Guid.NewGuid()) { }
        public TECConnectionType(TECConnectionType connectionTypeSource) : this()
        {
            copyPropertiesFromScope(connectionTypeSource);
            _cost = connectionTypeSource.Cost;
            _labor = connectionTypeSource.Labor;
        }

        public override object Copy()
        {
            var outType = new TECConnectionType();
            outType._guid = this._guid;
            outType.copyPropertiesFromScope(this);
            outType._cost = this._cost;
            outType._labor = this._labor;

            return outType;
        }

        public override object DragDropCopy()
        {
            throw new NotImplementedException();
        }
    }
}
