using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class SystemSummaryItem : ViewModelBase
    {
        public TECTypical typical;
        private TECEstimator estimate;
        private ChangeWatcher watcher;

        public string Name
        {
            get { return typical.Name; }
        }
        public int Quantity
        {
            get { return typical.Instances.Count; }
        }
        public double Total
        {
            get { return estimate.TotalPrice; }
        }
        
        public SystemSummaryItem(TECTypical typical, TECParameters parameters)
        {
            this.typical = typical;
            watcher = new ChangeWatcher(typical);
            estimate = new TECEstimator(typical, parameters, watcher);
        }
        
    }
}
