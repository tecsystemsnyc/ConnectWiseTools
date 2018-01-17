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
        private readonly TECCatalogs catalogs;

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
                _selectedParentable = value;
                RaisePropertyChanged("SelectedParentable");
                if(value is TECObject obj)
                {
                    Selected?.Invoke(obj);
                }
                AddNetConnectVM = new AddNetworkConnectionVM(value, catalogs.ConnectionTypes);
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

        public ICommand UpdateCommand { get; private set; }
        #endregion

        private NetworkVM(
            IEnumerable<INetworkConnectable> connectables,
            TECCatalogs catalogs,
            Action<INetworkParentable> updateExecute = null,
            Func<INetworkParentable, bool> updateCanExecute = null)
        {
            this.catalogs = catalogs;
            this.allParentables = new ObservableCollection<INetworkParentable>();
            this.allConnectables = new ObservableCollection<INetworkConnectable>(connectables);
            foreach(INetworkConnectable connectable in connectables)
            {
                if (connectable is INetworkParentable parentable)
                {
                    allParentables.Add(parentable);
                }
            }
            ParentableFilterVM = new ConnectableFilterVM<INetworkParentable>(allParentables);
            ConnectableFilterVM = new ConnectableFilterVM<INetworkConnectable>(allConnectables);
            UpdateCommand = new RelayCommand(() => updateExecute(SelectedParentable), () => updateCanExecute(SelectedParentable));
        }

        public event Action<TECObject> Selected;

        public void DragOver(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void Drop(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        private void handleChange(TECChangedEventArgs e)
        {
            throw new NotImplementedException();
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
            List<INetworkConnectable> connectables = new List<INetworkConnectable>();
            connectables.AddRange(sys.Controllers);
            connectables.AddRange(sys.GetAllSubScope());

            ChangeWatcher watcher = new ChangeWatcher(sys);

            NetworkVM netVM = new NetworkVM(connectables, catalogs);
            watcher.InstanceChanged += netVM.handleChange;
            return netVM;
        }
        #endregion
    }
}
