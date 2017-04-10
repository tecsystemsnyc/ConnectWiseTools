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

                updateCollections();
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
                RaisePropertyChanged("ControllerCollection");
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
            setupCollections();
            setupVMs();
        }
        #endregion

        #region Methods
        public void Refresh(TECBid bid)
        {
            Bid = bid;
            ControlledScope = null;
            ScopeSource = new ObservableCollection<TECControlledScope>();
            ScopeDataGrid.Refresh(Bid);

            setupCollections();
        }
        private void setupCollections()
        {
            ConduitTypeSelections = new ObservableCollection<TECConduitType>();
            var noneConduit = new TECConduitType();
            noneConduit.Name = "None";
            ConduitTypeSelections.Add(noneConduit);
            foreach (TECConduitType type in Bid.ConduitTypes)
            {
                ConduitTypeSelections.Add(type);
            }
            SubScopeConnectionCollection = new ObservableCollection<SubScopeConnection>();
            ControllerSelections = new ObservableCollection<TECController>();
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            PanelsCollection = new ObservableCollection<TECPanel>();
        }
        private void updateControllerSelections()
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
        private void updateControllerCollection()
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
        private void updatePanels()
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
        private void updateSubScopeConnections()
        {
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
                updateSubScopeConnections();
            }
        }
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            updateCollections();
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {

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
                }
            }
        }
        private void updateCollections()
        {
            ControllerCollection.CollectionChanged -= collectionChanged;
            updateSubScopeConnections();
            updateControllerSelections();
            updateControllerCollection();
            updatePanels();
            ControllerCollection.CollectionChanged += collectionChanged;
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
        }

        private void nullifySelections(object item)
        {
            if(!(item is TECPanel))
            {
                SelectedPanel = null;
            }
            if (!(item is ControllerInPanel))
            {
                SelectedControllerInPanel = null;
            }
        }

        private void addControlledScopeExecute()
        {
            for(int x = 0; x < ControlledScopeQuantity; x++)
            {
                Bid.addControlledScope(ControlledScope);
            }
        }
        private bool addControlledScopeCanExecute()
        {
            if(ControlledScope != null && ControlledScopeQuantity > 0)
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

            if (targetCollection.GetType().GetTypeInfo().GenericTypeArguments.Length > 0)
            {
                Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

                if (sourceItem != null && sourceType == targetType || (sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
            else if (sourceType == typeof(TECControlledScope))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
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
            else
            {
                var targetCollection = dropInfo.TargetCollection;

                if (sourceItem is TECScope)
                { sourceItem = ((TECScope)dropInfo.Data).DragDropCopy(); }
                Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];
                if ((sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
                {
                    var controllerInPanel = new ControllerInPanel(sourceItem as TECController, null);
                    ControlledScope.Controllers.Add(sourceItem as TECController);
                    sourceItem = controllerInPanel;
                }

                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    ((IList)dropInfo.TargetCollection).Add(sourceItem);
                    if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
                    { ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem); }
                }
                else
                {
                    if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
                    { ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem); }
                    if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                    { ((IList)dropInfo.TargetCollection).Add(sourceItem); }
                    else
                    { ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem); }
                }
            }
            

            
        }
        #endregion
    }
}