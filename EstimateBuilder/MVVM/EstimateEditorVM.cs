using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.ViewModels;
using TECUserControlLibrary.ViewModels.SummaryVMs;

namespace EstimateBuilder.MVVM
{
    public class EstimateEditorVM : EditorVM
    {
        public ScopeEditorVM ScopeEditorVM { get; }
        public LaborVM LaborVM { get; }
        public ReviewVM ReviewVM { get; }
        public ProposalVM ProposalVM { get; }
        public NetworkVM NetworkVM { get; }
        public ItemizedSummaryVM ItemizedSummaryVM { get; }
        public MaterialSummaryVM MaterialSummaryVM { get; }
        
        public EstimateEditorVM(TECBid bid, TECTemplates templates, ChangeWatcher watcher, TECEstimator estimate)
        {
            ScopeEditorVM = new ScopeEditorVM(bid, templates);
            LaborVM = new LaborVM(bid, templates, estimate);
            ReviewVM = new ReviewVM(bid, estimate);
            ProposalVM = new ProposalVM(bid);
            NetworkVM = new NetworkVM(bid, watcher);
            ItemizedSummaryVM = new ItemizedSummaryVM(bid, watcher);
            MaterialSummaryVM = new MaterialSummaryVM(bid, watcher);
        }

        public void Refresh(TECBid bid, TECTemplates templates, ChangeWatcher watcher, TECEstimator estimate)
        {
            ScopeEditorVM.Refresh(bid, templates);
            LaborVM.Refresh(bid, estimate, templates);
            ReviewVM.Refresh(estimate, bid);
            ProposalVM.Refresh(bid);
            NetworkVM.Refresh(bid, watcher);
            ItemizedSummaryVM.Refresh(bid, watcher);
            MaterialSummaryVM.Refresh(bid, watcher);
        }
    }
}
