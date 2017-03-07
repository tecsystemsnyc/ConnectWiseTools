using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECAssociatedCost : TECScope
    {
        #region Properties

        private double _cost;
        public double Cost
        {
            get { return _cost; }
            set
            {
                var temp = this.Copy();
                _cost = value;
                NotifyPropertyChanged("Cost", temp, this);
            }
        }
        #endregion

        #region Constructors
        public TECAssociatedCost(Guid guid) : base(guid)
        { _cost = 0; }

        public TECAssociatedCost() : this(Guid.NewGuid()) { }
        #endregion
        
        public override object Copy()
        {
            var outCost = new TECAssociatedCost();
            outCost.copyPropertiesFromScope(this);
            outCost._guid = this.Guid;
            outCost._cost = this.Cost;
            return outCost;
        }

        public override object DragDropCopy()
        {
            return this;
        }
    }
}
