using DebugLibrary;
using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Utilities
{
    public class ChangeWatcher
    {
        #region Fields
        private Dictionary<TECObject, OccuranceType> occuranceDictionary;
        #endregion

        #region Constructors
        public ChangeWatcher(TECBid bid)
        {
            initialize(bid);
        }
        public ChangeWatcher(TECTemplates templates)
        {
            throw new NotImplementedException();
        }
        public ChangeWatcher(TECSystem system)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Events
        public event Action<PropertyChangedExtendedEventArgs> BidChanged;
        public event Action<PropertyChangedExtendedEventArgs> InstanceChanged;
        public event Action<List<TECCost>> CostChanged;
        public event Action<int> PointChanged;
        #endregion

        #region Enums
        private enum OccuranceType { None, Typical, Instance };
        #endregion

        #region Methods
        public void Refresh(TECBid bid)
        {
            initialize(bid);
        }

        private void initialize(TECBid bid)
        {
            occuranceDictionary = new Dictionary<TECObject, OccuranceType>();
            registerBidChanges(bid);
        }

        #region Registration
        private void registerBidChanges(TECBid bid)
        {
            bid.PointChanged += (e) => handlePointChanged(bid, e);
            bid.CostChanged += (e) => handleCostChanged(bid, e);
            registerTECObject(bid, OccuranceType.None);
            registerTECObject(bid.Labor, OccuranceType.None);
            registerTECObject(bid.Parameters, OccuranceType.None);

            foreach(TECSystem typical in bid.Systems)
            {
                registerSystem(typical, OccuranceType.Typical);
            }
            foreach(TECController controller in bid.Controllers)
            {
                registerController(controller, OccuranceType.Instance);
            }
            foreach(TECPanel panel in bid.Panels)
            {
                registerTECObject(panel, OccuranceType.Instance);
            }
            foreach(TECMisc misc in bid.MiscCosts)
            {
                registerTECObject(misc, OccuranceType.Instance);
            }
            foreach(TECScopeBranch branch in bid.ScopeTree)
            {
                registerScopeBranch(branch);
            }
            foreach(TECLabeled note in bid.Notes)
            {
                registerTECObject(note, OccuranceType.None);
            }
            foreach(TECLabeled exclusion in bid.Exclusions)
            {
                registerTECObject(exclusion, OccuranceType.None);
            }
            foreach(TECLabeled location in bid.Locations)
            {
                registerTECObject(location, OccuranceType.None);
            }
        }

        private void registerTECObject(TECObject ob, OccuranceType ot)
        {
            occuranceDictionary.Add(ob, ot);
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
            occuranceDictionary.Remove(ob);
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

        private void registerSystem(TECSystem sys, OccuranceType ot)
        {
            registerTECObject(sys, ot);
            foreach(TECSystem instance in sys.SystemInstances)
            {
                registerSystem(instance, OccuranceType.Instance);
            }
            foreach(TECEquipment equip in sys.Equipment)
            {
                registerEquipment(equip, ot);
            }
            foreach(TECController controller in sys.Controllers)
            {
                registerController(controller, ot);
            }
            foreach(TECPanel panel in sys.Panels)
            {
                registerTECObject(panel, ot);
            }
            foreach(TECMisc misc in sys.MiscCosts)
            {
                registerTECObject(misc, ot);
            }
        }
        private void registerEquipment(TECEquipment equip, OccuranceType ot)
        {
            registerTECObject(equip, ot);
            foreach(TECSubScope ss in equip.SubScope)
            {
                registerSubScope(ss, ot);
            }
        }
        private void registerSubScope(TECSubScope ss, OccuranceType ot)
        {
            registerTECObject(ss, ot);
            foreach(TECPoint point in ss.Points)
            {
                registerTECObject(point, ot);
            }
        }
        private void registerController(TECController controller, OccuranceType ot)
        {
            registerTECObject(controller, ot);
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                if (connection is TECNetworkConnection)
                {
                    registerTECObject(connection, ot);
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
            if (isInstance(connection.ParentController) && isInstance(connection.SubScope))
            {
                registerTECObject(connection, OccuranceType.Instance);
            }
            else
            {
                registerTECObject(connection, OccuranceType.Typical);
            }
        }
        private void registerScopeBranch(TECScopeBranch branch)
        {
            registerTECObject(branch, OccuranceType.None);
            foreach(TECScopeBranch subBranch in branch.Branches)
            {
                registerScopeBranch(subBranch);
            }
        }

        private void unregisterSystem(TECSystem sys)
        {
            unregisterTECObject(sys);
            foreach(TECSystem instance in sys.SystemInstances)
            {
                unregisterSystem(instance);
            }
            foreach(TECEquipment equip in sys.Equipment)
            {
                unregisterEquipment(equip);
            }
            foreach(TECController controller in sys.Controllers)
            {
                unregisterController(controller);
            }
            foreach(TECPanel panel in sys.Panels)
            {
                unregisterTECObject(panel);
            }
            foreach(TECMisc misc in sys.MiscCosts)
            {
                unregisterTECObject(misc);
            }
        }
        private void unregisterEquipment(TECEquipment equip)
        {
            unregisterTECObject(equip);
            foreach(TECSubScope ss in equip.SubScope)
            {
                unregisterSubScope(ss);
            }
        }
        private void unregisterSubScope(TECSubScope ss)
        {
            unregisterTECObject(ss);
            foreach(TECPoint point in ss.Points)
            {
                unregisterTECObject(point);
            }
        }
        private void unregisterController(TECController controller)
        {
            unregisterTECObject(controller);
            foreach(TECConnection connection in controller.ChildrenConnections)
            {
                unregisterTECObject(connection);
            }
        }
        private void unregisterScopeBranch(TECScopeBranch branch)
        {
            unregisterTECObject(branch);
            foreach(TECScopeBranch subBranch in branch.Branches)
            {
                unregisterTECObject(subBranch);
            }
        }

        private void registerChange(PropertyChangedExtendedEventArgs args)
        {
            if (args.Value is TECObject value)
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
            OccuranceType parentOT = occuranceDictionary[parent];
            if (child is TECSystem sys)
            {
                if (parent is TECBid)
                {
                    registerSystem(sys, OccuranceType.Typical);
                }
                else if (parent is TECSystem)
                {
                    registerSystem(sys, OccuranceType.Instance);
                }
            }
            else if (child is TECEquipment equip)
            {
                registerEquipment(equip, parentOT);
            }
            else if (child is TECSubScope ss)
            {
                registerSubScope(ss, parentOT);
            }
            else if (child is TECPoint point)
            {
                registerTECObject(point, parentOT);
            }
            else if (child is TECController controller)
            {
                if (parent is TECBid)
                {
                    registerController(controller, OccuranceType.Instance);
                }
                else if (parent is TECSystem)
                {
                    registerController(controller, parentOT);
                }
            }
            else if (child is TECPanel panel)
            {
                registerTECObject(panel, parentOT);
            }
            else if (child is TECConnection connection)
            {
                if (connection is TECNetworkConnection netConnect)
                {
                    registerTECObject(netConnect, parentOT);
                }
                else if (connection is TECSubScopeConnection ssConnect)
                {
                    registerSubScopeConnection(ssConnect);
                }
            }
            else if (child is TECMisc misc)
            {
                registerTECObject(misc, parentOT);
            }
            else if (child is TECLabeled labelled)
            {
                if (labelled is TECScopeBranch branch)
                {
                    registerScopeBranch(branch);
                }
                else if (labelled.Flavor == Flavor.Location)
                {
                    if (parent is TECBid)
                    {
                        registerTECObject(labelled, OccuranceType.None);
                    }
                }
                else
                {
                    registerTECObject(labelled, OccuranceType.None);
                }
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
                if (newChild is TECLabor)
                {
                    unregisterTECObject(oldChild);
                    registerTECObject(newChild, OccuranceType.None);
                }
                else if (newChild is TECBidParameters)
                {
                    unregisterTECObject(oldChild);
                    registerTECObject(newChild, OccuranceType.None);
                }
            }
        }
        #endregion

        #region Event Handlers
        private void handleTECChanged(PropertyChangedExtendedEventArgs obj)
        {
            registerChange(obj);
            BidChanged?.Invoke(obj);
            if (isInstance(obj.Sender))
            {
                InstanceChanged?.Invoke(obj);
            }
        }
        private void handleCostChanged(TECObject sender, List<TECCost> obj)
        {
            if (isInstance(sender))
            {
                CostChanged?.Invoke(obj);
            }
        }
        private void handlePointChanged(TECObject sender, int num)
        {
            if (isInstance(sender))
            {
                PointChanged?.Invoke(num);
            }
        }
        #endregion

        private bool isInstance(TECObject ob)
        {
            if (occuranceDictionary.ContainsKey(ob))
            {
                OccuranceType ot = occuranceDictionary[ob];
                return (ot == OccuranceType.Instance);
            }
            else
            {
                throw new NullReferenceException("Occurance dictionary doesn't contain TECObject.");
            }
        }
        #endregion
    }
}
