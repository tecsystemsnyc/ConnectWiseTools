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
        private ObservableCollection<ScopeSummaryItem> riser;
        private ObservableCollection<ScopeSummaryItem> misc;
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
        public ObservableCollection<ScopeSummaryItem> Riser
        {
            get { return riser; }
            set
            {
                riser = value;
                RaisePropertyChanged("Riser");
            }
        }
        public ObservableCollection<ScopeSummaryItem> Misc
        {
            get { return misc; }
            set
            {
                misc = value;
                RaisePropertyChanged("Misc");
            }
        }
        public double SystemTotal
        {
            get { return getSystemTotal(); }
        }
        
        public double RiserTotal
        {
            get { return getRiserTotal(); }
        }
        
        public double MiscTotal
        {
            get { return getMiscTotal(); }
        }
        
        public SystemSummaryVM(TECBid bid, ChangeWatcher watcher)
        {
            this.bid = bid;
            populateSystems(bid.Systems);
            populateRiser(bid.Controllers, bid.Panels);
            populateMisc(bid.MiscCosts);
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
                        if(item.Typical == typical)
                        {
                            toRemove = item;
                            break;
                        }
                    }
                    if(toRemove != null )
                        Systems.Remove(toRemove);
                }
                RaisePropertyChanged("SystemTotal");
            }
            else if(e.Sender is TECBid)
            {
                if(e.Change == Change.Add)
                {
                    if (e.Value is TECController || e.Value is TECPanel)
                    {
                        Riser.Add(new ScopeSummaryItem(e.Value as TECScope, bid.Parameters));
                    }
                    else if (e.Value is TECMisc misc)
                    {
                        Misc.Add(new ScopeSummaryItem(misc, bid.Parameters));
                    }
                } else if (e.Change == Change.Remove)
                {
                    if (e.Value is TECController || e.Value is TECPanel)
                    {
                        removeFromCollection(Riser, e.Value as TECScope);
                    }
                    else if (e.Value is TECMisc misc)
                    {
                        removeFromCollection(Misc, misc);
                    }
                }
                RaisePropertyChanged("RiserTotal");
                RaisePropertyChanged("MiscTotal");
            }
        }

        private void removeFromCollection(ObservableCollection<ScopeSummaryItem> list, TECScope scope)
        {
            ScopeSummaryItem toRemove = null;
            foreach (var item in list)
            {
                if (item.Scope == scope)
                {
                    toRemove = item;
                    break;
                }
            }
            if (toRemove != null)
                list.Remove(toRemove);
        }

        private void populateSystems(ObservableCollection<TECTypical> typicals)
        {
            ObservableCollection<SystemSummaryItem> systemItems = new ObservableCollection<SystemSummaryItem>();
            foreach(TECTypical typical in typicals)
            {
                SystemSummaryItem summaryItem = new SystemSummaryItem(typical, bid.Parameters);
                summaryItem.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Total")
                    {
                        RaisePropertyChanged("SystemTotal");
                    }
                };
                systemItems.Add(summaryItem);
            }
            Systems = systemItems;
        }
        private void populateMisc(ObservableCollection<TECMisc> miscCosts)
        {
            ObservableCollection<ScopeSummaryItem> miscItems = new ObservableCollection<ScopeSummaryItem>();
            foreach(TECMisc misc in miscCosts)
            {
                ScopeSummaryItem summaryItem = new ScopeSummaryItem(misc, bid.Parameters);
                summaryItem.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Total")
                    {
                        RaisePropertyChanged("MiscTotal");
                    }
                };
                miscItems.Add(summaryItem);
            }
            misc = miscItems;
        }
        private void populateRiser(ReadOnlyObservableCollection<TECController> controllers, ObservableCollection<TECPanel> panels)
        {
            ObservableCollection<ScopeSummaryItem> riserItems = new ObservableCollection<ScopeSummaryItem>();
            foreach (TECController controller in controllers)
            {
                ScopeSummaryItem summaryItem = new ScopeSummaryItem(controller, bid.Parameters);
                summaryItem.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Total")
                    {
                        RaisePropertyChanged("RiserTotal");
                    }
                };
                riserItems.Add(summaryItem);
            }
            foreach(TECPanel panel in panels)
            {
                ScopeSummaryItem summaryItem = new ScopeSummaryItem(panel, bid.Parameters);
                summaryItem.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Total")
                    {
                        RaisePropertyChanged("RiserTotal");
                    }
                };
                riserItems.Add(summaryItem);
            }
            riser = riserItems;
        }

        private double getSystemTotal()
        {
            return systems.Aggregate(0.0, (acc, x) => acc + x.Estimate.TotalPrice);
        }
        private double getRiserTotal()
        {
            return riser.Aggregate(0.0, (acc, x) => acc + x.Estimate.TotalPrice);
        }
        private double getMiscTotal()
        {
            return misc.Aggregate(0.0, (acc, x) => acc + x.Estimate.TotalPrice);
        }
    }
}
