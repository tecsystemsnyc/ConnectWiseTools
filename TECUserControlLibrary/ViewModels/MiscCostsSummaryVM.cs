using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class MiscCostsSummaryVM : ViewModelBase
    {
        #region Fields
        private Dictionary<Guid, CostSummaryItem> costDictionary;

        private ObservableCollection<CostSummaryItem> _miscTECItems;
        private ObservableCollection<CostSummaryItem> _miscElecItems;
        private ObservableCollection<CostSummaryItem> _assocTECItems;
        private ObservableCollection<CostSummaryItem> _assocElecItems;

        private double _miscTECCostTotal;
        private double _miscTECLaborTotal;
        private double _miscElecCostTotal;
        private double _miscElecLaborTotal;
        private double _assocTECCostTotal;
        private double _assocTECLaborTotal;
        private double _assocElecCostTotal;
        private double _assocElecLaborTotal;
        #endregion

        //Constructor
        public MiscCostsSummaryVM()
        {
            initialize();
        }
        
        #region Properties
        public ObservableCollection<CostSummaryItem> MiscTECItems
        {
            get { return _miscTECItems; }
            private set
            {
                _miscTECItems = value;
                RaisePropertyChanged("MiscTECItems");
            }
        }
        public ObservableCollection<CostSummaryItem> MiscElecItems
        {
            get { return _miscElecItems; }
            private set
            {
                _miscElecItems = value;
                RaisePropertyChanged("MiscElecItems");
            }
        }
        public ObservableCollection<CostSummaryItem> AssocTECItems
        {
            get { return _assocTECItems; }
            private set
            {
                _assocTECItems = value;
                RaisePropertyChanged("AssocTECItems");
            }
        }
        public ObservableCollection<CostSummaryItem> AssocElecItems
        {
            get { return _assocElecItems; }
            private set
            {
                _assocElecItems = value;
                RaisePropertyChanged("AssocElecItems");
            }
        }

        public double MiscTECCostTotal
        {
            get { return _miscTECCostTotal; }
            private set
            {
                _miscTECCostTotal = value;
                RaisePropertyChanged("MiscTECCostTotal");
            }
        }
        public double MiscTECLaborTotal
        {
            get { return _miscTECLaborTotal; }
            private set
            {
                _miscTECLaborTotal = value;
                RaisePropertyChanged("MiscTECLaborTotal");
            }
        }
        public double MiscElecCostTotal
        {
            get { return _miscTECCostTotal; }
            private set
            {
                _miscTECCostTotal = value;
                RaisePropertyChanged("MiscElecCostTotal");
            }
        }
        public double MiscElecLaborTotal
        {
            get { return _miscElecLaborTotal; }
            private set
            {
                _miscElecLaborTotal = value;
                RaisePropertyChanged("MiscElecLaborTotal");
            }
        }
        public double AssocTECCostTotal
        {
            get { return _assocTECCostTotal; }
            private set
            {
                _assocTECCostTotal = value;
                RaisePropertyChanged("AssocTECCostTotal");
            }
        }
        public double AssocTECLaborTotal
        {
            get { return _assocElecLaborTotal; }
            private set
            {
                _assocElecLaborTotal = value;
                RaisePropertyChanged("AssocTECLaborTotal");
            }
        }
        public double AssocElecCostTotal
        {
            get { return _assocElecCostTotal; }
            private set
            {
                _assocElecCostTotal = value;
                RaisePropertyChanged("AssocElecCostTotal");
            }
        }
        public double AssocElecLaborTotal
        {
            get { return _assocElecLaborTotal; }
            private set
            {
                _assocElecLaborTotal = value;
                RaisePropertyChanged("AssocElecLaborTotal");
            }
        }
        #endregion

        #region Methods
        public void Reset()
        {
            initialize();
        }

        public void AddCost(TECCost cost)
        {
            bool containsItem = costDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = costDictionary[cost.Guid];
                if (cost is TECMisc misc)
                {
                    CostObject delta = item.AddQuantity(misc.Quantity);
                    if (cost.Type == CostType.TEC)
                    {
                        MiscTECCostTotal += delta.Cost;
                        MiscTECLaborTotal += delta.Labor;
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        MiscElecCostTotal += delta.Cost;
                        MiscElecLaborTotal += delta.Labor;
                    }
                }
                else
                {
                    CostObject delta = item.AddQuantity(1);
                    if (cost.Type == CostType.TEC)
                    {
                        AssocTECCostTotal += delta.Cost;
                        AssocTECLaborTotal += delta.Labor;
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        AssocElecCostTotal += delta.Cost;
                        AssocElecLaborTotal += delta.Labor;
                    }
                }
            }
            else
            {
                CostSummaryItem item = new CostSummaryItem(cost);
                costDictionary.Add(cost.Guid, item);
                if (cost is TECMisc misc)
                {
                    if (cost.Type == CostType.TEC)
                    {
                        MiscTECItems.Add(item);
                        MiscTECCostTotal += item.TotalCost;
                        MiscTECLaborTotal += item.TotalLabor;
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        MiscElecItems.Add(item);
                        MiscElecCostTotal += item.TotalCost;
                        MiscElecLaborTotal += item.TotalLabor;
                    }
                }
                else
                {
                    if (cost.Type == CostType.TEC)
                    {
                        AssocTECItems.Add(item);
                        AssocTECCostTotal += item.TotalCost;
                        AssocTECLaborTotal += item.TotalLabor;
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        AssocElecItems.Add(item);
                        AssocElecCostTotal += item.TotalCost;
                        AssocElecLaborTotal += item.TotalLabor;
                    }
                }
            }
        }
        public void RemoveCost(TECCost cost)
        {
            bool containsItem = costDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = costDictionary[cost.Guid];
                if (cost is TECMisc misc)
                {
                    CostObject delta = item.RemoveQuantity(misc.Quantity);
                    if (cost.Type == CostType.TEC)
                    {
                        MiscTECCostTotal += delta.Cost;
                        MiscTECLaborTotal += delta.Labor;
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        MiscElecCostTotal += delta.Cost;
                        MiscElecLaborTotal += delta.Labor;
                    }
                }
                else
                {
                    CostObject delta = item.RemoveQuantity(1);
                    if (cost.Type == CostType.TEC)
                    {
                        AssocTECCostTotal += delta.Cost;
                        AssocTECLaborTotal += delta.Labor;
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        AssocElecCostTotal += delta.Cost;
                        AssocElecLaborTotal += delta.Cost;
                    }
                }
                if (item.Quantity < 1)
                {
                    costDictionary.Remove(cost.Guid);
                    if (cost is TECMisc miscToRemove)
                    {
                        if (cost.Type == CostType.TEC)
                        {
                            MiscTECItems.Remove(item);
                        }
                        else if (cost.Type == CostType.Electrical)
                        {
                            MiscElecItems.Remove(item);
                        }
                    }
                    else
                    {
                        if (cost.Type == CostType.TEC)
                        {
                            AssocTECItems.Remove(item);
                        }
                        else if (cost.Type == CostType.Electrical)
                        {
                            AssocElecItems.Remove(item);
                        }
                    }
                }
            }
            else
            {
                throw new NullReferenceException("Cost item not present in dictionary.");
            }
        }
        public void ResetMiscQuantity(TECMisc misc)
        {
            bool containsItem = costDictionary.ContainsKey(misc.Guid);
            if (containsItem)
            {
                CostSummaryItem item = costDictionary[misc.Guid];
                CostObject delta = item.ResetMiscQuantity();
                if (misc.Type == CostType.TEC)
                {
                    MiscTECCostTotal += delta.Cost;
                    MiscTECLaborTotal += delta.Labor;
                }
                else if (misc.Type == CostType.Electrical)
                {
                    MiscElecCostTotal += delta.Cost;
                    MiscElecLaborTotal += delta.Labor;
                }
            }
            else
            {
                throw new NullReferenceException("Misc item not present in dictionary.");
            }
        }

        private void initialize()
        {
            costDictionary = new Dictionary<Guid, CostSummaryItem>();

            MiscTECItems = new ObservableCollection<CostSummaryItem>();
            MiscElecItems = new ObservableCollection<CostSummaryItem>();
            AssocTECItems = new ObservableCollection<CostSummaryItem>();
            AssocElecItems = new ObservableCollection<CostSummaryItem>();

            MiscTECCostTotal = 0;
            MiscTECLaborTotal = 0;
            MiscElecCostTotal = 0;
            MiscElecLaborTotal = 0;
            AssocTECCostTotal = 0;
            AssocTECLaborTotal = 0;
            AssocElecCostTotal = 0;
            AssocElecLaborTotal = 0;
        }
        #endregion
    }
}