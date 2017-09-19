using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECMisc : TECCost, IDragDropable, ITypicalable
    {
        #region Fields
        private int _quantity;
        #endregion

        #region Constructors
        public TECMisc(Guid guid, CostType type, bool isTypical) : base(guid, type)
        {
            IsTypical = isTypical;
            _quantity = 1;
        }
        public TECMisc(CostType type, bool isTypical) : this(Guid.NewGuid(), type, isTypical) { }
        public TECMisc(TECMisc miscSource, bool isTypical) : this(miscSource.Type, isTypical)
        {
            copyPropertiesFromCost(miscSource);
        }
        #endregion
        
        #region Properties
        public override double Cost
        {
            get
            {
                return base.Cost;
            }
            set
            {
                var old = new TECMisc(this, this.IsTypical);
                base.Cost = value;
                NotifyMiscChanged(this, old);
            }
        }
        public override double Labor
        {
            get
            {
                return base.Labor;
            }
            set
            {
                var old = new TECMisc(this, this.IsTypical);
                base.Labor = value;
                NotifyMiscChanged(this, old);
            }
        }
        public override CostType Type
        {
            get
            {
                return base.Type;
            }
            set
            {
                var old = new TECMisc(this, this.IsTypical);
                base.Type = value;
                NotifyMiscChanged(this, old);
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                var oldMisc = this;
                var old = Quantity;
                _quantity = value;
                notifyCombinedChanged(Change.Edit, "Quantity", this, value, old);
                NotifyMiscChanged(this, oldMisc);
            }
        }
        
        public bool IsTypical { get; private set; }
        #endregion

        #region Methods
        public override object DragDropCopy(TECScopeManager scopeManager)
        {
            return new TECMisc(this, this.IsTypical);
        }

        private void NotifyMiscChanged(TECMisc newMisc, TECMisc oldMisc)
        {
            CostBatch delta = newMisc.CostBatch - oldMisc.CostBatch;
            notifyCostChanged(delta);
        }

        protected override CostBatch getCosts()
        {
            return base.getCosts() * Quantity;
        }

        protected override void notifyCostChanged(CostBatch costs)
        {
            if (!IsTypical)
            {
                base.notifyCostChanged(costs);
            }
        }
        #endregion
    }
}
