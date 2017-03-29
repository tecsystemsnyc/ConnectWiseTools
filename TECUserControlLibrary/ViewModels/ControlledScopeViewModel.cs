using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModelExtensions;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ControlledScopeViewModel : ViewModelBase, IDropTarget
    {
        #region Properties
        
        #region VM Extenstions
        public ScopeDataGridExtension ScopeDataGrid { get; set; }
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
                SelectionChanged.Invoke(_selectedControllerInPanel.Controller);
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
            set {
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
        
        private TECControlledScope _selectedControlledScope;
        public TECControlledScope SelectedControlledScope
        {
            get { return _selectedControlledScope; }
            set
            {
                _selectedControlledScope = value;
                if(value != null)
                {
                    updateCollections();
                    registerChanges();
                }
                RaisePropertyChanged("SelectedControlledScope");

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

        public ControlledScopeViewModel(TECTemplates templates)
        {
            Templates = templates;
            ConduitTypeSelections = new ObservableCollection<TECConduitType>();
            var noneConduit = new TECConduitType();
            noneConduit.Name = "None";
            ConduitTypeSelections.Add(noneConduit);
            foreach(TECConduitType type in Templates.ConduitTypeCatalog)
            {
                ConduitTypeSelections.Add(type);
            }
            ControllerSelections = new ObservableCollection<TECController>();
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            SubScopeConnectionCollection = new ObservableCollection<SubScopeConnection>();
            PanelsCollection = new ObservableCollection<TECPanel>();
            DebugVisibility = Visibility.Visible;
            setupVMs();
        }

        private void setupVMs()
        {
            ScopeDataGrid = new ScopeDataGridExtension(Templates);
            ScopeDataGrid.SelectionChanged += SelectionChanged;
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            ScopeDataGrid.DataGridVisibilty.SystemLocation = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemSubScopeCount = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemEquipmentCount = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentLocation = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SubScopeLocation = Visibility.Collapsed;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            var targetCollection = dropInfo.TargetCollection;
            if(targetCollection.GetType().GetTypeInfo().GenericTypeArguments.Length > 0)
            {
                Type sourceType = sourceItem.GetType();
                Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

                if (sourceItem != null && sourceType == targetType || (sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
            
        }
        public void Drop(IDropInfo dropInfo)
        {
            Object sourceItem;
            sourceItem = dropInfo.Data;

            var targetCollection = dropInfo.TargetCollection;

            Type sourceType = sourceItem.GetType();
            if(sourceItem is TECScope)
            { sourceItem = ((TECScope)dropInfo.Data).DragDropCopy(); }
            Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];
            if ((sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
            {
                var controllerInPanel = new ControllerInPanel(sourceItem as TECController, null);
                SelectedControlledScope.Controllers.Add(sourceItem as TECController);
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
                {  ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem); }
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                { ((IList)dropInfo.TargetCollection).Add(sourceItem); }
                else
                { ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem); }
            }
        }

        private void updateControllerSelections()
        {
            ControllerSelections = new ObservableCollection<TECController>();
            var noneController = new TECController();
            noneController.Name = "None";
            ControllerSelections.Add(noneController);
            foreach(TECController controller in SelectedControlledScope.Controllers)
            {
                ControllerSelections.Add(controller);
            }
        }
        private void updateControllerCollection()
        {
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            foreach (TECController controller in SelectedControlledScope.Controllers)
            {
                TECPanel panelToAdd = null;
                foreach (TECPanel panel in SelectedControlledScope.Panels)
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
        private void updatePanels()
        {
            PanelsCollection = SelectedControlledScope.Panels;
            PanelSelections = new ObservableCollection<TECPanel>();
            var nonePanel = new TECPanel();
            nonePanel.Name = "None";
            PanelSelections.Add(nonePanel);
            foreach(TECPanel panel in SelectedControlledScope.Panels)
            {
                PanelSelections.Add(panel);
            }
        }
        private void updateSubScopeConnections()
        {
            SubScopeConnectionCollection = new ObservableCollection<SubScopeConnection>();
            foreach (TECSystem system in SelectedControlledScope.Systems)
            {
                foreach(TECEquipment equipment in system.Equipment)
                {
                    foreach(TECSubScope subScope in equipment.SubScope)
                    {
                        
                        var subConnectionToAdd = new SubScopeConnection(subScope);

                        subConnectionToAdd.ParentSystem = system;
                        subConnectionToAdd.ParentEquipment = equipment;

                        SubScopeConnectionCollection.Add(subConnectionToAdd);
                    }
                }
            }
        }
        
        private void registerChanges()
        {
            SelectedControlledScope.Systems.CollectionChanged += collectionChanged;
            SelectedControlledScope.Controllers.CollectionChanged += collectionChanged;
            SelectedControlledScope.Panels.CollectionChanged += collectionChanged;
        }

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            updateCollections();
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                //foreach(object item in e.NewItems)
                //{
                //}
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach(object item in e.OldItems)
                {
                    if(item is ControllerInPanel)
                    {
                        foreach(TECPanel panel in SelectedControlledScope.Panels)
                        {
                            if(panel.Controllers.Contains((item as ControllerInPanel).Controller))
                            {
                                panel.Controllers.Remove((item as ControllerInPanel).Controller);
                            }
                        }
                        SelectedControlledScope.Controllers.Remove((item as ControllerInPanel).Controller);
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
    }
}