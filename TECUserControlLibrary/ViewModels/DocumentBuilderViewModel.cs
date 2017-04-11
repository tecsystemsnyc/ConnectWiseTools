using EstimatingLibrary;
using GalaSoft.MvvmLight;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DocumentBuilderViewModel : ViewModelBase
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

        public DocumentBuilderViewModel(TECBid bid)
        {
            Bid = bid;
        }

        public void Refresh(TECBid bid)
        {
            Bid = bid;
        }
    }
}