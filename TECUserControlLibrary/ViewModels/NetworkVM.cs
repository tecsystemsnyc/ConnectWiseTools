using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels
{
    public class NetworkVM : ViewModelBase
    {
        #region Fields
        public ObservableCollection<ConnectableItem> Parentables { get; private set; }
        public ObservableCollection<ConnectableItem> NonParentables { get; private set; }

        private INetworkConnectable _selectedItem;
        private Dictionary<INetworkConnectable, ConnectableItem> connectableDictionary;
        #endregion

        //Constructor
        public NetworkVM(TECBid bid, ChangeWatcher cw)
        {
            resetCollections();
            addBid(bid);
            resubscribe(cw);
        }

        #region Properties
        public INetworkConnectable SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            private set
            {
                _selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }
        #endregion

        #region Methods
        public void Refresh(TECBid bid, ChangeWatcher cw)
        {
            resetCollections();
            addBid(bid);
            resubscribe(cw);
        }

        private void resetCollections()
        {
            Parentables = new ObservableCollection<ConnectableItem>();
            NonParentables = new ObservableCollection<ConnectableItem>();
            RaisePropertyChanged("Parentables");
            RaisePropertyChanged("NonParentables");
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
                    foreach(TECEquipment equip in system.Equipment)
                    {
                        foreach(TECSubScope ss in equip.SubScope)
                        {
                            addConnectableItem(ss);
                        }
                    }
                }
            }
        }
        private void resubscribe(ChangeWatcher cw)
        {
            cw.InstanceChanged -= instanceChanged;
            cw.InstanceChanged += instanceChanged;
        }

        private void addConnectableItem(INetworkConnectable connectable)
        {
            ConnectableItem item = new ConnectableItem(connectable);
            connectableDictionary.Add(connectable, item);
            if (connectable is INetworkParentable)
            {
                Parentables.Add(item);
            }
            else
            {
                NonParentables.Add(item);
            }
        }

        private void instanceChanged(TECChangedEventArgs obj)
        {
            if (obj.PropertyName == "IsServer" && obj.Sender is INetworkConnectable networkConnectable && obj.Value is bool isServer)
            {
                updateIsConnected(networkConnectable, isServer);
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
                        foreach (TECNetworkConnection netConnect in parentable.GetNetworkConnections())
                        {
                            foreach (INetworkConnectable child in netConnect.Children)
                            {
                                updateIsConnected(child, isConnected);
                            }
                        }
                    }
                }
                else
                {
                    connectableItem.IsConnected = isConnected;
                }
            }
        }
        #endregion

        public class ConnectableItem : INotifyPropertyChanged
        {
            public INetworkConnectable Item { get; private set; }
            private bool _isConnected;

            public ConnectableItem(INetworkConnectable item)
            {
                Item = item;
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
}
