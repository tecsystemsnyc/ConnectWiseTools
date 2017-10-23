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
        private PanelSummaryVM panelVM;
        private LocationSummaryVM locationVM;

        public SystemSummaryVM SystemVM
        {
            get { return systemVM; }
            set
            {
                systemVM = value;
                RaisePropertyChanged("SystemVM");
            }
        }
        public PanelSummaryVM PanelVM
        {
            get { return panelVM; }
            set
            {
                panelVM = value;
                RaisePropertyChanged("PanelVM");
            }
        }
        public LocationSummaryVM LocationVM
        {
            get { return locationVM; }
            set
            {
                locationVM = value;
                RaisePropertyChanged("LocationVM");
            }
        }

        public ItemizedSummaryVM(TECBid bid, ChangeWatcher watcher)
        {
            SystemVM = new SystemSummaryVM(bid, watcher);
            PanelVM = new PanelSummaryVM(bid);
            LocationVM = new LocationSummaryVM(bid);
        }

        public void Refresh(TECBid bid, ChangeWatcher watcher)
        {
            SystemVM = new SystemSummaryVM(bid, watcher);
            PanelVM = new PanelSummaryVM(bid);
            LocationVM = new LocationSummaryVM(bid);
        }
    }
}
