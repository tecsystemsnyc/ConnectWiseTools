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
    public class TECSystem : TECLocated, INotifyPointChanged, DragDropComponent, INotifyCostChanged
    {
        #region Fields
        private ObservableCollection<TECSystem> _instances;
        private ObservableCollection<TECEquipment> _equipment;
        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<TECPanel> _panels;
        private ObservableCollection<TECMisc> _miscCosts;
        private ObservableCollection<TECScopeBranch> _scopeBranches;
        
        private bool _proposeEquipment;

        private ListDictionary<TECObject> typicalInstanceDictionary;
        #endregion

        #region Constructors
        public TECSystem(Guid guid) : base(guid)
        {
            new ChangeWatcher(this).SystemChanged += handleSystemChanged;

            _proposeEquipment = false;

            _instances = new ObservableCollection<TECSystem>();
            _equipment = new ObservableCollection<TECEquipment>();
            _controllers = new ObservableCollection<TECController>();
            _panels = new ObservableCollection<TECPanel>();
            _miscCosts = new ObservableCollection<TECMisc>();
            _scopeBranches = new ObservableCollection<TECScopeBranch>();

            typicalInstanceDictionary = new ListDictionary<TECObject>();

            _instances.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "SystemInstances");
            _equipment.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "Equipment");
            _controllers.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "Controllers");
            _panels.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "Panels");
            _miscCosts.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "MiscCosts");
            _scopeBranches.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "ScopeBranches");
        }

        public TECSystem() : this(Guid.NewGuid()) { }

        public TECSystem(TECSystem source, Dictionary<Guid, Guid> guidDictionary = null,
            ListDictionary<TECObject> characteristicReference = null) : this()
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
            foreach (TECScopeBranch branch in source._scopeBranches)
            {
                var toAdd = new TECScopeBranch(branch);
                _scopeBranches.Add(toAdd);
            }
            foreach (TECMisc misc in source.MiscCosts)
            {
                var toAdd = new TECMisc(misc);
                _miscCosts.Add(toAdd);
            }
            this.copyPropertiesFromLocated(source);
        }
        #endregion

        #region Events
        public event Action<int> PointChanged;
        #endregion

        #region Properties
        public ObservableCollection<TECSystem> Instances
        {
            get { return _instances; }
            set
            {
                var old = _instances;
                _instances.CollectionChanged -= (sender, args) => handleCollectionChanged(sender, args, "SystemInstances");
                _instances = value;
                NotifyTECChanged(Change.Edit, "Instances", this, value, old);
                _instances.CollectionChanged += (sender, args) => handleCollectionChanged(sender, args, "SystemInstances");
            }
        }
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

        new public List<TECCost> Costs
        {
            get
            {
                return costs();
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
        public TECSystem AddInstance(TECBid bid)
        {
            Dictionary<Guid, Guid> guidDictionary = new Dictionary<Guid, Guid>();
            var newSystem = new TECSystem();
            newSystem.Name = Name;
            newSystem.Description = Description;
            foreach (TECEquipment equipment in Equipment)
            {
                var toAdd = new TECEquipment(equipment, guidDictionary, typicalInstanceDictionary);
                addTypicalInstance(equipment, toAdd);
                newSystem.Equipment.Add(toAdd);
            }
            foreach (TECController controller in Controllers)
            {
                var toAdd = new TECController(controller, guidDictionary);
                addTypicalInstance(controller, toAdd);
                newSystem.Controllers.Add(toAdd);
            }
            foreach (TECPanel panel in Panels)
            {
                var toAdd = new TECPanel(panel, guidDictionary);
                addTypicalInstance(panel, toAdd);
                newSystem.Panels.Add(toAdd);
            }
            foreach (TECCost cost in AssociatedCosts)
            {
                newSystem.AssociatedCosts.Add(cost);
            }
            ModelLinkingHelper.LinkSystem(newSystem, bid, guidDictionary);
            var newSubScope = newSystem.AllSubScope();
            foreach (TECSubScope subScope in AllSubScope())
            {
                var instances = typicalInstanceDictionary.GetInstances(subScope);
                foreach (TECSubScope subInstance in instances)
                {
                    if (newSubScope.Contains(subInstance))
                    {
                        if (subScope.Connection != null && subScope.Connection.ParentController.IsGlobal)
                        {
                            TECSubScopeConnection instanceSSConnect = subScope.Connection.ParentController.AddSubScope(subInstance);
                            instanceSSConnect.Length = subScope.Connection.Length;
                            instanceSSConnect.ConduitLength = subScope.Connection.ConduitLength;
                            instanceSSConnect.ConduitType = subScope.Connection.ConduitType;
                        }
                    }
                }
            }
            Instances.Add(newSystem);
            return (newSystem);
        }

        public object DragDropCopy(TECScopeManager scopeManager)
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
        private int points()
        {
            var totalPoints = 0;
            foreach (TECEquipment equipment in Equipment)
            {
                totalPoints += equipment.PointNumber;
            }
            return totalPoints;
        }

        private List<TECCost> costs()
        {
            var outCosts = new List<TECCost>();
            foreach (TECEquipment item in Equipment)
            {
                outCosts.AddRange(item.Costs);
            }
            foreach (TECController item in Controllers)
            {
                outCosts.AddRange(item.Costs);
            }
            foreach (TECPanel item in Panels)
            {
                outCosts.AddRange(item.Costs);
            }
            outCosts.AddRange(AssociatedCosts);
            foreach (TECMisc item in MiscCosts)
            {
                foreach (TECSystem system in Instances)
                {
                    outCosts.AddRange(item.Costs);
                }
            }
            return outCosts;
        }


        public void SetTypicalInstanceDictionary(ListDictionary<TECObject> newDictionary)
        {
            typicalInstanceDictionary = newDictionary;
        }

        private void addTypicalInstance(TECObject typical, TECObject instance)
        {
            typicalInstanceDictionary.AddItem(typical, instance);
            NotifyTECChanged(Change.Add, "typicalInstanceDictionary", typical, instance);
        }
        private void removeTypicalInstance(TECObject typical, TECObject instance)
        {
            typicalInstanceDictionary.RemoveItem(typical, instance);
            NotifyTECChanged(Change.Remove, "typicalInstanceDictionary", typical, instance);
        }

        #region Event Handlers
        private void handleSystemChanged(TECChangedEventArgs args)
        {
            if (Instances.Count > 0)
            {
                if (args.Change == Change.Add)
                {
                    handleAdd(args.Value as TECObject, args.Sender);
                }
                else if (args.Change == Change.Remove)
                {
                    handleRemove(args.Value as TECObject, args.Sender);
                }
                else if (args.Value is TECPoint && args.OldValue is TECPoint)
                {
                    handlePointChanged(args.Value as TECPoint, args.PropertyName);
                }
            }
            else if (args.PropertyName == "Connection" && args.Sender is TECSubScope)
            {
                handleSubScopeConnectionChanged(args.Sender as TECSubScope);
            }
        }
        private void handleCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item != null)
                    {
                        NotifyTECChanged(Change.Add, propertyName, this, item);
                        if (item is TECController controller)
                        {
                            controller.IsGlobal = false;
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item != null)
                    {
                        NotifyTECChanged(Change.Remove, propertyName, this, item);
                        if (item is TECSystem system)
                        {
                            handleInstanceRemoved(system);
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyTECChanged(Change.Edit, propertyName, this, sender);
            }
        }
        private void handleInstanceRemoved(TECSystem instance)
        {
            foreach (TECSubScope subScope in instance.AllSubScope())
            {
                if (subScope.Connection != null && subScope.Connection.ParentController.IsGlobal)
                {
                    subScope.Connection.ParentController.RemoveSubScope(subScope);
                }
            }
        }
        private void handlePointChanged(TECPoint point, string propertyName)
        {
            PropertyInfo property = typeof(TECPoint).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property != null && property.CanWrite && typicalInstanceDictionary.ContainsKey(point))
            {
                foreach (TECPoint instance in typicalInstanceDictionary.GetInstances(point))
                {
                    property.SetValue(instance, property.GetValue(point), null);
                }
            }
        }
        private void handleSubScopeConnectionChanged(TECSubScope subScope)
        {
            if (typicalInstanceDictionary.ContainsKey(subScope))
            {
                if (subScope.Connection == null)
                {
                    foreach (TECSubScope instance in typicalInstanceDictionary.GetInstances(subScope))
                    {
                        if (instance.Connection == null || !instance.Connection.ParentController.IsGlobal)
                        {
                            break;
                        }
                        instance.Connection.ParentController.RemoveSubScope(instance);
                    }
                }
                else if (subScope.Connection.ParentController.IsGlobal)
                {
                    foreach (TECSubScope instance in typicalInstanceDictionary.GetInstances(subScope))
                    {
                        subScope.Connection.ParentController.AddSubScope(instance);
                    }
                }
            }
        }
        private void handleAdd(TECObject value, TECObject sender)
        {
            if (sender is TECSystem)
            {
                if ((sender as TECSystem).Instances.Count == 0)
                {
                    return;
                }
            }
            if (value is TECController && sender is TECSystem)
            {
                var characteristicController = value as TECController;
                foreach (TECSystem system in Instances)
                {
                    var controllerToAdd = new TECController(characteristicController);
                    controllerToAdd.IsGlobal = false;
                    addTypicalInstance(characteristicController, controllerToAdd);
                    system.Controllers.Add(controllerToAdd);
                }
            }
            else if (value is TECPanel && sender is TECSystem)
            {
                var characteristicPanel = value as TECPanel;
                foreach (TECSystem system in Instances)
                {
                    var panelToAdd = new TECPanel(characteristicPanel);
                    addTypicalInstance(characteristicPanel, panelToAdd);
                    system.Panels.Add(panelToAdd);
                }
            }
            else if (value is TECEquipment && sender is TECSystem)
            {
                var characteristicEquipment = value as TECEquipment;
                foreach (TECSystem system in Instances)
                {
                    var equipmentToAdd = new TECEquipment(characteristicEquipment, characteristicReference: typicalInstanceDictionary);
                    addTypicalInstance(characteristicEquipment, equipmentToAdd);
                    system.Equipment.Add(equipmentToAdd);
                }
            }
            else if (value is TECSubScope && sender is TECEquipment)
            {
                var characteristicEquipment = sender as TECEquipment;
                var characteristicSubScope = value as TECSubScope;
                if (typicalInstanceDictionary.ContainsKey(characteristicEquipment))
                {
                    foreach (TECEquipment equipment in typicalInstanceDictionary.GetInstances(characteristicEquipment))
                    {
                        var subScopeToAdd = new TECSubScope(characteristicSubScope, characteristicReference: typicalInstanceDictionary);
                        addTypicalInstance(characteristicSubScope, subScopeToAdd);
                        equipment.SubScope.Add(subScopeToAdd);
                    }
                }
            }
            else if (value is TECDevice && sender is TECSubScope)
            {
                var characteristicSubScope = sender as TECSubScope;
                var device = value as TECDevice;
                if (typicalInstanceDictionary.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in typicalInstanceDictionary.GetInstances(characteristicSubScope))
                    {
                        subScope.Devices.Add(device);
                    }
                }
            }
            else if (value is TECPoint && sender is TECSubScope)
            {
                var characteristicSubScope = sender as TECSubScope;
                var characteristicPoint = value as TECPoint;
                if (typicalInstanceDictionary.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in typicalInstanceDictionary.GetInstances(characteristicSubScope))
                    {
                        var pointToAdd = new TECPoint(characteristicPoint);
                        addTypicalInstance(characteristicPoint, pointToAdd);
                        subScope.Points.Add(pointToAdd);
                    }
                }
            }
            else if (value is TECSubScopeConnection && sender is TECController)
            {
                var characteristicConnection = value as TECSubScopeConnection;
                var characteristicSubScope = (value as TECSubScopeConnection).SubScope;
                var characteristicController = (sender as TECController);
                if (typicalInstanceDictionary.ContainsKey(characteristicSubScope) && (typicalInstanceDictionary.ContainsKey(characteristicController) || characteristicController.IsGlobal))
                {
                    foreach (TECSystem system in Instances)
                    {
                        TECSubScope subScopeToConnect = null;
                        foreach (TECSubScope subScope in typicalInstanceDictionary.GetInstances(characteristicSubScope))
                        {
                            foreach (TECEquipment equipment in system.Equipment)
                            {
                                if (equipment.SubScope.Contains(subScope))
                                {
                                    subScopeToConnect = subScope;
                                    break;
                                }
                            }
                        }
                        if (subScopeToConnect != null)
                        {
                            if (characteristicController.IsGlobal)
                            {
                                var connection = characteristicController.AddSubScope(subScopeToConnect);
                                connection.Length = characteristicConnection.Length;
                                connection.ConduitLength = characteristicConnection.ConduitLength;
                                connection.ConduitType = characteristicConnection.ConduitType;
                            }
                            else
                            {
                                foreach (TECController controller in typicalInstanceDictionary.GetInstances(characteristicController))
                                {
                                    if (system.Controllers.Contains(controller))
                                    {
                                        var connection = controller.AddSubScope(subScopeToConnect);
                                        connection.Length = characteristicConnection.Length;
                                        connection.ConduitLength = characteristicConnection.ConduitLength;
                                        connection.ConduitType = characteristicConnection.ConduitType;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (value is TECController && sender is TECPanel)
            {
                var characteristicController = value as TECController;
                var characteristicPanel = sender as TECPanel;
                if (typicalInstanceDictionary.ContainsKey(characteristicPanel) && typicalInstanceDictionary.ContainsKey(characteristicController))
                {
                    foreach (TECSystem system in Instances)
                    {
                        TECController controllerToConnect = null;
                        foreach (TECController controller in typicalInstanceDictionary.GetInstances(characteristicController))
                        {
                            if (system.Controllers.Contains(controller))
                            {
                                controllerToConnect = controller;
                                break;
                            }
                        }
                        if (controllerToConnect != null)
                        {
                            foreach (TECPanel panel in typicalInstanceDictionary.GetInstances(characteristicPanel))
                            {
                                if (system.Panels.Contains(panel))
                                {
                                    panel.Controllers.Add(controllerToConnect);
                                }
                            }
                        }

                    }
                }
            }
            else if (value is TECCost && sender is TECScope && !(value is TECMisc)
                && !(value is TECController) && !(value is TECDevice))
            {
                if (sender is TECSystem)
                {
                    foreach (TECSystem system in Instances)
                    {
                        system.AssociatedCosts.Add(value as TECCost);
                    }
                }
                else
                {
                    var characteristicScope = sender as TECScope;
                    var cost = value as TECCost;
                    if (typicalInstanceDictionary.ContainsKey(characteristicScope))
                    {
                        foreach (TECScope scope in typicalInstanceDictionary.GetInstances(characteristicScope))
                        {
                            scope.AssociatedCosts.Add(cost);
                        }
                    }
                }

            }
        }
        private void handleRemove(TECObject value, TECObject sender)
        {
            if (sender is TECSystem)
            {
                if ((sender as TECSystem).Instances.Count == 0)
                {
                    return;
                }
            }
            if (value is TECController && sender is TECSystem)
            {
                var characteristicController = value as TECController;
                foreach (TECSystem system in Instances)
                {
                    var controllersToRemove = new List<TECController>();
                    foreach (TECController controller in system.Controllers)
                    {
                        if (typicalInstanceDictionary.GetInstances(characteristicController).Contains(controller))
                        {
                            controllersToRemove.Add(controller);
                            removeTypicalInstance(characteristicController, controller);
                        }
                    }
                    foreach (TECController controller in controllersToRemove)
                    {
                        system.Controllers.Remove(controller);
                    }
                }
            }
            else if (value is TECPanel && sender is TECSystem)
            {
                var characteristicPanel = value as TECPanel;
                foreach (TECSystem system in Instances)
                {
                    var panelsToRemove = new List<TECPanel>();
                    foreach (TECPanel panel in system.Panels)
                    {
                        if (typicalInstanceDictionary.GetInstances(characteristicPanel).Contains(panel))
                        {
                            panelsToRemove.Add(panel);
                            removeTypicalInstance(characteristicPanel, panel);
                        }
                    }
                    foreach (TECPanel panel in panelsToRemove)
                    {
                        system.Panels.Remove(panel);
                    }
                }
            }
            else if (value is TECEquipment && sender is TECSystem)
            {
                var characteristicEquipment = value as TECEquipment;
                foreach (TECSystem system in Instances)
                {
                    var equipmentToRemove = new List<TECEquipment>();
                    foreach (TECEquipment equipment in system.Equipment)
                    {
                        if (typicalInstanceDictionary.GetInstances(characteristicEquipment).Contains(equipment))
                        {
                            equipmentToRemove.Add(equipment);
                            removeTypicalInstance(characteristicEquipment, equipment);
                        }
                    }
                    foreach (TECEquipment equipment in equipmentToRemove)
                    {
                        system.Equipment.Remove(equipment);
                    }
                }
            }
            else if (value is TECSubScope && sender is TECEquipment)
            {
                var characteristicEquipment = sender as TECEquipment;
                var characteristicSubScope = value as TECSubScope;
                if (typicalInstanceDictionary.ContainsKey(characteristicEquipment))
                {
                    foreach (TECEquipment equipment in typicalInstanceDictionary.GetInstances(characteristicEquipment))
                    {
                        var subScopeToRemove = new List<TECSubScope>();
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {
                            if (typicalInstanceDictionary.GetInstances(characteristicSubScope).Contains(subScope))
                            {
                                subScopeToRemove.Add(subScope);
                                removeTypicalInstance(characteristicSubScope, subScope);

                            }
                        }
                        foreach (TECSubScope subScope in subScopeToRemove)
                        {
                            equipment.SubScope.Remove(subScope);
                        }
                    }
                }
            }
            else if (value is TECDevice && sender is TECSubScope)
            {
                var characteristicSubScope = sender as TECSubScope;
                var device = value as TECDevice;
                if (typicalInstanceDictionary.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in typicalInstanceDictionary.GetInstances(characteristicSubScope))
                    {
                        subScope.Devices.Remove(device);
                    }
                }
            }
            else if (value is TECPoint && sender is TECSubScope)
            {
                var characteristicSubScope = sender as TECSubScope;
                var characteristicPoint = value as TECPoint;
                if (typicalInstanceDictionary.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in typicalInstanceDictionary.GetInstances(characteristicSubScope))
                    {
                        var pointsToRemove = new List<TECPoint>();
                        foreach (TECPoint point in subScope.Points)
                        {
                            if (typicalInstanceDictionary.GetInstances(characteristicPoint).Contains(point))
                            {
                                pointsToRemove.Add(point);
                                removeTypicalInstance(characteristicPoint, point);
                            }
                        }
                        foreach (TECPoint point in pointsToRemove)
                        {
                            subScope.Points.Remove(point);
                        }
                    }
                }
            }
            else if (value is TECSubScopeConnection && sender is TECController)
            {
                var characteristicConnection = value as TECSubScopeConnection;
                var characteristicSubScope = (value as TECSubScopeConnection).SubScope;
                var characteristicController = (sender as TECController);
                if (typicalInstanceDictionary.ContainsKey(characteristicSubScope) && (typicalInstanceDictionary.ContainsKey(characteristicController) || characteristicController.IsGlobal))
                {
                    foreach (TECSystem system in Instances)
                    {
                        TECSubScope subScopeToRemove = null;
                        foreach (TECSubScope subScope in typicalInstanceDictionary.GetInstances(characteristicSubScope))
                        {
                            foreach (TECEquipment equipment in system.Equipment)
                            {
                                if (equipment.SubScope.Contains(subScope))
                                {
                                    subScopeToRemove = subScope;
                                    break;
                                }
                            }
                        }
                        if (subScopeToRemove != null)
                        {
                            if (characteristicController.IsGlobal)
                            {
                                characteristicController.RemoveSubScope(subScopeToRemove);
                            }
                            else
                            {
                                foreach (TECController controller in typicalInstanceDictionary.GetInstances(characteristicController))
                                {
                                    if (system.Controllers.Contains(controller))
                                    {
                                        controller.RemoveSubScope(subScopeToRemove);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (value is TECController && sender is TECPanel)
            {
                var characteristicController = value as TECController;
                var characteristicPanel = sender as TECPanel;
                if (typicalInstanceDictionary.ContainsKey(characteristicController) && typicalInstanceDictionary.ContainsKey(characteristicPanel))
                {
                    foreach (TECSystem system in Instances)
                    {
                        TECController controllerToRemove = null;
                        foreach (TECController controller in typicalInstanceDictionary.GetInstances(characteristicController))
                        {
                            foreach (TECPanel panel in system.Panels)
                            {
                                if (panel.Controllers.Contains(controller))
                                {
                                    controllerToRemove = controller;
                                    break;
                                }
                            }
                        }
                        if (controllerToRemove != null)
                        {
                            foreach (TECPanel panel in typicalInstanceDictionary.GetInstances(characteristicPanel))
                            {
                                if (system.Panels.Contains(panel))
                                {
                                    panel.Controllers.Remove(controllerToRemove);
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            else if (value is TECCost && sender is TECScope && !(value is TECMisc)
                && !(value is TECController) && !(value is TECDevice))
            {
                if (sender is TECSystem)
                {
                    foreach (TECSystem system in Instances)
                    {
                        system.AssociatedCosts.Remove(value as TECCost);
                    }
                }
                else
                {
                    var characteristicScope = sender as TECScope;
                    var cost = value as TECCost;
                    if (typicalInstanceDictionary.ContainsKey(characteristicScope))
                    {
                        foreach (TECScope scope in typicalInstanceDictionary.GetInstances(characteristicScope))
                        {
                            scope.AssociatedCosts.Remove(cost);
                        }
                    }
                }
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
