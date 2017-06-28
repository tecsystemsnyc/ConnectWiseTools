using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
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
    public class InstanceSystemVM : ViewModelBase, IDropTarget
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
            ComponentVM.SelectedSystem = selected;
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

        private ObservableCollection<TECSystem> _scopeSource;
        public ObservableCollection<TECSystem> ScopeSource
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

        #region VM Extenstions
        public SystemComponentVM ComponentVM { get; set; }
        public SystemsVM SystemsVM {get; set;}
        #endregion

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion
        #endregion

        #region Constructors
        public InstanceSystemVM(TECScopeManager scopeManager)
        {
            if (scopeManager is TECBid)
            {
                _bid = scopeManager as TECBid;
                _scopeSource = Bid.Systems;
            }
            else
            {
                _templates = scopeManager as TECTemplates;
                _scopeSource = Templates.SystemTemplates;
            }
            _selectedSystem = null;
            DebugVisibility = Visibility.Collapsed;
            InstanceVisibility = Visibility.Collapsed;
            setupVMs(scopeManager);
        }
        #endregion

        #region Methods
        public void Refresh(TECScopeManager scopeManager)
        {
            if (scopeManager is TECBid)
            {
                Bid = scopeManager as TECBid;
                ScopeSource = Bid.Systems;
            }
            else
            {
                Templates = scopeManager as TECTemplates;
                ScopeSource = Templates.SystemTemplates;
            }
            SelectedSystem = null;
            ComponentVM.Refresh(scopeManager);
            SystemsVM.Refresh(scopeManager);
        }
        private void setupVMs(TECScopeManager scopeManager)
        {
            ComponentVM = new SystemComponentVM(scopeManager, false);
            ComponentVM.SelectionChanged += SelectionChanged;

            SystemsVM = new SystemsVM(scopeManager);
            SystemsVM.SelectionChanged += updateSelectedSystem;
            SystemsVM.DataGridVisibilty.SystemExpander = Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemPointNumber = Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemModifierPrice = Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemEquipmentCount = Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemUnitPrice = Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemSubScopeCount = Visibility.Collapsed;
            SystemsVM.DragHandler += DragHandler;
            SystemsVM.DropHandler += DropHandler;
            SystemsVM.AssignChildDelegates();
           
        }

        private void updateSelectedSystem(object item)
        {
            if(item is TECSystem)
            {
                SelectedSystem = item as TECSystem;
            }
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
                TECScopeManager scopeManager;
                if (Templates != null)
                {
                    scopeManager = Templates;
                }
                else
                {
                    scopeManager = Bid;
                }
                UIHelpers.StandardDrop(dropInfo, scopeManager);
            }
        }
        #endregion
    }
}