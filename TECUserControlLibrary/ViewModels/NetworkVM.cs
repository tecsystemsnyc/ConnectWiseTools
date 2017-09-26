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
        public ObservableCollection<INetworkParentable> Parentables { get; private set; }
        public ObservableCollection<INetworkConnectable> NonParentables { get; private set; }

        private INetworkConnectable _selectedItem;
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
        private void resetCollections()
        {
            Parentables = new ObservableCollection<INetworkParentable>();
            NonParentables = new ObservableCollection<INetworkConnectable>();
            RaisePropertyChanged("Parentables");
            RaisePropertyChanged("NonParentables");
        }
        private void addBid(TECBid bid)
        {
            foreach (TECController controller in bid.Controllers)
            {
                Parentables.Add(controller);
            }
            foreach (TECTypical typical in bid.Systems)
            {
                foreach (TECSystem system in typical.Instances)
                {
                    foreach (TECController controller in system.Controllers)
                    {
                        Parentables.Add(controller);
                    }
                    foreach(TECEquipment equip in system.Equipment)
                    {
                        foreach(TECSubScope ss in equip.SubScope)
                        {
                            NonParentables.Add(ss);
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

        private void instanceChanged(TECChangedEventArgs obj)
        {
            throw new NotImplementedException();
        }
        #endregion

        public class Connectable : INotifyPropertyChanged
        {
            public INetworkConnectable Item { get; private set; }
            private bool _isConnected;

            public Connectable(INetworkConnectable item)
            {

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
