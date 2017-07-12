using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECMisc : TECCost, CostComponent
    {
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

        public override object Copy()
        {
            var outCost = new TECMisc();
            outCost.copyPropertiesFromCost(this);
            outCost._guid = this.Guid;
            return outCost;
        }

        public override object DragDropCopy(TECScopeManager scopeManager)
        {
            return new TECMisc(this);
        }
    }
}
