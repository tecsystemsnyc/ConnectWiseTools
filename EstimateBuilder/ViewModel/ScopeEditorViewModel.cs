using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary;
using TECUserControlLibrary.Models;
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

            setupEditTab();
            setupScopeCollection();
            setupScopeDataGrid();
            setupLocationDataGrid();
            setupControllersPanelsTab();
            setupAddControlledScope();

            DGTabIndex = GridIndex.AddControlledScope;

            TemplatesVisibility = Visibility.Visible;

            LocationDataGrid.PropertyChanged += LocationDataGrid_PropertyChanged;
        }
        
        #region Properties

        private GridIndex _dGTabIndex;
        public GridIndex DGTabIndex
        {
            get { return _dGTabIndex; }
            set
            {
                _dGTabIndex = value;
                RaisePropertyChanged("DGTabIndex");
                updateVisibility();
            }
        }

        #region Extensions
        public ScopeDataGridExtension ScopeDataGrid { get; set; }
        public LocationDataGridExtension LocationDataGrid { get; set; }
        public ScopeCollectionExtension ScopeCollection { get; set; }
        public EditTabExtension EditTab { get; set; }
        public ControllersPanelsViewModel ControllersPanelsTab { get; set; } 
        public AddControlledScopeExtension AddControlledScopeTab { get; set; }
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
        public void Refresh(TECBid bid, TECTemplates templates)
        {
            Bid = bid;
            Templates = templates;

            EditTab.Refresh(Bid);
            ScopeCollection.Refresh(Templates);
            ScopeDataGrid.Refresh(Bid);
            LocationDataGrid.Refresh(Bid);
            ControllersPanelsTab.Refresh(Bid);
            AddControlledScopeTab.Refresh(Bid);
            
            LocationDataGrid.PropertyChanged += LocationDataGrid_PropertyChanged;
        }

        #region Setup Extensions
        private void setupScopeDataGrid()
        {
            ScopeDataGrid = new ScopeDataGridExtension(Bid);
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            ScopeDataGrid.DataGridVisibilty.SubScopeLength = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemUnitPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentUnitPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentTotalPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemModifierPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;
            ScopeDataGrid.SelectionChanged += EditTab.updateSelection;
        }
        private void setupLocationDataGrid()
        {
            LocationDataGrid = new LocationDataGridExtension(Bid);
            LocationDataGrid.DataGridVisibilty.SystemUnitPrice = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.EquipmentUnitPrice = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.EquipmentTotalPrice = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.SystemModifierPrice = Visibility.Collapsed;
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
            EditTab.DragHandler += DragOver;
            EditTab.DropHandler += Drop;
        }
        private void setupControllersPanelsTab()
        {
            ControllersPanelsTab = new ControllersPanelsViewModel(Bid);
            ControllersPanelsTab.SelectionChanged += EditTab.updateSelection;
        }
        private void setupAddControlledScope()
        {
            AddControlledScopeTab = new AddControlledScopeExtension(Bid);
            AddControlledScopeTab.SelectionChanged += EditTab.updateSelection;
            AddControlledScopeTab.ScopeDataGrid.SelectionChanged += EditTab.updateSelection;
        }
        #endregion
        
        #region Drag Drop
        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            Type sourceType;
            if (sourceItem is IList && ((IList)sourceItem).Count > 0)
            {
                sourceType = ((IList)sourceItem)[0].GetType();
            }else
            {
                sourceType = sourceItem.GetType();
            }
            
            var targetCollection = dropInfo.TargetCollection;
            if (targetCollection.GetType().GetTypeInfo().GenericTypeArguments.Length > 0)
            {
                Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];
                bool isControllerInPanel = sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel);

                if (sourceItem != null && sourceType == targetType || isControllerInPanel)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }
        }
        public void Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            Type sourceType = dropInfo.Data.GetType();
            Type targetType = dropInfo.TargetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];
            
            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            {
                if(sourceItem is IList)
                {
                    var outSource = new List<object>();
                    foreach(object item in ((IList)sourceItem))
                    {
                        outSource.Add(((TECScope)item).DragDropCopy());
                    }
                    sourceItem = outSource;
                }else
                {
                    sourceItem = ((TECScope)dropInfo.Data).DragDropCopy();
                }

                if ((sourceType == typeof(TECController) && targetType == typeof(ControllerInPanel)))
                {
                    var controllerInPanel = new ControllerInPanel(sourceItem as TECController, null);
                    Bid.Controllers.Add(sourceItem as TECController);
                    sourceItem = controllerInPanel;
                }
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    if(sourceItem is IList)
                    {
                        foreach(object item in ((IList)sourceItem))
                        {
                            ((IList)dropInfo.TargetCollection).Add(item);
                        }
                    }
                    else
                    {
                        ((IList)dropInfo.TargetCollection).Add(sourceItem);
                    }
                }
                else
                {
                    if (sourceItem is IList)
                    {
                        var x = dropInfo.InsertIndex;
                        foreach (object item in ((IList)sourceItem))
                        {
                            ((IList)dropInfo.TargetCollection).Insert(x, item);
                            x += 1;
                        }
                    }
                    else
                    {
                        ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);

                    }
                }
            }
            else
            {
                sourceItem = dropInfo.Data;
                int currentIndex = ((IList)dropInfo.TargetCollection).IndexOf(sourceItem);
                int finalIndex = dropInfo.InsertIndex;
                
                if (sourceItem is IList)
                {
                    currentIndex = ((IList)dropInfo.TargetCollection).IndexOf(((IList)sourceItem)[0]);
                    if (dropInfo.InsertIndex > currentIndex)
                    {
                        finalIndex -= 1;
                    }
                    if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                    {
                        finalIndex = ((IList)dropInfo.TargetCollection).Count - 1;
                    }
                    var x = 0;
                    foreach (object item in ((IList)sourceItem))
                    {
                        currentIndex = ((IList)dropInfo.TargetCollection).IndexOf(((IList)sourceItem)[x]);
                        ((dynamic)dropInfo.TargetCollection).Move(currentIndex, finalIndex);
                        x += 1;
                    }
                }
                else
                {
                    if (dropInfo.InsertIndex > currentIndex)
                    {
                        finalIndex -= 1;
                    }
                    if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                    {
                        finalIndex = ((IList)dropInfo.TargetCollection).Count - 1;
                    }
                    ((dynamic)dropInfo.TargetCollection).Move(currentIndex, finalIndex);
                }
                
            }
        }
        #endregion

        #region Helper Methods
        private void updateVisibility()
        {
            if (DGTabIndex == GridIndex.Scope)
            {
                ScopeCollection.SystemsVisibility = Visibility.Visible;
                ScopeCollection.EquipmentVisibility = Visibility.Visible;
                ScopeCollection.SubScopeVisibility = Visibility.Visible;
                ScopeCollection.DevicesVisibility = Visibility.Visible;
                ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                ScopeCollection.TagsVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                ScopeCollection.PanelsVisibility = Visibility.Collapsed;
                ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

                ScopeCollection.TabIndex = ScopeCollectionIndex.System;
            }
            else if (DGTabIndex == GridIndex.DDC)
            {
                ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                ScopeCollection.DevicesVisibility = Visibility.Collapsed;
                ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                ScopeCollection.TagsVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerVisibility = Visibility.Visible;
                ScopeCollection.AssociatedCostsVisibility = Visibility.Visible;
                ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                ScopeCollection.PanelsVisibility = Visibility.Visible;
                ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

                ScopeCollection.TabIndex = ScopeCollectionIndex.Controllers;
            }
            else if (DGTabIndex == GridIndex.Location)
            {
                if (LocationDataGrid.SelectedScopeType == LocationScopeType.System)
                {
                    ScopeCollection.EquipmentVisibility = Visibility.Visible;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.Equipment;
                }
                else if (LocationDataGrid.SelectedScopeType == LocationScopeType.Equipment)
                {
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.SubScope;
                }
                else if (LocationDataGrid.SelectedScopeType == LocationScopeType.SubScope)
                {
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.Devices;
                }
                else
                {
                    throw new NotImplementedException();
                }
                ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                ScopeCollection.TagsVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                ScopeCollection.PanelsVisibility = Visibility.Collapsed;
                ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;
            }
            else if (DGTabIndex == GridIndex.Misc)
            {
                ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                ScopeCollection.DevicesVisibility = Visibility.Collapsed;
                ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                ScopeCollection.TagsVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                ScopeCollection.PanelsVisibility = Visibility.Collapsed;
                ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                ScopeCollection.MiscCostVisibility = Visibility.Visible;
                ScopeCollection.MiscWiringVisibility = Visibility.Visible;

                ScopeCollection.TabIndex = ScopeCollectionIndex.MiscCosts;
            }
            else if (DGTabIndex == GridIndex.AddControlledScope)
            {
                ScopeCollection.SystemsVisibility = Visibility.Visible;
                ScopeCollection.EquipmentVisibility = Visibility.Visible;
                ScopeCollection.SubScopeVisibility = Visibility.Visible;
                ScopeCollection.DevicesVisibility = Visibility.Visible;
                ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                ScopeCollection.TagsVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                ScopeCollection.ControllerVisibility = Visibility.Visible;
                ScopeCollection.AssociatedCostsVisibility = Visibility.Visible;
                ScopeCollection.ControlledScopeVisibility = Visibility.Visible;
                ScopeCollection.PanelsVisibility = Visibility.Visible;
                ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

                ScopeCollection.TabIndex = ScopeCollectionIndex.ControlledScope;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        #endregion //Helper Methods

        #region Event Handlers
        private void LocationDataGrid_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedScopeType")
            {
                updateVisibility();
            }
        }
        #endregion
        #endregion //Methods
    }
}