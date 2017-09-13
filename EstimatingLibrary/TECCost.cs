using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum CostType { TEC, Electrical }

    public class TECCost : TECScope, IDragDropable
    { 
        #region Properties
        protected double _cost;
        protected double _labor;
        protected CostType _type;
        
        public virtual double Cost
        {
            get { return _cost; }
            set
            {
                var old = Cost;
                _cost = value;
                NotifyCombinedChanged(Change.Edit, "Cost", this, value, old);
                NotifyCostChanged(new CostBatch(value - old, 0, Type));
            }
        }
        public virtual double Labor
        {
            get { return _labor; }
            set
            {
                var old = Labor;
                _labor = value;
                NotifyCombinedChanged(Change.Edit, "Labor", this, value, old);
                NotifyCostChanged(new CostBatch(0, value - old, Type));
            }
        }
        public virtual CostType Type
        {
            get { return _type; }
            set
            {
                var old = Type;
                _type = value;
                NotifyCombinedChanged(Change.Edit, "Type", this, value, old);
                NotifyCostChanged(new CostBatch(-Cost, -Labor, old));
                NotifyCostChanged(new CostBatch(Cost, Labor, value));
            }
        }

        public override CostBatch CostBatch
        {
            get { return base.CostBatch + new CostBatch(this); }
        }
        #endregion

        #region Constructors
        public TECCost(Guid guid, CostType type) : base(guid)
        {
            _cost = 0;
            _labor = 0;
            _type = type;
        }
        public TECCost(TECCost cost) : this(cost.Type)
        {
            copyPropertiesFromCost(cost);
        }

        public TECCost(CostType type) : this(Guid.NewGuid(), type) { }
        #endregion

        protected void copyPropertiesFromCost(TECCost cost)
        {
            copyPropertiesFromScope(cost);
            _cost = cost.Cost;
            _labor = cost.Labor;
            _type = cost.Type;
        }

        public virtual object DragDropCopy(TECScopeManager scopeManager)
        {
            return new TECCost(this);
        }

        public static TECCost operator *(TECCost left, double right)
        {
            TECCost newCost = new TECCost(left);
            newCost.Cost *= right;
            newCost.Labor *= right;
            return newCost;
        }
    }
}
