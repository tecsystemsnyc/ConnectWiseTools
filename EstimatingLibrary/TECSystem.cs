using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECSystem : TECLocated, INotifyPointChanged, IDragDropable
    {
        #region Fields
        private ObservableCollection<TECEquipment> _equipment;
        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<TECPanel> _panels;
        private ObservableCollection<TECMisc> _miscCosts;
        private ObservableCollection<TECScopeBranch> _scopeBranches;

        private bool _proposeEquipment;
        #endregion

        #region Constructors
        public TECSystem(Guid guid) : base(guid)
        {
            _proposeEquipment = false;
            _equipment = new ObservableCollection<TECEquipment>();
            _controllers = new ObservableCollection<TECController>();
            _panels = new ObservableCollection<TECPanel>();
            _miscCosts = new ObservableCollection<TECMisc>();
            _scopeBranches = new ObservableCollection<TECScopeBranch>();

            _equipment.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "Equipment");
            _controllers.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "Controllers");
            _panels.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "Panels");
            _miscCosts.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "MiscCosts");
            _scopeBranches.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "ScopeBranches");
        }

        public TECSystem() : this(Guid.NewGuid()) { }

        public TECSystem(TECSystem source, Dictionary<Guid, Guid> guidDictionary = null,
            ObservableListDictionary<TECObject> characteristicReference = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = source.Guid; }
            foreach (TECEquipment equipment in source.Equipment)
            {
                var toAdd = new TECEquipment(equipment, guidDictionary, characteristicReference);
                if (characteristicReference != null)
                {
                    characteristicReference.AddItem(equipment, toAdd);
                }
                Equipment.Add(toAdd);
            }
            foreach (TECController controller in source._controllers)
            {
                var toAdd = new TECController(controller, guidDictionary);
                if (characteristicReference != null)
                {
                    characteristicReference.AddItem(controller, toAdd);
                }
                _controllers.Add(toAdd);
            }
            foreach (TECPanel panel in source._panels)
            {
                var toAdd = new TECPanel(panel, guidDictionary);
                if (characteristicReference != null)
                {
                    characteristicReference.AddItem(panel, toAdd);
                }
                _panels.Add(toAdd);
            }
            foreach (TECMisc misc in source.MiscCosts)
            {
                var toAdd = new TECMisc(misc);
                _miscCosts.Add(toAdd);
            }
            foreach (TECScopeBranch branch in source._scopeBranches)
            {
                var toAdd = new TECScopeBranch(branch);
                _scopeBranches.Add(toAdd);
            }
            this.copyPropertiesFromLocated(source);
        }
        #endregion

        #region Events
        public event Action<int> PointChanged;
        #endregion

        #region Properties
        public ObservableCollection<TECEquipment> Equipment
        {
            get { return _equipment; }
            set
            {
                var old = _equipment;
                _equipment.CollectionChanged -= (sender, args) => handleCollectionChanged(sender, args, "Equipment");
                _equipment = value;
                NotifyTECChanged(Change.Edit, "Equipment", this, value, old);
                _equipment.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "Equipment");
            }
        }
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                var old = _controllers;
                _controllers.CollectionChanged -= (sender, args) => handleCollectionChanged(sender, args, "Controllers");
                _controllers = value;
                NotifyTECChanged(Change.Edit, "Controllers", this, value, old);
                _controllers.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "Controllers");
            }
        }
        public ObservableCollection<TECPanel> Panels
        {
            get { return _panels; }
            set
            {
                var old = _panels;
                _panels.CollectionChanged -= (sender, args) => handleCollectionChanged(sender, args, "Panels");
                _panels = value;
                NotifyTECChanged(Change.Edit, "Panels", this, value, old);
                _panels.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "Panels");
            }
        }
        public ObservableCollection<TECMisc> MiscCosts
        {
            get { return _miscCosts; }
            set
            {
                var old = _miscCosts;
                _miscCosts.CollectionChanged -= (sender, args) => handleCollectionChanged(sender, args, "MiscCosts");
                _miscCosts = value;
                NotifyTECChanged(Change.Edit, "MiscCosts", this, value, old);
                _miscCosts.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "MiscCosts");
            }
        }
        public ObservableCollection<TECScopeBranch> ScopeBranches
        {
            get { return _scopeBranches; }
            set
            {
                var old = _scopeBranches;
                _scopeBranches.CollectionChanged -= (sender, args) => handleCollectionChanged(sender, args, "ScopeBranches");
                _scopeBranches = value;
                NotifyTECChanged(Change.Edit, "ScopeBranches", this, value, old);
                _scopeBranches.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "ScopeBranches");
            }
        }

        public bool ProposeEquipment
        {
            get { return _proposeEquipment; }
            set
            {
                var old = ProposeEquipment;
                _proposeEquipment = value;
                NotifyTECChanged(Change.Edit, "ProposeEquipment", this, value, old);
            }
        }
        public int PointNumber
        {
            get
            {
                return points();
            }
        }
        #endregion

        #region Methods
        public virtual object DragDropCopy(TECScopeManager scopeManager)
        {
            Dictionary<Guid, Guid> guidDictionary = new Dictionary<Guid, Guid>();
            TECSystem outSystem = new TECSystem(this, guidDictionary);
            ModelLinkingHelper.LinkSystem(outSystem, scopeManager, guidDictionary);
            return outSystem;
        }

        public List<TECSubScope> AllSubScope()
        {
            var outSubScope = new List<TECSubScope>();
            foreach (TECEquipment equip in Equipment)
            {
                foreach (TECSubScope sub in equip.SubScope)
                {
                    outSubScope.Add(sub);
                }
            }
            return outSubScope;
        }
        protected int points()
        {
            var totalPoints = 0;
            foreach (TECEquipment equipment in Equipment)
            {
                totalPoints += equipment.PointNumber;
            }
            return totalPoints;
        }

        protected override CostBatch getCosts()
        {
            CostBatch costs = base.getCosts();
            foreach (TECEquipment item in Equipment)
            {
                costs += item.CostBatch;
            }
            foreach (TECController item in Controllers)
            {
                costs += item.CostBatch;
            }
            foreach (TECPanel item in Panels)
            {
                costs += item.CostBatch;
            }
            foreach (TECMisc item in MiscCosts)
            {
                costs += item.CostBatch;
            }
            return costs;
        }
        protected override SaveableMap saveObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.saveObjects());
            saveList.AddRange(this.Equipment, "Equipment");
            saveList.AddRange(this.Panels, "Panels");
            saveList.AddRange(this.Controllers, "Controllers");
            saveList.AddRange(this.MiscCosts, "MiscCosts");
            return saveList;
        }

        protected void invokePointChanged(int pointNum)
        {
            PointChanged?.Invoke(pointNum);
        }

        #region Event Handlers
        protected virtual void handleCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                CostBatch costs = new CostBatch();
                int pointNum = 0;
                foreach (object item in e.NewItems)
                {
                    if (item != null)
                    {
                        if (item is INotifyCostChanged costItem) { costs += costItem.CostBatch; }
                        if (item is INotifyPointChanged pointItem)
                        {
                            pointNum += pointItem.PointNumber;
                        }
                        NotifyTECChanged(Change.Add, propertyName, this, item);
                        if (item is TECController controller)
                        {
                            controller.IsGlobal = false;
                        }
                    }
                }
                NotifyCostChanged(costs);
                if (pointNum != 0)
                {
                    PointChanged?.Invoke(pointNum);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                CostBatch costs = new CostBatch();
                int pointNum = 0;
                foreach (object item in e.OldItems)
                {
                    if (item != null)
                    {
                        if (item is INotifyCostChanged costItem) { costs += costItem.CostBatch; }
                        if (item is INotifyPointChanged pointItem) { pointNum += pointItem.PointNumber; }
                        NotifyTECChanged(Change.Remove, propertyName, this, item);
                    }
                }
                NotifyCostChanged(costs * -1);
                if (pointNum != 0)
                {
                    PointChanged?.Invoke(pointNum * -1);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyTECChanged(Change.Edit, propertyName, this, sender);
            }
        }
        private void handleSystemSubScopeRemoval(TECSystem system)
        {
            foreach (TECEquipment equipment in system.Equipment)
            {
                handleEquipmentSubScopeRemoval(equipment);
            }
        }
        private void handleEquipmentSubScopeRemoval(TECEquipment equipment)
        {
            foreach (TECSubScope subScope in equipment.SubScope)
            {
                handleSubScopeRemovalInConnections(subScope);
            }
        }
        private void handleSubScopeRemovalInConnections(TECSubScope subScope)
        {
            foreach (TECController controller in Controllers)
            {
                ObservableCollection<TECSubScope> subScopeToRemove = new ObservableCollection<TECSubScope>();
                foreach (TECSubScopeConnection connection in controller.ChildrenConnections)
                {
                    if (connection.SubScope == subScope)
                    {
                        subScopeToRemove.Add(subScope as TECSubScope);
                    }
                }
                foreach (TECSubScope sub in subScopeToRemove)
                {
                    controller.RemoveSubScope(sub);
                }
            }
        }
        #endregion
        #endregion
    }
}
