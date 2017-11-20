using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class NetworkVM : ViewModelBase
    {
        #region Fields
        bool isConnecting;

        private ObservableCollection<ConnectableItem> _parentables;
        private ObservableCollection<ConnectableItem> _nonParentables;

        private ConnectableItem _selectedItem;
        private ConnectableItem _selectedParentable;
        private ConnectableItem _selectedNonParentable;
        private Dictionary<INetworkConnectable, ConnectableItem> connectableDictionary;

        private ObservableCollection<TECElectricalMaterial> selectedConnectionTypes;
        private TECElectricalMaterial _selectedPotentialConnectionType;
        private TECElectricalMaterial _selectedChosenConnectionType;
        private IOType _selectedIOType;

        private TECNetworkConnection _selectedConnection;

        private INetworkConnectable _selectedChildConnectable;
        #endregion

        //Constructor
        public NetworkVM(TECBid bid, ChangeWatcher cw)
        {
            setupCommands();
            Refresh(bid, cw);
        }
        public NetworkVM(TECSystem system, TECCatalogs catalogs)
        {
            setupCommands();
            Refresh(system, catalogs);
        }

        #region Properties
        //Item Properties
        public ReadOnlyObservableCollection<ConnectableItem> Parentables
        {
            get { return new ReadOnlyObservableCollection<ConnectableItem>(_parentables); }
        }
        public ReadOnlyObservableCollection<ConnectableItem> NonParentables
        {
            get { return new ReadOnlyObservableCollection<ConnectableItem>(_nonParentables); }
        }

        public ConnectableItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }
        public ConnectableItem SelectedParentable
        {
            get
            {
                return _selectedParentable;
            }
            set
            {
                if (!isConnecting)
                {
                    _selectedParentable = value;
                    RaisePropertyChanged("SelectedParentable");
                    _selectedNonParentable = null;
                    RaisePropertyChanged("SelectedNonParentable");
                    SelectedItem = SelectedParentable;
                }
            }
        }
        public ConnectableItem SelectedNonParentable
        {
            get
            {
                return _selectedNonParentable;
            }
            set
            {
                if (!isConnecting)
                {
                    _selectedNonParentable = value;
                    RaisePropertyChanged("SelectedNonParentable");
                    _selectedParentable = null;
                    RaisePropertyChanged("SelectedParentable");
                    SelectedItem = SelectedNonParentable;
                }
            }
        }

        public ICommand SetParentAsSelectedCommand { get; private set; }

        //Add Connection Properties
        public ReadOnlyObservableCollection<TECElectricalMaterial> AllConnectionTypes { get; private set; }
        public List<IOType> IOTypes { get; private set; }
        public ReadOnlyObservableCollection<TECElectricalMaterial> SelectedConnectionTypes
        {
            get { return new ReadOnlyObservableCollection<TECElectricalMaterial>(selectedConnectionTypes); }
        }

        public TECElectricalMaterial SelectedPotentialConnectionType
        {
            get { return _selectedPotentialConnectionType; }
            set
            {
                _selectedPotentialConnectionType = value;
                RaisePropertyChanged("SelectedPotentialConnectionType");
            }
        }
        public TECElectricalMaterial SelectedChosenConnectionType
        {
            get { return _selectedChosenConnectionType; }
            set
            {
                _selectedChosenConnectionType = value;
                RaisePropertyChanged("SelectedChosenConnectionType");
            }
        }
        public IOType SelectedIOType
        {
            get { return _selectedIOType; }
            set
            {
                _selectedIOType = value;
                RaisePropertyChanged("SelectedIOType");
            }
        }

        public INetworkConnectable SelectedChildConnectable
        {
            get
            {
                return _selectedChildConnectable;
            }
            set
            {
                _selectedChildConnectable = value;
                RaisePropertyChanged("SelectedChildConnectable");
            }
        }

        public ICommand AddConnectionTypeCommand { get; private set; }
        public ICommand RemoveConnectionTypeCommand { get; private set; }

        public ICommand AddConnectionCommand { get; private set; }

        public ICommand DoneConnectionCommand { get; private set; }

        public ICommand RemoveConnectableCommand { get; private set; }

        //Add Controller Properties
        public TECNetworkConnection SelectedConnection
        {
            get { return _selectedConnection; }
            set
            {
                _selectedConnection = value;
                RaisePropertyChanged("SelectedConnection");
                handleSelectedConnectionChanged(SelectedConnection);
            }
        }
        #endregion

        #region Methods
        public void Refresh(TECBid bid, ChangeWatcher cw)
        {
            isConnecting = false;
            resetCollections(bid.Catalogs);
            addBid(bid);
            resubscribe(cw);
        }
        public void Refresh(TECSystem system, TECCatalogs catalogs)
        {
            isConnecting = false;
            resetCollections(catalogs);
            addSystem(system);
            resubscribe(new ChangeWatcher(system));
        }
        private void setupCommands()
        {
            SetParentAsSelectedCommand = new RelayCommand(setParentAsSelectedExecute);
            AddConnectionTypeCommand = new RelayCommand(addConnectionTypeExecute, canAddConnectionType);
            RemoveConnectionTypeCommand = new RelayCommand(removeConnectionTypeExecute, canRemoveConnectionType);
            AddConnectionCommand = new RelayCommand(addConnectionExecute, canAddConnection);
            DoneConnectionCommand = new RelayCommand(doneConnectionExecute);
            RemoveConnectableCommand = new RelayCommand(removeConnectableExecute, canRemoveConnectable);
        }
        private void resetCollections(TECCatalogs catalogs)
        {
            connectableDictionary = new Dictionary<INetworkConnectable, ConnectableItem>();
            _parentables = new ObservableCollection<ConnectableItem>();
            _nonParentables = new ObservableCollection<ConnectableItem>();
            AllConnectionTypes = new ReadOnlyObservableCollection<TECElectricalMaterial>(catalogs.ConnectionTypes);
            IOTypes = new List<IOType>(TECIO.NetworkIO);
            selectedConnectionTypes = new ObservableCollection<TECElectricalMaterial>();
            RaisePropertyChanged("Parentables");
            RaisePropertyChanged("NonParentables");
            RaisePropertyChanged("AllConnectionTypes");
            RaisePropertyChanged("IOTypes");
            RaisePropertyChanged("SelectedConnectionTypes");
        }
        private void addBid(TECBid bid)
        {
            foreach (TECController controller in bid.Controllers)
            {
                addConnectableItem(controller);
            }
            foreach (TECTypical typical in bid.Systems)
            {
                foreach (TECSystem system in typical.Instances)
                {
                    foreach (TECController controller in system.Controllers)
                    {
                        addConnectableItem(controller);
                    }
                    foreach (TECEquipment equip in system.Equipment)
                    {
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            addConnectableItem(ss);
                        }
                    }
                }
            }
        }
        private void addSystem(TECSystem system)
        {
            foreach (TECController controller in system.Controllers)
            {
                addConnectableItem(controller);
            }
            foreach (TECSubScope ss in system.GetAllSubScope())
            {
                addConnectableItem(ss);
            }
        }
        private void resubscribe(ChangeWatcher cw)
        {
            cw.InstanceChanged -= handleInstanceChanged;
            cw.InstanceConstituentChanged -= handleInstanceConstituentChanged;

            cw.InstanceChanged += handleInstanceChanged;
            cw.InstanceConstituentChanged += handleInstanceConstituentChanged;
        }

        private void addConnectableItem(INetworkConnectable connectable)
        {
            bool parentConnected = false;
            if (connectable.ParentConnection != null && connectable.ParentConnection.ParentController != null)
            {
                INetworkConnectable parentNetConnectable = connectable.ParentConnection.ParentController;
                parentConnected = connectableDictionary[parentNetConnectable].IsConnected;
            }

            bool isServer = false;
            if (connectable is INetworkParentable parent)
            {
                isServer = parent.IsServer;
            }

            bool isConnected = (parentConnected || isServer);

            ConnectableItem item = new ConnectableItem(connectable, isConnected);
            connectableDictionary.Add(connectable, item);
            if (connectable is INetworkParentable)
            {
                _parentables.Add(item);
            }
            else
            {
                _nonParentables.Add(item);
            }
        }
        private void removeConnectableItem(INetworkConnectable connectable)
        {
            ConnectableItem item = connectableDictionary[connectable];
            if (connectable is INetworkParentable parentable)
            {
                foreach (TECNetworkConnection connection in parentable.ChildNetworkConnections)
                {
                    foreach (INetworkConnectable child in connection.Children)
                    {
                        updateIsConnected(child, false);
                    }
                }
                _parentables.Remove(item);
            }
            else
            {
                _nonParentables.Remove(item);
            }

            connectableDictionary.Remove(connectable);
        }

        private void handleInstanceConstituentChanged(Change change, TECObject obj)
        {
            if (change == Change.Add)
            {
                if (obj is INetworkConnectable networkConnectable)
                {
                    addConnectableItem(networkConnectable);
                }
            }
            else if (change == Change.Remove)
            {
                if (obj is INetworkConnectable networkConnectable)
                {
                    removeConnectableItem(networkConnectable);
                }
            }
        }
        private void handleInstanceChanged(TECChangedEventArgs e)
        {
            if (e.Change == Change.Add)
            {
                if (e.Sender is TECNetworkConnection netConnection)
                {
                    if (e.PropertyName == "Children" && e.Value is INetworkConnectable childConnectable)
                    {
                        ConnectableItem parentConnectable = connectableDictionary[netConnection.ParentController];
                        bool parentIsConnected = parentConnectable.IsConnected;
                        updateIsConnected(childConnectable, parentIsConnected);
                    }
                }
            }
            else if (e.Change == Change.Remove)
            {
                if (e.Sender is TECNetworkConnection netConnection)
                {
                    if (e.PropertyName == "Children" && e.Value is INetworkConnectable childConnectable)
                    {
                        updateIsConnected(childConnectable, false);
                    }
                }
            }
            else if (e.Change == Change.Edit)
            {
                if (e.Sender is INetworkConnectable networkConnectable && e.PropertyName == "IsServer" && e.Value is bool isServer)
                {
                    updateIsConnected(networkConnectable, isServer);
                }
            }
        }

        private void updateIsConnected(INetworkConnectable networkConnectable, bool isConnected)
        {
            ConnectableItem connectableItem = connectableDictionary[networkConnectable];
            if (connectableItem.IsConnected != isConnected)
            {
                if (networkConnectable is INetworkParentable parentable)
                {
                    if (!(parentable.IsServer && !isConnected))
                    {
                        connectableItem.IsConnected = isConnected;
                        updateChildrenConnected(parentable);
                    }
                }
                else
                {
                    connectableItem.IsConnected = isConnected;
                }
            }
        }
        private void updateChildrenConnected(INetworkParentable parentable)
        {
            bool isConnected = connectableDictionary[parentable as INetworkConnectable].IsConnected;
            foreach (TECNetworkConnection netConnect in parentable.ChildNetworkConnections)
            {
                foreach (INetworkConnectable child in netConnect.Children)
                {
                    updateIsConnected(child, isConnected);
                }
            }
        }

        private void setParentAsSelectedExecute()
        {
            INetworkConnectable parent = SelectedItem.Item.ParentConnection.ParentController;
            ConnectableItem selected = connectableDictionary[parent];
            if (selected.Item is INetworkParentable)
            {
                SelectedParentable = selected;
            }
            else
            {
                SelectedNonParentable = selected;
            }
        }
        private void addConnectionTypeExecute()
        {
            selectedConnectionTypes.Add(SelectedPotentialConnectionType);
            SelectedPotentialConnectionType = null;
        }
        private void removeConnectionTypeExecute()
        {
            selectedConnectionTypes.Remove(SelectedChosenConnectionType);
            SelectedChosenConnectionType = null;
        }
        private void addConnectionExecute()
        {
            if (SelectedParentable.Item is INetworkParentable parentable)
            {
                parentable.AddNetworkConnection(false, selectedConnectionTypes, SelectedIOType);
                selectedConnectionTypes = new ObservableCollection<TECElectricalMaterial>();
                RaisePropertyChanged("SelectedConnectionTypes");
            }
            else
            {
                throw new InvalidOperationException("Item in Parentables is not an INetworkParentable.");
            }
        }
        private void doneConnectionExecute()
        {
            SelectedConnection = null;
        }
        private void removeConnectableExecute()
        {
            SelectedConnection.RemoveINetworkConnectable(SelectedChildConnectable);
        }

        private bool canAddConnectionType()
        {
            return (SelectedPotentialConnectionType != null);
        }
        private bool canRemoveConnectionType()
        {
            return (SelectedChosenConnectionType != null);
        }
        private bool canAddConnection()
        {
            if (SelectedParentable.Item is INetworkParentable parentable)
            {
                bool parentCanAdd = parentable.CanAddNetworkConnection(SelectedIOType);
                return parentCanAdd;
            }
            else
            {
                throw new InvalidOperationException("Item in Parentables is not an INetworkParentable.");
            }
        }
        private bool canRemoveConnectable()
        {
            return (SelectedChildConnectable != null);
        }

        private void handleSelectedConnectionChanged(TECNetworkConnection selected)
        {
            if (selected != null)
            {
                isConnecting = true;
            }
            else
            {
                isConnecting = false;
            }
        }
        #endregion
    }

    public class ConnectableItem : INotifyPropertyChanged
    {
        public INetworkConnectable Item { get; private set; }
        private bool _isConnected;

        public ConnectableItem(INetworkConnectable item, bool isConnected)
        {
            Item = item;
            _isConnected = isConnected;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
                notifyPropertyChanged("IsConnected");
            }
        }

        private void notifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
