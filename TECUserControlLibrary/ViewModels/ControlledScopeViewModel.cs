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

        private ObservableCollection<ControllerInPanel> _controllerSelections;
        public ObservableCollection<ControllerInPanel> ControllerSelections
        {
            get { return _controllerSelections; }
            set {
                _controllerSelections = value;
                RaisePropertyChanged("ControllerSelections");
            }
        }

        private TECControlledScope _selectedControlledScope;
        public TECControlledScope SelectedControlledScope
        {
            get { return _selectedControlledScope; }
            set
            {
                _selectedControlledScope = value;
                updateControllerSelections();
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
            ControllerSelections = new ObservableCollection<ControllerInPanel>();
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
            Type sourceType = sourceItem.GetType();
            Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (sourceItem != null && sourceType == targetType || (sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
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
            ControllerSelections = new ObservableCollection<ControllerInPanel>();
            foreach(TECController controller in SelectedControlledScope.Controllers)
            {
                TECPanel panelToAdd = null;
                ControllerInPanel controllerInPanelToAdd = new ControllerInPanel();
                foreach(TECPanel panel in SelectedControlledScope.Panels)
                {
                    if (panel.Controllers.Contains(controller))
                    {
                        panelToAdd = panel;
                    }
                }
                controllerInPanelToAdd.Controller = controller;
                controllerInPanelToAdd.Panel = panelToAdd;
                ControllerSelections.Add(controllerInPanelToAdd);

            }
        }
    }
}