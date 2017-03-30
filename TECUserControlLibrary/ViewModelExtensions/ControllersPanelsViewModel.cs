using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModelExtensions
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ControllersPanelsViewModel : ViewModelBase, IDropTarget
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
                registerChanges();
            }
        }
        
        private TECController _selectedController;
        public TECController SelectedController
        {
            get { return _selectedController; }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
                SelectionChanged?.Invoke(value);
            }
        }
        private TECPanel _selectedPanel;
        public TECPanel SelectedPanel
        {
            get { return _selectedPanel; }
            set
            {
                _selectedPanel = value;
                RaisePropertyChanged("SelectedPanel");
                SelectionChanged?.Invoke(value);
            }
        }

        private ObservableCollection<ControllerInPanel> _controllerCollection;
        public ObservableCollection<ControllerInPanel> ControllerCollection
        {
            get
            {
                return _controllerCollection;
            }
            set
            {
                _controllerCollection = value;
                RaisePropertyChanged("ControllerCollection");
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

        private ControllerInPanel _selectedControllerInPanel;
        public ControllerInPanel SelectedControllerInPanel
        {
            get { return _selectedControllerInPanel; }
            set
            {
                _selectedControllerInPanel = value;
                RaisePropertyChanged("SelectedControllerInPanel");
                if (value != null)
                {
                    SelectionChanged?.Invoke(value.Controller);
                }
            }
        }

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion

        #endregion

        /// <summary>
        /// Initializes a new instance of the ControllersPanelsViewModel class.
        /// </summary>
        public ControllersPanelsViewModel(TECBid bid)
        {
            _bid = bid;
            populateControllerCollection();
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            updateCollections();
        }

        private void populateControllerCollection()
        {
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            foreach (TECController controller in Bid.Controllers)
            {
                TECController controllerToAdd = controller;
                TECPanel panelToAdd = null;
                foreach (TECPanel panel in Bid.Panels)
                {
                    if (panel.Controllers.Contains(controller))
                    {
                        panelToAdd = panel;
                        break;
                    }
                }
                ControllerCollection.Add(new ControllerInPanel(controllerToAdd, panelToAdd));
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            var targetCollection = dropInfo.TargetCollection;
            if (targetCollection.GetType().GetTypeInfo().GenericTypeArguments.Length > 0)
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
            if (sourceItem is TECScope)
            { sourceItem = ((TECScope)dropInfo.Data).DragDropCopy(); }
            Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];
            if ((sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
            {
                var controllerInPanel = new ControllerInPanel(sourceItem as TECController, null);
                Bid.Controllers.Add(sourceItem as TECController);
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
        
        private void updateControllerCollection()
        {
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            foreach (TECController controller in Bid.Controllers)
            {
                TECPanel panelToAdd = null;
                foreach (TECPanel panel in Bid.Panels)
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
            PanelSelections = new ObservableCollection<TECPanel>();
            var nonePanel = new TECPanel();
            nonePanel.Name = "None";
            PanelSelections.Add(nonePanel);
            foreach (TECPanel panel in Bid.Panels)
            {
                PanelSelections.Add(panel);
            }
        }
        private void registerChanges()
        {
            Bid.Controllers.CollectionChanged += collectionChanged;
            Bid.Panels.CollectionChanged += collectionChanged;
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
                        foreach (TECPanel panel in Bid.Panels)
                        {
                            if (panel.Controllers.Contains((item as ControllerInPanel).Controller))
                            {
                                panel.Controllers.Remove((item as ControllerInPanel).Controller);
                            }
                        }
                        Bid.Controllers.Remove((item as ControllerInPanel).Controller);
                    }
                }
            }
        }

        private void updateCollections()
        {
            ControllerCollection.CollectionChanged -= collectionChanged;
            updateControllerCollection();
            updatePanels();
            ControllerCollection.CollectionChanged += collectionChanged;
        }
    }
}