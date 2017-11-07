using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels.Interfaces;

namespace TECUserControlLibrary.ViewModels
{
    public class HardwareSummaryVM : ViewModelBase, IComponentSummaryVM
    {
        #region Fields
        private Dictionary<Guid, HardwareSummaryItem> hardwareDictionary;
        private Dictionary<Guid, CostSummaryItem> assocCostDictionary;

        private ObservableCollection<HardwareSummaryItem> _hardwareItems;
        private ObservableCollection<CostSummaryItem> _assocTECItems;
        private ObservableCollection<CostSummaryItem> _assocElecItems;

        private double _hardwareCost;
        private double _hardwareLabor;
        private double _assocTECCostTotal;
        private double _assocTECLaborTotal;
        private double _assocElecCostTotal;
        private double _assocElecLaborTotal;
        #endregion

        //Constructor
        public HardwareSummaryVM()
        {
            initialize();
        }

        #region Properties
        public ObservableCollection<HardwareSummaryItem> HardwareItems
        {
            get { return _hardwareItems; }
            private set
            {
                _hardwareItems = value;
                RaisePropertyChanged("HardwareItems");
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

        public double HardwareCost
        {
            get { return _hardwareCost; }
            private set
            {
                _hardwareCost = value;
                RaisePropertyChanged("HardwareCost");
                RaisePropertyChanged("TotalTECCost");
            }
        }
        public double HardwareLabor
        {
            get { return _hardwareLabor; }
            private set
            {
                _hardwareLabor = value;
                RaisePropertyChanged("HardwareLabor");
                RaisePropertyChanged("TotalTECLabor");
            }
        }
        public double AssocTECCostTotal
        {
            get { return _assocTECCostTotal; }
            private set
            {
                _assocTECCostTotal = value;
                RaisePropertyChanged("AssocTECCostTotal");
                RaisePropertyChanged("TotalTECCost");
            }
        }
        public double AssocTECLaborTotal
        {
            get { return _assocTECLaborTotal; }
            private set
            {
                _assocTECLaborTotal = value;
                RaisePropertyChanged("AssocTECLaborTotal");
                RaisePropertyChanged("TotalTECLabor");
            }
        }
        public double AssocElecCostTotal
        {
            get { return _assocElecCostTotal; }
            private set
            {
                _assocElecCostTotal = value;
                RaisePropertyChanged("AssocElecCostTotal");
                RaisePropertyChanged("TotalElecCost");
            }
        }
        public double AssocElecLaborTotal
        {
            get { return _assocElecLaborTotal; }
            private set
            {
                _assocElecLaborTotal = value;
                RaisePropertyChanged("AssocElecLaborTotal");
                RaisePropertyChanged("TotalElecLabor");
            }
        }

        public double TotalTECCost
        {
            get
            {
                return (HardwareCost + AssocTECCostTotal);
            }
        }
        public double TotalTECLabor
        {
            get
            {
                return (HardwareLabor + AssocTECLaborTotal);
            }
        }
        public double TotalElecCost
        {
            get
            {
                return AssocElecCostTotal;
            }
        }
        public double TotalElecLabor
        {
            get
            {
                return AssocElecLaborTotal;
            }
        }
        #endregion

        #region Methods
        public void Reset()
        {
            initialize();
        }

        public CostBatch AddHardware(TECHardware hardware)
        {
            CostBatch deltas = new CostBatch();
            bool containsItem = hardwareDictionary.ContainsKey(hardware.Guid);
            if (containsItem)
            {
                HardwareSummaryItem item = hardwareDictionary[hardware.Guid];
                CostBatch delta = item.Increment();
                HardwareCost += delta.GetCost(hardware.Type);
                HardwareLabor += delta.GetLabor(hardware.Type);
                deltas += delta;
            }
            else
            {
                HardwareSummaryItem item = new HardwareSummaryItem(hardware);
                hardwareDictionary.Add(hardware.Guid, item);
                HardwareItems.Add(item);
                HardwareCost += item.TotalCost;
                HardwareLabor += item.TotalLabor;
                deltas += new CostBatch(item.TotalCost, item.TotalLabor, hardware.Type);
            }
            foreach (TECCost cost in hardware.AssociatedCosts)
            {
                deltas += AddCost(cost);
            }
            return deltas;
        }
        public CostBatch RemoveHardware(TECHardware hardware)
        {
            bool containsItem = hardwareDictionary.ContainsKey(hardware.Guid);
            if (containsItem)
            {
                CostBatch deltas = new CostBatch();
                HardwareSummaryItem item = hardwareDictionary[hardware.Guid];
                CostBatch delta = item.Decrement();
                deltas += delta;
                HardwareCost += delta.GetCost(hardware.Type);
                HardwareLabor += delta.GetLabor(hardware.Type);

                if (item.Quantity < 1)
                {
                    HardwareItems.Remove(item);
                    hardwareDictionary.Remove(hardware.Guid);
                }
                foreach (TECCost cost in hardware.AssociatedCosts)
                {
                    deltas += RemoveCost(cost);
                }
                return deltas;
            }
            else
            {
                throw new NullReferenceException("Hardware item not present in dictionary.");
            }
        }

        public CostBatch AddCost(TECCost cost)
        {
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = assocCostDictionary[cost.Guid];
                CostBatch delta = item.AddQuantity(1);
                if (cost.Type == CostType.TEC)
                {
                    AssocTECCostTotal += delta.GetCost(CostType.TEC);
                    AssocTECLaborTotal += delta.GetLabor(CostType.TEC);
                }
                else if (cost.Type == CostType.Electrical)
                {
                    AssocElecCostTotal += delta.GetCost(CostType.Electrical);
                    AssocElecLaborTotal += delta.GetLabor(CostType.Electrical);
                }
                return delta;
            }
            else
            {
                CostSummaryItem item = new CostSummaryItem(cost);
                assocCostDictionary.Add(cost.Guid, item);
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
                return new CostBatch(item.TotalCost, item.TotalLabor, cost.Type);
            }
        }
        public CostBatch RemoveCost(TECCost cost)
        {
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = assocCostDictionary[cost.Guid];
                CostBatch delta = item.RemoveQuantity(1);
                if (cost.Type == CostType.TEC)
                {
                    AssocTECCostTotal += delta.GetCost(CostType.TEC);
                    AssocTECLaborTotal += delta.GetLabor(CostType.TEC);
                }
                else if (cost.Type == CostType.Electrical)
                {
                    AssocElecCostTotal += delta.GetCost(CostType.Electrical);
                    AssocElecLaborTotal += delta.GetLabor(CostType.Electrical);
                }
                if (item.Quantity < 1)
                {
                    assocCostDictionary.Remove(cost.Guid);
                    if (cost.Type == CostType.TEC)
                    {
                        AssocTECItems.Remove(item);
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        AssocElecItems.Remove(item);
                    }
                }
                return delta;
            }
            else
            {
                throw new NullReferenceException("Cost item not present in dictionary.");
            }
        }

        private void initialize()
        {
            hardwareDictionary = new Dictionary<Guid, HardwareSummaryItem>();
            assocCostDictionary = new Dictionary<Guid, CostSummaryItem>();

            HardwareItems = new ObservableCollection<HardwareSummaryItem>();
            AssocTECItems = new ObservableCollection<CostSummaryItem>();
            AssocElecItems = new ObservableCollection<CostSummaryItem>();

            HardwareCost = 0;
            HardwareLabor = 0;
            AssocTECCostTotal = 0;
            AssocTECLaborTotal = 0;
            AssocElecCostTotal = 0;
            AssocElecLaborTotal = 0;
        }
        #endregion
    }
}
