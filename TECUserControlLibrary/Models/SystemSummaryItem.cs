using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;

namespace TECUserControlLibrary.Models
{
    public class SystemSummaryItem : ViewModelBase
    {
        public TECTypical Typical { get; }
        public TECEstimator Estimate { get; }

        public SystemSummaryItem(TECTypical typical, TECParameters parameters)
        {
            this.Typical = typical;
            Estimate = new TECEstimator(Typical, parameters, new TECExtraLabor(Guid.NewGuid()), new ChangeWatcher(Typical));
        }
        
    }
}
