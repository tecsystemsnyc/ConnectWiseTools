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

        public SystemSummaryItem(TECTypical typical, TECParameters parameters, double duration = 0.0)
        {
            this.Typical = typical;
            Estimate = new TECEstimator(Typical, parameters, new TECExtraLabor(Guid.NewGuid()), duration, new ChangeWatcher(Typical));
            Console.WriteLine(string.Format("New SystemSummaryItem guid: {0}", Typical.Guid));
        }
        
    }
}
