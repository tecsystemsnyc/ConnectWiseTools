using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibrary.DatabaseHelpers
{
    public class DatabaseSaver
    {

        public static void Save(TECScopeManager scopeManager, string path)
        {
            throw new NotImplementedException();
        }

        public static void Save(TECBid bid, string path)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            saveStack.AddRange(scopeManagerSaveStack(bid));
            saveStack.AddRange(DeltaStacker.AddStack(bid, bid.Parameters));
            foreach(TECSystem system in bid.Systems)
            {
                saveStack.AddRange(DeltaStacker.AddStack(bid, system));
                systemChildrenSaveStack(system);
            }
            foreach(TECPanel panel in bid.Panels)
            {
                saveStack.AddRange(DeltaStacker.AddStack(bid, panel));
                saveStack.AddRange(panelChildrenSaveStack(panel));
            }
            foreach(TECController controller in bid.Controllers)
            {
                saveStack.AddRange(DeltaStacker.AddStack(bid, controller));
                saveStack.AddRange(controllerChildrenSaveStack(controller));
            }
            foreach(TECMisc misc in bid.MiscCosts)
            {
                saveStack.AddRange(DeltaStacker.AddStack(bid, misc));
                saveStack.AddRange(scopeChildrenSaveStack(misc));
            }
            foreach(TECScopeBranch scopeBranch in bid.ScopeTree)
            {
                saveStack.AddRange(DeltaStacker.AddStack(bid, scopeBranch));
                saveStack.AddRange(scopeBranchChildrenSaveStack(scopeBranch));
            }
        }


        public static void Save(TECTemplates templates, string path)
        {
            throw new NotImplementedException();
        }

        public static List<UpdateItem> scopeManagerSaveStack(TECScopeManager scopeManager)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            saveStack.AddRange(DeltaStacker.AddStack(scopeManager, scopeManager.Labor));
            saveStack.AddRange(catalogsSaveStack(scopeManager.Catalogs));
            return saveStack;
        }

        public static List<UpdateItem> catalogsSaveStack(TECCatalogs catalogs)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            saveStack.AddRange(catalogsHardwareCollectionSaveStack(catalogs, catalogs.Devices));
            saveStack.AddRange(catalogsHardwareCollectionSaveStack(catalogs, catalogs.IOModules));
            saveStack.AddRange(catalogsHardwareCollectionSaveStack(catalogs, catalogs.ControllerTypes));
            saveStack.AddRange(catalogsHardwareCollectionSaveStack(catalogs, catalogs.PanelTypes));
            saveStack.AddRange(electricalMaterialCollectionSaveStack(catalogs, catalogs.ConnectionTypes));
            saveStack.AddRange(electricalMaterialCollectionSaveStack(catalogs, catalogs.ConduitTypes));
            saveStack.AddRange(labeledSaveStack(catalogs, catalogs.Tags));
            saveStack.AddRange(labeledSaveStack(catalogs, catalogs.Manufacturers));
            return saveStack;

        }
        public static List<UpdateItem> catalogsHardwareCollectionSaveStack(TECCatalogs catalogs, IEnumerable<TECHardware> hardware)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach(TECHardware item in hardware)
            {
                saveStack.AddRange(DeltaStacker.AddStack(catalogs, item));
                saveStack.AddRange(hardwareChildrenSaveStack(item));
            }
            return saveStack;
        }
        public static List<UpdateItem> electricalMaterialCollectionSaveStack(TECCatalogs catalogs, IEnumerable<TECElectricalMaterial> materials)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach (TECElectricalMaterial item in materials)
            {
                saveStack.AddRange(DeltaStacker.AddStack(catalogs, item));
                saveStack.AddRange(electricalMaterialChildrenSaveStack(item));
            }
            return saveStack;
        }

        public static List<UpdateItem> labeledSaveStack(TECObject parent, IEnumerable<TECLabeled> labeled)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach(TECLabeled item in labeled)
            {
                saveStack.AddRange(DeltaStacker.AddStack(parent, item));
            }
            return saveStack;
        }

        private static List<UpdateItem> electricalMaterialChildrenSaveStack(TECElectricalMaterial item)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach (TECCost cost in item.RatedCosts)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, cost));
            }
            saveStack.AddRange(scopeChildrenSaveStack(item));
            return saveStack;
        }

        private static List<UpdateItem> systemChildrenSaveStack(TECSystem item)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            saveStack.AddRange(locatedChildrenSaveStack(item));
            foreach (TECEquipment equipment in item.Equipment)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, equipment));
                saveStack.AddRange(equipmentChildrenSaveStack(equipment));
            }
            foreach(TECController controller in item.Controllers)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, controller));
                saveStack.AddRange(controllerChildrenSaveStack(controller));
            }
            foreach(TECPanel panel in item.Panels)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, panel));
                saveStack.AddRange(panelChildrenSaveStack(panel));
            }
            foreach(TECScopeBranch branch in item.ScopeBranches)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, branch));
                saveStack.AddRange(scopeBranchChildrenSaveStack(branch));
            }
            foreach(TECSystem instance in item.Instances)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, instance));
                saveStack.AddRange(systemChildrenSaveStack(instance));
            }
            return saveStack;
        }

        private static IEnumerable<UpdateItem> panelChildrenSaveStack(TECPanel panel)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach (TECController controller in panel.Controllers)
            {
                saveStack.AddRange(DeltaStacker.AddStack(panel, controller));
            }
            saveStack.AddRange(DeltaStacker.AddStack(panel, panel.Type));
            saveStack.AddRange(scopeChildrenSaveStack(panel));
            return saveStack;
        }

        private static List<UpdateItem> controllerChildrenSaveStack(TECController controller)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                saveStack.AddRange(DeltaStacker.AddStack(controller, connection));
                if(connection is TECNetworkConnection netConnection)
                {
                    saveStack.AddRange(networkConnectionChildrenSaveStack(netConnection));

                } else if (connection is TECSubScopeConnection subConnection)
                {
                    saveStack.AddRange(subscopeConnectionChildrenSaveStack(subConnection));
                }
            }
            saveStack.AddRange(scopeChildrenSaveStack(controller));
            return saveStack;

        }

        private static List<UpdateItem> scopeBranchChildrenSaveStack(TECScopeBranch scopeBranch)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach (TECScopeBranch branch in scopeBranch.Branches)
            {
                saveStack.AddRange(DeltaStacker.AddStack(scopeBranch, branch));
                saveStack.AddRange(scopeBranchChildrenSaveStack(branch));
            }
            return saveStack;
        }

        private static List<UpdateItem> subscopeConnectionChildrenSaveStack(TECSubScopeConnection subConnection)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            saveStack.AddRange(DeltaStacker.AddStack(subConnection, subConnection.SubScope));
            return saveStack;

        }

        private static List<UpdateItem> networkConnectionChildrenSaveStack(TECNetworkConnection netConnection)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach(TECController controller in netConnection.ChildrenControllers)
            {
                saveStack.AddRange(DeltaStacker.AddStack(netConnection, controller));
            }
            return saveStack;
        }

        private static List<UpdateItem> equipmentChildrenSaveStack(TECEquipment item)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach(TECSubScope subScope in item.SubScope)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, subScope));
                saveStack.AddRange(subscopeChildrenSaveStack(subScope));
            }
            saveStack.AddRange(locatedChildrenSaveStack(item));
            return saveStack;

        }

        private static List<UpdateItem> subscopeChildrenSaveStack(TECSubScope item)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            foreach(TECPoint point in item.Points)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, point));
            }
            foreach (TECDevice device in item.Devices)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, device));
            }
            saveStack.AddRange(locatedChildrenSaveStack(item));
            return saveStack;
        }
        
        private static List<UpdateItem> scopeChildrenSaveStack(TECScope item)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            saveStack.AddRange(labeledSaveStack(item, item.Tags));
            foreach(TECCost cost in item.AssociatedCosts)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, cost));
            }
            return saveStack;
        }

        private static List<UpdateItem> locatedChildrenSaveStack(TECLocated item)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            if(item.Location != null)
            {
                saveStack.AddRange(DeltaStacker.AddStack(item, item.Location));
            }
            saveStack.AddRange(scopeChildrenSaveStack(item));
            return saveStack;
        }

        private static List<UpdateItem> hardwareChildrenSaveStack(TECHardware item)
        {
            List<UpdateItem> saveStack = new List<UpdateItem>();
            saveStack.AddRange(DeltaStacker.AddStack(item, item.Manufacturer));
            saveStack.AddRange(scopeChildrenSaveStack(item));
            return saveStack;
        }
        
    }
}
