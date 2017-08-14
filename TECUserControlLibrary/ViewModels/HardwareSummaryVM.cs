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

namespace TECUserControlLibrary.ViewModels
{
    public class HardwareSummaryVM : ViewModelBase
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
            }
        }
        public double HardwareLabor
        {
            get { return _hardwareLabor; }
            private set
            {
                _hardwareLabor = value;
                RaisePropertyChanged("HardwareLabor");
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
            get { return _assocTECLaborTotal; }
            private set
            {
                _assocTECLaborTotal = value;
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

        public List<CostObject> AddHardware(TECHardware hardware)
        {
            List<CostObject> deltas = new List<CostObject>();
            bool containsItem = hardwareDictionary.ContainsKey(hardware.Guid);
            if (containsItem)
            {
                HardwareSummaryItem item = hardwareDictionary[hardware.Guid];
                CostObject delta = item.Increment();
                HardwareCost += delta.Cost;
                HardwareLabor += delta.Labor;
                delta.Type = CostType.TEC;
                deltas.Add(delta);
            }
            else
            {
                HardwareSummaryItem item = new HardwareSummaryItem(hardware);
                hardwareDictionary.Add(hardware.Guid, item);
                HardwareItems.Add(item);
                HardwareCost += item.TotalCost;
                HardwareLabor += item.TotalLabor;
                deltas.Add(new CostObject(item.TotalCost, item.TotalLabor, CostType.TEC));
            }
            foreach (TECCost cost in hardware.AssociatedCosts)
            {
                deltas.AddRange(AddCost(cost));
            }
            return deltas;
        }
        public List<CostObject> RemoveHardware(TECHardware hardware)
        {
            bool containsItem = hardwareDictionary.ContainsKey(hardware.Guid);
            if (containsItem)
            {
                List<CostObject> deltas = new List<CostObject>();
                HardwareSummaryItem item = hardwareDictionary[hardware.Guid];
                CostObject delta = item.Decrement();
                deltas.Add(delta);
                HardwareCost += delta.Cost;
                HardwareLabor += delta.Labor;

                if (item.Quantity < 1)
                {
                    HardwareItems.Remove(item);
                    hardwareDictionary.Remove(hardware.Guid);
                }
                foreach (TECCost cost in hardware.AssociatedCosts)
                {
                    deltas.AddRange(RemoveCost(cost));
                }
                return deltas;
            }
            else
            {
                throw new NullReferenceException("Hardware item not present in dictionary.");
            }
        }

        public List<CostObject> AddCost(TECCost cost)
        {
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = assocCostDictionary[cost.Guid];
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
                List<CostObject> deltas = new List<CostObject>();
                deltas.Add(delta);
                return deltas;
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
                List<CostObject> deltas = new List<CostObject>();
                deltas.Add(new CostObject(item.TotalCost, item.TotalLabor, cost.Type));
                return deltas;
            }
        }
        public List<CostObject> RemoveCost(TECCost cost)
        {
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = assocCostDictionary[cost.Guid];
                CostObject delta = item.RemoveQuantity(1);
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
                List<CostObject> deltas = new List<CostObject>();
                deltas.Add(delta);
                return deltas;
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
