using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels.SummaryVMs
{
    public class ItemizedSummaryVM : ViewModelBase
    {
        private SystemSummaryVM systemVM;

        public SystemSummaryVM SystemVM
        {
            get { return systemVM; }
            set
            {
                systemVM = value;
                RaisePropertyChanged("SystemVM");
            }
        }

        public ItemizedSummaryVM(TECBid bid, ChangeWatcher watcher)
        {
            SystemVM = new SystemSummaryVM(bid, watcher);
        }

        public void Refresh(TECBid bid, ChangeWatcher watcher)
        {
            SystemVM = new SystemSummaryVM(bid, watcher);
        }
    }
}
