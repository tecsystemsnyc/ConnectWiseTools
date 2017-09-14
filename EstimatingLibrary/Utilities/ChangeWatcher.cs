using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;

namespace EstimatingLibrary.Utilities
{
    public class ChangeWatcher
    {
        #region Fields
        private List<TECObject> typicalList;
        private List<TECObject> toRemove;
        #endregion

        #region Constructors
        public ChangeWatcher(TECBid bid)
        {
            initialize(bid);
        }
        public ChangeWatcher(TECTemplates templates)
        {
            initialize(templates);
        }
        public ChangeWatcher(TECTypical typical)
        {
            initialize(typical);
        }
        #endregion

        #region Events
        public event Action<TECChangedEventArgs> Changed;
        public event Action<TECChangedEventArgs> InstanceChanged;
        public event Action<CostBatch> CostChanged;
        public event Action<int> PointChanged;
        #endregion

        #region Methods
        public void Refresh(TECBid bid)
        {
            initialize(bid);
        }

        private void initialize(TECBid bid)
        {
            typicalList = new List<TECObject>();
            toRemove = new List<TECObject>();
            registerBidChanges(bid);
        }
        private void initialize(TECTemplates templates)
        {
            typicalList = new List<TECObject>();
            toRemove = new List<TECObject>();
            registerTemplateChanges(templates);
        }
        private void initialize(TECTypical typical)
        {
            typicalList = new List<TECObject>();
            toRemove = new List<TECObject>();
            registerSystem(typical);
        }

        #region Registration
        private void registerBidChanges(TECBid bid)
        {
            registerTECObject(bid);
            registerTECObject(bid.ExtraLabor);
            registerTECObject(bid.Parameters);

            foreach (TECTypical typical in bid.Systems)
            {
                registerTypical(typical);
            }
            foreach (TECController controller in bid.Controllers)
            {
                registerController(controller, false);
            }
            foreach (TECPanel panel in bid.Panels)
            {
                registerTECObject(panel);
            }
            foreach (TECMisc misc in bid.MiscCosts)
            {
                registerTECObject(misc);
            }
            foreach (TECScopeBranch branch in bid.ScopeTree)
            {
                registerScopeBranch(branch, false);
            }
            foreach (TECLabeled note in bid.Notes)
            {
                registerTECObject(note);
            }
            foreach (TECLabeled exclusion in bid.Exclusions)
            {
                registerTECObject(exclusion);
            }
            foreach (TECLabeled location in bid.Locations)
            {
                registerTECObject(location);
            }
        }
        private void registerTemplateChanges(TECTemplates templates)
        {
            registerTECObject(templates);
            registerCatalogs(templates.Catalogs);
            foreach (TECSystem system in templates.SystemTemplates)
            {
                registerSystem(system);
            }
            foreach (TECEquipment equipment in templates.EquipmentTemplates)
            {
                registerEquipment(equipment, false);
            }
            foreach (TECSubScope subScope in templates.SubScopeTemplates)
            {
                registerSubScope(subScope, false);
            }
            foreach (TECController controller in templates.ControllerTemplates)
            {
                registerController(controller, false);
            }
            foreach (TECPanel panel in templates.PanelTemplates)
            {
                registerTECObject(panel);
            }
            foreach (TECMisc misc in templates.MiscCostTemplates)
            {
                registerTECObject(misc);
            }
            foreach (TECParameters parameter in templates.Parameters)
            {
                registerTECObject(parameter);
            }
        }

        private void registerTECObject(TECObject ob, bool isTypical = false)
        {
            if (isTypical)
            {
                typicalList.Add(ob);
            }
            ob.TECChanged += handleTECChanged;
            if (ob is INotifyCostChanged costOb)
            {
                costOb.CostChanged += (e) => handleCostChanged(ob, e);
            }
            if (ob is INotifyPointChanged pointOb)
            {
                pointOb.PointChanged += (e) => handlePointChanged(ob, e);
            }
        }
        private void unregisterTECObject(TECObject ob)
        {
            toRemove.Add(ob);
            ob.TECChanged -= handleTECChanged;
            if (ob is INotifyCostChanged costOb)
            {
                costOb.CostChanged -= (e) => handleCostChanged(ob, e);
            }
            if (ob is INotifyPointChanged pointOb)
            {
                pointOb.PointChanged -= (e) => handlePointChanged(ob, e);
            }
        }

        private void registerCatalogs(TECCatalogs catalogs)
        {
            registerTECObject(catalogs);
            foreach (TECDevice item in catalogs.Devices)
            {
                registerTECObject(item);
            }
            foreach (TECElectricalMaterial item in catalogs.ConnectionTypes)
            {
                registerTECObject(item);
            }
            foreach (TECElectricalMaterial item in catalogs.ConduitTypes)
            {
                registerTECObject(item);
            }
            foreach (TECControllerType item in catalogs.ControllerTypes)
            {
                registerTECObject(item);
            }
            foreach (TECIOModule item in catalogs.IOModules)
            {
                registerTECObject(item);
            }
            foreach (TECPanelType item in catalogs.PanelTypes)
            {
                registerTECObject(item);
            }
            foreach (TECManufacturer item in catalogs.Manufacturers)
            {
                registerTECObject(item);
            }
            foreach (TECLabeled item in catalogs.Tags)
            {
                registerTECObject(item);
            }
            foreach (TECCost item in catalogs.AssociatedCosts)
            {
                registerTECObject(item);
            }
            foreach(TECValve item in catalogs.Valves)
            {
                registerTECObject(item);
            }
        }

