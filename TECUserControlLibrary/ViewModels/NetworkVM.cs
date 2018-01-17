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

        public ObservableCollection<INetworkParentable> FilteredParentables
        {
            get { return ParentableFilterVM.FilteredConnectables; }
        }
        public ObservableCollection<INetworkConnectable> FilteredConnectables
        {
            get { return ConnectableFilterVM.FilteredConnectables; }
        }

        private AddNetworkConnectionVM _addNetConnectVM;

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
