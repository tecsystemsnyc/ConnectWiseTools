using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Interfaces;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class SystemConnectionsVM : ViewModelBase, IDropTarget
    {
        #region Fields
        private TECSystem system;

        private readonly ObservableCollection<TECElectricalMaterial> _conduitTypes;

        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<ISubScopeConnectionItem> _subScope;
        private ObservableCollection<TECSubScope> _unconnectedSubScope;
        private TECController _selectedController;

        private UpdateConnectionVM _updateConnectionVM;
        #endregion

        #region Properties
        public IUserConfirmable ConfirmationObject { get; set; }

        public ObservableCollection<TECElectricalMaterial> ConduitTypes { get { return _conduitTypes; } }

        public ObservableCollection<TECController> Controllers
        {
            get
            {
                return _controllers;
            }
            set
            {
                _controllers = value;
                RaisePropertyChanged("Controllers");
            }
        }
        public ObservableCollection<ISubScopeConnectionItem> SubScope
        {
            get { return _subScope; }
            set
            {
                _subScope = value;
                RaisePropertyChanged("SubScope");
            }
        }

        public ObservableCollection<TECSubScope> UnconnectedSubScope
        {
            get
            {
                return _unconnectedSubScope;
            }
            set
            {
                _unconnectedSubScope = value;
                RaisePropertyChanged("UnconnectedSubScope");
            }
        }
        public TECController SelectedController
        {
            get
            {
                return _selectedController;
            }
            set
            {
                if (!anItemNeedsUpdate())
                {
                    setController();
                }
                else
                {
                    string checkUpdateString = "Some connections haven't been updated. Would you like to update these connections?";
                    bool? result = ConfirmationObject.Show(checkUpdateString);
                    if (result.HasValue)
                    {
                        if (result.Value)
                        {
                            updateNeedsUpdate();
                        }
                        else
                        {
                            setController();
                        }
                    }
                    else
                    {
                        RaisePropertyChanged("SelectedController");
                    }
                }

                void setController()
                {
                    _selectedController = value;
                    RaisePropertyChanged("SelectedController");
                    handleControllerSelected(value);
                }
            }
        }

        public UpdateConnectionVM UpdateConnectionVM
        {
            get { return _updateConnectionVM; }
            private set
            {
                _updateConnectionVM = value;
                RaisePropertyChanged("UpdateConnectionVM");
                UpdateVM?.Invoke(value);
            }
        }

        public ICommand UpdateAllCommand { get; private set; }
        public ICommand UpdateItemCommand { get; private set; }
        public bool CanLeave
        {
            get { return !anItemNeedsUpdate(); }
        }
        #endregion

        public SystemConnectionsVM(TECSystem system, IEnumerable<TECElectricalMaterial> conduitTypes)
        {
            this.system = system;
            this.ConfirmationObject = new MessageBoxService();
            _conduitTypes = new ObservableCollection<TECElectricalMaterial>(conduitTypes);
            initializeCollections();
            foreach (TECSubScope ss in system.GetAllSubScope())
            {
                if (ss.ParentConnection == null && ss.Connection == null)
                {
                    UnconnectedSubScope.Add(ss);
                }
            }
            foreach (TECController controller in system.Controllers)
            {
                Controllers.Add(controller);
            }
            UpdateAllCommand = new RelayCommand(updateAllExecute);
            UpdateItemCommand = new RelayCommand<SubScopeConnectionItem>(updateItem);
        }

        public event Action<UpdateConnectionVM> UpdateVM;

        public void DragOver(IDropInfo dropInfo)
        {
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            if (subScope != null && SelectedController != null && SelectedController.CanConnectSubScope(subScope)
                && UIHelpers.TargetCollectionIsType(dropInfo, typeof(ISubScopeConnectionItem)))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            SelectedController.AddSubScope(subScope);
            UnconnectedSubScope.Remove(subScope);
            if (system is TECTypical)
            {
                SubScope.Add(new SubScopeConnectionItem(subScope, needsUpdate: true));
            }
            else
            {
                SubScope.Add(new SubScopeConnectionItem(subScope));
            }
        }

        private void initializeCollections()
        {
            _controllers = new ObservableCollection<TECController>();
            _subScope = new ObservableCollection<ISubScopeConnectionItem>();
            _unconnectedSubScope = new ObservableCollection<TECSubScope>();
        }

        private void updateAllExecute()
        {
            updateSubScope(SubScope);
        }
        private void updateNeedsUpdate()
        {
            List<ISubScopeConnectionItem> ssNeedsUpdate = new List<ISubScopeConnectionItem>();
            foreach (ISubScopeConnectionItem item in SubScope)
            {
                if (item.NeedsUpdate)
                {
                    ssNeedsUpdate.Add(item);
                }
            }
            updateSubScope(ssNeedsUpdate);
        }
        private void updateItem(ISubScopeConnectionItem item)
        {
            updateSubScope(new List<ISubScopeConnectionItem>() { item });
        }
        private void updateSubScope(IEnumerable<ISubScopeConnectionItem> subScope)
        {
            if (system is TECTypical typical)
            {
                foreach(ISubScopeConnectionItem item in subScope)
                {
                    item.NeedsUpdate = false;
                }
                UpdateConnectionVM = new UpdateConnectionVM(subScope, typical);
            }
            else
            {
                throw new InvalidOperationException("Can not update when system is not typical.");
            }
        }

        private bool anItemNeedsUpdate()
        {
            foreach (ISubScopeConnectionItem item in SubScope)
            {
                if (item.NeedsUpdate)
                {
                    return true;
                }
            }
            return false;
        }
        private void handleControllerSelected(TECController controller)
        {
            ObservableCollection<ISubScopeConnectionItem> ssItems = new ObservableCollection<ISubScopeConnectionItem>();
            if(controller != null)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection is TECSubScopeConnection ssConnect)
                    {
                        SubScopeConnectionItem ssConnectItem = new SubScopeConnectionItem(ssConnect.SubScope);
                        ssConnectItem.NeedsUpdateChanged += () =>
                        {
                            RaisePropertyChanged("CanLeave");
                        };
                        ssItems.Add(ssConnectItem);
                    }
                }
                SubScope = ssItems;
            }
            RaisePropertyChanged("CanLeave");
        }
    }
}
