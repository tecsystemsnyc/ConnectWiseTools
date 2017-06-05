﻿using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SystemComponentVM : ViewModelBase, IDropTarget
    {
        #region Properties
        private TECSystem _selectedSystem;
        public TECSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                _selectedSystem = value;
                refreshSelected(value);
                RaisePropertyChanged("SelectedSystem");
            }
        }

        private void refreshSelected(TECSystem selected)
        {
            if (ControllersPanelsVM != null)
            {
                if (selected != null)
                {
                    ControllersPanelsVM.Refresh(SelectedSystem);
                }
                else
                {
                    ControllersPanelsVM.Refresh(new TECSystem());
                }
            }
            if (ConnectionVM != null)
            {
                ConnectionVM.SelectedSystem = selected;
            }
        }

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
        
        #region VM Extenstions
        public EquipmentVM ScopeDataGrid { get; set; }
        public ControllersPanelsVM ControllersPanelsVM { get; set; }
        public ConnectionVM ConnectionVM { get; set; }
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
        public SystemComponentVM(TECScopeManager scopeManager)
        {
            if (scopeManager is TECBid)
            {
                _bid = scopeManager as TECBid;
            }
            else
            {
                _templates = scopeManager as TECTemplates;
            }
            _selectedSystem = null;
            setupVMs(scopeManager);
        }

        #endregion

        #region Methods
        public void Refresh(TECScopeManager scopeManager)
        {
            if (scopeManager is TECBid)
            {
                Bid = scopeManager as TECBid;
            }
            else
            {
                Templates = scopeManager as TECTemplates;
            }
            ScopeDataGrid.Refresh(scopeManager);
            ConnectionVM.Refresh(scopeManager);
        }
        private void setupVMs(TECScopeManager scopeManager)
        {
            ScopeDataGrid = new EquipmentVM(scopeManager);
            ScopeDataGrid.SelectionChanged += SelectionChanged;
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            ScopeDataGrid.AssignChildDelegates();
            ScopeDataGrid.DataGridVisibilty.SystemLocation = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemSubScopeCount = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemEquipmentCount = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemModifierPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemUnitPrice = Visibility.Collapsed;
            ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
            ScopeDataGrid.ChildVM.DataGridVisibilty.EquipmentLocation = Visibility.Collapsed;
            ScopeDataGrid.ChildVM.DataGridVisibilty.EquipmentUnitPrice = Visibility.Collapsed;
            ScopeDataGrid.ChildVM.DataGridVisibilty.EquipmentTotalPrice = Visibility.Collapsed;
            ScopeDataGrid.ChildVM.DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
            ScopeDataGrid.ChildVM.ChildVM.DataGridVisibilty.SubScopeLocation = Visibility.Collapsed;
            ScopeDataGrid.ChildVM.ChildVM.DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;

            ControllersPanelsVM = new ControllersPanelsVM(new TECSystem());
            ConnectionVM = new ConnectionVM(scopeManager);
        }
        
        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            Type sourceType = sourceItem.GetType();

            var targetCollection = dropInfo.TargetCollection;
            if (sourceType == typeof(TECController) && SelectedSystem != null)
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
            Type sourceType = dropInfo.Data.GetType();
            Object sourceItem;
            sourceItem = dropInfo.Data;

            if (dropInfo.Data is TECSystem)
            {
                Dictionary<Guid, Guid> guidDictionary = new Dictionary<Guid, Guid>();
                var controlledScopeToAdd = new TECSystem(dropInfo.Data as TECSystem, guidDictionary);
                if (Bid != null)
                {
                    ModelLinkingHelper.LinkSystem(controlledScopeToAdd, Bid, guidDictionary);
                    Bid.Systems.Add(controlledScopeToAdd);
                }
                else if (Templates != null)
                {
                    ModelLinkingHelper.LinkSystem(controlledScopeToAdd, Templates, guidDictionary);
                    Templates.SystemTemplates.Add(controlledScopeToAdd);
                }
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

        #endregion
    }
}