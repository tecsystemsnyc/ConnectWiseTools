using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{ 
    public class TECCost : TECScope
    {
        #region Properties

        protected double _cost;
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
        public TECCost(Guid guid) : base(guid)
        {
            _cost = 0;
        }

        public TECCost() : this(Guid.NewGuid()) { }
        #endregion

        protected void copyPropertiesFromCost(TECCost cost)
        {
            copyPropertiesFromScope(cost);
            _cost = cost.Cost;
        }
        
        public override object Copy()
        {
            var outCost = new TECCost();
            outCost.copyPropertiesFromCost(this);
            outCost._guid = this.Guid;
            return outCost;
        }

        public override object DragDropCopy()
        {
            throw new NotImplementedException();
        }
    }
}
