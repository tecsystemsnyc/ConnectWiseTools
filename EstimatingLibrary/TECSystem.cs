using EstimatingLibrary.Interfaces;
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
    public class TECSystem : TECScope, CostComponent, PointComponent
    {//TECSystem is the largest encapsulating object in the System-Equipment hierarchy, offering a specific structure for the needs of the estimating tool. A seperate hierarchy exists for TECScopeBranch as a more generic object.
        #region Properties
        private ObservableCollection<TECEquipment> _equipment;
        public ObservableCollection<TECEquipment> Equipment
        {
            get { return _equipment; }
            set
            {
                var temp = this.Copy();
                if (Equipment != null)
                {
                    Equipment.CollectionChanged -= CollectionChanged;
                }
                _equipment = value;
                NotifyPropertyChanged("Equipment", temp, this);
                Equipment.CollectionChanged += CollectionChanged;
            }
        }

        public double BudgetPriceModifier
        {
            get { return _budgetPriceModifier; }
            set
            {
                var temp = this.Copy();
                if (_budgetPriceModifier != value)
                {
                    if (value < 0)
                    {
                        _budgetPriceModifier = -1;
                    }
                    else
                    {
                        _budgetPriceModifier = value;
                    }
                    NotifyPropertyChanged("BudgetPriceModifier", temp, this);
                    RaisePropertyChanged("TotalBudgetPrice");
                    RaisePropertyChanged("BudgetUnitPrice");
                }
            }
        }
        private double _budgetPriceModifier;
        public double BudgetUnitPrice
        {
            get
            {
                double price = 0;
                bool priceExists = false;
                if (BudgetPriceModifier >= 0)
                {
                    price += BudgetPriceModifier;
                    priceExists = true;
                }
                foreach (TECEquipment equip in Equipment)
                {
                    if (equip.TotalBudgetPrice >= 0)
                    {
                        price += (equip.TotalBudgetPrice);
                        priceExists = true;
                    }
                }
                if (priceExists)
                { return price; }
                else
                { return -1; }
            }
        }
        new public int Quantity
        {
            get { return _quantity; }
            set
            {
                var temp = this.Copy();
                _quantity = value;
                NotifyPropertyChanged("Quantity", temp, this);
                RaisePropertyChanged("TotalBudgetPrice");
            }
        }
        public int EquipmentQuantity
        {
            get
            {
                int equipQuantity = 0;
                foreach (TECEquipment equip in Equipment)
                { equipQuantity += equip.Quantity; }
                return equipQuantity;
            }
        }
        public int SubScopeQuantity
        {
            get
            {
                int ssQuantity = 0;
                foreach (TECEquipment equip in Equipment)
                { ssQuantity += (equip.SubScopeQuantity * equip.Quantity); }
                return ssQuantity;
            }
        }

        public double TotalBudgetPrice
        {
            get
            {
                if (Quantity > 0)
                {
                    return (Quantity * BudgetUnitPrice);
                }
                else
                {
                    return -1;
                }
            }
        }

        public ObservableCollection<TECSubScope> SubScope
        {
            get
            {
                var outSubScope = new ObservableCollection<TECSubScope>();
                foreach (TECEquipment equip in Equipment)
                {
                    foreach (TECSubScope sub in equip.SubScope)
                    {
                        outSubScope.Add(sub);
                    }
                }
                return outSubScope;
            }
        }

        public int PointNumber
        {
            get
            {
                return getPointNumber();
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

        private ObservableCollection<TECSystem> _systemInstances;
        public ObservableCollection<TECSystem> SystemInstances
        {
            get { return _systemInstances; }
            set
            {
                var temp = this.Copy();
                if (Panels != null)
                {
                    SystemInstances.CollectionChanged -= CollectionChanged;
                }

                _systemInstances = value;
                SystemInstances.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("SystemInstances", temp, this);
            }
        }

        private ObservableCollection<TECScopeBranch> _scopeBranches;
        public ObservableCollection<TECScopeBranch> ScopeBranches
        {
            get { return _scopeBranches; }
            set
            {
                var temp = this.Copy();
                if (Panels != null)
                {
                    ScopeBranches.CollectionChanged -= CollectionChanged;
                }

                _scopeBranches = value;
                ScopeBranches.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ScopeBranches", temp, this);

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
            foreach(TECEquipment item in Equipment)
            {
                foreach(TECCost cost in item.Costs)
                {
                    outCosts.Add(cost);
                }
            }
            foreach (TECController item in Controllers)
            {
                foreach (TECCost cost in item.Costs)
                {
                    outCosts.Add(cost);
                }
            }
            foreach (TECPanel item in Panels)
            {
                foreach (TECCost cost in item.Costs)
                {
                    outCosts.Add(cost);
                }
            }
            foreach (TECCost item in AssociatedCosts)
            {
                outCosts.Add(item);
            }
            return outCosts;
        }

        public ObservableItemToInstanceList<TECScope> CharactersticInstances;
        private ChangeWatcher watcher;
        
        #endregion //Properties

        #region Constructors
        public TECSystem(Guid guid) : base(guid)
        {
            _budgetPriceModifier = -1;
            base.PropertyChanged += TECSystem_PropertyChanged;
            
            
            _equipment = new ObservableCollection<TECEquipment>();

            _controllers = new ObservableCollection<TECController>();
            _panels = new ObservableCollection<TECPanel>();
            _systemInstances = new ObservableCollection<TECSystem>();
            CharactersticInstances = new ObservableItemToInstanceList<TECScope>();
            CharactersticInstances.PropertyChanged += CharactersticInstances_PropertyChanged;
            Equipment.CollectionChanged += CollectionChanged;
            Controllers.CollectionChanged += CollectionChanged;
            Panels.CollectionChanged += CollectionChanged;
            SystemInstances.CollectionChanged += CollectionChanged;
            watcher = new ChangeWatcher(this);
            watcher.Changed += Object_PropertyChanged;
        }
        public TECSystem() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECSystem(TECSystem source, Dictionary<Guid, Guid> guidDictionary = null,
            ObservableItemToInstanceList<TECScope> characteristicReference = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = source.Guid; }
            foreach (TECEquipment equipment in source.Equipment)
            {
                var toAdd = new TECEquipment(equipment, guidDictionary, characteristicReference);
                if(characteristicReference != null)
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
            _budgetPriceModifier = source.BudgetPriceModifier;
            this.copyPropertiesFromScope(source);
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECSystem outSystem = new TECSystem(_guid);
            foreach (TECEquipment equipment in this.Equipment)
            { outSystem.Equipment.Add(equipment.Copy() as TECEquipment); }
            foreach (TECController controller in Controllers)
            {
                outSystem.Controllers.Add(controller.Copy() as TECController);
            }
            foreach (TECPanel panel in Panels)
            {
                outSystem.Panels.Add(panel.Copy() as TECPanel);
            }
            foreach(TECSystem system in SystemInstances)
            {
                outSystem.SystemInstances.Add(system.Copy() as TECSystem);
            }
            outSystem._budgetPriceModifier = this.BudgetPriceModifier;
            outSystem.copyPropertiesFromScope(this);
            return outSystem;
        }
        public override object DragDropCopy()
        {
            TECSystem outSystem = new TECSystem(this);
            return outSystem;
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
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender, typeof(TECSystem), typeof(TECEquipment));
            }
        }
        private void TECSystem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ObjectPropertyChanged")
            {
                var args = e as PropertyChangedExtendedEventArgs<object>;
                var oldNew = args.NewValue as Tuple<object, object>;
                foreach (TECEquipment equipment in this.Equipment)
                {
                    if (equipment.Location == oldNew.Item1)
                    {
                        equipment.SetLocationFromParent(this.Location);
                    }
                }
            }
        }

        private int getPointNumber()
        {
            var totalPoints = 0;
            foreach (TECEquipment equipment in Equipment)
            {
                totalPoints += equipment.PointNumber;
            }
            return totalPoints;
        }

        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object> && SystemInstances.Count > 0)
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
            if (targetObject is TECEquipment && referenceObject is TECSystem)
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
            else if (targetObject is TECSubScopeConnection && referenceObject is TECController)
            {
                var characteristicConnection = targetObject as TECSubScopeConnection;
                var characteristicSubScope = (targetObject as TECSubScopeConnection).SubScope;
                var characteristicController = (referenceObject as TECController);
                if (CharactersticInstances.ContainsKey(characteristicSubScope) && CharactersticInstances.ContainsKey(characteristicController))
                {
                    foreach (TECSystem system in SystemInstances)
                    {
                        TECSubScope subScopeToConnect = null;
                        foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
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
                            foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
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
            else if (targetObject is TECController && referenceObject is TECPanel)
            {
                var characteristicController = targetObject as TECController;
                var characteristicPanel = referenceObject as TECPanel;
                if (CharactersticInstances.ContainsKey(characteristicPanel) && CharactersticInstances.ContainsKey(characteristicController))
                {
                    foreach (TECSystem system in SystemInstances)
                    {
                        TECController controllerToConnect = null;
                        foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
                        {
                            if (system.Controllers.Contains(controller))
                            {
                                controllerToConnect = controller;
                                break;
                            }
                        }
                        if (controllerToConnect != null)
                        {
                            foreach (TECPanel panel in CharactersticInstances.GetInstances(characteristicPanel))
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
                    foreach (TECSystem system in SystemInstances)
                    {
                        TECSubScope subScopeToRemove = null;
                        foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
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
                            foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
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
            else if (targetObject is TECController && referenceObject is TECPanel)
            {
                var characteristicController = targetObject as TECController;
                var characteristicPanel = referenceObject as TECPanel;
                if (CharactersticInstances.ContainsKey(characteristicController) && CharactersticInstances.ContainsKey(characteristicPanel))
                {
                    foreach (TECSystem system in SystemInstances)
                    {
                        TECController controllerToRemove = null;
                        foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
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
                            foreach (TECPanel panel in CharactersticInstances.GetInstances(characteristicPanel))
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

        public void RefreshReferences()
        {
            watcher = new ChangeWatcher(this);
            watcher.Changed += Object_PropertyChanged;
        }
        #endregion
    }
}
