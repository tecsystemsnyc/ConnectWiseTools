using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECElectricalMaterial : TECScope
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

        #region Initializers
        public TECElectricalMaterial() : this(Guid.NewGuid()) { }
        public TECElectricalMaterial(Guid guid) : base(guid)
        {
            _cost = 0;
            _labor = 0;
        }
        #endregion
        
        public override object DragDropCopy()
        {
            throw new NotImplementedException();
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
