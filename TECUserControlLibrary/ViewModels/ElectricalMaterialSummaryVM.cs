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
                    _changeWatcher.InstanceChanged -= instanceChanged;
                }
                _changeWatcher = value;
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
        private void instanceChanged(PropertyChangedExtendedEventArgs args)
        {
            var parent = args.Sender;
            var child = args.Value;

            if (args.Change == Change.Add)
            {
                if (child is TECSystem)
                {
                    if (parent is TECBid)
                    {
                        addTypicalSystem(child as TECSystem);
                    }
                    else if (parent is TECSystem)
                    {
                        addInstanceSystem(child as TECSystem, parent as TECSystem);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (child is TECController)
                {
                    addController(child as TECController);
                }
                else if (child is TECPanel)
                {
                    addPanel(child as TECPanel);
                }
                else if (child is TECConnection)
                {
                    addConnection(child as TECConnection);
                }
                else if (child is TECEquipment)
                {
                    addEquipment(child as TECEquipment);
                }
                else if (child is TECSubScope)
                {
                    addSubScope(child as TECSubScope);
                }
                else if (child is TECDevice)
                {
                    addDevice(child as TECDevice);
                }
                else if (child is TECMisc)
                {
                    if (parent is TECBid)
                    {
                        addMiscCost(child as TECMisc);
                    }
                    else if (parent is TECSystem)
                    {
                        foreach (TECSystem instance in (parent as TECSystem).SystemInstances)
                        {
                            addMiscCost(child as TECMisc);
                        }
                    }
                }
                else if (child is TECCost)
                {
                    addAssCost(child as TECCost);
                }
            }
            else if (args.Change == Change.Remove)
            {
                if (child is TECSystem)
                {
                    if (parent is TECBid)
                    {
                        removeTypicalSystem(child as TECSystem);
                    }
                    else if (parent is TECSystem)
                    {
                        removeInstanceSystem(child as TECSystem, parent as TECSystem);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (child is TECController)
                {
                    removeController(child as TECController);
                }
                else if (child is TECPanel)
                {
                    removePanel(child as TECPanel);
                }
                else if (child is TECConnection)
                {
                    removeConnection(child as TECConnection);
                }
                else if (child is TECEquipment)
                {
                    removeEquipment(child as TECEquipment);
                }
                else if (child is TECSubScope)
                {
                    removeSubScope(child as TECSubScope);
                }
                else if (child is TECDevice)
                {
                    removeDevice(child as TECDevice);
                }
                else if (child is TECMisc)
                {
                    if (parent is TECBid)
                    {
                        removeMiscCost(child as TECMisc);
                    }
                    else if (parent is TECSystem)
                    {
                        foreach(TECSystem instance in (parent as TECSystem).SystemInstances)
                        {
                            removeMiscCost(child as TECMisc);
                        }
                    }
                }
                else if (child is TECCost)
                {
                    removeAssCost(child as TECCost);
                }
            }
            else if (args.Change == Change.Edit)
            {
                //Do nothing
                //This used to handle connection property changed, but that responsibility has been offloaded to LengthSummaryVM.
            }
            else
            {
                throw new NotImplementedException("Change type not recognized.");
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