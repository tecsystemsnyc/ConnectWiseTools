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
            
            MiscCostSummaryItems = new ObservableCollection<CostSummaryItem>();

            assCostDictionary = new Dictionary<Guid, CostSummaryItem>();
            AssCostSummaryItems = new ObservableCollection<CostSummaryItem>();

            MiscCostSubTotalCost = 0;
            MiscCostSubTotalLabor = 0;
            AssCostSubTotalCost = 0;
            AssCostSubTotalLabor = 0;

            foreach(TECMisc misc in bid.MiscCosts)
            {
                AddMiscCost(misc);
            }
            foreach(TECSystem typical in bid.Systems)
            {
                AddTypicalSystem(typical);
                foreach(TECSystem instance in typical.SystemInstances)
                {
                    AddInstanceSystem(instance);
                }
            }
            foreach(TECPanel panel in bid.Panels)
            {
                AddPanel(panel);
            }
            foreach(TECController controller in bid.Controllers)
            {
                AddController(controller);
            }
        }

        #region Add/Remove
        public void AddTypicalSystem(TECSystem system)
        {
            foreach(TECMisc misc in system.MiscCosts)
            {
                AddMiscCost(misc, system);
            }
        }
        public void RemoveTypicalSystem(TECSystem system)
        {
            foreach(TECMisc misc in system.MiscCosts)
            {
                RemoveMiscCost(misc);
            }
        }

        public void AddInstanceSystem(TECSystem system)
        {
            foreach (TECCost cost in system.AssociatedCosts)
            {
                AddAssCost(cost);
            }
            foreach (TECEquipment equip in system.Equipment)
            {
                AddEquipment(equip);
            }
            foreach (TECPanel panel in system.Panels)
            {
                AddPanel(panel);
            }
            foreach (TECController controller in system.Controllers)
            {
                AddController(controller);
            }
        }
        public void RemoveInstanceSystem(TECSystem system)
        {
            foreach (TECCost cost in system.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
            foreach (TECEquipment equip in system.Equipment)
            {
                RemoveEquipment(equip);
            }
            foreach (TECPanel panel in system.Panels)
            {
                RemovePanel(panel);
            }
            foreach (TECController controller in system.Controllers)
            {
                RemoveController(controller);
            }
        }

        public void AddController(TECController controller)
        {
            foreach(TECCost cost in controller.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) AddAssCost(cost);
            }
        }
        public void RemoveController(TECController controller)
        {
            foreach(TECCost cost in controller.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) RemoveAssCost(cost);
            }
        }

        public void AddConnection(TECConnection connection)
        {
            if (connection is TECNetworkConnection)
            {
                foreach(TECCost cost in (connection as TECNetworkConnection).ConnectionType.AssociatedCosts)
                {
                    if (cost.Type == CostType.TEC)
                    {
                        AddAssCost(cost);
                    }
                }
            }
            else if (connection is TECSubScopeConnection)
            {
                foreach(TECConnectionType type in (connection as TECSubScopeConnection).ConnectionTypes)
                {
                    foreach(TECCost cost in type.AssociatedCosts)
                    {
                        if (cost.Type == CostType.TEC)
                        {
                            AddAssCost(cost);
                        }
                    }
                }
            }

            if (connection.ConduitType != null)
            {
                foreach(TECCost cost in connection.ConduitType.AssociatedCosts)
                {
                    if (cost.Type == CostType.TEC)
                    {
                        AddAssCost(cost);
                    }
                }
            }
        }
        public void RemoveConnection(TECConnection connection)
        {
            if (connection is TECNetworkConnection)
            {
                foreach (TECCost cost in (connection as TECNetworkConnection).ConnectionType.AssociatedCosts)
                {
                    if (cost.Type == CostType.TEC)
                    {
                        RemoveAssCost(cost);
                    }
                }
            }
            else if (connection is TECSubScopeConnection)
            {
                foreach (TECConnectionType type in (connection as TECSubScopeConnection).ConnectionTypes)
                {
                    foreach (TECCost cost in type.AssociatedCosts)
                    {
                        if (cost.Type == CostType.TEC)
                        {
                            RemoveAssCost(cost);
                        }
                    }
                }
            }

            if (connection.ConduitType != null)
            {
                foreach (TECCost cost in connection.ConduitType.AssociatedCosts)
                {
                    if (cost.Type == CostType.TEC)
                    {
                        RemoveAssCost(cost);
                    }
                }
            }
        }

        public void AddEquipment(TECEquipment equip)
        {
            foreach (TECCost cost in equip.AssociatedCosts)
            {
                AddAssCost(cost);
            }
            foreach (TECSubScope ss in equip.SubScope)
            {
                AddSubScope(ss);
            }
        }
        public void RemoveEquipment(TECEquipment equip)
        {
            foreach (TECCost cost in equip.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
            foreach (TECSubScope ss in equip.SubScope)
            {
                RemoveSubScope(ss);
            }
        }

        public void AddSubScope(TECSubScope subScope)
        {
            foreach (TECCost cost in subScope.AssociatedCosts)
            {
                AddAssCost(cost);
            }
            foreach (TECPoint point in subScope.Points)
            {
                AddPoint(point);
            }
            foreach (TECDevice dev in subScope.Devices)
            {
                AddDevice(dev);
            }
        }
        public void RemoveSubScope(TECSubScope subScope)
        {
            foreach (TECCost cost in subScope.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
            foreach (TECPoint point in subScope.Points)
            {
                RemovePoint(point);
            }
            foreach (TECDevice dev in subScope.Devices)
            {
                RemoveDevice(dev);
            }
        }

        public void AddDevice(TECDevice dev)
        {
            foreach(TECCost cost in dev.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) AddAssCost(cost);
            }
        }
        public void RemoveDevice(TECDevice dev)
        {
            foreach(TECCost cost in dev.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) RemoveAssCost(cost);
            }
        }

        public void AddPoint(TECPoint point)
        {
            foreach(TECCost cost in point.AssociatedCosts)
            {
                AddAssCost(cost);
            }
        }
        public void RemovePoint(TECPoint point)
        {
            foreach(TECCost cost in point.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
        }

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

        public void AddMiscCost(TECMisc misc, TECSystem system = null)
        {
            if (misc.Type == this.CostType)
            {
                CostSummaryItem miscItem = null;
                if (system != null)
                {
                    miscItem = new CostSummaryItem(misc, system);
                }
                else
                {
                    miscItem = new CostSummaryItem(misc);
                }
                MiscCostSubTotalCost += miscItem.TotalCost;
                MiscCostSubTotalLabor += miscItem.TotalLabor;
                MiscCostSummaryItems.Add(miscItem);
            }
        }
        public void RemoveMiscCost(TECMisc misc)
        {
            if (misc.Type == this.CostType)
            {
                CostSummaryItem itemToRemove = null;
                foreach (CostSummaryItem miscItem in MiscCostSummaryItems)
                {
                    if (misc.Guid == miscItem.Cost.Guid)
                    {
                        MiscCostSubTotalCost -= miscItem.TotalCost;
                        MiscCostSubTotalLabor -= miscItem.TotalLabor;
                        itemToRemove = miscItem;
                        break;
                    }
                }
                if (itemToRemove != null)
                {
                    MiscCostSummaryItems.Remove(itemToRemove);
                }
                else
                {
                    throw new InvalidOperationException("Misc not found in summary items.");
                }
            }
        }

        public void AddPanel(TECPanel panel)
        {
            foreach(TECCost cost in panel.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) AddAssCost(cost);
            }
        }
        public void RemovePanel(TECPanel panel)
        {
            foreach(TECCost cost in panel.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) RemoveAssCost(cost);
            }
        }
        #endregion
    }
}