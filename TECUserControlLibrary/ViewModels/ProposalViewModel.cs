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
    public class ProposalViewModel : DocumentBuilderVM
    {
        public ProposalViewModel(TECBid bid) : base(bid) { }
    }
}