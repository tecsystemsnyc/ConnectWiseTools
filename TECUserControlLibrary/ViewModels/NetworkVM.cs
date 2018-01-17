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
        private readonly List<INetworkParentable> parentables;
        private readonly List<INetworkConnectable> connectables;

        private ObservableCollection<INetworkParentable> _filteredParentables;
        private ObservableCollection<INetworkConnectable> _filteredConnectables;

        public ObservableCollection<INetworkParentable> FilteredParentables
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
        public ObservableCollection<INetworkConnectable> FilteredConnectables
        {
            get { return _filteredConnectables; }
            set
            {
                if (FilteredConnectables != value)
                {
                    _filteredConnectables = value;
                    RaisePropertyChanged("FilteredConnectables");
                }
            }
        }

        private NetworkVM(IEnumerable<INetworkConnectable> connectables,
            TECCatalogs catalogs,
            Action<TECController> updateExecute = null,
            Func<TECController, bool> updateCanExecute = null)
        {
            this.connectables = new List<INetworkConnectable>(connectables);
            this.parentables = new List<INetworkParentable>();
            foreach(INetworkConnectable connectable in connectables)
            {
                if (connectable is INetworkParentable parentable)
                {
                    parentables.Add(parentable);
                }
            }
        }

        private void handleChange(TECChangedEventArgs e)
        {

        }

        #region Static Constructors
        public static NetworkVM GetNetworkVMFromBid(TECBid bid, ChangeWatcher watcher)
        {
            List<INetworkConnectable> connectables = new List<INetworkConnectable>();
            connectables.AddRange(bid.GetAllInstanceControllers());
            connectables.AddRange(bid.GetAllInstanceSubScope());

            NetworkVM networkVM = new NetworkVM(connectables, bid.Catalogs);
            watcher.InstanceChanged += networkVM.handleChange;
            return networkVM;
        }

        public static NetworkVM GetNetworkVMFromTypical(TECTypical typ, TECCatalogs catalogs)
        {
            List<INetworkConnectable> connectables = new List<INetworkConnectable>();
            connectables.AddRange(typ.Controllers);
            connectables.AddRange(typ.GetAllSubScope());

            ChangeWatcher watcher = new ChangeWatcher(typ);

            return new NetworkVM(connectables, watcher, catalogs, updateExecute, updateCanExecute);

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

            return new NetworkVM(connectables, watcher, catalogs);
        }
        #endregion
    }
}
