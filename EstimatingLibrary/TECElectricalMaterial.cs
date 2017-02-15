using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECElectricalMaterial : TECScope
    {
        #region Properties

        private double _cost;
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

        private double _labor;
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

        #region Initializers
        public TECElectricalMaterial() : this(Guid.NewGuid())
        {

        }
        public TECElectricalMaterial(Guid guid) : base("","",guid)
        {
            _cost = 0;
            _labor = 0;
        }
        #endregion

        public override object Copy()
        {
            var outType = new TECElectricalMaterial();
            outType._guid = this._guid;
            outType._name = this._name;
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
