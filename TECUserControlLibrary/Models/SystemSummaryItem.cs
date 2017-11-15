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
            Estimate = new TECEstimator(typical, parameters, new TECExtraLabor(Guid.NewGuid()), new ChangeWatcher(typical));
            Estimate.PropertyChanged += Estimate_PropertyChanged;
        }

        private void Estimate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string typicalGuid = Typical.Guid.ToString();
            Console.WriteLine("The estimate for {0} changed: {1}", Typical.Name, e.PropertyName);
        }
    }
}
