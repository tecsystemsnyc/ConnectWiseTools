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

            foreach (TECMisc misc in bid.MiscCosts)
            {
                AddMiscCost(misc);
            }
            foreach (TECSystem typical in bid.Systems)
            {
                foreach (TECSystem instance in typical.SystemInstances)
                {
                    AddInstanceSystem(instance, typical);
                }
            }
            foreach (TECPanel panel in bid.Panels)
            {
                AddPanel(panel);
            }
            foreach (TECController controller in bid.Controllers)
            {
                AddController(controller);
            }
        }

        #region Add/Remove
        public void AddTypicalSystem(TECSystem typical)
        {
            foreach (TECSystem instance in typical.SystemInstances)
            {
                foreach (TECMisc misc in typical.MiscCosts)
                {
                    AddMiscCost(misc);
                }
            }
        }
        public void RemoveTypicalSystem(TECSystem typical)
        {
            foreach (TECSystem instance in typical.SystemInstances)
            {
                foreach (TECMisc misc in typical.MiscCosts)
                {
                    RemoveMiscCost(misc);
                }
            }
        }

        public void AddInstanceSystem(TECSystem instance, TECSystem typical)
        {
            foreach (TECCost cost in instance.AssociatedCosts)
            {
                AddAssCost(cost);
            }
            foreach (TECEquipment equip in instance.Equipment)
            {
                AddEquipment(equip);
            }
            foreach (TECPanel panel in instance.Panels)
            {
                AddPanel(panel);
            }
            foreach (TECController controller in instance.Controllers)
            {
                AddController(controller);
            }
            foreach (TECMisc misc in typical.MiscCosts)
            {
                AddMiscCost(misc);
            }
        }
        public void RemoveInstanceSystem(TECSystem instance, TECSystem typical)
        {
            foreach (TECCost cost in instance.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
            foreach (TECEquipment equip in instance.Equipment)
            {
                RemoveEquipment(equip);
            }
            foreach (TECPanel panel in instance.Panels)
            {
                RemovePanel(panel);
            }
            foreach (TECController controller in instance.Controllers)
            {
                RemoveController(controller);
            }
            foreach (TECMisc misc in typical.MiscCosts)
            {
                RemoveMiscCost(misc);
            }
        }

        public void AddController(TECController controller)
        {
            foreach (TECCost cost in controller.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) AddAssCost(cost);
            }
        }
        public void RemoveController(TECController controller)
        {
            foreach (TECCost cost in controller.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) RemoveAssCost(cost);
            }
        }

        public void AddConnection(TECConnection connection)
        {
            if (connection is TECNetworkConnection)
            {
                foreach (TECCost cost in (connection as TECNetworkConnection).ConnectionType.AssociatedCosts)
                {
                    if (cost.Type == CostType.TEC)
                    {
                        AddAssCost(cost);
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
                            AddAssCost(cost);
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
            foreach (TECCost cost in dev.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) AddAssCost(cost);
            }
        }
        public void RemoveDevice(TECDevice dev)
        {
            foreach (TECCost cost in dev.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) RemoveAssCost(cost);
            }
        }

        public void AddPoint(TECPoint point)
        {
            foreach (TECCost cost in point.AssociatedCosts)
            {
                AddAssCost(cost);
            }
        }
        public void RemovePoint(TECPoint point)
        {
            foreach (TECCost cost in point.AssociatedCosts)
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

        public void AddMiscCost(TECMisc misc)
        {
            if (misc.Type == this.CostType)
            {
                bool containsMisc = miscCostDictionary.ContainsKey(misc.Guid);
                if (containsMisc)
                {
                    CostSummaryItem miscItem = miscCostDictionary[misc.Guid];
                    MiscCostSubTotalCost -= miscItem.TotalCost;
                    MiscCostSubTotalLabor -= miscItem.TotalLabor;
                    miscItem.Quantity += misc.Quantity;
                    MiscCostSubTotalCost += miscItem.TotalCost;
                    MiscCostSubTotalLabor += miscItem.TotalLabor;
                }
                else
                {
                    misc.PropertyChanged += Misc_PropertyChanged;
                    CostSummaryItem miscItem = new CostSummaryItem(misc);
                    miscItem.PropertyChanged += MiscItem_PropertyChanged;
                    miscCostDictionary.Add(misc.Guid, miscItem);
                    MiscCostSummaryItems.Add(miscItem);
                    MiscCostSubTotalCost += miscItem.TotalCost;
                    MiscCostSubTotalLabor += miscItem.TotalLabor;
                }
            }
        }



        public void RemoveMiscCost(TECMisc misc)
        {
            if (misc.Type == this.CostType)
            {
                bool containsMisc = miscCostDictionary.ContainsKey(misc.Guid);
                if (containsMisc)
                {
                    CostSummaryItem miscItem = miscCostDictionary[misc.Guid];
                    MiscCostSubTotalCost -= miscItem.TotalCost;
                    MiscCostSubTotalLabor -= miscItem.TotalLabor;
                    miscItem.Quantity -= misc.Quantity;
                    MiscCostSubTotalCost += miscItem.TotalCost;
                    MiscCostSubTotalLabor += miscItem.TotalLabor;

                    if (miscItem.Quantity < 1)
                    {
                        misc.PropertyChanged -= Misc_PropertyChanged;
                        miscItem.PropertyChanged -= MiscItem_PropertyChanged;
                        MiscCostSummaryItems.Remove(miscItem);
                        miscCostDictionary.Remove(misc.Guid);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Misc not found in misc dictionary.");
                }
            }
        }

        public void AddPanel(TECPanel panel)
        {
            foreach (TECCost cost in panel.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) AddAssCost(cost);
            }
        }
        public void RemovePanel(TECPanel panel)
        {
            foreach (TECCost cost in panel.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical) RemoveAssCost(cost);
            }
        }
        #endregion

        #region Event Handlers
        private void Misc_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                if (args.PropertyName == "Quantity")
                {
                    CostSummaryItem miscItem = miscCostDictionary[(args.OldValue as TECMisc).Guid];
                    miscItem.Quantity /= (args.OldValue as TECMisc).Quantity;
                    miscItem.Quantity *= (args.NewValue as TECMisc).Quantity;
                }
            }
        }
        private void MiscItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;

                if (args.PropertyName == "Total")
                {
                    MiscCostSubTotalCost -= (args.OldValue as CostSummaryItem).TotalCost;
                    MiscCostSubTotalLabor -= (args.OldValue as CostSummaryItem).TotalLabor;
                    MiscCostSubTotalCost += (args.NewValue as CostSummaryItem).TotalCost;
                    MiscCostSubTotalLabor += (args.NewValue as CostSummaryItem).TotalLabor;
                }
            }
        }
        #endregion
    }
}