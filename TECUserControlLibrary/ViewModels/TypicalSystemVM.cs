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
using System.Collections.Generic;
using TECUserControlLibrary.Utilities;
using EstimatingLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TypicalSystemVM : ViewModelBase, IDropTarget
    {
        #region Properties
        private TECTypical _selectedSystem;
        public TECTypical SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                _selectedSystem = value;
                refreshSelected(value);
                SelectionChanged?.Invoke(value);
                RaisePropertyChanged("SelectedSystem");
            }
        }

        private void refreshSelected(TECSystem selected)
        {
            ComponentVM.SelectedSystem = selected;
            ComponentChanged?.Invoke(ComponentIndex);
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

        private TECSystem _selectedChild;
        public TECSystem SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                _selectedChild = value;
                RaisePropertyChanged("SelectedChild");
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

        private ObservableCollection<TECTypical> _scopeSource;
        public ObservableCollection<TECTypical> ScopeSource
        {
            get { return _scopeSource; }
            set
            {
                _scopeSource = value;
                RaisePropertyChanged("ScopeSource");
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
        public ICommand DeleteControlledScopeCommand { get; private set; }
        
        private Visibility _debugVisibility;
        public Visibility DebugVisibility
        {
            get { return _debugVisibility; }
            set
            {
                _debugVisibility = value;
                RaisePropertyChanged("DebugVisibility");
            }
        }

        private Visibility _instanceVisibility;
        public Visibility InstanceVisibility
        {
            get { return _instanceVisibility; }
            set
            {
                _instanceVisibility = value;
                RaisePropertyChanged("InstanceVisibility");
            }
        }

        private SystemComponentIndex _componentIndex;
        public SystemComponentIndex ComponentIndex
        {
            get { return _componentIndex; }
            set
            {
                _componentIndex = value;
                RaisePropertyChanged("ComponentIndex");
                ComponentChanged?.Invoke(value);
            }
        }

        #region VM Extenstions
        public SystemComponentVM ComponentVM { get; set; }
        #endregion

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        public Action<SystemComponentIndex> ComponentChanged;
        #endregion
        #endregion

        #region Constructors
        public TypicalSystemVM(TECBid bid)
        {
            
            _bid = bid;
            _scopeSource = Bid.Systems;
            InstanceVisibility = Visibility.Visible;
            AddControlledScopeCommand = new RelayCommand(addControlledScopeExecute, addControlledScopeCanExecute);
            DeleteControlledScopeCommand = new RelayCommand(deleteControlledScopeExecute, deleteControlledScopeCanExecute);
            _selectedSystem = null;
            DebugVisibility = Visibility.Collapsed;
            
            setupVMs(bid);
        }
        public TypicalSystemVM(TECTemplates templates)
        {
            
            _templates = templates;
            _scopeSource = new ObservableCollection<TECTypical>();
            InstanceVisibility = Visibility.Visible;
            AddControlledScopeCommand = new RelayCommand(addControlledScopeExecute, addControlledScopeCanExecute);
            DeleteControlledScopeCommand = new RelayCommand(deleteControlledScopeExecute, deleteControlledScopeCanExecute);
            _selectedSystem = null;
            DebugVisibility = Visibility.Collapsed;

            setupVMs(templates);
        }

        #endregion

        #region Methods
        public void Refresh(TECBid bid)
        {
            Bid = bid;
            ScopeSource = Bid.Systems;
            ComponentVM.Refresh(bid);
        }
        private void setupVMs(TECScopeManager scopeManager)
        {
            ComponentVM = new SystemComponentVM(scopeManager);
        }
        public void AssignChildDelegates()
        {
            ComponentVM.SelectionChanged += SelectionChanged;
            ComponentVM.AssignChildDelegates();
        }

        private void addControlledScopeExecute()
        {
            for (int x = 0; x < ControlledScopeQuantity; x++)
            {
                SelectedSystem.AddInstance(Bid);
            }
            ControlledScopeQuantity = 0;
        }
        private bool addControlledScopeCanExecute()
        {
            if (SelectedSystem != null && ControlledScopeQuantity > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool deleteControlledScopeCanExecute()
        {
            if (SelectedChild != null && SelectedSystem.Instances.Contains(SelectedChild))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void deleteControlledScopeExecute()
        {
            SelectedSystem.Instances.Remove(SelectedChild);
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
            TECScopeManager scopeManager;
            if (Templates != null)
            {
                scopeManager = Templates;
            }
            else
            {
                scopeManager = Bid;
            }
            Type sourceType = dropInfo.Data.GetType();
            Object sourceItem;
            sourceItem = dropInfo.Data;

            if (dropInfo.Data is TECSystem)
            {
                Dictionary<Guid, Guid> guidDictionary = new Dictionary<Guid, Guid>();
                var controlledScopeToAdd = new TECTypical(dropInfo.Data as TECSystem, guidDictionary);
                if(Bid != null)
                {
                    ModelLinkingHelper.LinkSystem(controlledScopeToAdd, Bid, guidDictionary);
                    Bid.Systems.Add(controlledScopeToAdd);
                } else if(Templates != null)
                {
                    ModelLinkingHelper.LinkSystem(controlledScopeToAdd, Templates, guidDictionary);
                    Templates.SystemTemplates.Add(controlledScopeToAdd);
                }
            }
            else if (dropInfo.Data is TECController)
            {
                UIHelpers.ControllerInPanelDrop(dropInfo, SelectedSystem.AddController, scopeManager);
            }
            else
            {
                
                UIHelpers.StandardDrop(dropInfo, scopeManager);
            }
        }
        
        #endregion
    }
}