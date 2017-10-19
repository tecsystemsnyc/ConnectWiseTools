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
        public SystemSummaryVM SystemVM { get; private set; }
        public PanelSummaryVM PanelVM { get; private set; }
        public LocationSummaryVM LocationVM { get; private set; }

        public ItemizedSummaryVM(TECBid bid, ChangeWatcher watcher)
        {
            SystemVM = new SystemSummaryVM(bid, watcher);
            PanelVM = new PanelSummaryVM(bid);
            LocationVM = new LocationSummaryVM(bid);
        }
    }
}
