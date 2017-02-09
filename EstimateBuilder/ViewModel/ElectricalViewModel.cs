using EstimatingLibrary;
using GalaSoft.MvvmLight;
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

        private ObservableCollection<TECSubScope> _allSubScope;
        public ObservableCollection<TECSubScope> AllSubScope
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

        private void getAllSubScope()
        {
            AllSubScope = new ObservableCollection<TECSubScope>();
            foreach(TECSystem sys in Bid.Systems)
            {
                foreach(TECSubScope sub in sys.SubScope)
                {
                    AllSubScope.Add(sub);
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
            getAllSubScope();
        }
    }
}