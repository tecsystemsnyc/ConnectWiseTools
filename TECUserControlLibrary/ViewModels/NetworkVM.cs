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
        private readonly TECCatalogs catalogs;
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
        #endregion

        private NetworkVM(
            IEnumerable<INetworkConnectable> connectables,
            TECCatalogs catalogs,
            Action<TECController> updateExecute = null,
            Func<TECController, bool> updateCanExecute = null)
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
                        if(e.Change == Change.Add)
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

            void updateExecute(TECController controller)
            {
                throw new NotImplementedException();
            }

            bool updateCanExecute(TECController controller)
            {
                throw new NotImplementedException();
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
