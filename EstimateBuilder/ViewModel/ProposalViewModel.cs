using EstimatingLibrary;
using GalaSoft.MvvmLight;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProposalViewModel : ViewModelBase
    {
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set { _bid = value; }
        }

        public ProposalViewModel(TECBid bid)
        {
            Bid = bid;
        }
    }
}