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
        bool _isChild;
        public bool IsChild
        {
            get { return _isChild; }
            set
            {
                _isChild = value;
                RaisePropertyChanged("IsChild");
            }
        }

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

        private ObservableCollection<TECSystem> _systemInstances;
        private ObservableCollection<TECController> _controllerInstances;
        private ObservableCollection<TECPanel> _panelInstances;
        public ObservableCollection<TECSystem> SystemInstances
        {
            get { return _systemInstances; }
            set
            {
                _systemInstances = value;
                RaisePropertyChanged("SystemInstances");
            }
        }
        public ObservableCollection<TECController> ControllerInstances
        {
            get { return _controllerInstances; }
            set
            {
                _controllerInstances = value;
                RaisePropertyChanged("ControllerInstances");
            }
        }
        public ObservableCollection<TECPanel> PanelInstances
        {
            get { return _panelInstances; }
            set
            {
                _panelInstances = value;
                RaisePropertyChanged("PanelInstances");
            }
        }

        public TECControlledScope(Guid guid, bool isChild = false) : base(guid)
        {
            _isChild = isChild;
            _systems = new ObservableCollection<TECSystem>();
            _controllers = new ObservableCollection<TECController>();
            _panels = new ObservableCollection<TECPanel>();
            _scopeInstances = new ObservableCollection<TECControlledScope>();
            _systemInstances = new ObservableCollection<TECSystem>();
            _controllerInstances = new ObservableCollection<TECController>();
            _panelInstances = new ObservableCollection<TECPanel>();
            CharactersticInstances = new ObservableItemToInstanceList<TECScope>();
            CharactersticInstances.PropertyChanged += CharactersticInstances_PropertyChanged;
            Systems.CollectionChanged += CollectionChanged;
            Controllers.CollectionChanged += CollectionChanged;
            Panels.CollectionChanged += CollectionChanged;
            ScopeInstances.CollectionChanged += CollectionChanged;
            watcher = new ChangeWatcher(this);
            watcher.Changed += Object_PropertyChanged;
            registerSystems();
        }
        public TECControlledScope(bool isChild = false) : this(Guid.NewGuid(), isChild) { }
        public TECControlledScope(TECControlledScope source, Dictionary<Guid, Guid> guidDictionary = null, bool isChild = false) : this(isChild)
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
                        if (_isChild)
                        {
                            NotifyPropertyChanged("AddRelationship", this, item);
                        }
                        else
                        {
                            NotifyPropertyChanged("Add", this, item);
                            if (item is TECSystem)
                            {
                                (item as TECSystem).PropertyChanged += System_PropertyChanged;
                            } else if (item is TECControlledScope)
                            {
                                handleAddChild(item as TECControlledScope);
                                (item as TECControlledScope).PropertyChanged += TECControlledScope_PropertyChanged;
                            }
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
                        if (_isChild)
                        {
                            NotifyPropertyChanged("RemoveRelationship", this, item);
                        }
                        else
                        {
                            NotifyPropertyChanged("Remove", this, item);
                            if (item is TECSystem)
                            {
                                (item as TECSystem).PropertyChanged -= System_PropertyChanged;
                            }
                            else if (item is TECControlledScope)
                            {
                                handleRemoveChild(item as TECControlledScope);
                                (item as TECControlledScope).PropertyChanged -= TECControlledScope_PropertyChanged;
                            }
                        }
                    }
                }
            }
        }

        private void TECControlledScope_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = e as PropertyChangedExtendedEventArgs<object>;
            if(args != null)
            {
                if(e.PropertyName == "Add")
                {
                    if(args.NewValue is TECSystem)
                    {
                        SystemInstances.Add(args.NewValue as TECSystem);
                    }
                    else if (args.NewValue is TECController)
                    {
                        ControllerInstances.Add(args.NewValue as TECController);
                    }
                    else if (args.NewValue is TECPanel)
                    {
                        PanelInstances.Add(args.NewValue as TECPanel);
                    }
                }
                else if (e.PropertyName == "Remove")
                {
                    if (args.NewValue is TECSystem)
                    {
                        SystemInstances.Remove(args.NewValue as TECSystem);
                    }
                    else if (args.NewValue is TECController)
                    {
                        ControllerInstances.Remove(args.NewValue as TECController);
                    }
                    else if (args.NewValue is TECPanel)
                    {
                        PanelInstances.Remove(args.NewValue as TECPanel);
                    }
                }
            }
        }

        private void handleRemoveChild(TECControlledScope scope)
        {
            foreach(TECSystem system in scope.Systems)
            {
                SystemInstances.Remove(system);
            }
            foreach(TECController controller in scope.Controllers)
            {
                ControllerInstances.Remove(controller);
            }
            foreach(TECPanel panel in scope.Panels)
            {
                PanelInstances.Remove(panel);
            }
        }
        private void handleAddChild(TECControlledScope scope)
        {
            foreach (TECSystem system in scope.Systems)
            {
                SystemInstances.Add(system);
            }
            foreach (TECController controller in scope.Controllers)
            {
                ControllerInstances.Add(controller);
            }
            foreach (TECPanel panel in scope.Panels)
            {
                PanelInstances.Add(panel);
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
                    handleAdd(newValue, oldValue);
                }
                else if (e.PropertyName == "RemoveRelationship")
                {
                    handleRemove(newValue, oldValue);
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
        private void CharactersticInstances_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseExtendedPropertyChanged(sender, e);
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
                        if(subScopeToConnect != null)
                        {
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
            else if (targetObject is TECController && referenceObject is TECPanel)
            {
                var characteristicController = targetObject as TECController;
                var characteristicPanel = referenceObject as TECPanel;
                if (CharactersticInstances.ContainsKey(characteristicPanel) && CharactersticInstances.ContainsKey(characteristicController))
                {
                    foreach (TECControlledScope controlledScope in ScopeInstances)
                    {
                        TECController controllerToConnect = null;
                        foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
                        {
                            if (controlledScope.Controllers.Contains(controller))
                            {
                                controllerToConnect = controller;
                                break;
                            }
                        }
                        if(controllerToConnect != null)
                        {
                            foreach (TECPanel panel in CharactersticInstances.GetInstances(characteristicPanel))
                            {
                                if (controlledScope.Panels.Contains(panel))
                                {
                                    panel.Controllers.Add(controllerToConnect);
                                }
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
                        if(subScopeToRemove != null)
                        {
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
            else if (targetObject is TECController && referenceObject is TECPanel)
            {
                var characteristicController = targetObject as TECController;
                var characteristicPanel = referenceObject as TECPanel;
                if (CharactersticInstances.ContainsKey(characteristicController) && CharactersticInstances.ContainsKey(characteristicPanel))
                {
                    foreach (TECControlledScope controlledScope in ScopeInstances)
                    {
                        TECController controllerToRemove = null;
                        foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
                        {
                            foreach (TECPanel panel in controlledScope.Panels)
                            {
                                if (panel.Controllers.Contains(controller))
                                {
                                    controllerToRemove = controller;
                                    break;
                                }
                            }
                        }
                        if(controllerToRemove != null)
                        {
                            foreach (TECPanel panel in CharactersticInstances.GetInstances(characteristicPanel))
                            {
                                if (controlledScope.Panels.Contains(panel))
                                {
                                    panel.Controllers.Remove(controllerToRemove);
                                    break;
                                }
                            }
                        }
                       
                    }
                }
            }
        }
    }
}
