using EstimatingLibrary;
using GalaSoft.MvvmLight;
using TECUserControlLibrary.ViewModels;

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
        public DocumentBuilderViewModel DocumentBuilderVM { get; set; }

        public ProposalViewModel(TECBid bid)
        {
            DocumentBuilderVM = new DocumentBuilderViewModel(bid);
        }
    }
}