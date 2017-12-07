using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
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

        public event Action<TECObject> Selected;

        public GlobalConnectionsVM(TECBid bid, ChangeWatcher watcher)
        {
            UnconnectedSystems = new ObservableCollection<TECSystem>();
            GlobalControllers = new ObservableCollection<TECController>();

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
                    
                }
            }
        }
        
        private void handleSelectedControllerChanged()
        {
            ConnectedSubScope.Clear();

            foreach(TECSubScopeConnection ssConnect in SelectedController?.ChildrenConnections.Where(
                (connection) => connection is TECSubScopeConnection))
            {
                ConnectedSubScope.Add(ssConnect);
            }
        }
        private void handleSelectedSystemChanged()
        {
            UnconnectedEquipment.Clear();
            UnconnectedSubScope.Clear();
            SelectedEquipment = null;

            foreach(TECEquipment equip in SelectedSystem.Equipment)
            {
                bool equipHasUnconnected = false;
                foreach (TECSubScope ss in equip.SubScope)
                {
                    if (!ss.IsNetwork && ss.Connection == null)
                    {
                        equipHasUnconnected = true;
                        break;
                    }
                }
                if (equipHasUnconnected)
                {
                    UnconnectedEquipment.Add(equip);
                }
            }
        }
        private void handleSelectedEquipmentChanged()
        {
            UnconnectedSubScope.Clear();

            foreach(TECSubScope ss in SelectedEquipment.SubScope)
            {
                if (!ss.IsNetwork && ss.Connection == null)
                {
                    UnconnectedSubScope.Add(ss);
                }
            }
        }

        private bool systemHasUnconnected(TECSystem sys)
        {
            bool sysHasUnconnected = false;
            foreach (TECEquipment equip in sys.Equipment)
            {
                foreach (TECSubScope ss in equip.SubScope)
                {
                    if (!ss.IsNetwork && ss.Connection == null)
                    {
                        sysHasUnconnected = true;
                        break;
                    }
                }
                if (sysHasUnconnected)
                {
                    break;
                }
            }
            return sysHasUnconnected;
        }

        private void handleInstanceChanged(TECChangedEventArgs args)
        {
            Change change = args.Change;
            object obj = args.Value;
            TECObject sender = args.Sender;

            if (change == Change.Add)
            {
                if (sender is TECBid && obj is TECController controller)
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
            }
        }
    }
}
