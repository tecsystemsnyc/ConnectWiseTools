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
