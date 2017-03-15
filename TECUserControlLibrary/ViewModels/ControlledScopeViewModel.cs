using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.ObjectModel;
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

        #region VM Extenstions
        public ScopeDataGridExtension ScopeDataGrid { get; set; }
        #endregion

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;
        #endregion

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


        private TECControlledScope _selectedControlledScope;
        public TECControlledScope SelectedControlledScope
        {
            get { return _selectedControlledScope; }
            set
            {
                _selectedControlledScope = value;
                if(value != null)
                {
                    updateControllerSelections();
                    updateControllerCollection();
                    updatePanels();
                    updateSubScopeConnections();
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

        public ControlledScopeViewModel(TECTemplates templates)
        {
            Templates = templates;
            ControllerSelections = new ObservableCollection<TECController>();
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            SubScopeConnectionCollection = new ObservableCollection<SubScopeConnection>();
            PanelsCollection = new ObservableCollection<TECPanel>();
            setupVMs();
        }

        private void setupVMs()
        {
            ScopeDataGrid = new ScopeDataGridExtension(Templates);
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
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
            Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];
            if ((sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
            {
                var controllerInPanel = new ControllerInPanel();
                (controllerInPanel as ControllerInPanel).Controller = sourceItem as TECController;
                sourceItem = controllerInPanel;
            }
            else if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                sourceItem = ((TECScope)dropInfo.Data).DragDropCopy();
            }

            if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
            {
                ((IList)dropInfo.TargetCollection).Add(sourceItem);
                if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
                {
                    ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem);
                }
            }
            else
            {
                if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
                {
                    ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem);
                }
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    ((IList)dropInfo.TargetCollection).Add(sourceItem);
                }
                else
                {
                    ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                }
            }
        }

        private void updateControllerSelections()
        {
            ControllerSelections = SelectedControlledScope.Controllers;
        }
        private void updateControllerCollection()
        {
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            foreach (TECController controller in SelectedControlledScope.Controllers)
            {
                TECPanel panelToAdd = null;
                ControllerInPanel controllerInPanelToAdd = new ControllerInPanel();
                foreach (TECPanel panel in SelectedControlledScope.Panels)
                {
                    if (panel.Controllers.Contains(controller))
                    {
                        panelToAdd = panel;
                    }
                }
                controllerInPanelToAdd.Controller = controller;
                controllerInPanelToAdd.Panel = panelToAdd;
                ControllerCollection.Add(controllerInPanelToAdd);

            }
        }
        private void updatePanels()
        {
            PanelsCollection = SelectedControlledScope.Panels;
        }
        private void updateSubScopeConnections()
        {
            foreach(TECSystem system in SelectedControlledScope.Systems)
            {
                foreach(TECEquipment equipment in system.Equipment)
                {
                    foreach(TECSubScope subScope in equipment.SubScope)
                    {
                        var subConnectionToAdd = new SubScopeConnection();
                        subConnectionToAdd.SubScope = subScope;
                        foreach(TECConnection connection in SelectedControlledScope.Connections)
                        {
                            if (connection.Scope.Contains(subScope))
                            {
                                subConnectionToAdd.Connection = connection;
                                subConnectionToAdd.Controller = connection.Controller;
                            }
                        }
                        SubScopeConnectionCollection.Add(subConnectionToAdd);
                    }
                }
            }
        }

        private void registerChanges()
        {
            SelectedControlledScope.Systems.CollectionChanged += collectionChanged;
        }

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            updateSubScopeConnections();
            updateControllerSelections();
        }
    }
}