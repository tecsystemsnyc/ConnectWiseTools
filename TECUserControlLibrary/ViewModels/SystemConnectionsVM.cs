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
        private ObservableCollection<SubScopeConnectionItem> _subScope;
        private ObservableCollection<TECSubScope> _unconnectedSubScope;
        private TECController _selectedController;

        private UpdateConnectionVM _updateConnectionVM;
        #endregion

        #region Properties
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
        public ObservableCollection<SubScopeConnectionItem> SubScope
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
                if (selectedControllerCanSwitch())
                {
                    setController();
                }
                else
                {
                    string checkUpdateString = "Some connections haven't been updated. Would you like to update these connections?";
                    MessageBoxResult result = MessageBox.Show(checkUpdateString, "Update?", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        updateNeedsUpdate();
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        setController();
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
                RaisePropertyChanged("UpdateVM");
                UpdateVM?.Invoke(value);
            }
        }

        public ICommand UpdateAllCommand { get; private set; }
        #endregion

        public SystemConnectionsVM(TECSystem system, IEnumerable<TECElectricalMaterial> conduitTypes)
        {
            this.system = system;
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
        }

        public event Action<UpdateConnectionVM> UpdateVM;

        public void DragOver(IDropInfo dropInfo)
        {
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            if (subScope != null && SelectedController != null && SelectedController.CanConnectSubScope(subScope)
                && UIHelpers.TargetCollectionIsType(dropInfo, typeof(SubScopeConnectionItem)))
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
            _subScope = new ObservableCollection<SubScopeConnectionItem>();
            _unconnectedSubScope = new ObservableCollection<TECSubScope>();
        }

        private void updateAllExecute()
        {
            updateSubScope(SubScope);
        }
        private void updateNeedsUpdate()
        {
            List<SubScopeConnectionItem> ssNeedsUpdate = new List<SubScopeConnectionItem>();
            foreach (SubScopeConnectionItem item in SubScope)
            {
                if (item.NeedsUpdate)
                {
                    ssNeedsUpdate.Add(item);
                }
            }
            updateSubScope(ssNeedsUpdate);
        }
        private void updateItem(SubScopeConnectionItem item)
        {
            updateSubScope(new List<SubScopeConnectionItem>() { item });
        }
        private void updateSubScope(IEnumerable<SubScopeConnectionItem> subScope)
        {
            if (system is TECTypical typical)
            {
                UpdateConnectionVM = new UpdateConnectionVM(subScope, typical);
            }
            else
            {
                throw new InvalidOperationException("Can not update when system is not typical.");
            }
        }

        private bool selectedControllerCanSwitch()
        {
            foreach (SubScopeConnectionItem item in SubScope)
            {
                if (item.NeedsUpdate)
                {
                    return false;
                }
            }
            return true;
        }
        private void handleControllerSelected(TECController controller)
        {
            ObservableCollection<SubScopeConnectionItem> ssItems = new ObservableCollection<SubScopeConnectionItem>();
            if(controller != null)
            {
                foreach (TECConnection connection in controller.ChildrenConnections)
                {
                    if (connection is TECSubScopeConnection ssConnect)
                    {
                        ssItems.Add(new SubScopeConnectionItem(ssConnect.SubScope));
                    }
                }
                SubScope = ssItems;
            }
        }
    }
}
