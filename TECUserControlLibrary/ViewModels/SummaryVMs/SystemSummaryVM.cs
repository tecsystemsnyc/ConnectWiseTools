using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels.SummaryVMs
{
    public class SystemSummaryVM : ViewModelBase
    {
        private ObservableCollection<SystemSummaryItem> systems;
        private TECTypical selected;
        private TECBid bid;

        public ObservableCollection<SystemSummaryItem> Systems
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
            this.bid = bid;
            populateSystems(bid.Systems);
            watcher.Changed += changed;
        }

        private void changed(TECChangedEventArgs e)
        {
            if(e.Value is TECTypical typical)
            {
                if (e.Change == Change.Add)
                {
                    Systems.Add(new SystemSummaryItem(typical, bid.Parameters));
                }
                else if (e.Change == Change.Remove)
                {
                    SystemSummaryItem toRemove = null;
                    foreach(var item in Systems)
                    {
                        if(item.typical == typical)
                        {
                            toRemove = item;
                            break;
                        }
                    }
                    if(toRemove != null )
                        Systems.Remove(toRemove);
                }
            }
            
        }

        private void populateSystems(ObservableCollection<TECTypical> typicals)
        {
            ObservableCollection<SystemSummaryItem> systemItems = new ObservableCollection<SystemSummaryItem>();
            foreach(TECTypical typical in typicals)
            {
                systemItems.Add(new SystemSummaryItem(typical, bid.Parameters));
            }
            Systems = systemItems;
        }
    }
}
