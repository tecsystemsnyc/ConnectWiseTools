using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{
    public static class ScopeExtensions
    {
        public static void CopyPropertiesFromScope(this TECScope scope, TECScope otherScope)
        {
            if(scope == otherScope)
            {
                return;
            }
            scope.Name = otherScope.Name;
            scope.Description = otherScope.Description;
            scope.Tags.ObservablyClear();
            foreach (TECTag tag in otherScope.Tags)
            {
                scope.Tags.Add(tag);
            }
            scope.AssociatedCosts.ObservablyClear();
            foreach (TECCost cost in otherScope.AssociatedCosts)
            {
                scope.AssociatedCosts.Add(cost);
            }
        }
        
        public static TECEquipment FindParentEquipment(this TECSubScope subScope, TECScopeManager manager)
        {
            foreach (TECEquipment equip in manager.GetAllEquipment())
            {
                if (equip.SubScope.Contains(subScope))
                {
                    return equip;
                }
            }
            return null;
        }

        public static TECEquipment FindParentEquipment(this TECSubScope subScope, TECSystem system)
        {
            foreach(TECEquipment equip in system.Equipment)
            {
                if (equip.SubScope.Contains(subScope))
                {
                    return equip;
                }
            }
            return null;
        }

        public static List<TECEquipment> GetAllEquipment(this TECScopeManager manager)
        {
            List<TECEquipment> equip = new List<TECEquipment>();
            if (manager is TECBid bid)
            {
                foreach(TECTypical typ in bid.Systems)
                {
                    foreach(TECSystem sys in typ.Instances)
                    {
                        equip.AddRange(sys.Equipment);
                    }
                }
            }
            else if (manager is TECTemplates templates)
            {
                foreach(TECSystem sys in templates.SystemTemplates)
                {
                    equip.AddRange(sys.Equipment);
                }
                equip.AddRange(templates.EquipmentTemplates);
            }
            return equip;
        }

        public static List<TECController> GetAllInstanceControllers(this TECBid bid)
        {
            List<TECController> instanceControllers = new List<TECController>();
            instanceControllers.AddRange(bid.Controllers);
            foreach(TECTypical typ in bid.Systems)
            {
                foreach(TECSystem sys in typ.Instances)
                {
                    instanceControllers.AddRange(sys.Controllers);
                }
            }
            return instanceControllers;
        }

        public static List<TECSubScope> GetAllInstanceSubScope(this TECBid bid)
        {
            List<TECSubScope> instanceSubScope = new List<TECSubScope>();
            foreach(TECTypical typ in bid.Systems)
            {
                foreach(TECSystem sys in typ.Instances)
                {
                    foreach(TECEquipment equip in sys.Equipment)
                    {
                        instanceSubScope.AddRange(equip.SubScope);
                    }
                }
            }
            return instanceSubScope;
        }
    }
}
