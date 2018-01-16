using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class NetworkVM : ViewModelBase, IDropTarget
    {
        #region Properties and Fields
        private readonly List<ConnectableItem> allParentables;
        private readonly List<ConnectableItem> allNonParentables;
        private readonly Dictionary<INetworkConnectable, ConnectableItem> connectableDictionary;

        //Used to determine whether or not to allow selecting of a connectable.
        private bool isConnecting;

        private ObservableCollection<ConnectableItem> _filteredParentables;
        private ObservableCollection<ConnectableItem> _filteredNonParentables;
        private ConnectableItem _selectedConnectable;
        private ConnectableItem _selectedParentable;
        private ConnectableItem _selectedNonParentable;
        private TECNetworkConnection _selectedConnection;
        private INetworkConnectable _selectedChild;
        private string _cannotConnectMessage;

        private AddNetworkConnectionVM _addNetConnectVM;

        public TECCatalogs Catalogs { get; }

        public ObservableCollection<ConnectableItem> FilteredParentables
        {
            get { return _filteredParentables; }
            set
            {
                if (FilteredParentables != value)
                {
                    _filteredParentables = value;
                    RaisePropertyChanged("FilteredParentables");
                }
            }
        }
        public ObservableCollection<ConnectableItem> FilteredNonParentables
        {
            get { return _filteredNonParentables; }
            set
            {
                if (FilteredNonParentables != value)
                {
                    _filteredNonParentables = value;
                    RaisePropertyChanged("FilteredNonParentables");
                }
            }
        }
        public ConnectableItem SelectedConnectable
        {
            get { return _selectedConnectable; }
            set
            {
                if (SelectedConnectable != value)
                {
                    _selectedConnectable = value;
                    RaisePropertyChanged("SelectedConnectable");
                    ObjectSelected?.Invoke(value.Item as TECObject);
                }
            }
        }
        //Properties to bind to to handle selecting connectables
        public ConnectableItem SelectedParentable
        {
            get { return _selectedParentable; }
            set
            {
                //Must not allow Selected Item to change while connecting
                if (!isConnecting && SelectedParentable != value)
                {
                    _selectedParentable = value;
                    RaisePropertyChanged("SelectedParentable");
                    //Unselect NonParentable
                    _selectedNonParentable = null;
                    RaisePropertyChanged("SelectedNonParentable");
                    SelectedConnectable = SelectedParentable;
                    if (SelectedParentable.Item is INetworkParentable parentable)
                    {
                        AddNetConnectVM = new AddNetworkConnectionVM(parentable, Catalogs.ConnectionTypes);
                    }
                    else
                    {
                        throw new DataMisalignedException("A ConnectableItem exists in ParentableItems that isn't an INetworkParentable.");
                    }
                }
            }
        }
        public ConnectableItem SelectedNonParentable
        {
            get { return _selectedNonParentable; }
            set
            {
                //Must not allow Selected Item to change while connecting
                if (!isConnecting && SelectedNonParentable != value)
                {
                    _selectedNonParentable = value;
                    RaisePropertyChanged("SelectedNonParentable");
                    //Unselect Parentable
                    _selectedParentable = null;
                    RaisePropertyChanged("SelectedParentable");
                    SelectedConnectable = SelectedNonParentable;
                }
            }
        }
        public TECNetworkConnection SelectedConnection
        {
            get { return _selectedConnection; }
            set
            {
                if (SelectedConnection != value)
                {
                    _selectedConnection = value;
                    RaisePropertyChanged("SelectedConnection");
                    isConnecting = (SelectedConnection != null);
                    ObjectSelected?.Invoke(SelectedConnection);
                }
            }
        }
        public INetworkConnectable SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                if (SelectedChild != value)
                {
                    _selectedChild = value;
                    RaisePropertyChanged("SelectedChild");
                    ObjectSelected?.Invoke(SelectedChild as TECObject);
                }
            }
        }
        public string CannotConnectMessage
        {
            get { return _cannotConnectMessage; }
            set
            {
                if (CannotConnectMessage != value)
                {
                    _cannotConnectMessage = value;
                    RaisePropertyChanged("CannotConnectMessage");
                }
            }
        }

        public AddNetworkConnectionVM AddNetConnectVM
        {
            get { return _addNetConnectVM; }
            set
            {
                if (AddNetConnectVM != value)
                {
                    _addNetConnectVM = value;
                    RaisePropertyChanged("AddNetConnectVM");
                }
            }
        }

        public ICommand SetParentAsSelectedCommand { get; }
        public ICommand DoneConnectionCommand { get; }
        public ICommand RemoveChildCommand { get; }
        public ICommand UpdateInstancesCommand { get; }
        public ICommand RemoveConnectionCommand { get; }

        public event Action<TECObject> ObjectSelected;
        #endregion
        
        #region Constructors
        private NetworkVM()
        {
            //Setup Commands
            SetParentAsSelectedCommand = new RelayCommand(setParentAsSelectedExecute, setParentAsSelectedCanExecute);
            DoneConnectionCommand = new RelayCommand(doneConnectionExecute, doneConnectionCanExecute);
            RemoveChildCommand = new RelayCommand(removeChildExecute, removeChildCanExecute);
            UpdateInstancesCommand = new RelayCommand(updateInstancesExecute, updateInstancesCanExecute);
            RemoveConnectionCommand = new RelayCommand(removeConnectionExecute, removeConnectionCanExecute);

            //Instantiate Fields
            allParentables = new List<ConnectableItem>();
            allNonParentables = new List<ConnectableItem>();
            connectableDictionary = new Dictionary<INetworkConnectable, ConnectableItem>();
            _filteredParentables = new ObservableCollection<ConnectableItem>();
            _selectedConnectable = null;
            _cannotConnectMessage = "";
            _addNetConnectVM = null;
        }
        
        public NetworkVM(TECBid bid, ChangeWatcher cw) : this()
        {
            this.Catalogs = bid.Catalogs;

            cw.InstanceChanged += handleChange;
            cw.InstanceConstituentChanged += handleConstituentChange;

            foreach(TECController controller in bid.GetAllInstanceControllers())
            {
                addNetworkConnectable(controller);
            }
            foreach(TECSubScope ss in bid.GetAllInstanceSubScope())
            {
                addNetworkConnectable(ss);
            }

            refilter();
        }
        public NetworkVM(TECSystem system, TECCatalogs catalogs) : this()
        {
            this.Catalogs = catalogs;

            ChangeWatcher cw = new ChangeWatcher(system);

            if (system.IsTypical)
            {
                cw.Changed += handleChange;
                cw.TypicalConstituentChanged += handleConstituentChange;
            }
            else
            {
                cw.InstanceChanged += handleChange;
                cw.InstanceConstituentChanged += handleConstituentChange;
            }

            foreach(TECController controller in system.Controllers)
            {
                addNetworkConnectable(controller);
            }
            foreach(TECSubScope ss in system.GetAllSubScope())
            {
                addNetworkConnectable(ss);
            }

            refilter();
        }
        #endregion
        
        #region Drag/Drop Methods
        public void DragOver(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void Drop(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Commands Methods
        private void setParentAsSelectedExecute()
        {
            throw new NotImplementedException();
        }
        private bool setParentAsSelectedCanExecute()
        {
            throw new NotImplementedException();
        }

        private void doneConnectionExecute()
        {
            throw new NotImplementedException();
        }
        private bool doneConnectionCanExecute()
        {
            throw new NotImplementedException();
        }

        private void removeChildExecute()
        {
            throw new NotImplementedException();
        }
        private bool removeChildCanExecute()
        {
            throw new NotImplementedException();
        }

        private void updateInstancesExecute()
        {
            throw new NotImplementedException();
        }
        private bool updateInstancesCanExecute()
        {
            throw new NotImplementedException();
        }

        private void removeConnectionExecute()
        {
            throw new NotImplementedException();
        }
        private bool removeConnectionCanExecute()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Event Handlers
        private void handleChange(TECChangedEventArgs e)
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
                else if (e.Sender is TECSubScope ss && e.Value is TECPoint)
                {
                    if (!connectableDictionary.ContainsKey(ss))
                    {
                        addNetworkConnectable(ss);
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
                else if (e.Sender is TECSubScope ss && e.Value is TECPoint)
                {
                    removeNetworkConnectable(ss);
                }
            }
            else if (e.Change == Change.Edit)
            {
                if (e.Sender is INetworkConnectable networkConnectable &&
                    e.PropertyName == "IsServer" && e.Value is bool isServer)
                {
                    updateIsConnected(networkConnectable, isServer);
                }
            }
        }
        private void handleConstituentChange(Change change, TECObject obj)
        {
            if (obj is INetworkConnectable networkConnectable)
            {
                if (change == Change.Add)
                {
                    addNetworkConnectable(networkConnectable);
                }
                else if (change == Change.Remove)
                {
                    removeNetworkConnectable(networkConnectable);
                }
            }
        }
        #endregion

        private void addNetworkConnectable(INetworkConnectable connectable)
        {
            //Checks to make sure connectable doesn't already exist and isn't a non network subscope
            if (connectableDictionary.ContainsKey(connectable) ||
                (connectable is TECSubScope ss && !ss.IsNetwork))
            {
                return;
            }

            //Adds parent as connectable if doesn't exist
            bool parentConnected = false;
            if (connectable.ParentConnection != null && connectable.ParentConnection.ParentController != null)
            {
                INetworkConnectable parentNetConnectable = connectable.ParentConnection.ParentController;
                if (!connectableDictionary.ContainsKey(parentNetConnectable))
                {
                    addNetworkConnectable(parentNetConnectable);
                }
                parentConnected = connectableDictionary[parentNetConnectable].IsConnected;
            }

            //Determine if connectable is already connected
            bool isServer = false;
            if (connectable is INetworkParentable parentable)
            {
                isServer = parentable.IsServer;
            }

            bool isConnected = (parentConnected || isServer);

            //Add and sort connectable item
            ConnectableItem item = new ConnectableItem(connectable, isConnected);
            if (!connectableDictionary.ContainsKey(connectable))
            {
                connectableDictionary.Add(connectable, item);
            }
            if (connectable is INetworkParentable)
            {
                allParentables.Add(item);
            }
            else
            {
                allNonParentables.Add(item);
            }
            refilter();
        }
        private void removeNetworkConnectable(INetworkConnectable connectable)
        {
            if (connectableDictionary.ContainsKey(connectable))
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
                    allParentables.Remove(item);
                }
                else
                {
                    allNonParentables.Remove(item);
                }

                connectableDictionary.Remove(connectable);
            }
            refilter();
        }

        private void refilter()
        {
            throw new NotImplementedException();
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
    }

    public class ConnectableItem : INotifyPropertyChanged
    {
        public INetworkConnectable Item { get; }
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
