using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ConnectionVM : ViewModelBase
    {
        private TECSystem _selectedSystem;
        public TECSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                unregisterChanges();
                _selectedSystem = value;

                setupCollections();
                registerChanges();

                RaisePropertyChanged("SelectedSystem");
            }
        }

        private TECScopeManager _scopeManager;
        public TECScopeManager ScopeManager
        {
            get { return _scopeManager; }
            set
            {
                unregisterScopeManagerChanges();
                _scopeManager = value;
                registerScopeManagerChanges();
                RaisePropertyChanged("ScopeManager");
            }
        }
        
        private ObservableCollection<SubScopeConnection> _subScopeConnectionCollection;
        public ObservableCollection<SubScopeConnection> SubScopeConnectionCollection
        {
            get { return _subScopeConnectionCollection; }
            set
            {
                _subScopeConnectionCollection = value;
                RaisePropertyChanged("SubScopeConnectionCollection");
            }
        }
        private ObservableCollection<TECElectricalMaterial> _conduitTypeSelections;
        public ObservableCollection<TECElectricalMaterial> ConduitTypeSelections
        {
            get { return _conduitTypeSelections; }
            set
            {
                _conduitTypeSelections = value;
                RaisePropertyChanged("ConduitTypeSelections");
            }
        }
        private ObservableCollection<TECController> _controllerSelections;
        public ObservableCollection<TECController> ControllerSelections
        {
            get { return _controllerSelections; }
            set
            {
                _controllerSelections = value;
                RaisePropertyChanged("ControllerSelections");
            }
        }
        private TECController _noneController;
        public TECController NoneController
        {
            get { return _noneController; }
            set
            {
                _noneController = value;
                RaisePropertyChanged("NoneController");
            }
        }
        private TECElectricalMaterial _noneConduitType;
        public TECElectricalMaterial NoneConduitType
        {
            get { return _noneConduitType; }
            set
            {
                _noneConduitType = value;
                RaisePropertyChanged("NoneConduitType");
            }
        }

        public bool ControllerSelectionReadOnly { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the ConnectionVM class.
        /// </summary>
        public ConnectionVM(TECScopeManager scopeManager, bool isTypical = true)
        {
            ControllerSelectionReadOnly = !isTypical;
            _scopeManager = scopeManager;
            _selectedSystem = null;
            setupCatalogCollections();
            setupCollections();
        }

        public void Refresh(TECScopeManager scopeManager)
        {
            ScopeManager = scopeManager;
            setupCatalogCollections();
            setupCollections();
        }
        
        private void setupCollections()
        {
            populateSubScopeConnections();
            populateControllerSelections();
        }

        private void checkForRemovedSubScope()
        {
            if (SelectedSystem != null && SubScopeConnectionCollection != null)
            {
                var currentSubScope = new ObservableCollection<TECSubScope>();
                foreach (TECEquipment equipment in SelectedSystem.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        currentSubScope.Add(subScope);
                    }
                }
                foreach (SubScopeConnection connection in SubScopeConnectionCollection)
                {
                    if (!currentSubScope.Contains(connection.SubScope) && connection.Controller != null)
                    {
                        connection.Controller.RemoveSubScope(connection.SubScope);
                    }
                }
            }
        }

        private void setupCatalogCollections()
        {
            ConduitTypeSelections = new ObservableCollection<TECElectricalMaterial>();
            var noneConduit = new TECElectricalMaterial();
            noneConduit.Name = "None";
            NoneConduitType = noneConduit;
            ConduitTypeSelections.Add(NoneConduitType);
            foreach (TECElectricalMaterial type in ScopeManager.Catalogs.ConduitTypes)
            {
                ConduitTypeSelections.Add(type);
            }
        }
        private void populateControllerSelections()
        {
            ControllerSelections = new ObservableCollection<TECController>();
            if (SelectedSystem != null )
            {
                var noneController = new TECController(new TECControllerType(new TECManufacturer()));
                noneController.Name = "None";
                NoneController = noneController;
                ControllerSelections.Add(NoneController);
                foreach (TECController controller in SelectedSystem.Controllers)
                {
                    ControllerSelections.Add(controller);
                }
                var bid = ScopeManager as TECBid;
                if(bid != null)
                {
                    foreach (TECController controller in bid.Controllers)
                    {
                        ControllerSelections.Add(controller);
                    }
                }
            }
        }
        private void populateSubScopeConnections()
        {
            SubScopeConnectionCollection = new ObservableCollection<SubScopeConnection>();
            if (SelectedSystem != null)
            {
                foreach (TECEquipment equipment in SelectedSystem.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        var subConnectionToAdd = new SubScopeConnection(subScope);

                        subConnectionToAdd.ParentEquipment = equipment;

                        SubScopeConnectionCollection.Add(subConnectionToAdd);
                    }
                }
            }
        }
        private void registerChanges()
        {
            if (SelectedSystem != null)
            {
                SelectedSystem.Controllers.CollectionChanged += collectionChanged;
                SelectedSystem.Equipment.CollectionChanged += collectionChanged;
                foreach (TECEquipment equipment in SelectedSystem.Equipment)
                {
                    equipment.PropertyChanged += Equipment_PropertyChanged;
                }
            }
        }
        private void unregisterChanges()
        {
            if (SelectedSystem != null)
            {
                SelectedSystem.Controllers.CollectionChanged -= collectionChanged;
                SelectedSystem.Equipment.CollectionChanged -= collectionChanged;
                foreach (TECEquipment equipment in SelectedSystem.Equipment)
                {
                    equipment.PropertyChanged -= Equipment_PropertyChanged;
                }
            }
        }
        private void registerScopeManagerChanges()
        {
            var bid = ScopeManager as TECBid;
            if (bid != null)
            {
                bid.Controllers.CollectionChanged += collectionChanged;
            }
        }

        private void unregisterScopeManagerChanges()
        {
            var bid = ScopeManager as TECBid;
            if (bid != null)
            {
                bid.Controllers.CollectionChanged -= collectionChanged;
            }
        }

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECEquipment)
                    {
                        (item as TECEquipment).PropertyChanged += Equipment_PropertyChanged;
                        populateSubScopeConnections();
                    }
                    else if (item is TECController)
                    {
                        ControllerSelections.Add(item as TECController);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECEquipment)
                    {
                        (item as TECEquipment).PropertyChanged -= Equipment_PropertyChanged;
                        checkForRemovedSubScope();
                    }
                    else if (item is TECController)
                    {
                        ControllerSelections.Remove(item as TECController);
                    }
                }
            }
        }

        private void Equipment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = e as PropertyChangedExtendedEventArgs;
            if(args != null)
            {
                if(args.Change == Change.Add && args.Value is TECSubScope)
                {
                    populateSubScopeConnections();
                }
                else if(args.Change == Change.Remove && args.Value is TECSubScope)
                {
                    checkForRemovedSubScope();
                }
            }
        }
    }
}