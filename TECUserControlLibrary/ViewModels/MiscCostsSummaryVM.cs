using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class MiscCostsSummaryVM : ViewModelBase
    {
        #region Properties
        public TECBid Bid { get; private set; }

        private Dictionary<Guid, MiscCostSummaryItem> miscCostDictionary;

        private ObservableCollection<MiscCostSummaryItem> _miscCostSummaryItems;
        public ObservableCollection<MiscCostSummaryItem> MiscCostSummaryItems
        {
            get { return _miscCostSummaryItems; }
            set
            {
                _miscCostSummaryItems = value;
                RaisePropertyChanged("MiscCostSummaryItems");
            }
        }

        private double _miscCostSubTotalCost;
        public double MiscCostSubTotalCost
        {
            get { return _miscCostSubTotalCost; }
            set
            {
                _miscCostSubTotalCost = value;
                RaisePropertyChanged("MiscCostSubTotalCost");
                RaisePropertyChanged("TotalMiscCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _miscCostSubTotalLabor;
        public double MiscCostSubTotalLabor
        {
            get { return _miscCostSubTotalLabor; }
            set
            {
                _miscCostSubTotalLabor = value;
                RaisePropertyChanged("MiscCostSubTotalLabor");
                RaisePropertyChanged("TotalMiscLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        public MiscCostsSummaryVM(TECBid bid)
        {
            reinitialize(bid);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            Bid = bid;

            miscCostDictionary = new Dictionary<Guid, MiscCostSummaryItem>();
            MiscCostSummaryItems = new ObservableCollection<MiscCostSummaryItem>();

            MiscCostSubTotalCost = 0;
            MiscCostSubTotalLabor = 0;

            foreach (TECMisc cost in bid.MiscCosts)
            {
                AddMiscCost(cost);
            }
        }

        #region Add/Remove
        public void AddMiscCost(TECMisc cost)
        {
            bool containsCost = miscCostDictionary.ContainsKey(cost.Guid);
            if (containsCost)
            {
                MiscCostSubTotalCost -= miscCostDictionary[cost.Guid].TotalCost;
                MiscCostSubTotalLabor -= miscCostDictionary[cost.Guid].TotalLabor;
                miscCostDictionary[cost.Guid].Quantity += cost.Quantity;
                MiscCostSubTotalCost += miscCostDictionary[cost.Guid].TotalCost;
                MiscCostSubTotalLabor += miscCostDictionary[cost.Guid].TotalLabor;
            }
            else
            {
                MiscCostSummaryItem costItem = new MiscCostSummaryItem(cost);
                costItem.PropertyChanged += CostItem_PropertyChanged;
                miscCostDictionary.Add(cost.Guid, costItem);
                MiscCostSummaryItems.Add(costItem);
                MiscCostSubTotalCost += costItem.TotalCost;
                MiscCostSubTotalLabor += costItem.TotalLabor;
                cost.PropertyChanged += Cost_PropertyChanged;
            }
        }

        public void RemoveMiscCost(TECMisc cost)
        {
            bool containsCost = miscCostDictionary.ContainsKey(cost.Guid);
            if (containsCost)
            {
                MiscCostSubTotalCost -= miscCostDictionary[cost.Guid].TotalCost;
                MiscCostSubTotalLabor -= miscCostDictionary[cost.Guid].TotalLabor;
                miscCostDictionary[cost.Guid].Quantity -= cost.Quantity;
                MiscCostSubTotalCost += miscCostDictionary[cost.Guid].TotalCost;
                MiscCostSubTotalLabor += miscCostDictionary[cost.Guid].TotalLabor;

                if (miscCostDictionary[cost.Guid].Quantity < 1)
                {
                    miscCostDictionary[cost.Guid].PropertyChanged -= CostItem_PropertyChanged;
                    MiscCostSummaryItems.Remove(miscCostDictionary[cost.Guid]);
                    miscCostDictionary.Remove(cost.Guid);
                    cost.PropertyChanged -= Cost_PropertyChanged;
                }
            }
            else
            {
                throw new InvalidOperationException("Cost not found in cost dictionary.");
            }
        }
        #endregion

        #region Event Handlers
        private void CostItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;

                if (args.PropertyName == "TotalCost")
                {
                    MiscCostSubTotalCost -= (double)args.OldValue;
                    MiscCostSubTotalCost += (double)args.NewValue;
                }
                else if (args.PropertyName == "TotalLabor")
                {
                    MiscCostSubTotalLabor -= (double)args.OldValue;
                    MiscCostSubTotalLabor += (double)args.NewValue;
                }
            }
        }

        private void Cost_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;

                if (args.PropertyName == "Quantity")
                {
                    TECMisc misc = args.NewValue as TECMisc;
                    int deltaQuantity = ((TECMisc)args.NewValue).Quantity - ((TECMisc)args.OldValue).Quantity;

                    bool containsCost = false;
                    foreach (TECSystem sys in Bid.Systems)
                    {
                        //Costs is not what we want
                        foreach (TECMisc cost in sys.Costs)
                        {
                            if (misc.Guid == cost.Guid)
                            {
                                containsCost = true;
                                break;
                            }
                        }
                        if (containsCost)
                        {
                            int sysQuantity = sys.SystemInstances.Count;
                            miscCostDictionary[misc.Guid].Quantity += deltaQuantity * sysQuantity;
                            break;
                        }
                    }
                    if (!containsCost)
                    {
                        miscCostDictionary[misc.Guid].Quantity += deltaQuantity;
                    }
                }
            }
        }
        #endregion
    }
}