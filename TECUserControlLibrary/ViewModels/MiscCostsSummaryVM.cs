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

        public CostType CostType { get; private set; }

        private Dictionary<Guid, CostSummaryItem> miscCostDictionary;

        private ObservableCollection<CostSummaryItem> _miscCostSummaryItems;
        public ObservableCollection<CostSummaryItem> MiscCostSummaryItems
        {
            get { return _miscCostSummaryItems; }
            set
            {
                _miscCostSummaryItems = value;
                RaisePropertyChanged("MiscCostSummaryItems");
            }
        }

        private Dictionary<Guid, CostSummaryItem> assCostDictionary;

        private ObservableCollection<CostSummaryItem> _assCostSummaryItems;
        public ObservableCollection<CostSummaryItem> AssCostSummaryItems
        {
            get { return _assCostSummaryItems; }
            set
            {
                _assCostSummaryItems = value;
                RaisePropertyChanged("AssCostSummaryItems");
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
            }
        }

        private double _assCostSubTotalCost;
        public double AssCostSubTotalCost
        {
            get { return _assCostSubTotalCost; }
            set
            {
                _assCostSubTotalCost = value;
                RaisePropertyChanged("AssCostSubTotalCost");
            }
        }

        private double _assCostSubTotalLabor;
        public double AssCostSubTotalLabor
        {
            get { return _assCostSubTotalLabor; }
            set
            {
                _assCostSubTotalLabor = value;
                RaisePropertyChanged("AssCostSubTotalLabor");
            }
        }
        #endregion

        public MiscCostsSummaryVM(TECBid bid, CostType type)
        {
            CostType = type;
            reinitialize(bid);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            Bid = bid;

            miscCostDictionary = new Dictionary<Guid, CostSummaryItem>();
            MiscCostSummaryItems = new ObservableCollection<CostSummaryItem>();

            assCostDictionary = new Dictionary<Guid, CostSummaryItem>();
            AssCostSummaryItems = new ObservableCollection<CostSummaryItem>();

            MiscCostSubTotalCost = 0;
            MiscCostSubTotalLabor = 0;
            AssCostSubTotalCost = 0;
            AssCostSubTotalLabor = 0;

            foreach (TECMisc cost in bid.MiscCosts)
            {
                AddMiscCost(cost);
            }
            foreach (TECSystem typical in bid.Systems)
            {
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    AddSystem(instance);
                    foreach(TECMisc misc in typical.MiscCosts)
                    {
                        AddMiscCost(misc);
                    }
                }
            }
        }

        #region Add/Remove
        public void AddAssCost(TECCost cost)
        {
            if (cost.Type == this.CostType)
            {
                Tuple<double, double> delta = ElectricalMaterialSummaryVM.AddCost(cost, assCostDictionary, AssCostSummaryItems);
                AssCostSubTotalCost += delta.Item1;
                AssCostSubTotalLabor += delta.Item2;
            }
        }

        public void RemoveAssCost(TECCost cost)
        {
            if (cost.Type == this.CostType)
            {
                Tuple<double, double> delta = ElectricalMaterialSummaryVM.RemoveCost(cost, assCostDictionary, AssCostSummaryItems);
                AssCostSubTotalCost += delta.Item1;
                AssCostSubTotalLabor += delta.Item2;
            }
        }

        public void AddMiscCost(TECMisc misc)
        {
            if (misc.Type == this.CostType)
            {
                Tuple<double, double> delta = ElectricalMaterialSummaryVM.AddCost(misc, miscCostDictionary, MiscCostSummaryItems);
                MiscCostSubTotalCost += delta.Item1;
                MiscCostSubTotalLabor += delta.Item2;
            }
        }

        public void RemoveMiscCost(TECMisc misc)
        {
            if (misc.Type == this.CostType)
            {
                Tuple<double, double> delta = ElectricalMaterialSummaryVM.RemoveCost(misc, miscCostDictionary, MiscCostSummaryItems);
                MiscCostSubTotalCost += delta.Item1;
                MiscCostSubTotalLabor += delta.Item2;
            }
        }

        public void AddSubScope(TECSubScope subScope)
        {
            foreach(TECCost cost in subScope.AssociatedCosts)
            {
                AddAssCost(cost);
            }
        }

        public void RemoveSubScope(TECSubScope subScope)
        {
            foreach(TECCost cost in subScope.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
        }

        public void AddEquipment(TECEquipment equip)
        {
            foreach(TECCost cost in equip.AssociatedCosts)
            {
                AddAssCost(cost);
            }
            foreach(TECSubScope ss in equip.SubScope)
            {
                AddSubScope(ss);
            }
        }

        public void RemoveEquipment(TECEquipment equip)
        {
            foreach(TECCost cost in equip.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
            foreach(TECSubScope ss in equip.SubScope)
            {
                RemoveSubScope(ss);
            }
        }

        public void AddSystem(TECSystem system)
        {
            foreach(TECCost cost in system.AssociatedCosts)
            {
                AddAssCost(cost);
            }
            foreach(TECEquipment equip in system.Equipment)
            {
                AddEquipment(equip);
            }
        }

        public void RemoveSystem(TECSystem system)
        {
            foreach(TECCost cost in system.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
            foreach(TECEquipment equip in system.Equipment)
            {
                RemoveEquipment(equip);
            }
        }
        #endregion
    }
}