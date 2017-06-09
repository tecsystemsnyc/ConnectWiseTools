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

        public ObservableCollection<TECConnectionType> WireTypes { get; private set; }
        public ObservableCollection<TECConduitType> ConduitTypes { get; private set; }

        public IOType SelectedIO { get; set; }
        public TECConnectionType SelectedWire { get; set; }

        public ICommand AddConnectionCommand { get; private set; }

        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public TECConnectionType NoneWireType { get; set; }
        public TECConduitType NoneConduitType { get; set; }
        #endregion

        public NetworkControllerVM(Visibility detailsVisibility, TECBid bid = null)
        {
            if (detailsVisibility == Visibility.Visible)
            {
                if (bid != null)
                {
                    setupNoneTypes();

                    populateWireAndConduitTypes(bid);

                    AddConnectionCommand = new RelayCommand<TECController>(x => AddConnectionExecute(x), x => CanAddConnectionExecute());
                }
                else
                {
                    throw new NullReferenceException("Bid cannot be null when details visibility is visible.");
                }
            }
            DetailsVisibility = detailsVisibility;
            NetworkControllers = new ObservableCollection<NetworkController>();
        }

        public void Refresh(TECBid bid = null)
        {
            if (DetailsVisibility == Visibility.Visible)
            {
                if (bid != null)
                {
                    populateWireAndConduitTypes(bid);
                }
                else
                {
                    throw new NullReferenceException("Bid cannot be null when details visibility is visible.");
                }
            }
            NetworkControllers = new ObservableCollection<NetworkController>();
        }

        public void AddController(TECController controller)
        {
            NetworkControllers.Add(new NetworkController(controller));
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
                netController.PossibleParents = possibleParents;
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

        private void setupNoneTypes()
        {
            NoneWireType = new TECConnectionType();
            NoneWireType.Name = "None";
            NoneConduitType = new TECConduitType();
            NoneConduitType.Name = "None";
        }
        private void populateWireAndConduitTypes(TECBid bid)
        {
            WireTypes = new ObservableCollection<TECConnectionType>();
            WireTypes.Add(NoneWireType);
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
