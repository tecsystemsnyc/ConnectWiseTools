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
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class NetworkVM : ViewModelBase, IDropTarget
    {
        #region Fields and Properties
        private readonly ObservableCollection<INetworkParentable> allParentables;
        private readonly ObservableCollection<INetworkConnectable> allConnectables;
        private INetworkParentable _selectedParentable;
        private INetworkConnectable _selectedConnectable;
        private AddNetworkConnectionVM _addNetConnectVM;
        private TECNetworkConnection _selectedConnection;
        private TECCatalogs _catalogs;
        private string _cannotConnectMessage;

        public ObservableCollection<INetworkParentable> FilteredParentables
        {
            get { return ParentableFilterVM.FilteredConnectables; }
        }
        public ObservableCollection<INetworkConnectable> FilteredConnectables
        {
            get { return ConnectableFilterVM.FilteredConnectables; }
        }
        public INetworkParentable SelectedParentable
        {
            get{ return _selectedParentable; }
            set
            {
                if (SelectedParentable != value)
                {
                    _selectedParentable = value;
                    RaisePropertyChanged("SelectedParentable");
                    if(value is TECObject obj)
                    {
                        Selected?.Invoke(obj);
                    }
                    AddNetConnectVM = value != null ? new AddNetworkConnectionVM(value, Catalogs.ConnectionTypes) : null;
                }
            }
        }
        public INetworkConnectable SelectedConnectable
        {
            get { return _selectedConnectable; }
            set
            {
                _selectedConnectable = value;
                RaisePropertyChanged("SelectedConnectable");
                if(value is TECObject obj)
                {
                    Selected?.Invoke(obj);
                }
            }
        }
        public TECNetworkConnection SelectedConnection
        {
            get { return _selectedConnection; }
            set
            {
                _selectedConnection = value;
                RaisePropertyChanged("SelectedConnection");
                Selected?.Invoke(value);
            }
        }
        public string CannotConnectMessage
        {
            get { return _cannotConnectMessage; }
            set
            {
                _cannotConnectMessage = value;
                RaisePropertyChanged("CannotConnectMessage");
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
        public ConnectableFilterVM<INetworkParentable> ParentableFilterVM
        {
            get;
        }
        public ConnectableFilterVM<INetworkConnectable> ConnectableFilterVM
        {
            get;
        }
        public TECCatalogs Catalogs
        {
            get { return _catalogs; }
            set
            {
                if(Catalogs != value)
                {
                    _catalogs = value;
                    RaisePropertyChanged("Catalogs");
                }
            }
        }

        public ICommand UpdateCommand { get; private set; }
        #endregion

        private NetworkVM(
            IEnumerable<INetworkConnectable> connectables,
            TECCatalogs catalogs,
            Action<INetworkParentable> updateExecute = null,
            Func<INetworkParentable, bool> updateCanExecute = null)
        {
            this.Catalogs = catalogs;
            this.allParentables = new ObservableCollection<INetworkParentable>();
            this.allConnectables = new ObservableCollection<INetworkConnectable>(connectables.Where(item => item.IsNetwork));
            foreach(INetworkConnectable connectable in connectables)
            {
                if (connectable is INetworkParentable parentable && parentable.IsNetwork)
                {
                    allParentables.Add(parentable);
                }
            }
            ParentableFilterVM = new ConnectableFilterVM<INetworkParentable>(allParentables);
            ConnectableFilterVM = new ConnectableFilterVM<INetworkConnectable>(allConnectables);
            if (updateExecute != null && updateCanExecute != null)
            {
                UpdateCommand = new RelayCommand(() => updateExecute(SelectedParentable), () => updateCanExecute(SelectedParentable));
            }
        }

        public event Action<TECObject> Selected;

        public void DragOver(IDropInfo dropInfo)
        {
            CannotConnectMessage = "";

            UIHelpers.DragOver(dropInfo, (sourceItem, sourceType, targetType) =>
            {
                bool targetIsChildren = dropInfo.TargetCollection == SelectedConnection?.Children;
                if (sourceItem is INetworkConnectable connectable && targetIsChildren)
                {
                    if (SelectedConnection.CanAddINetworkConnectable(connectable) && connectable.ParentConnection == null)
                    {
                        return true;
                    }
                }
                return false;

            }, () =>
            {
                CannotConnectMessage = "Connectable isn't compatible with connection.";
            }
            );
        }

        public void Drop(IDropInfo dropInfo)
        {
            UIHelpers.Drop(dropInfo,
                (item) =>
                {
                    if (item is INetworkConnectable connectable)
                    {
                        SelectedConnection.AddINetworkConnectable(connectable);
                    }
                    else
                    {
                        throw new DataMisalignedException("Data misalignment between DragOver and Drop.");
                    }
                    return null;
                },
                false
            );
        }

        private void handleChange(TECChangedEventArgs e)
        {
            if(e.Change == Change.Add || e.Change == Change.Remove)
            {
                if(e.Value is TECObject item)
                {
                    foreach(INetworkConnectable connectable in getConnectables(item))
                    {
                        if(e.Change == Change.Add && connectable.IsNetwork)
                        {
                            allConnectables.Add(connectable);
                            if(connectable is INetworkParentable parentable)
                            {
                                allParentables.Add(parentable);
                            }
                        }
                        else if (e.Change == Change.Remove)
                        {
                            allConnectables.Remove(connectable);
                            if(connectable is INetworkParentable parentable)
                            {
                                allParentables.Remove(parentable);
                            }
                        }
                    }
                }
            }
        }

        private List<INetworkConnectable> getConnectables(TECObject item)
        {
            List<INetworkConnectable> connectables = new List<INetworkConnectable>();
            if(item is INetworkConnectable connectable)
            {
                connectables.Add(connectable);
            }
            if(item is IRelatable relatable)
            {
                foreach(TECObject child in relatable.PropertyObjects.Objects)
                {
                    connectables.AddRange(getConnectables(child));
                }
            }
            return connectables;
        }

        #region Static Constructors
        public static NetworkVM GetNetworkVMFromBid(TECBid bid, ChangeWatcher watcher)
        {
            List<INetworkConnectable> connectables = new List<INetworkConnectable>();
            connectables.AddRange(bid.GetAllInstanceControllers());
            connectables.AddRange(bid.GetAllInstanceSubScope());

            NetworkVM netVM = new NetworkVM(connectables, bid.Catalogs);
            watcher.InstanceChanged += netVM.handleChange;
            return netVM;
        }

        public static NetworkVM GetNetworkVMFromTypical(TECTypical typ, TECCatalogs catalogs)
        {
            List<INetworkConnectable> connectables = new List<INetworkConnectable>();
            connectables.AddRange(typ.Controllers);
            connectables.AddRange(typ.GetAllSubScope());

            ChangeWatcher watcher = new ChangeWatcher(typ);

            NetworkVM netVM = new NetworkVM(connectables, catalogs, updateExecute, updateCanExecute);
            watcher.Changed += netVM.handleChange;
            return netVM;

            void updateExecute(INetworkParentable controller)
            {
                foreach(TECController instance in typ.TypicalInstanceDictionary.GetInstances(controller as TECObject))
                {
                    instance.RemoveAllChildNetworkConnections();
                    foreach (TECNetworkConnection connection in controller.ChildNetworkConnections)
                    {
                        TECNetworkConnection instanceConnection = instance.AddNetworkConnection(false, connection.ConnectionTypes, connection.IOType);
                        instanceConnection.Length = connection.Length;
                        instanceConnection.ConduitType = connection.ConduitType;
                        instanceConnection.ConduitLength = connection.ConduitLength;
                        foreach (INetworkConnectable child in connection.Children)
                        {
                            if (child is TECController childController)
                            {
                                foreach (TECController instanceChild in typ.TypicalInstanceDictionary.GetInstances(childController))
                                {
                                    foreach (TECSystem system in typ.Instances)
                                    {
                                        if (system.Controllers.Contains(instanceChild))
                                        {
                                            instanceConnection.AddINetworkConnectable(instanceChild);
                                        }
                                    }
                                }
                            }
                            else if (child is TECSubScope childSubScope)
                            {
                                foreach (TECSubScope instanceChild in typ.TypicalInstanceDictionary.GetInstances(childSubScope))
                                {
                                    foreach (TECSystem system in typ.Instances)
                                    {
                                        if (system.GetAllSubScope().Contains(instanceChild))
                                        {
                                            instanceConnection.AddINetworkConnectable(instanceChild);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            bool updateCanExecute(INetworkParentable controller)
            {
                bool canExecute =
                    (controller != null) &&
                    (typ.TypicalInstanceDictionary.GetInstances(controller as TECObject).Count > 0);

                return canExecute;
            }
        }

        public static NetworkVM GetNetworkVMFromSystem(TECSystem sys, TECCatalogs catalogs)
        {
            if(sys is TECTypical typical)
            {
                return GetNetworkVMFromTypical(typical, catalogs);
            }
            else
            {
                List<INetworkConnectable> connectables = new List<INetworkConnectable>();
                connectables.AddRange(sys.Controllers);
                connectables.AddRange(sys.GetAllSubScope());

                ChangeWatcher watcher = new ChangeWatcher(sys);

                NetworkVM netVM = new NetworkVM(connectables, catalogs);
                watcher.InstanceChanged += netVM.handleChange;
                return netVM;
            }
            
        }
        #endregion
    }
}
