using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels.SummaryVMs
{
    public class SystemSummaryVM : ViewModelBase
    {
        private ObservableCollection<SystemSummaryItem> _systems;
        private ObservableCollection<ScopeSummaryItem> _riser;
        private ObservableCollection<ScopeSummaryItem> _misc;
        private TECBid bid;
        
        public ObservableCollection<SystemSummaryItem> Systems
        {
            get { return _systems; }
            set
            {
                _systems = value;
                RaisePropertyChanged("Systems");
            }
        }
        public ObservableCollection<ScopeSummaryItem> Riser
        {
            get { return _riser; }
            set
            {
                _riser = value;
                RaisePropertyChanged("Riser");
            }
        }
        public ObservableCollection<ScopeSummaryItem> Misc
        {
            get { return _misc; }
            set
            {
                _misc = value;
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
                    SystemSummaryItem summaryItem = new SystemSummaryItem(typical, bid.Parameters);
                    summaryItem.Estimate.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "TotalPrice")
                        {
                            RaisePropertyChanged("SystemTotal");
                        }
                    };
                    Systems.Add(summaryItem);

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
                        ScopeSummaryItem summaryItem = new ScopeSummaryItem(e.Value as TECScope, bid.Parameters);
                        summaryItem.Estimate.PropertyChanged += (sender, args) =>
                        {
                            if (args.PropertyName == "TotalPrice")
                            {
                                RaisePropertyChanged("RiserTotal");
                            }
                        };
                        Riser.Add(summaryItem);
                    }
                    else if (e.Value is TECMisc misc)
                    {
                        ScopeSummaryItem summaryItem = new ScopeSummaryItem(misc, bid.Parameters);
                        summaryItem.Estimate.PropertyChanged += (sender, args) =>
                        {
                            if (args.PropertyName == "TotalPrice")
                            {
                                RaisePropertyChanged("MiscTotal");
                            }
                        };
                        Misc.Add(summaryItem);
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
                summaryItem.Estimate.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "TotalPrice")
                    {
                        RaisePropertyChanged("SystemTotal");
                    }
                };
                systemItems.Add(summaryItem);
            }
            Systems = systemItems;
            RaisePropertyChanged("SystemTotal");
        }
        private void populateMisc(ObservableCollection<TECMisc> miscCosts)
        {
            ObservableCollection<ScopeSummaryItem> miscItems = new ObservableCollection<ScopeSummaryItem>();
            foreach(TECMisc misc in miscCosts)
            {
                ScopeSummaryItem summaryItem = new ScopeSummaryItem(misc, bid.Parameters);
                summaryItem.Estimate.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "TotalPrice")
                    {
                        RaisePropertyChanged("MiscTotal");
                    }
                };
                miscItems.Add(summaryItem);
            }
            Misc = miscItems;
            RaisePropertyChanged("MiscTotal");
        }
        private void populateRiser(ReadOnlyObservableCollection<TECController> controllers, ObservableCollection<TECPanel> panels)
        {
            ObservableCollection<ScopeSummaryItem> riserItems = new ObservableCollection<ScopeSummaryItem>();
            foreach (TECController controller in controllers)
            {
                ScopeSummaryItem summaryItem = new ScopeSummaryItem(controller, bid.Parameters);
                summaryItem.Estimate.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "TotalPrice")
                    {
                        RaisePropertyChanged("RiserTotal");
                    }
                };
                riserItems.Add(summaryItem);
            }
            foreach(TECPanel panel in panels)
            {
                ScopeSummaryItem summaryItem = new ScopeSummaryItem(panel, bid.Parameters);
                summaryItem.Estimate.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "TotalPrice")
                    {
                        RaisePropertyChanged("RiserTotal");
                    }
                };
                riserItems.Add(summaryItem);
            }
            Riser = riserItems;
            RaisePropertyChanged("RiserTotal");
        }

        private double getSystemTotal()
        {
            return Systems.Sum(item => item.Estimate.TotalPrice);
        }
        private double getRiserTotal()
        {
            return Riser.Sum(item => item.Estimate.TotalPrice);
        }
        private double getMiscTotal()
        {
            return Misc.Sum(item => item.Estimate.TotalPrice);
        }
    }
}
