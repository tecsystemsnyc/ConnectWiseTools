using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ControlledScopeVM : ViewModelBase, IDropTarget
    {
        #region Properties

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

        #region VM Extenstions
        public SystemsVM ScopeDataGrid { get; set; }
        #endregion

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion

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

        private Visibility _debugVisibilty;
        public Visibility DebugVisibility
        {
            get { return _debugVisibilty; }
            set
            {
                _debugVisibilty = value;
                RaisePropertyChanged("DebugVisibility");
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

        private TECSystem _selectedSystem;
        public TECSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                unregisterChanges();
                _selectedSystem = value;

                updateCollections();
                registerChanges();
                RaisePropertyChanged("SelectedSystem");
            }
        }

        private TECTemplates _templates;
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }

        #endregion

        #region Constructor
        public ControlledScopeVM(TECTemplates templates)
        {
            Templates = templates;
            setupCollections();
            DebugVisibility = Visibility.Collapsed;
            setupVMs();
        }
        #endregion

        #region Methods
        public void Refresh(TECTemplates templates)
        {
            Templates = templates;
            SelectedSystem = null;
            ScopeDataGrid.Refresh(Templates);
            setupCollections();
        }

        private void setupCollections()
        {
            ConduitTypeSelections = new ObservableCollection<TECConduitType>();
            var noneConduit = new TECConduitType();
            noneConduit.Name = "None";
            ConduitTypeSelections.Add(noneConduit);
            foreach (TECConduitType type in Templates.Catalogs.ConduitTypes)
            {
                ConduitTypeSelections.Add(type);
            }
            SubScopeConnectionCollection = new ObservableCollection<SubScopeConnection>();
            ControllerSelections = new ObservableCollection<TECController>();
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            PanelsCollection = new ObservableCollection<TECPanel>();
        }

        private void setupVMs()
        {
            ScopeDataGrid = new SystemsVM(Templates);
            ScopeDataGrid.SelectionChanged += SelectionChanged;
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            ScopeDataGrid.DataGridVisibilty.SystemLocation = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemSubScopeCount = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemEquipmentCount = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentLocation = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SubScopeLocation = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            if (sourceItem is TECController)
            {
                UIHelpers.ControllerInPanelDragOver(dropInfo);
            }
            else
            {
                UIHelpers.StandardDragOver(dropInfo);
            }
        }
        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is TECSystem)
            {
                UIHelpers.ControlledScopeDrop(dropInfo, Templates);
            }
            else if (dropInfo.Data is TECController)
            {
                UIHelpers.ControllerInPanelDrop(dropInfo, SelectedSystem.Controllers);
            }
            else
            {
                UIHelpers.StandardDrop(dropInfo);
            }
        }

        private void updateControllerSelections()
        {
            ControllerSelections = new ObservableCollection<TECController>();
            if (SelectedSystem != null)
            {
                var noneController = new TECController(new TECManufacturer());
                noneController.Name = "None";
                ControllerSelections.Add(noneController);
                foreach (TECController controller in SelectedSystem.Controllers)
                {
                    ControllerSelections.Add(controller);
                }
            }
        }
        private void updateControllerCollection()
        {
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            if (SelectedSystem != null)
            {
                foreach (TECController controller in SelectedSystem.Controllers)
                {
                    TECPanel panelToAdd = null;
                    foreach (TECPanel panel in SelectedSystem.Panels)
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
            if (SelectedSystem != null)
            {
                PanelsCollection = SelectedSystem.Panels;
                var nonePanel = new TECPanel(new TECPanelType());
                nonePanel.Name = "None";
                PanelSelections.Add(nonePanel);
                foreach (TECPanel panel in SelectedSystem.Panels)
                {
                    PanelSelections.Add(panel);
                }
            }
        }
        private void updateSubScopeConnections()
        {
            SubScopeConnectionCollection = new ObservableCollection<SubScopeConnection>();
            if (SelectedSystem != null)
            {
                foreach (TECEquipment equipment in SelectedSystem.Equipment)
                {
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        var subConnectionToAdd = new SubScopeConnection(subScope);

                        subConnectionToAdd.ParentSystem = SelectedSystem;
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
                SelectedSystem.Equipment.CollectionChanged += collectionChanged;
                SelectedSystem.Controllers.CollectionChanged += collectionChanged;
                SelectedSystem.Panels.CollectionChanged += collectionChanged;
                foreach (TECEquipment equipment in SelectedSystem.Equipment)
                {
                    equipment.PropertyChanged += Equipment_PropertyChanged;
                }
            }
        }

        private void Equipment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RemovedSubScope" || e.PropertyName == "SubScopeQuantity")
            {
                checkForRemovedSubScope();
                updateSubScopeConnections();
            }
        }

        private void unregisterChanges()
        {
            if (SelectedSystem != null)
            {
                SelectedSystem.Controllers.CollectionChanged -= collectionChanged;
                SelectedSystem.Panels.CollectionChanged -= collectionChanged;
                foreach (TECEquipment equipment in SelectedSystem.Equipment)
                {
                    equipment.PropertyChanged -= Equipment_PropertyChanged;
                }
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
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is ControllerInPanel)
                    {
                        foreach (TECPanel panel in SelectedSystem.Panels)
                        {
                            if (panel.Controllers.Contains((item as ControllerInPanel).Controller))
                            {
                                panel.Controllers.Remove((item as ControllerInPanel).Controller);
                            }
                        }
                        SelectedSystem.Controllers.Remove((item as ControllerInPanel).Controller);
                    }
                    else if (item is TECSystem)
                    {
                        checkForRemovedSubScope();
                    }
                }
            }
            updateCollections();
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
        #endregion
    }
}