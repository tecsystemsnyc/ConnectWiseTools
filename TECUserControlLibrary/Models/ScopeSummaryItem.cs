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
    public class ScopeSummaryItem : ViewModelBase
    {
        public TECScope scope;
        private TECEstimator estimate;
        private ChangeWatcher watcher;

        public string Name
        {
            get { return scope.Name; }
        }
        public double Total
        {
            get { return estimate.TotalPrice; }
        }

        public ScopeSummaryItem(TECScope scope, TECParameters parameters)
        {
            this.scope = scope;
            watcher = new ChangeWatcher(scope);
            estimate = new TECEstimator(scope, parameters, watcher);
        }

    }
}
