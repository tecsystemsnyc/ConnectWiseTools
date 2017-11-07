using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;

namespace TECUserControlLibrary.Models
{
    public class SystemSummaryItem : ViewModelBase
    {
        
        private ChangeWatcher watcher;

        public TECTypical Typical { get; private set; }
        public TECEstimator Estimate { get; private set; }

        public SystemSummaryItem(TECTypical typical, TECParameters parameters)
        {
            this.Typical = typical;
            watcher = new ChangeWatcher(typical);
            Estimate = new TECEstimator(typical, parameters, watcher);
        }
        
    }
}
