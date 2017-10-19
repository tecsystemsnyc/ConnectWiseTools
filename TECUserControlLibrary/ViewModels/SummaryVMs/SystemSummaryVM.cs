using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels.SummaryVMs
{
    public class SystemSummaryVM : ViewModelBase
    {
        private ObservableCollection<TECTypical> systems;
        private TECTypical selected;

        public ObservableCollection<TECTypical> Systems
        {
            get { return systems; }
            set
            {
                systems = value;
                RaisePropertyChanged("Systems");
            }
        }
        public TECTypical Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                RaisePropertyChanged("Selected");
            }
        }
        
        public SystemSummaryVM(TECBid bid, ChangeWatcher watcher)
        {
            populateSystems(bid.Systems);
        }

        private void populateSystems(ObservableCollection<TECTypical> typicals)
        {
            systems = typicals;
        }
    }
}
