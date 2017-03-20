using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECConduitType : TECScope
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

        public TECConduitType(Guid guid) : base(guid)
        {
            _cost = 0;
            _labor = 0;
        }
        public TECConduitType() : this(Guid.NewGuid()) { }
        public TECConduitType(TECConduitType conduitSource) : this()
        {
            copyPropertiesFromScope(conduitSource);
            _cost = conduitSource.Cost;
            _labor = conduitSource.Labor;
        }
        public override object Copy()
        {
            var outType = new TECConduitType();
            outType.copyPropertiesFromScope(this);
            outType._guid = this._guid;
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
