using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary;
using TECUserControlLibrary.ViewModelExtensions;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ScopeEditorViewModel : ViewModelBase, IDropTarget
    {
        //Initializer
        public ScopeEditorViewModel(TECBid bid, TECTemplates templates)
        {
            Bid = bid;
            Templates = templates;
            DGTabIndex = 0;

            setupEditTab();
            setupScopeCollection();
            setupScopeDataGrid();
            setupLocationDataGrid();

            setVisibility();

            ToggleTemplatesVisibilityCommand = new RelayCommand(ToggleTemplatesVisibilityExecute);
            TemplatesVisibility = Visibility.Visible;

        }

        #region Properties

        private int _dGTabIndex;
        public int DGTabIndex
        {
            get { return _dGTabIndex; }
            set
            {
                _dGTabIndex = value;
                RaisePropertyChanged("DGTabIndex");
            }
        }

        #region Extensions
        public ScopeDataGridExtension ScopeDataGrid { get; set; }
        public LocationDataGridExtension LocationDataGrid { get; set; }
        public ScopeCollectionExtension ScopeCollection { get; set; }
        public EditTabExtension EditTab { get; set; }
        #endregion

        #region Interface Properties
        

        #region Scope Properties
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }
        private TECTemplates _templates;

        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }
        private TECBid _bid;
        #endregion Scope Properties

        #endregion //Interface Properties

        #region Commands Properties
        public ICommand ToggleTemplatesVisibilityCommand { get; private set; }
        #endregion //Commands Properties

        #region Visibility Properties
        private Visibility _templatesVisibility;
        public Visibility TemplatesVisibility
        {
            get { return _templatesVisibility; }
            set
            {
                if (value != _templatesVisibility)
                {
                    _templatesVisibility = value;
                    RaisePropertyChanged("TemplatesVisibility");
                }
            }
        }
        #endregion Visibility Properties
        #endregion //Properties

        #region Methods
        #region Setup Extensions
        private void setupScopeDataGrid()
        {
            ScopeDataGrid = new ScopeDataGridExtension(Bid);
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            ScopeDataGrid.SelectionChanged += EditTab.updateSelection;
        }
        private void setupLocationDataGrid()
        {
            LocationDataGrid = new LocationDataGridExtension(Bid);
            LocationDataGrid.DragHandler += DragOver;
            LocationDataGrid.DropHandler += Drop;
        }
        private void setupScopeCollection()
        {
            ScopeCollection = new ScopeCollectionExtension(Templates);
            ScopeCollection.DragHandler += DragOver;
            ScopeCollection.DropHandler += Drop;
        }
        private void setupEditTab()
        {
            EditTab = new EditTabExtension(Bid);
        }
        #endregion
        
        #region Commands Methods
        private void ToggleTemplatesVisibilityExecute()
        {
            if (TemplatesVisibility == Visibility.Visible)
            {
                TemplatesVisibility = Visibility.Hidden;
            }
            else if (TemplatesVisibility == Visibility.Hidden)
            {
                TemplatesVisibility = Visibility.Visible;
            }
        }

        #endregion //Commands Methods
        
        #region Drag Drop
        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            var targetCollection = dropInfo.TargetCollection;
            Type sourceType = sourceItem.GetType();
            Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (sourceItem != null && sourceType == targetType)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            Object sourceItem;
            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                sourceItem = ((TECScope)dropInfo.Data).DragDropCopy();
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    ((IList)dropInfo.TargetCollection).Add(sourceItem);
                }
                else
                {
                    ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                }
            }
            else
            {
                sourceItem = dropInfo.Data;
                int currentIndex = ((IList)dropInfo.TargetCollection).IndexOf(sourceItem);
                int removeIndex = currentIndex;
                if (dropInfo.InsertIndex < currentIndex)
                {
                    removeIndex += 1;
                }
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    ((IList)dropInfo.TargetCollection).Add(sourceItem);
                }
                else
                {
                    ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                }
                ((IList)dropInfo.TargetCollection).RemoveAt(removeIndex);
            }
        }

        #endregion

        #region Helper Methods
        private void setVisibility()
        {
            ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
            ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
            ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
            ScopeCollection.TagsVisibility = Visibility.Collapsed;
            ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
        }
        
        #endregion //Helper Methods
        #endregion //Methods
    }
}