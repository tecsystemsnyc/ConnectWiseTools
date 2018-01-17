using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels
{
    public class ConnectableFilterVM<T> : ViewModelBase where T : INetworkConnectable
    {
        private readonly ReadOnlyObservableCollection<T> allConnectables;

        public ObservableCollection<T> FilteredConnectables { get; }

        public ConnectableFilterVM(ObservableCollection<T> connectables)
        {
            allConnectables = new ReadOnlyObservableCollection<T>(connectables);
            (allConnectables as INotifyCollectionChanged).CollectionChanged += allConnectablesCollectionChanged;
            FilteredConnectables = new ObservableCollection<T>();
            refilter();
        }

        private void allConnectablesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //Add any connectables that pass
                foreach(object value in e.NewItems)
                {
                    if (value is T connectable && passesFilter(connectable))
                    {
                        FilteredConnectables.Add(connectable);
                    } 
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //Remove any connectables that exist
                foreach(object value in e.OldItems)
                {
                    if (value is T connectable && FilteredConnectables.Contains(connectable))
                    {
                        FilteredConnectables.Remove(connectable);
                    }
                }
            }
        }

        private void refilter()
        {
            //Remove connectables that don't pass
            List<T> toRemove = new List<T>();
            foreach(T connectable in FilteredConnectables)
            {
                if (!passesFilter(connectable))
                {
                    toRemove.Add(connectable);
                }
            }
            foreach(T connectable in toRemove)
            {
                FilteredConnectables.Remove(connectable);
            }

            //Add connectables that do pass
            foreach(T connectable in allConnectables)
            {
                if (!FilteredConnectables.Contains(connectable) && passesFilter(connectable))
                {
                    FilteredConnectables.Add(connectable);
                }
            }
        }

        private bool passesFilter(T connectable)
        {
            throw new NotImplementedException();
        }
    }
}
