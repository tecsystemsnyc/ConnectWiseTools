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

namespace TECUserControlLibrary.ViewModels
{
    public class NetworkControllerVM : ViewModelBase, IDropTarget
    {
        #region Properties
        private ObservableCollection<NetworkController> _networkControllers;
        public ObservableCollection<NetworkController> NetworkControllers
        {
            get { return _networkControllers; }
            set
            {
                _networkControllers = value;
                RaisePropertyChanged("NetworkControllers");
            }
        }

        public Visibility DetailsVisibility { get; private set; }

        private ObservableCollection<TECConnectionType> _wireTypes;
        public ObservableCollection<TECConnectionType> WireTypes
        {
            get { return _wireTypes; }
            private set
            {
                _wireTypes = value;
                RaisePropertyChanged("WireTypes");
            }
        }
        private ObservableCollection<TECConduitType> _conduitTypes;
        public ObservableCollection<TECConduitType> ConduitTypes
        {
            get { return _conduitTypes; }
            set
            {
                _conduitTypes = value;
                RaisePropertyChanged("ConduitTypes");
            }
        }

        public IOType SelectedIO { get; set; }
        public TECConnectionType SelectedWire { get; set; }

        private TECController _selectedChild;
        public TECController SelectedChild
        {
            get
            {
                return _selectedChild;
            }
            set
            {
                _selectedChild = value;
                RaisePropertyChanged("SelectedChild");
            }
        }

        public ICommand AddConnectionCommand { get; private set; }
        public ICommand RemoveControllerCommand { get; private set; }

        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;
        
        public TECConduitType NoneConduitType { get; set; }
        public TECController NoneController { get; set; }

        private TECConnectionType defaultWireType;
        #endregion

        public NetworkControllerVM(Visibility detailsVisibility, TECBid bid)
        {
            if (detailsVisibility == Visibility.Visible)
            {
                setupNoneTypes();

                populateWireAndConduitTypes(bid);

                AddConnectionCommand = new RelayCommand<TECController>(x => AddConnectionExecute(x), x => CanAddConnectionExecute());
                RemoveControllerCommand = new RelayCommand<TECController>(x => RemoveControllerExecute(x), x => CanRemoveControllerExecute());
            }
            DetailsVisibility = detailsVisibility;
            NetworkControllers = new ObservableCollection<NetworkController>();
            if (bid.Catalogs.ConnectionTypes.Count > 0)
            {
                defaultWireType = bid.Catalogs.ConnectionTypes[0];
            }
        }

        public void Refresh(TECBid bid)
        {
            if (DetailsVisibility == Visibility.Visible)
            {
                populateWireAndConduitTypes(bid);
            }
            NetworkControllers = new ObservableCollection<NetworkController>();
            if (bid.Catalogs.ConnectionTypes.Count > 0)
            {
                defaultWireType = bid.Catalogs.ConnectionTypes[0];
            }
        }

        public void AddController(TECController controller)
        {
            NetworkControllers.Add(new NetworkController(controller, defaultWireType));
        }

        public void RemoveController(TECController controller)
        {
            NetworkController toRemove = null;
            foreach(NetworkController netController in NetworkControllers)
            {
                if (controller == netController.Controller)
                {
                    toRemove = netController;
                    break;
                }
            }
            if (toRemove != null)
            {
                NetworkControllers.Remove(toRemove);
            }
            else
            {
                throw new IndexOutOfRangeException("Network controller not in collection.");
            }
        }

        public void RefreshPossibleParents(ObservableCollection<NetworkController> netControllers)
        {
            ObservableCollection<TECController> possibleParents = new ObservableCollection<TECController>();
            foreach(NetworkController possibleParent in netControllers)
            {
                possibleParents.Add(possibleParent.Controller);
            }
            foreach(NetworkController netController in NetworkControllers)
            {
                netController.SetPossibleParents(possibleParents, NoneController);
            }
        }

        private void AddConnectionExecute(TECController controller)
        {
            TECNetworkConnection newConnection = new TECNetworkConnection();
            newConnection.IOType = SelectedIO;
            newConnection.ConnectionType = SelectedWire;
            newConnection.ParentController = controller;
            controller.ChildrenConnections.Add(newConnection);
        }
        private bool CanAddConnectionExecute()
        {
            if (SelectedIO != 0 && SelectedWire != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RemoveControllerExecute(TECController controller)
        {
            controller.RemoveController(SelectedChild);
            SelectedChild = null;
        }
        private bool CanRemoveControllerExecute()
        {
            return (SelectedChild != null);
        }

        private void setupNoneTypes()
        {
            NoneConduitType = new TECConduitType();
            NoneConduitType.Name = "None";
        }
        private void populateWireAndConduitTypes(TECBid bid)
        {
            WireTypes = new ObservableCollection<TECConnectionType>();
            foreach(TECConnectionType type in bid.Catalogs.ConnectionTypes)
            {
                WireTypes.Add(type);
            }

            ConduitTypes = new ObservableCollection<TECConduitType>();
            ConduitTypes.Add(NoneConduitType);
            foreach(TECConduitType type in bid.Catalogs.ConduitTypes)
            {
                ConduitTypes.Add(type);
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler.Invoke(dropInfo);
        }

        public void Drop(IDropInfo dropInfo)
        {
            DropHandler.Invoke(dropInfo);
        }
    }
}
