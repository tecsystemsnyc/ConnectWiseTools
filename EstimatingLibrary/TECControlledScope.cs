using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECControlledScope : TECScope
    {
        private ObservableCollection<TECSystem> _systems { get; set; }
        public ObservableCollection<TECSystem> Systems
        {
            get { return _systems; }
            set
            {
                var temp = this.Copy();
                if (Systems != null)
                {
                    Systems.CollectionChanged -= CollectionChanged;
                }
                _systems = value;
                registerSystems();
                Systems.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Systems", temp, this);
            }
        }
        
        private ObservableCollection<TECController> _controllers { get; set; }
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                var temp = this.Copy();
                if (Controllers != null)
                {
                    Controllers.CollectionChanged -= CollectionChanged;
                }
                _controllers = value;
                Controllers.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Controllers", temp, this);
            }
        }
        
        private ObservableCollection<TECPanel> _panels { get; set; }
        public ObservableCollection<TECPanel> Panels
        {
            get { return _panels; }
            set
            {
                var temp = this.Copy();
                if (Panels != null)
                {
                    Panels.CollectionChanged -= CollectionChanged;
                }

                _panels = value;
                Panels.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Panels", temp, this);
            }
        }

        private ObservableCollection<TECControlledScope> _scopeInstances;
        public ObservableCollection<TECControlledScope> ScopeInstances
        {
            get { return _scopeInstances; }
            set
            {
                var temp = this.Copy();
                if (Panels != null)
                {
                    ScopeInstances.CollectionChanged -= CollectionChanged;
                }
                _scopeInstances = value;
                ScopeInstances.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ScopeInstances", temp, this);
            }
        }

        public ObservableItemToInstanceList<TECScope> CharactersticInstances;
        private ChangeWatcher watcher;

        #region Derived
        public ObservableCollection<TECSystem> SystemInstances
        {
            get { return getSystemInstances(); }
        }
        public ObservableCollection<TECController> ControllerInstances
        {
            get { return getControllerInstances(); }
        }
        public ObservableCollection<TECPanel> PanelInstances
        {
            get { return getPanelInstances(); }
        }
        #endregion

        public TECControlledScope(Guid guid) : base(guid)
        {
            _systems = new ObservableCollection<TECSystem>();
            _controllers = new ObservableCollection<TECController>();
            _panels = new ObservableCollection<TECPanel>();
            _scopeInstances = new ObservableCollection<TECControlledScope>();
            CharactersticInstances = new ObservableItemToInstanceList<TECScope>();
            Systems.CollectionChanged += CollectionChanged;
            Controllers.CollectionChanged += CollectionChanged;
            Panels.CollectionChanged += CollectionChanged;
            ScopeInstances.CollectionChanged += CollectionChanged;
            watcher = new ChangeWatcher(this);
            watcher.Changed += Object_PropertyChanged;
            registerSystems();
        }
        public TECControlledScope() : this(Guid.NewGuid()) { }
        public TECControlledScope(TECControlledScope source, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            copyPropertiesFromScope(source);
            foreach (TECSystem system in source._systems)
            {
                _systems.Add(new TECSystem(system, guidDictionary));
            }
            foreach (TECController controller in source._controllers)
            {
                _controllers.Add(new TECController(controller, guidDictionary));
            }
            foreach (TECPanel panel in source._panels)
            {
                _panels.Add(new TECPanel(panel, guidDictionary));
            }
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item != null)
                    {
                        NotifyPropertyChanged("Add", this, item);
                        if (item is TECSystem)
                        {
                            (item as TECSystem).PropertyChanged += System_PropertyChanged;
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
                        NotifyPropertyChanged("Remove", this, item);
                        if (item is TECSystem)
                        {
                            (item as TECSystem).PropertyChanged -= System_PropertyChanged;
                        }
                    }
                }
            }
        }

        private void registerSystems()
        {
            foreach (TECSystem system in Systems)
            {
                system.PropertyChanged += System_PropertyChanged;
            }
        }
        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemovedSubScope")
            {
                var args = e as PropertyChangedExtendedEventArgs<object>;
                if (args.NewValue is TECEquipment)
                {
                    handleEquipmentSubScopeRemoval(args.NewValue as TECEquipment);
                }
                else
                {
                    handleSubScopeRemovalInConnections(args.NewValue as TECSubScope);
                }
            }
        }

        public override object Copy()
        {
            var outScope = new TECControlledScope(_guid);
            outScope.copyPropertiesFromScope(this);
            foreach (TECController controller in Controllers)
            {
                outScope.Controllers.Add(controller.Copy() as TECController);
            }
            foreach (TECPanel panel in Panels)
            {
                outScope.Panels.Add(panel.Copy() as TECPanel);
            }
            foreach (TECSystem system in Systems)
            {
                outScope.Systems.Add(system.Copy() as TECSystem);
            }
            return outScope;
        }

        public override object DragDropCopy()
        {
            var outScope = new TECControlledScope(this);
            return outScope;
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

        private ObservableCollection<TECSystem> getSystemInstances()
        {
            ObservableCollection<TECSystem> systems = new ObservableCollection<TECSystem>();
            foreach(TECControlledScope scope in ScopeInstances)
            {
                foreach(TECSystem system in scope.Systems)
                {
                    systems.Add(system);
                }
            }
            return systems;
        }
        private ObservableCollection<TECController> getControllerInstances()
        {
            ObservableCollection<TECController> controllers = new ObservableCollection<TECController>();
            foreach (TECControlledScope scope in ScopeInstances)
            {
                foreach (TECController controller in scope.Controllers)
                {
                    controllers.Add(controller);
                }
            }
            return controllers;
        }
        private ObservableCollection<TECPanel> getPanelInstances()
        {
            ObservableCollection<TECPanel> panels = new ObservableCollection<TECPanel>();
            foreach (TECControlledScope scope in ScopeInstances)
            {
                foreach (TECPanel panel in scope.Panels)
                {
                    panels.Add(panel);
                }
            }
            return panels;
        }

        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object> && ScopeInstances.Count > 0)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                object oldValue = args.OldValue;
                object newValue = args.NewValue;
                if (e.PropertyName == "Add")
                {
                    handleAdd(newValue, oldValue);
                }
                else if (e.PropertyName == "Remove")
                {
                    handleRemove(newValue, oldValue);
                }
                else if (e.PropertyName == "Edit")
                {
                }
                else if (e.PropertyName == "ChildChanged")
                {
                    
                }
                else if (e.PropertyName == "ObjectPropertyChanged")
                {
                    
                }
                else if (e.PropertyName == "RelationshipPropertyChanged")
                {
                    
                }
                else if (e.PropertyName == "MetaAdd")
                {
                    
                }
                else if (e.PropertyName == "MetaRemove")
                {
                    
                }
                else if (e.PropertyName == "AddRelationship")
                {
                    
                }
                else if (e.PropertyName == "RemoveRelationship")
                {
                    
                }
                else if (e.PropertyName == "RemovedSubScope") { }
                else if (e.PropertyName == "AddCatalog")
                {
                    
                }
                else if (e.PropertyName == "RemoveCatalog")
                {
                    
                }
                else if (e.PropertyName == "Catalogs")
                {
                    
                }
                else
                {
                    
                }

            }
            else
            {
            }
        }
        
        private void handleAdd(object targetObject, object referenceObject)
        {
            if(targetObject is TECEquipment && referenceObject is TECSystem)
            {
                var characteristicEquipment = targetObject as TECEquipment;
                var characteristicSystem = referenceObject as TECSystem;
                if (CharactersticInstances.ContainsKey(characteristicSystem))
                {
                    foreach (TECSystem system in CharactersticInstances.GetInstances(characteristicSystem))
                    {
                        var equipmentToAdd = new TECEquipment(characteristicEquipment, characteristicReference: CharactersticInstances);
                        CharactersticInstances.AddItem(characteristicEquipment, equipmentToAdd);
                        system.Equipment.Add(equipmentToAdd);
                    }
                }
            }
            else if (targetObject is TECSubScope && referenceObject is TECEquipment)
            {
                var characteristicEquipment = referenceObject as TECEquipment;
                var characteristicSubScope = targetObject as TECSubScope;
                if (CharactersticInstances.ContainsKey(characteristicEquipment))
                {
                    foreach (TECEquipment equipment in CharactersticInstances.GetInstances(characteristicEquipment))
                    {
                        var subScopeToAdd = new TECSubScope(characteristicSubScope, characteristicReference: CharactersticInstances);
                        CharactersticInstances.AddItem(characteristicSubScope, subScopeToAdd);
                        equipment.SubScope.Add(subScopeToAdd);
                    }
                }
            }
            else if (targetObject is TECDevice && referenceObject is TECSubScope)
            {
                var characteristicSubScope = referenceObject as TECSubScope;
                var device = targetObject as TECDevice;
                if (CharactersticInstances.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                    {
                        subScope.Devices.Add(device);
                    }
                }
            }
            else if (targetObject is TECPoint && referenceObject is TECSubScope)
            {
                var characteristicSubScope = referenceObject as TECSubScope;
                var characteristicPoint = targetObject as TECPoint;
                if (CharactersticInstances.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                    {
                        var pointToAdd = new TECPoint(characteristicPoint);
                        CharactersticInstances.AddItem(characteristicPoint, pointToAdd);
                        subScope.Points.Add(pointToAdd);
                    }
                }
            }
            else if(targetObject is TECSubScopeConnection && referenceObject is TECController)
            {
                var characteristicConnection = targetObject as TECSubScopeConnection;
                var characteristicSubScope = (targetObject as TECSubScopeConnection).SubScope;
                var characteristicController = (referenceObject as TECController);
                if (CharactersticInstances.ContainsKey(characteristicSubScope) && CharactersticInstances.ContainsKey(characteristicController))
                {
                    foreach (TECControlledScope controlledScope in ScopeInstances)
                    {
                        TECSubScope subScopeToConnect = null;
                        foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                        {
                            foreach (TECSystem system in controlledScope.Systems)
                            {
                                if (system.SubScope.Contains(subScope))
                                {
                                    subScopeToConnect = subScope;
                                    break;
                                }
                            }
                        }
                        foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
                        {
                            if (controlledScope.Controllers.Contains(controller))
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

        private void handleRemove(object targetObject, object referenceObject)
        {
            if (targetObject is TECEquipment && referenceObject is TECSystem)
            {
                var characteristicEquipment = targetObject as TECEquipment;
                var characteristicSystem = referenceObject as TECSystem;
                if (CharactersticInstances.ContainsKey(characteristicSystem))
                {
                    foreach (TECSystem system in CharactersticInstances.GetInstances(characteristicSystem))
                    {
                        var equipmentToRemove = new List<TECEquipment>();
                        foreach (TECEquipment equipment in system.Equipment)
                        {
                            if (CharactersticInstances.GetInstances(characteristicEquipment).Contains(equipment))
                            {
                                equipmentToRemove.Add(equipment);
                            }
                        }
                        foreach (TECEquipment equipment in equipmentToRemove)
                        {
                            system.Equipment.Remove(equipment);
                        }
                    }
                }
            }
            else if (targetObject is TECSubScope && referenceObject is TECEquipment)
            {
                var characteristicEquipment = referenceObject as TECEquipment;
                var characteristicSubScope = targetObject as TECSubScope;
                if (CharactersticInstances.ContainsKey(characteristicEquipment))
                {
                    foreach (TECEquipment equipment in CharactersticInstances.GetInstances(characteristicEquipment))
                    {
                        var subScopeToRemove = new List<TECSubScope>();
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {
                            if (CharactersticInstances.GetInstances(characteristicSubScope).Contains(subScope))
                            {
                                subScopeToRemove.Add(subScope);
                            }
                        }
                        foreach (TECSubScope subScope in subScopeToRemove)
                        {
                            equipment.SubScope.Remove(subScope);
                        }
                    }
                }
            }
            else if (targetObject is TECDevice && referenceObject is TECSubScope)
            {
                var characteristicSubScope = referenceObject as TECSubScope;
                var device = targetObject as TECDevice;
                if (CharactersticInstances.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                    {
                        subScope.Devices.Remove(device);
                    }
                }
            }
            else if (targetObject is TECPoint && referenceObject is TECSubScope)
            {
                var characteristicSubScope = referenceObject as TECSubScope;
                var characteristicPoint = targetObject as TECPoint;
                if (CharactersticInstances.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                    {
                        var pointsToRemove = new List<TECPoint>();
                        foreach (TECPoint point in subScope.Points)
                        {
                            if (CharactersticInstances.GetInstances(characteristicPoint).Contains(point))
                            {
                                pointsToRemove.Add(point);
                            }
                        }
                        foreach (TECPoint point in pointsToRemove)
                        {
                            subScope.Points.Remove(point);
                        }
                    }
                }
            }
            else if (targetObject is TECSubScopeConnection && referenceObject is TECController)
            {
                var characteristicConnection = targetObject as TECSubScopeConnection;
                var characteristicSubScope = (targetObject as TECSubScopeConnection).SubScope;
                var characteristicController = (referenceObject as TECController);
                if (CharactersticInstances.ContainsKey(characteristicSubScope) && CharactersticInstances.ContainsKey(characteristicController))
                {
                    foreach (TECControlledScope controlledScope in ScopeInstances)
                    {
                        TECSubScope subScopeToRemove = null;
                        foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                        {
                            foreach (TECSystem system in controlledScope.Systems)
                            {
                                if (system.SubScope.Contains(subScope))
                                {
                                    subScopeToRemove = subScope;
                                    break;
                                }
                            }
                        }
                        foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
                        {
                            if (controlledScope.Controllers.Contains(controller))
                            {
                                controller.RemoveSubScope(subScopeToRemove);
                            }
                        }
                    }
                }
            }
        }
    }
}
