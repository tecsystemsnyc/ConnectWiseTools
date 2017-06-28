using EstimateBuilder.Model;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class ElectricalMaterialSummaryVM : ViewModelBase
    {
        #region Properties
        private ChangeWatcher _changeWatcher;
        private ChangeWatcher changeWatcher
        {
            get { return _changeWatcher; }
            set
            {
                if (_changeWatcher != null)
                {
                    _changeWatcher.Changed -= bidChanged;
                    _changeWatcher.InstanceChanged -= instanceChanged;
                }
                _changeWatcher = value;
                _changeWatcher.Changed += bidChanged;
                _changeWatcher.InstanceChanged += instanceChanged;
            }
        }

        #region View Models
        public LengthSummaryVM WireSummaryVM { get; private set; }
        public LengthSummaryVM ConduitSummaryVM { get; private set; }
        public MiscCostsSummaryVM MiscCostsSummaryVM { get; private set; }
        #endregion

        #region Summary Totals
        public double TotalWireCost
        {
            get { return (WireSummaryVM.LengthSubTotalCost + WireSummaryVM.AssCostSubTotalCost + WireSummaryVM.RatedCostSubTotalCost); }
        }

        public double TotalWireLabor
        {
            get { return (WireSummaryVM.LengthSubTotalLabor + WireSummaryVM.AssCostSubTotalLabor + WireSummaryVM.RatedCostSubTotalLabor); }
        }

        public double TotalConduitCost
        {
            get { return (ConduitSummaryVM.LengthSubTotalCost + ConduitSummaryVM.AssCostSubTotalCost + ConduitSummaryVM.RatedCostSubTotalCost); }
        }

        public double TotalConduitLabor
        {
            get { return (ConduitSummaryVM.LengthSubTotalLabor + ConduitSummaryVM.AssCostSubTotalLabor + ConduitSummaryVM.RatedCostSubTotalLabor); }
        }

        public double TotalMiscCost
        {
            get { return MiscCostsSummaryVM.MiscCostSubTotalCost + MiscCostsSummaryVM.AssCostSubTotalCost; }
        }

        public double TotalMiscLabor
        {
            get { return MiscCostsSummaryVM.MiscCostSubTotalLabor + MiscCostsSummaryVM.AssCostSubTotalLabor; }
        }

        public double TotalCost
        {
            get { return TotalWireCost + TotalConduitCost + TotalMiscCost; }
        }

        public double TotalLabor
        {
            get { return TotalWireLabor + TotalConduitLabor + TotalMiscLabor; }
        }
        #endregion

        #endregion

        public ElectricalMaterialSummaryVM(TECBid bid)
        {
            initializeVMs(bid);
            reinitialize(bid);
        }
        
        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
            refreshVMs(bid);
        }

        private void reinitialize(TECBid bid)
        {
            changeWatcher = new ChangeWatcher(bid);
        }

        private void initializeVMs(TECBid bid)
        {
            WireSummaryVM = new LengthSummaryVM(bid, LengthType.Wire);
            ConduitSummaryVM = new LengthSummaryVM(bid, LengthType.Conduit);
            MiscCostsSummaryVM = new MiscCostsSummaryVM(bid, CostType.Electrical);

            WireSummaryVM.PropertyChanged += WireSummaryVM_PropertyChanged;
            ConduitSummaryVM.PropertyChanged += ConduitSummaryVM_PropertyChanged;
            MiscCostsSummaryVM.PropertyChanged += MiscCostsSummaryVM_PropertyChanged;
        }

        private void refreshVMs(TECBid bid)
        {
            WireSummaryVM.Refresh(bid);
            ConduitSummaryVM.Refresh(bid);
            MiscCostsSummaryVM.Refresh(bid);
        }

        #region Event Handlers
        private void bidChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                var targetObject = args.NewValue;
                var referenceObject = args.OldValue;

                if (args.PropertyName == "Add")
                {
                    if (targetObject is TECSystem && referenceObject is TECBid)
                    {
                        addTypicalSystem(targetObject as TECSystem);
                    }
                }
                else if (args.PropertyName == "Remove")
                {
                    if (targetObject is TECSystem && referenceObject is TECBid)
                    {
                        removeTypicalSystem(targetObject as TECSystem);
                    }
                }
            }
        }
        private void instanceChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                var targetObject = args.NewValue;
                var referenceObject = args.OldValue;

                if (args.PropertyName == "Add" || args.PropertyName == "AddCatalog")
                {
                    if (targetObject is TECSystem)
                    {
                        addInstanceSystem(targetObject as TECSystem, referenceObject as TECSystem);
                    }
                    else if (targetObject is TECController)
                    {
                        addController(targetObject as TECController);
                    }
                    else if (targetObject is TECPanel)
                    {
                        addPanel(targetObject as TECPanel);
                    }
                    else if (targetObject is TECConnection)
                    {
                        addConnection(targetObject as TECConnection);
                    }
                    else if (targetObject is TECEquipment)
                    {
                        addEquipment(targetObject as TECEquipment);
                    }
                    else if (targetObject is TECSubScope)
                    {
                        addSubScope(targetObject as TECSubScope);
                    }
                    else if (targetObject is TECPoint)
                    {
                        addPoint(targetObject as TECPoint);
                    }
                    else if (targetObject is TECDevice)
                    {
                        addDevice(targetObject as TECDevice);
                    }
                    else if (targetObject is TECMisc)
                    {
                        if (referenceObject is TECBid)
                        {
                            addMiscCost(targetObject as TECMisc);
                        }
                        else if (referenceObject is TECSystem)
                        {
                            foreach (TECSystem instance in (referenceObject as TECSystem).SystemInstances)
                            {
                                addMiscCost(targetObject as TECMisc);
                            }
                        }
                    }
                    else if (targetObject is TECCost)
                    {
                        addAssCost(targetObject as TECCost);
                    }
                }
                else if (args.PropertyName == "Remove" || args.PropertyName == "RemoveCatalog")
                {
                    if (targetObject is TECSystem)
                    {
                        removeInstanceSystem(targetObject as TECSystem, referenceObject as TECSystem);
                    }
                    else if (targetObject is TECController)
                    {
                        removeController(targetObject as TECController);
                    }
                    else if (targetObject is TECPanel)
                    {
                        removePanel(targetObject as TECPanel);
                    }
                    else if (targetObject is TECConnection)
                    {
                        removeConnection(targetObject as TECConnection);
                    }
                    else if (targetObject is TECEquipment)
                    {
                        removeEquipment(targetObject as TECEquipment);
                    }
                    else if (targetObject is TECSubScope)
                    {
                        removeSubScope(targetObject as TECSubScope);
                    }
                    else if (targetObject is TECPoint)
                    {
                        removePoint(targetObject as TECPoint);
                    }
                    else if (targetObject is TECDevice)
                    {
                        removeDevice(targetObject as TECDevice);
                    }
                    else if (targetObject is TECMisc)
                    {
                        if (referenceObject is TECBid)
                        {
                            removeMiscCost(targetObject as TECMisc);
                        }
                        else if (referenceObject is TECSystem)
                        {
                            foreach(TECSystem instance in (referenceObject as TECSystem).SystemInstances)
                            {
                                removeMiscCost(targetObject as TECMisc);
                            }
                        }
                    }
                    else if (targetObject is TECCost)
                    {
                        removeAssCost(targetObject as TECCost);
                    }
                }
                else if (args.PropertyName == "Length" || args.PropertyName == "ConduitLength" || args.PropertyName == "ConnectionType" || args.PropertyName == "ConduitType")
                {
                    if (args.NewValue is TECConnection && args.OldValue is TECConnection)
                    {
                        removeConnection(args.OldValue as TECConnection);
                        addConnection(args.NewValue as TECConnection);
                    }
                }
            }
        }

        private void WireSummaryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LengthSubTotalCost" || e.PropertyName == "AssCostSubTotalCost" || e.PropertyName == "RatedCostSubTotalCost")
            {
                RaisePropertyChanged("TotalWireCost");
                RaisePropertyChanged("TotalCost");
            }
            else if (e.PropertyName == "LengthSubTotalLabor" || e.PropertyName == "AssCostSubTotalLabor" || e.PropertyName == "RatedCostSubTotalLabor")
            {
                RaisePropertyChanged("TotalWireLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }

        private void ConduitSummaryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LengthSubTotalCost" || e.PropertyName == "AssCostSubTotalCost" || e.PropertyName == "RatedCostSubTotalCost")
            {
                RaisePropertyChanged("TotalConduitCost");
                RaisePropertyChanged("TotalCost");
            }
            else if (e.PropertyName == "LengthSubTotalLabor" || e.PropertyName == "AssCostSubTotalLabor" || e.PropertyName == "RatedCostSubTotalLabor")
            {
                RaisePropertyChanged("TotalConduitLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }

        private void MiscCostsSummaryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MiscCostSubTotalCost" || e.PropertyName == "AssCostSubTotalCost")
            {
                RaisePropertyChanged("TotalMiscCost");
                RaisePropertyChanged("TotalCost");
            }
            else if (e.PropertyName == "MiscCostSubTotalLabor" || e.PropertyName == "AssCostSubTotalLabor")
            {
                RaisePropertyChanged("TotalMiscLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Add/Remove
        static public Tuple<double, double> AddCost(TECCost cost, Dictionary<Guid, CostSummaryItem> dictionary, ObservableCollection<CostSummaryItem> collection)
        {
            double costChange = 0;
            double laborChange = 0;
            bool containsCost = dictionary.ContainsKey(cost.Guid);
            if (containsCost)
            {
                costChange -= dictionary[cost.Guid].TotalCost;
                laborChange -= dictionary[cost.Guid].TotalLabor;
                dictionary[cost.Guid].Quantity++;
                costChange += dictionary[cost.Guid].TotalCost;
                laborChange += dictionary[cost.Guid].TotalLabor;
            }
            else
            {
                CostSummaryItem costItem = new CostSummaryItem(cost);
                dictionary.Add(cost.Guid, costItem);
                collection.Add(costItem);
                costChange += dictionary[cost.Guid].TotalCost;
                laborChange += dictionary[cost.Guid].TotalLabor;
            }
            return Tuple.Create(costChange, laborChange);
        }
        static public Tuple<double, double> RemoveCost(TECCost cost, Dictionary<Guid, CostSummaryItem> dictionary, ObservableCollection<CostSummaryItem> collection)
        {
            double costChange = 0;
            double laborChange = 0;
            bool containsCost = dictionary.ContainsKey(cost.Guid);
            if (containsCost)
            {
                costChange -= dictionary[cost.Guid].TotalCost;
                laborChange -= dictionary[cost.Guid].TotalLabor;
                dictionary[cost.Guid].Quantity--;
                costChange += dictionary[cost.Guid].TotalCost;
                laborChange += dictionary[cost.Guid].TotalLabor;

                if (dictionary[cost.Guid].Quantity < 1)
                {
                    collection.Remove(dictionary[cost.Guid]);
                    dictionary.Remove(cost.Guid);
                }

                return Tuple.Create(costChange, laborChange);
            }
            else
            {
                throw new InvalidOperationException("Associated Cost not found in dictionary.");
            }
        }
        
        private void addTypicalSystem(TECSystem system)
        {
            MiscCostsSummaryVM.AddTypicalSystem(system);
        }
        private void removeTypicalSystem(TECSystem system)
        {
            MiscCostsSummaryVM.RemoveTypicalSystem(system);
        }

        private void addInstanceSystem(TECSystem instance, TECSystem typical)
        {
            WireSummaryVM.AddInstanceSystem(instance);
            ConduitSummaryVM.AddInstanceSystem(instance);
            MiscCostsSummaryVM.AddInstanceSystem(instance, typical);
        }
        private void removeInstanceSystem(TECSystem instance, TECSystem typical)
        {
            WireSummaryVM.RemoveInstanceSystem(instance);
            ConduitSummaryVM.RemoveInstanceSystem(instance);
            MiscCostsSummaryVM.RemoveInstanceSystem(instance, typical);
        }

        private void addController(TECController controller)
        {
            WireSummaryVM.AddController(controller);
            ConduitSummaryVM.AddController(controller);
            MiscCostsSummaryVM.AddController(controller);
        }
        private void removeController(TECController controller)
        {
            WireSummaryVM.RemoveController(controller);
            ConduitSummaryVM.RemoveController(controller);
            MiscCostsSummaryVM.RemoveController(controller);
        }

        private void addPanel(TECPanel panel)
        {
            MiscCostsSummaryVM.AddPanel(panel);
        }
        private void removePanel(TECPanel panel)
        {
            MiscCostsSummaryVM.RemovePanel(panel);
        }

        private void addConnection(TECConnection connection)
        {
            WireSummaryVM.AddConnection(connection);
            ConduitSummaryVM.AddConnection(connection);
        }
        private void removeConnection(TECConnection connection)
        {
            WireSummaryVM.RemoveConnection(connection);
            ConduitSummaryVM.RemoveConnection(connection);
        }

        private void addEquipment(TECEquipment equipment)
        {
            MiscCostsSummaryVM.AddEquipment(equipment);
        }
        private void removeEquipment(TECEquipment equipment)
        {
            MiscCostsSummaryVM.RemoveEquipment(equipment);
        }

        private void addSubScope(TECSubScope subscope)
        {
            MiscCostsSummaryVM.AddSubScope(subscope);
        }
        private void removeSubScope(TECSubScope subscope)
        {
            MiscCostsSummaryVM.RemoveSubScope(subscope);
        }

        private void addDevice(TECDevice dev)
        {
            MiscCostsSummaryVM.AddDevice(dev);
        }
        private void removeDevice(TECDevice dev)
        {
            MiscCostsSummaryVM.RemoveDevice(dev);
        }

        private void addPoint(TECPoint point)
        {
            MiscCostsSummaryVM.AddPoint(point);
        }
        private void removePoint(TECPoint point)
        {
            MiscCostsSummaryVM.RemovePoint(point);
        }

        private void addMiscCost(TECMisc misc)
        {
            MiscCostsSummaryVM.AddMiscCost(misc);
        }
        private void removeMiscCost(TECMisc misc)
        {
            MiscCostsSummaryVM.RemoveMiscCost(misc);
        }

        private void addAssCost(TECCost cost)
        {
            MiscCostsSummaryVM.AddAssCost(cost);
        }
        private void removeAssCost(TECCost cost)
        {
            MiscCostsSummaryVM.RemoveAssCost(cost);
        }
        #endregion
    }
}