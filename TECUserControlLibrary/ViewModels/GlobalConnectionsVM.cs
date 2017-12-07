using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels
{
    public class GlobalConnectionsVM : ViewModelBase, IDropTarget
    {
        private TECController _selectedController;
        private TECSystem _selectedSystem;
        private TECEquipment _selectedEquipment;
        private TECSubScopeConnection _selectedConnectedSubScope;
        private TECSubScope _selectedUnconnectedSubScope;

        public ObservableCollection<TECController> GlobalControllers { get; }
        public ObservableCollection<TECSubScopeConnection> ConnectedSubScope { get; }
        public ObservableCollection<TECSystem> UnconnectedSystems { get; }
        public ObservableCollection<TECEquipment> UnconnectedEquipment { get; }
        public ObservableCollection<TECSubScope> UnconnectedSubScope { get; }

        public TECController SelectedController
        {
            get
            {
                return _selectedController;
            }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
                handleSelectedControllerChanged();
                Selected?.Invoke(SelectedController);
            }
        }
        public TECSystem SelectedSystem
        {
            get
            {
                return _selectedSystem;
            }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
                handleSelectedSystemChanged();
                Selected?.Invoke(SelectedSystem);
            }
        }
        public TECEquipment SelectedEquipment
        {
            get
            {
                return _selectedEquipment;
            }
            set
            {
                _selectedEquipment = value;
                RaisePropertyChanged("SelectedEquipment");
                handleSelectedEquipmentChanged();
                Selected?.Invoke(SelectedEquipment);
            }
        }
        public TECSubScopeConnection SelectedConnectedSubScope
        {
            get
            {
                return _selectedConnectedSubScope;
            }
            set
            {
                _selectedConnectedSubScope = value;
                RaisePropertyChanged("SelectedConnectedSubScope");
                Selected?.Invoke(SelectedConnectedSubScope);
            }
        }
        public TECSubScope SelectedUnconnectedSubScope
        {
            get
            {
                return _selectedUnconnectedSubScope;
            }
            set
            {
                _selectedUnconnectedSubScope = value;
                RaisePropertyChanged("SelectedUnconnectedSubScope");
                Selected?.Invoke(SelectedUnconnectedSubScope);
            }
        }

        public RelayCommand<TECSubScopeConnection> DisconnectSubScopeCommand { get; }

        public event Action<TECObject> Selected;

        public GlobalConnectionsVM(TECBid bid, ChangeWatcher watcher)
        {
            GlobalControllers = new ObservableCollection<TECController>();
            ConnectedSubScope = new ObservableCollection<TECSubScopeConnection>();
            UnconnectedSystems = new ObservableCollection<TECSystem>();
            UnconnectedEquipment = new ObservableCollection<TECEquipment>();
            UnconnectedSubScope = new ObservableCollection<TECSubScope>();

            DisconnectSubScopeCommand = new RelayCommand<TECSubScopeConnection>(disconnectSubScopeExecute);

            filterSystems(bid);

            foreach(TECController controller in bid.Controllers)
            {
                GlobalControllers.Add(controller);
            }

            watcher.InstanceChanged += handleInstanceChanged;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void Drop(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        private void filterSystems(TECBid bid)
        {
            UnconnectedSystems.Clear();
            foreach (TECTypical typ in bid.Systems)
            {
                foreach (TECSystem sys in typ.Instances)
                {
                    if (systemHasUnconnected(sys))
                    {
                        UnconnectedSystems.Add(sys);
                    }
                }
            }
        }
        
        private void handleSelectedControllerChanged()
        {
            ConnectedSubScope.Clear();

            if (SelectedController != null)
            {
                foreach (TECSubScopeConnection ssConnect in SelectedController.ChildrenConnections.Where(
                (connection) => connection is TECSubScopeConnection))
                {
                    ConnectedSubScope.Add(ssConnect);
                }
            }
        }
        private void handleSelectedSystemChanged()
        {
            UnconnectedEquipment.Clear();
            UnconnectedSubScope.Clear();
            SelectedEquipment = null;

            if (SelectedSystem != null)
            {
                foreach (TECEquipment equip in SelectedSystem.Equipment)
                {
                    if (equipHasUnconnected(equip))
                    {
                        UnconnectedEquipment.Add(equip);
                    }
                }
            }
        }
        private void handleSelectedEquipmentChanged()
        {
            UnconnectedSubScope.Clear();

            if (SelectedEquipment != null)
            {
                foreach (TECSubScope ss in SelectedEquipment.SubScope)
                {
                    if (ssIsUnconnected(ss))
                    {
                        UnconnectedSubScope.Add(ss);
                    }
                }
            }
        }

        private bool systemHasUnconnected(TECSystem sys)
        {
            foreach (TECEquipment equip in sys.Equipment)
            {
                if (equipHasUnconnected(equip))
                {
                    return true;
                }
            }
            return false;
        }
        private bool equipHasUnconnected(TECEquipment equip)
        {
            foreach (TECSubScope ss in equip.SubScope)
            {
                if (ssIsUnconnected(ss)) { return true; }
            }
            return false;
        }
        private bool ssIsUnconnected(TECSubScope ss)
        {
            return (!ss.IsNetwork && ss.Connection == null);
        }

        private void handleInstanceChanged(TECChangedEventArgs args)
        {
            Change change = args.Change;
            object obj = args.Value;
            TECObject sender = args.Sender;

            if (change == Change.Add)
            {
                if (obj is TECController controller && sender is TECBid)
                {
                    GlobalControllers.Add(controller);
                }
                else if (obj is TECSystem sys)
                {
                    if (systemHasUnconnected(sys))
                    {
                        UnconnectedSystems.Add(sys);
                    }
                }
                else if (obj is TECEquipment equip && sender == SelectedSystem)
                {
                    if (equipHasUnconnected(equip))
                    {
                        UnconnectedEquipment.Add(equip);
                    }
                }
                else if (obj is TECSubScope ss && sender == SelectedEquipment)
                {
                    if (ssIsUnconnected(ss))
                    {
                        UnconnectedSubScope.Add(ss);
                    }
                }
            }
            else if (change == Change.Remove)
            {
                if (obj is TECController controller && sender is TECBid)
                {
                    if (controller == SelectedController)
                    {
                        SelectedController = null;
                    }
                    GlobalControllers.Remove(controller);
                } 
                else if (obj is TECSystem sys && UnconnectedSystems.Contains(sys))
                {
                    if (sys == SelectedSystem)
                    {
                        SelectedSystem = null;
                    }
                    UnconnectedSystems.Remove(sys);
                }
                else if (obj is TECEquipment equip && UnconnectedEquipment.Contains(equip))
                {
                    if (equip == SelectedEquipment)
                    {
                        SelectedEquipment = null;
                    }
                    UnconnectedEquipment.Remove(equip);
                }
                else if (obj is TECSubScope ss && UnconnectedSubScope.Contains(ss))
                {
                    if (ss == SelectedUnconnectedSubScope)
                    {
                        SelectedUnconnectedSubScope = null;
                    }
                    UnconnectedSubScope.Remove(ss);
                }
            }
            else if (change == Change.Edit)
            {
                if (obj is TECSubScopeConnection newConnection)
                {
                    if (UnconnectedSubScope.Contains(newConnection.SubScope))
                    {
                        UnconnectedSubScope.Remove(newConnection.SubScope);
                    }
                    if (SelectedController.ChildrenConnections.Contains(newConnection))
                    {
                        ConnectedSubScope.Add(newConnection);
                    }

                    TECSubScopeConnection oldConnection = args.OldValue as TECSubScopeConnection;
                    if (SelectedEquipment.SubScope.Contains(oldConnection.SubScope))
                    {
                        UnconnectedSubScope.Add(oldConnection.SubScope);
                    }
                    if (ConnectedSubScope.Contains(oldConnection))
                    {
                        ConnectedSubScope.Remove(oldConnection);
                    }
                }
            }
        }

        private void disconnectSubScopeExecute(TECSubScopeConnection ssConnect)
        {
            ssConnect.ParentController.RemoveSubScope(ssConnect.SubScope);
        }
    }
}
