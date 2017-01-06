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
        public TECBid Bid { get; set; }

        public DocumentBuilderViewModel(TECBid bid)
        {
            Bid = bid;
        }
    }
}