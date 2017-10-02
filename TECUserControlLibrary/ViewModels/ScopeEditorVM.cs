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
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ScopeEditorVM : ViewModelBase, IDropTarget
    {
        //Initializer
        public ScopeEditorVM(TECBid bid, TECTemplates templates)
        {
            Bid = bid;
            Templates = templates;

            setupEditTab();
            setupScopeCollection();
            setupScopeDataGrid();
            setupLocationDataGrid();
            setupControllersPanelsTab();
            setupAddControlledScope();
            setupMiscVM();
            setupInstanceSystemVM();

            DGTabIndex = GridIndex.Systems;

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

        private SystemsSubIndex _systemsSubIndex;
        public SystemsSubIndex SystemsSubIndex
        {
            get { return _systemsSubIndex; }
            set {
                _systemsSubIndex = value;
                RaisePropertyChanged("SystemsSubIndex");
                updateVisibilityFromSystemSub(value);
            }
        }


        #region Extensions
        public SystemsVM ScopeDataGrid { get; set; }
        public LocationVM LocationDataGrid { get; set; }
        public ScopeCollectionsTabVM ScopeCollection { get; set; }
        public EditTabVM EditTab { get; set; }
        public ControllersPanelsVM ControllersPanelsTab { get; set; }
        public TypicalSystemVM TypicalSystemsTab { get; set; }
        public MiscCostsVM MiscVM { get; set; }
        public InstanceSystemVM InstanceSystemVM { get; set; }
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
            TypicalSystemsTab.Refresh(Bid);
            MiscVM.Refresh(Bid);
            InstanceSystemVM.Refresh(Bid);

            LocationDataGrid.PropertyChanged += LocationDataGrid_PropertyChanged;
        }

        #region Setup Extensions
        private void setupScopeDataGrid()
        {
            ScopeDataGrid = new SystemsVM(Bid);
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            ScopeDataGrid.SelectionChanged += EditTab.updateSelection;
            ScopeDataGrid.AssignChildDelegates();
            ScopeDataGrid.DataGridVisibilty.SubScopeLength = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemUnitPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentUnitPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentTotalPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemModifierPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemEquipmentCount = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemSubScopeCount = Visibility.Collapsed;
            
        }
        private void setupLocationDataGrid()
        {
            LocationDataGrid = new LocationVM(Bid);
            LocationDataGrid.DataGridVisibilty.SubScopeLength = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.SystemEquipmentCount = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.SystemSubScopeCount = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.SystemUnitPrice = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.EquipmentUnitPrice = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.EquipmentTotalPrice = Visibility.Collapsed;
            LocationDataGrid.DataGridVisibilty.SystemModifierPrice = Visibility.Collapsed;
            LocationDataGrid.DragHandler += DragOver;
            LocationDataGrid.DropHandler += Drop;
            ScopeDataGrid.AssignChildDelegates();
        }
        private void setupScopeCollection()
        {
            ScopeCollection = new ScopeCollectionsTabVM(Templates);
            ScopeCollection.DragHandler += DragOver;
            ScopeCollection.DropHandler += Drop;
        }
        private void setupEditTab()
        {
            EditTab = new EditTabVM(Bid);
            EditTab.DragHandler += DragOver;
            EditTab.DropHandler += Drop;
        }
        private void setupControllersPanelsTab()
        {
            ControllersPanelsTab = new ControllersPanelsVM(Bid);
            ControllersPanelsTab.SelectionChanged += EditTab.updateSelection;
        }
        private void setupAddControlledScope()
        {
            TypicalSystemsTab = new TypicalSystemVM(Bid);
            TypicalSystemsTab.SelectionChanged += EditTab.updateSelection;
            TypicalSystemsTab.AssignChildDelegates();
            TypicalSystemsTab.ComponentChanged += updateVisibilityFromSystems;
        }
        private void setupMiscVM()
        {
            MiscVM = new MiscCostsVM(Bid);
        }
        private void setupInstanceSystemVM()
        {
            InstanceSystemVM = new InstanceSystemVM(Bid);
        }
        #endregion

        #region Drag Drop
        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            UIHelpers.StandardDrop(dropInfo, Bid);
        }
        #endregion

        #region Helper Methods
        private void updateVisibility()
        {
            ScopeDataGrid.NullifySelected();

            if (DGTabIndex == GridIndex.Systems)
            {
                updateVisibilityFromSystemSub(SystemsSubIndex);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        private void updateVisibilityFromSystemSub(SystemsSubIndex value)
        {
            if (value == SystemsSubIndex.Typical)
            {
                updateVisibilityFromSystems(TypicalSystemsTab.ComponentIndex);
            }
        }
        private void updateVisibilityFromSystems(SystemComponentIndex index)
        {
            TypicalSystemsTab.ComponentVM.NullifySelected();

            var systemsVisibility = Visibility.Collapsed;
            var equipmentVisibility = Visibility.Collapsed;
            var subScopeVisibility = Visibility.Collapsed;
            var devicesVisibility = Visibility.Collapsed;
            var devicesEditVisibility = Visibility.Collapsed;
            var manufacturerVisibility = Visibility.Collapsed;
            var tagsVisibility = Visibility.Collapsed;
            var controllerEditVisibility = Visibility.Collapsed;
            var controllerVisibility = Visibility.Collapsed;
            var associatedCostsVisibility = Visibility.Collapsed;
            var panelsVisibility = Visibility.Collapsed;
            var addPanelsVisibility = Visibility.Collapsed;
            var miscCostsVisibility = Visibility.Collapsed;
            var miscWiringVisibility = Visibility.Collapsed;


            if (TypicalSystemsTab.SelectedSystem == null)
            {
                systemsVisibility = Visibility.Visible;
            }
            else
            {
                if (index == SystemComponentIndex.Controllers)
                {
                    systemsVisibility = Visibility.Visible;
                    controllerVisibility = Visibility.Visible;
                    panelsVisibility = Visibility.Visible;
                    associatedCostsVisibility = Visibility.Visible;
                }
                else if (index == SystemComponentIndex.Equipment)
                {
                    associatedCostsVisibility = Visibility.Visible;
                    systemsVisibility = Visibility.Visible;
                    equipmentVisibility = Visibility.Visible;
                    subScopeVisibility = Visibility.Visible;
                    devicesVisibility = Visibility.Visible;
                }
                else if (index == SystemComponentIndex.Misc)
                {
                    EditTab.updateSelection(TypicalSystemsTab.SelectedSystem);
                    systemsVisibility = Visibility.Visible;
                    miscWiringVisibility = Visibility.Visible;
                    miscCostsVisibility = Visibility.Visible;
                    associatedCostsVisibility = Visibility.Visible;
                }
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