        private void registerTypical(TECTypical typ)
        {
            registerSystem(typ);
            foreach (TECSystem instance in typ.Instances)
            {
                registerSystem(instance);
            }
        }
        private void registerSystem(TECSystem sys)
        {
            bool isTypical = (sys is TECTypical);
            registerTECObject(sys, isTypical);
            foreach (TECEquipment equip in sys.Equipment)
            {
                registerEquipment(equip, isTypical);
            }
            foreach (TECController controller in sys.Controllers)
            {
                registerController(controller, isTypical);
            }
            foreach (TECPanel panel in sys.Panels)
            {
                registerTECObject(panel, isTypical);
            }
            foreach (TECMisc misc in sys.MiscCosts)
            {
                registerTECObject(misc, isTypical);
            }
            foreach (TECScopeBranch branch in sys.ScopeBranches)
            {
                registerScopeBranch(branch, isTypical);
            }
        }
        private void registerEquipment(TECEquipment equip, bool isTypical)
        {
            registerTECObject(equip, isTypical);
            foreach (TECSubScope ss in equip.SubScope)
            {
                registerSubScope(ss, isTypical);
            }
        }
        private void registerSubScope(TECSubScope ss, bool isTypical)
        {
            registerTECObject(ss, isTypical);
            foreach (TECPoint point in ss.Points)
            {
                registerTECObject(point, isTypical);
            }
        }
        private void registerController(TECController controller, bool isTypical)
        {
            registerTECObject(controller, isTypical);
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                if (connection is TECNetworkConnection)
                {
                    registerTECObject(connection, isTypical);
                }
                else if (connection is TECSubScopeConnection ssConnect)
                {
                    registerSubScopeConnection(ssConnect);
                }
                else
                {
                    throw new InvalidCastException("Connection type not recognized.");
                }
            }
        }
        private void registerSubScopeConnection(TECSubScopeConnection connection)
        {
            bool connectionIsTypical = isTypical(connection.ParentController);
            registerTECObject(connection, connectionIsTypical);
        }
        private void registerScopeBranch(TECScopeBranch branch, bool isTypical)
        {
            registerTECObject(branch, isTypical);
            foreach (TECScopeBranch subBranch in branch.Branches)
            {
                registerScopeBranch(subBranch, isTypical);
            }
        }

        private void unregisterTypical(TECTypical typ)
        {
            unregisterSystem(typ);
            foreach (TECSystem instance in typ.Instances)
            {
                unregisterSystem(instance);
            }
        }
        private void unregisterSystem(TECSystem sys)
        {
            unregisterTECObject(sys);
            foreach (TECEquipment equip in sys.Equipment)
            {
                unregisterEquipment(equip);
            }
            foreach (TECController controller in sys.Controllers)
            {
                unregisterController(controller);
            }
            foreach (TECPanel panel in sys.Panels)
            {
                unregisterTECObject(panel);
            }
            foreach (TECMisc misc in sys.MiscCosts)
            {
                unregisterTECObject(misc);
            }
            foreach (TECScopeBranch branch in sys.ScopeBranches)
            {
                unregisterScopeBranch(branch);
            }
        }
        private void unregisterEquipment(TECEquipment equip)
        {
            unregisterTECObject(equip);
            foreach (TECSubScope ss in equip.SubScope)
            {
                unregisterSubScope(ss);
            }
        }
        private void unregisterSubScope(TECSubScope ss)
        {
            unregisterTECObject(ss);
            foreach (TECPoint point in ss.Points)
            {
                unregisterTECObject(point);
            }
        }
        private void unregisterController(TECController controller)
        {
            unregisterTECObject(controller);
            foreach (TECConnection connection in controller.ChildrenConnections)
            {
                unregisterTECObject(connection);
            }
        }
        private void unregisterScopeBranch(TECScopeBranch branch)
        {
            unregisterTECObject(branch);
            foreach (TECScopeBranch subBranch in branch.Branches)
            {
                unregisterTECObject(subBranch);
            }
        }

