using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ElectricalViewModel : ViewModelBase
    {

        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }

        private ObservableCollection<Tuple<string, string, TECSubScope>> _allSubScope;
        public ObservableCollection<Tuple<string, string, TECSubScope>> AllSubScope
        {
            get
            {
                return _allSubScope;
            }
            set
            {
                _allSubScope = value;
                RaisePropertyChanged("AllSubScope");
            }
        }

        /// <summary>
        /// Initializes a new instance of the ElectricalViewModel class.
        /// </summary>
        public ElectricalViewModel(TECBid bid)
        {
            Bid = bid;
            getAllSubScope();
            registerSubScope();
        }

        public void refresh(TECBid bid)
        {
            Bid = bid;
            getAllSubScope();
            registerSubScope();
        }
        
        private void getAllSubScope()
        {
            AllSubScope = new ObservableCollection<Tuple<string, string, TECSubScope>>();
            foreach(TECSystem sys in Bid.Systems)
            {
                foreach(TECEquipment equip in sys.Equipment)
                {
                    foreach(TECSubScope sub in equip.SubScope)
                    {
                        AllSubScope.Add(Tuple.Create<string, string, TECSubScope>(sys.Name, equip.Name, sub));
                    }
                }
            }

        }

        private void registerSubScope()
        {
            Bid.Systems.CollectionChanged += SubScope_CollectionChanged;
            foreach (TECSystem sys in Bid.Systems)
            {
                sys.Equipment.CollectionChanged += SubScope_CollectionChanged;

                foreach (TECEquipment equip in sys.Equipment)
                {
                    equip.SubScope.CollectionChanged += SubScope_CollectionChanged;
                }
            }
        }

        private void SubScope_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(object item in e.NewItems)
                {
                    if(item is TECSystem)
                    {
                        (item as TECSystem).Equipment.CollectionChanged += SubScope_CollectionChanged;
                    }
                    else if (item is TECEquipment)
                    {
                        (item as TECEquipment).SubScope.CollectionChanged += SubScope_CollectionChanged;
                    }
                }
            }
            else if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECSystem)
                    {
                        (item as TECSystem).Equipment.CollectionChanged -= SubScope_CollectionChanged;
                    }
                    else if (item is TECEquipment)
                    {
                        (item as TECEquipment).SubScope.CollectionChanged -= SubScope_CollectionChanged;
                    }
                }
            }
            getAllSubScope();
        }
    }
}