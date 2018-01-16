using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;

namespace TECUserControlLibrary.Models
{
    public class SystemSummaryItem : ViewModelBase
    {
        private double _untiPrice;

        public TECTypical Typical { get; }
        public TECEstimator Estimate { get; }
        public double UnitPrice
        {
            get { return _untiPrice; }
            set
            {
                _untiPrice = value;
                RaisePropertyChanged("UnitPrice");
            }
        }

        public SystemSummaryItem(TECTypical typical, TECParameters parameters, double duration = 0.0)
        {
            this.Typical = typical;
            Estimate = new TECEstimator(Typical, parameters, new TECExtraLabor(Guid.NewGuid()), duration, new ChangeWatcher(Typical));
            UnitPrice = Estimate.TotalPrice / Typical.Instances.Count;
            typical.CostChanged += change => {
                UnitPrice = Estimate.TotalPrice / Typical.Instances.Count;
            };
        }
        
    }
}
