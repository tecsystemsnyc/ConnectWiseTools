using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System.Windows.Input;
using System;
using System.Reflection;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Models;
using System.ComponentModel;

namespace TECUserControlLibrary.ViewModelExtensions
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddControlledScopeExtension : ViewModelBase, IDropTarget
    {
        #region Properties
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }

        private TECControlledScope _controlledScope;
        public TECControlledScope ControlledScope
        {
            get { return _controlledScope; }
            set
            {
                unregisterChanges();
                _controlledScope = value;
                setupCollections();
                registerChanges();
                RaisePropertyChanged("ControlledScope");
            }
        }

        private TECPanel _selectedPanel;
        public TECPanel SelectedPanel
        {
            get
            {
                return _selectedPanel;
            }
            set
            {
                _selectedPanel = value;
                RaisePropertyChanged("SelectedPanel");
                SelectionChanged?.Invoke(value);
            }
        }

        private ControllerInPanel _selectedControllerInPanel;
        public ControllerInPanel SelectedControllerInPanel
        {
            get
            {
                return _selectedControllerInPanel;
            }
            set
            {
                _selectedControllerInPanel = value;
                RaisePropertyChanged("SelectedControllerInPanel");
                TECController senderObject = null;
                if (_selectedControllerInPanel != null)
                {
                    senderObject = _selectedControllerInPanel.Controller;
                }
                SelectionChanged?.Invoke(senderObject);
            }
        }

        private int _controlledScopeQuantity;
        public int ControlledScopeQuantity
        {
            get { return _controlledScopeQuantity; }
            set
            {
                _controlledScopeQuantity = value;
                RaisePropertyChanged("ControlledScopeQuantity");
            }
        }

        private ObservableCollection<TECControlledScope> _scopeSource;
        public ObservableCollection<TECControlledScope> ScopeSource
        {
            get { return _scopeSource; }
            set
            {
                _scopeSource = value;
                RaisePropertyChanged("ScopeSource");
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
        private ObservableCollection<ControllerInPanel> _controllerCollection;
        public ObservableCollection<ControllerInPanel> ControllerCollection
        {
            get { return _controllerCollection; }
            set
            {
                _controllerCollection = value;
                ControllerCollection.CollectionChanged -= collectionChanged;
                RaisePropertyChanged("ControllerCollection");
                ControllerCollection.CollectionChanged += collectionChanged;
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
        private ObservableCollection<TECPanel> _panelsCollection;
        public ObservableCollection<TECPanel> PanelsCollection
        {
            get { return _panelsCollection; }
            set
            {
                _panelsCollection = value;
                RaisePropertyChanged("PanelsCollection");
            }
        }
        private ObservableCollection<TECPanel> _panelSelections;
        public ObservableCollection<TECPanel> PanelSelections
        {
            get { return _panelSelections; }
            set
            {
                _panelSelections = value;
                RaisePropertyChanged("PanelSelections");
            }
        }
        private ObservableCollection<TECConduitType> _conduitTypeSelections;
        public ObservableCollection<TECConduitType> ConduitTypeSelections
        {
            get { return _conduitTypeSelections; }
            set
            {
                _conduitTypeSelections = value;
                RaisePropertyChanged("ConduitTypeSelections");
            }
        }

        private VisibilityModel _dataGridVisibilty;
        public VisibilityModel DataGridVisibilty
        {
            get { return _dataGridVisibilty; }
            set
            {
                _dataGridVisibilty = value;
                RaisePropertyChanged("DataGridVisibilty");
            }
        }

        public ICommand AddControlledScopeCommand { get; private set; }

        #region VM Extenstions
        public ScopeDataGridExtension ScopeDataGrid { get; set; }
        #endregion

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the AddControlledScopeExtension class.
        /// </summary>
        public AddControlledScopeExtension(TECBid bid)
        {
            _bid = bid;
            AddControlledScopeCommand = new RelayCommand(addControlledScopeExecute, addControlledScopeCanExecute);
            ControlledScope = new TECControlledScope();
            setupCatalogCollections();
            setupVMs();
        }
        #endregion

        #region Methods
        public void Refresh(TECBid bid)
        {
            Bid = bid;
            ScopeSource = new ObservableCollection<TECControlledScope>();
            ScopeDataGrid.Refresh(Bid);
            setupCatalogCollections();
            ModelLinkingHelper.LinkControlledScopeObjects(ControlledScope.Systems, ControlledScope.Controllers,
                ControlledScope.Panels, Bid);
        }
        private void setupCatalogCollections()
        {
            ConduitTypeSelections = new ObservableCollection<TECConduitType>();
            var noneConduit = new TECConduitType();
            noneConduit.Name = "None";
            ConduitTypeSelections.Add(noneConduit);
            foreach (TECConduitType type in Bid.Catalogs.ConduitTypes)
            {
                ConduitTypeSelections.Add(type);
            }
        }
        private void populateControllerSelections()
        {
            ControllerSelections = new ObservableCollection<TECController>();
            if (ControlledScope != null)
            {
                var noneController = new TECController();
                noneController.Name = "None";
                ControllerSelections.Add(noneController);
                foreach (TECController controller in ControlledScope.Controllers)
                {
                    ControllerSelections.Add(controller);
                }
            }
        }
        private void populateControllerCollection()
        {
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            if (ControlledScope != null)
            {
                foreach (TECController controller in ControlledScope.Controllers)
                {
                    TECPanel panelToAdd = null;
                    foreach (TECPanel panel in ControlledScope.Panels)
                    {
                        if (panel.Controllers.Contains(controller))
                        {
                            panelToAdd = panel;
                        }
                    }
                    ControllerInPanel controllerInPanelToAdd = new ControllerInPanel(controller, panelToAdd);
                    ControllerCollection.Add(controllerInPanelToAdd);
                }
            }
        }
        private void populatePanels()
        {
            PanelsCollection = new ObservableCollection<TECPanel>();
            PanelSelections = new ObservableCollection<TECPanel>();
            if (ControlledScope != null)
            {
                PanelsCollection = ControlledScope.Panels;
                var nonePanel = new TECPanel();
                nonePanel.Name = "None";
                PanelSelections.Add(nonePanel);
                foreach (TECPanel panel in ControlledScope.Panels)
                {
                    PanelSelections.Add(panel);
                }
            }
        }
        private void populateSubScopeConnections()
        {
            if (ControlledScope != null && SubScopeConnectionCollection != null)
            {
                var currentSubScope = new ObservableCollection<TECSubScope>();

                foreach (TECSystem system in ControlledScope.Systems)
                {
                    foreach (TECEquipment equipment in system.Equipment)
                    {
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {
                            currentSubScope.Add(subScope);
                        }
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
            SubScopeConnectionCollection = new ObservableCollection<SubScopeConnection>();
            if (ControlledScope != null)
            {
                foreach (TECSystem system in ControlledScope.Systems)
                {
                    foreach (TECEquipment equipment in system.Equipment)
                    {
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {

                            var subConnectionToAdd = new SubScopeConnection(subScope);

                            subConnectionToAdd.ParentSystem = system;
                            subConnectionToAdd.ParentEquipment = equipment;

                            SubScopeConnectionCollection.Add(subConnectionToAdd);
                        }
                    }
                }
            }
        }
        private void registerChanges()
        {
            if (ControlledScope != null)
            {
                ControlledScope.Systems.CollectionChanged += collectionChanged;
                ControlledScope.Controllers.CollectionChanged += collectionChanged;
                ControlledScope.Panels.CollectionChanged += collectionChanged;
                foreach (TECSystem system in ControlledScope.Systems)
                {
                    system.PropertyChanged += System_PropertyChanged;
                }
            }
        }
        private void unregisterChanges()
        {
            if (ControlledScope != null)
            {
                ControlledScope.Systems.CollectionChanged -= collectionChanged;
                ControlledScope.Controllers.CollectionChanged -= collectionChanged;
                ControlledScope.Panels.CollectionChanged -= collectionChanged;
                foreach (TECSystem system in ControlledScope.Systems)
                {
                    system.PropertyChanged -= System_PropertyChanged;
                }
            }
        }
        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemovedSubScope" || e.PropertyName == "SubScopeQuantity")
            {
                populateSubScopeConnections();
            }
        }
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECSystem)
                    {
                        (item as TECSystem).PropertyChanged += System_PropertyChanged;
                        populateSubScopeConnections();
                    }
                    else if (item is TECController)
                    {
                        ControllerSelections.Add(item as TECController);
                        ControllerInPanel controllerInPanelToAdd = new ControllerInPanel(item as TECController, null);
                        ControllerCollection.Add(controllerInPanelToAdd);
                    }
                    else if (item is TECPanel)
                    {
                        PanelSelections.Add(item as TECPanel);
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is ControllerInPanel)
                    {
                        foreach (TECPanel panel in ControlledScope.Panels)
                        {
                            if (panel.Controllers.Contains((item as ControllerInPanel).Controller))
                            {
                                panel.Controllers.Remove((item as ControllerInPanel).Controller);
                            }
                        }
                        ControlledScope.Controllers.Remove((item as ControllerInPanel).Controller);
                    }
                    else if (item is TECSystem)
                    {
                        (item as TECSystem).PropertyChanged -= System_PropertyChanged;
                        checkForRemovedSubScope();
                    }
                    else if (item is TECController)
                    {
                        ControllerSelections.Remove(item as TECController);
                        foreach (ControllerInPanel controllerInPanel in ControllerCollection)
                        {
                            if (controllerInPanel.Controller == item as TECController)
                            {
                                ControllerCollection.Add(controllerInPanel);
                                break;
                            }
                        }
                    }
                    else if (item is TECPanel)
                    {
                        PanelSelections.Add(item as TECPanel);
                    }
                }
            }
        }
        private void setupCollections()
        {
            populateSubScopeConnections();
            populateControllerSelections();
            populateControllerCollection();
            populatePanels();
        }
        private void checkForRemovedSubScope()
        {
            if (ControlledScope != null && SubScopeConnectionCollection != null)
            {
                var currentSubScope = new ObservableCollection<TECSubScope>();

                foreach (TECSystem system in ControlledScope.Systems)
                {
                    foreach (TECEquipment equipment in system.Equipment)
                    {
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {
                            currentSubScope.Add(subScope);
                        }
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
        private void setupVMs()
        {
            ScopeDataGrid = new ScopeDataGridExtension(Bid);
            ScopeDataGrid.SelectionChanged += SelectionChanged;
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            ScopeDataGrid.DataGridVisibilty.SystemLocation = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemSubScopeCount = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemEquipmentCount = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentLocation = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SubScopeLocation = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemModifierPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemUnitPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentUnitPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentTotalPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;
        }

        private void addControlledScopeExecute()
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int x = 0; x < ControlledScopeQuantity; x++)
            {
                //var subWatch = System.Diagnostics.Stopwatch.StartNew();
                Bid.addControlledScope(ControlledScope);
                //subWatch.Stop();
                //Console.WriteLine("Add " + x + " controlled scope: " + subWatch.ElapsedMilliseconds);
            }
            //watch.Stop();
            //Console.WriteLine("Add all controlled scope: " + watch.ElapsedMilliseconds);
            ControlledScopeQuantity = 0;
        }
        private bool addControlledScopeCanExecute()
        {
            if (ControlledScope != null && ControlledScopeQuantity > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            Type sourceType = sourceItem.GetType();

            var targetCollection = dropInfo.TargetCollection;
            if (sourceType == typeof(TECController) && ControlledScope != null)
            {
                UIHelpers.ControllerInPanelDragOver(dropInfo);
            }
            else if (sourceType == typeof(TECControlledScope))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
            else if (ControlledScope != null)
            {
                UIHelpers.StandardDragOver(dropInfo);
            }
        }
        public void Drop(IDropInfo dropInfo)
        {
            Type sourceType = dropInfo.Data.GetType();
            Object sourceItem;
            sourceItem = dropInfo.Data;

            if (dropInfo.Data is TECControlledScope)
            {
                var controlledScopeToAdd = (dropInfo.Data as TECControlledScope).Copy() as TECControlledScope;
                ModelLinkingHelper.LinkControlledScopeObjects(controlledScopeToAdd.Systems, controlledScopeToAdd.Controllers,
                controlledScopeToAdd.Panels, Bid);
                ControlledScope = controlledScopeToAdd;
            }
            else if (dropInfo.Data is TECController)
            {
                UIHelpers.ControllerInPanelDrop(dropInfo, ControlledScope.Controllers);
            }
            else
            {
                UIHelpers.StandardDrop(dropInfo);
            }

        }
        #endregion
    }
}