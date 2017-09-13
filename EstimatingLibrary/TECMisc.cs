using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECMisc : TECCost, INotifyCostChanged, IDragDropable
    {
        #region Fields
        private int _quantity;
        #endregion

        #region Constructors
        public TECMisc(Guid guid, CostType type) : base(guid, type)
        {
            _quantity = 1;
        }
        public TECMisc(CostType type) : this(Guid.NewGuid(), type) { }
        public TECMisc(TECMisc miscSource) : this(miscSource.Type)
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
                var old = new TECMisc(this);
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
                var old = new TECMisc(this);
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
                var old = new TECMisc(this);
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
                NotifyCombinedChanged(Change.Edit, "Quantity", this, value, old);
                NotifyMiscChanged(this, oldMisc);
            }
        }

        public override CostBatch CostBatch
        {
            get
            {
                return new CostBatch(this * Quantity);
            }
        }
        #endregion

        #region Methods
        public override object DragDropCopy(TECScopeManager scopeManager)
        {
            return new TECMisc(this);
        }

        private void NotifyMiscChanged(TECMisc newMisc, TECMisc oldMisc)
        {
            CostBatch delta = newMisc.CostBatch - oldMisc.CostBatch;
            NotifyCostChanged(delta);
        }
        #endregion
    }
}
