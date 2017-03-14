using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECAssociatedCost : TECCost
    {
        #region Properties
        
        private double _labor;
        public double Labor
        {
            get { return _labor; }
            set
            {
                var temp = this.Copy();
                _labor = value;
                NotifyPropertyChanged("Labor", temp, this);
            }
        }
        #endregion

        #region Constructors
        public TECAssociatedCost(Guid guid) : base(guid)
        {
            _labor = 0;
        }

        public TECAssociatedCost() : this(Guid.NewGuid()) { }
        #endregion
        
        public override object Copy()
        {
            var outCost = new TECAssociatedCost();
            outCost.copyPropertiesFromScope(this);
            outCost._guid = this.Guid;
            outCost._cost = this.Cost;
            outCost._labor = this.Labor;
            return outCost;
        }

        public override object DragDropCopy()
        {
            return this;
        }
    }
}
