using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECMisc : TECCost, INotifyCostChanged, DragDropComponent
    {
        private int _quantity;

        public event Action<List<TECCost>> CostChanged;

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                var old = Quantity;
                _quantity = value;
                NotifyPropertyChanged(Change.Edit, "Quantity", this, value, old);

            }
        }

        public List<TECCost> Costs
        {
            get
            {
                return getCosts();
            }
        }
        private List<TECCost> getCosts()
        {
            var outCosts = new List<TECCost>();
            outCosts.Add(this);
            foreach(TECCost cost in AssociatedCosts)
            {
                outCosts.Add(cost);
            }
            return outCosts;
        }

        public TECMisc(Guid guid) : base(guid) { }
        public TECMisc() : this(Guid.NewGuid()) { }
        public TECMisc(TECMisc miscSource) : this()
        {
            copyPropertiesFromCost(miscSource);
        }

        public new object DragDropCopy(TECScopeManager scopeManager)
        {
            return new TECMisc(this);
        }

        public void NotifyCostChanged(List<TECCost> costs)
        {
            CostChanged?.Invoke(costs);
        }
    }
}
