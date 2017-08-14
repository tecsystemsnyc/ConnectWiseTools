using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum CostType { None, TEC, Electrical }

    public class TECCost : TECScope, DragDropComponent
    { 
        #region Properties

        protected double _cost;
        protected double _labor;
        protected CostType _type;
        
        public double Cost
        {
            get { return _cost; }
            set
            {
                var old = Cost;
                _cost = value;
                NotifyPropertyChanged(Change.Edit, "Cost", this, value, old);
            }
        }
        public double Labor
        {
            get { return _labor; }
            set
            {
                var old = Labor;
                _labor = value;
                NotifyPropertyChanged(Change.Edit, "Labor", this, value, old);
            }
        }
        public CostType Type
        {
            get { return _type; }
            set
            {
                var old = Type;
                _type = value;
                NotifyPropertyChanged(Change.Edit, "Type", this, value, old);
            }
        }
        #endregion

        #region Constructors
        public TECCost(Guid guid) : base(guid)
        {
            _cost = 0;
            _labor = 0;
            _type = 0;
        }
        public TECCost(TECCost cost) : this()
        {
            copyPropertiesFromCost(cost);
        }

        public TECCost() : this(Guid.NewGuid()) { }
        #endregion

        protected void copyPropertiesFromCost(TECCost cost)
        {
            copyPropertiesFromScope(cost);
            _cost = cost.Cost;
            _labor = cost.Labor;
            _type = cost.Type;
        }

        public object DragDropCopy(TECScopeManager scopeManager)
        {
            var copy = new TECCost();
            copy.copyPropertiesFromCost(this);
            return copy;
        }
    }
}