        private void registerChange(TECChangedEventArgs args)
        {
            if (args.Value is TECObject value && args.PropertyName != "TypicalInstanceDictionary")
            {
                if (args.Change == Change.Add)
                {
                    registerAdd(args.Sender, value);
                }
                else if (args.Change == Change.Remove)
                {
                    registerRemove(value);
                }
                else if (args.Change == Change.Edit && args.OldValue is TECObject old)
                {
                    registerEdit(args.Sender, value, old);
                }
            }
        }
        private void registerAdd(TECObject parent, TECObject child)
        {
            if (child is TECTypical typ)
            {
                registerTypical(typ);
            }
            else if (child is TECSystem sys)
            {
                registerSystem(sys);
            }
            else if (child is TECEquipment equip)
            {
                registerEquipment(equip, isTypical(parent));
            }
            else if (child is TECSubScope ss)
            {
                registerSubScope(ss, isTypical(parent));
            }
            else if (child is TECPoint point)
            {
                registerTECObject(point, isTypical(parent));
            }
            else if (child is TECController controller)
            {
                if (parent is TECBid)
                {
                    registerController(controller, false);
                }
                else if (parent is TECSystem)
                {
                    registerController(controller, isTypical(parent));
                }
            }
            else if (child is TECPanel panel)
            {
                registerTECObject(panel, isTypical(parent));
            }
            else if (child is TECConnection connection && parent is TECController)
            {
                if (connection is TECNetworkConnection netConnect)
                {
                    registerTECObject(netConnect);
                }
                else if (connection is TECSubScopeConnection ssConnect)
                {
                    registerSubScopeConnection(ssConnect);
                }
            }
            else if (child is TECMisc misc)
            {
                registerTECObject(misc, isTypical(parent));
            }
            else if (child is TECLabeled labelled)
            {
                if (labelled is TECScopeBranch branch)
                {
                    registerScopeBranch(branch, parent is TECTypical);
                }
                else if (!(parent is TECScope))
                {
                    registerTECObject(labelled);
                }
            }
            else if (child is TECDevice)
            {
                return;
            }
            else if (parent is TECCatalogs)
            {
                registerTECObject(child);
            }
            else if (parent is TECScope && child is TECCost)
            {
                return;
            }
            else if (child is TECParameters)
            {
                registerTECObject(child);
            }
            else
            {
                throw new InvalidOperationException("TECObject type not recognized.");
            }
        }
        private void registerRemove(TECObject oldchild)
        {
            if (oldchild is TECSystem sys)
            {
                unregisterSystem(sys);
            }
            else if (oldchild is TECEquipment equip)
            {
                unregisterEquipment(equip);
            }
            else if (oldchild is TECSubScope ss)
            {
                unregisterSubScope(ss);
            }
            else if (oldchild is TECController controller)
            {
                unregisterController(controller);
            }
            else if (oldchild is TECScopeBranch branch)
            {
                unregisterScopeBranch(branch);
            }
            else
            {
                unregisterTECObject(oldchild);
            }
        }
        private void registerEdit(TECObject parent, TECObject newChild, TECObject oldChild)
        {
            if (parent is TECBid)
            {
                if (newChild is TECExtraLabor)
                {
                    unregisterTECObject(oldChild);
                    registerTECObject(newChild);
                }
                else if (newChild is TECParameters)
                {
                    unregisterTECObject(oldChild);
                    registerTECObject(newChild);
                }
            }
        }
        #endregion

        #region Event Handlers
        private void handleTECChanged(TECChangedEventArgs obj)
        {
            registerChange(obj);
            Changed?.Invoke(obj);
            if (obj.Value is TECSubScopeConnection ssConnect)
            {
                if (ssConnectIsInstance(ssConnect))
                {
                    InstanceChanged?.Invoke(obj);
                }
            }
            else if (obj.PropertyName != "TypicalInstanceDictionary")
            {
                if (!(obj.Value is TECObject value))
                {
                    if (!(isTypical(obj.Sender)))
                    {
                        InstanceChanged?.Invoke(obj);
                    }
                }
                //If the sender is TECBid or TECTypical, check isTypical on value instead of sender.
                else if (obj.Sender is TECBid)
                {
                    if (!isTypical(value))
                    {
                        InstanceChanged?.Invoke(obj);
                    }
                }
                //If the sender is TECTypical, check that the value isn't a "catalog" object.
                else if (obj.Sender is TECTypical typ)
                {
                    string propName = obj.PropertyName;
                    SaveableMap map = typ.RelatedObjects;
                    if (!typ.RelatedObjects.Contains(propName) && !(isTypical(value)))
                    {
                        InstanceChanged?.Invoke(obj);
                    }
                }
                else if (!isTypical(obj.Sender))
                {
                    InstanceChanged?.Invoke(obj);
                }
            }

            foreach(TECObject ob in toRemove)
            {
                typicalList.Remove(ob);
            }
            toRemove = new List<TECObject>();
        }
        private void handleCostChanged(TECObject sender, CostBatch obj)
        {
            if ((sender is TECTypical) || !isTypical(sender))
            {
                CostChanged?.Invoke(obj);
            }
        }
        private void handlePointChanged(TECObject sender, int num)
        {
            if ((sender is TECTypical) || !isTypical(sender))
            {
                PointChanged?.Invoke(num);
            }
        }
        #endregion

        private bool isTypical(TECObject obj)
        {
            return typicalList.Contains(obj);
        }
        private bool ssConnectIsInstance(TECSubScopeConnection ssConnect)
        {
            return (!isTypical(ssConnect.ParentController) && !isTypical(ssConnect.SubScope));
        }
        #endregion
    }
}