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
        #region Properties
        public Type HardwareType { get; private set; }

        private Dictionary<Guid, HardwareSummaryItem> hardwareDictionary;

        private ObservableCollection<HardwareSummaryItem> _hardwareItems;
        public ObservableCollection<HardwareSummaryItem> HardwareItems
        {
            get { return _hardwareItems; }
            private set
            {
                _hardwareItems = value;
                RaisePropertyChanged("HardwareItems");
            }
        }

        private Dictionary<Guid, CostSummaryItem> assCostDictionary;

        private ObservableCollection<CostSummaryItem> _assCostItems;
        public ObservableCollection<CostSummaryItem> AssCostItems
        {
            get { return _assCostItems; }
            private set
            {
                _assCostItems = value;
                RaisePropertyChanged("CostItems");
            }
        }

        private double _hardwareCost;
        public double HardwareCost
        {
            get { return _hardwareCost; }
            set
            {
                _hardwareCost = value;
                RaisePropertyChanged("HardwareCost");
            }
        }

        private double _hardwareLabor;
        public double HardwareLabor
        {
            get { return _hardwareLabor; }
            set
            {
                _hardwareLabor = value;
                RaisePropertyChanged("HardwareLabor");
            }
        }

        private double _associatedCostTotal;
        public double AssociatedCostTotal
        {
            get { return _associatedCostTotal; }
            set
            {
                _associatedCostTotal = value;
                RaisePropertyChanged("AssociatedCostTotal");
            }
        }

        private double _associatedLaborTotal;
        public double AssociatedLaborTotal
        {
            get { return _associatedLaborTotal; }
            set
            {
                _associatedLaborTotal = value;
                RaisePropertyChanged("AssociatedLaborTotal");
            }
        }
        #endregion

        public HardwareSummaryVM(Type hardwareType)
        {
            if (hardwareType.IsSubclassOf(typeof(TECHardware)))
            {
                HardwareType = hardwareType;
                initialize();
            }
            else
            {
                throw new ArgumentException("Invalid hardware type.");
            }
        }

        public void Reset()
        {
            initialize();
        }

        private void initialize()
        {
            hardwareDictionary = new Dictionary<Guid, HardwareSummaryItem>();
            assCostDictionary = new Dictionary<Guid, CostSummaryItem>();

            HardwareItems = new ObservableCollection<HardwareSummaryItem>();
            AssCostItems = new ObservableCollection<CostSummaryItem>();

            HardwareCost = 0;
            HardwareLabor = 0;
            AssociatedCostTotal = 0;
            AssociatedLaborTotal = 0;
        }

        public void AddHardware(TECHardware hardware)
        {
            if (hardware.GetType() == HardwareType)
            {
                bool containsItem = hardwareDictionary.ContainsKey(hardware.Guid);
                if (containsItem)
                {
                    HardwareSummaryItem item = hardwareDictionary[hardware.Guid];
                    CostObject delta = item.Increment();
                    HardwareCost += delta.Cost;
                    HardwareLabor += delta.Labor;
                }
                else
                {
                    HardwareSummaryItem item = new HardwareSummaryItem(hardware);
                    hardwareDictionary.Add(hardware.Guid, item);
                    HardwareItems.Add(item);
                    HardwareCost += item.TotalCost;
                    HardwareLabor += item.TotalLabor;
                }
                foreach(TECCost cost in hardware.AssociatedCosts)
                {
                    AddCost(cost);
                }
            }
            else
            {
                throw new ArgumentException("Invalid hardware type.");
            }
        }
        public void RemoveHardware(TECHardware hardware)
        {
            bool containsItem = hardwareDictionary.ContainsKey(hardware.Guid);
            if (containsItem)
            {
                HardwareSummaryItem item = hardwareDictionary[hardware.Guid];
                CostObject delta = item.Decrement();
                HardwareCost += delta.Cost;
                HardwareLabor += delta.Labor;

                if (item.Quantity < 1)
                {
                    HardwareItems.Remove(item);
                    hardwareDictionary.Remove(hardware.Guid);
                }
                foreach(TECCost cost in hardware.AssociatedCosts)
                {
                    RemoveCost(cost);
                }
            }
            else
            {
                throw new NullReferenceException("Hardware item not present in dictionary.");
            }
        }

        private void AddCost(TECCost cost)
        {
            bool containsItem = assCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = assCostDictionary[cost.Guid];
                CostObject delta = null;
                if (cost is TECMisc misc)
                {
                    delta = item.AddQuantity(misc.Quantity);
                }
                else
                {
                    delta = item.AddQuantity(1);
                }
                AssociatedCostTotal += delta.Cost;
                AssociatedLaborTotal += delta.Labor;
            }
            else
            {
                CostSummaryItem item = new CostSummaryItem(cost);
                assCostDictionary.Add(cost.Guid, item);
                AssCostItems.Add(item);
                AssociatedCostTotal += item.TotalCost;
                AssociatedLaborTotal += item.TotalLabor;
            }
        }
        private void RemoveCost(TECCost cost)
        {
            bool containsItem = assCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = assCostDictionary[cost.Guid];
                CostObject delta = null;
                if (cost is TECMisc misc)
                {
                    delta = item.RemoveQuantity(misc.Quantity);
                }
                else
                {
                    delta = item.RemoveQuantity(1);
                }
                AssociatedCostTotal -= delta.Cost;
                AssociatedLaborTotal -= delta.Labor;
                
                if (item.Quantity < 1)
                {
                    AssCostItems.Remove(item);
                    assCostDictionary.Remove(cost.Guid);
                }
            }
            else
            {
                throw new NullReferenceException("Cost item not present in dictionary.");
            }
        }

        private void Item_CostChanged(double deltaCost, double deltaLabor)
        {
            HardwareCost += deltaCost;
            HardwareLabor += deltaLabor;
        }
    }
}
