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
                    _changeWatcher.Changed -= bidChanged;
                }
                _changeWatcher = value;
                _changeWatcher.InstanceChanged += instanceChanged;
                _changeWatcher.Changed += bidChanged;
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
            get { return MiscCostsSummaryVM.MiscCostSubTotalCost; }
        }

        public double TotalMiscLabor
        {
            get { return MiscCostsSummaryVM.MiscCostSubTotalLabor; }
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

        public void reinitialize(TECBid bid)
        {
            changeWatcher = new ChangeWatcher(bid);
        }

        public void initializeVMs(TECBid bid)
        {
            WireSummaryVM = new LengthSummaryVM(bid, LengthType.Wire);
            ConduitSummaryVM = new LengthSummaryVM(bid, LengthType.Conduit);
            MiscCostsSummaryVM = new MiscCostsSummaryVM(bid, CostType.Electrical);

            WireSummaryVM.PropertyChanged += WireSummaryVM_PropertyChanged;
            ConduitSummaryVM.PropertyChanged += ConduitSummaryVM_PropertyChanged;
            MiscCostsSummaryVM.PropertyChanged += MiscCostsSummaryVM_PropertyChanged;
        }

        public void refreshVMs(TECBid bid)
        {
            WireSummaryVM.Refresh(bid);
            ConduitSummaryVM.Refresh(bid);
            MiscCostsSummaryVM.Refresh(bid);
        }

        #region Event Handlers
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
                        addSystem(targetObject as TECSystem);
                    }
                    else if (targetObject is TECController)
                    {
                        addController(targetObject as TECController);
                    }
                    else if (targetObject is TECConnection)
                    {
                        addConnection(targetObject as TECConnection);
                    }
                }
                else if (args.PropertyName == "Remove" || args.PropertyName == "RemoveCatalog")
                {
                    if (targetObject is TECSystem)
                    {
                        removeSystem(targetObject as TECSystem);
                    }
                    else if (targetObject is TECController)
                    {
                        removeController(targetObject as TECController);
                    }
                    else if (targetObject is TECConnection)
                    {
                        removeConnection(targetObject as TECConnection);
                    }
                }
            }
        }
        private void bidChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                var targetObject = args.NewValue;
                var referenceObject = args.OldValue;

                if (args.PropertyName == "Add" || args.PropertyName == "AddCatalog")
                {
                    if (targetObject is TECMisc)
                    {
                        TECMisc cost = targetObject as TECMisc;
                        if (referenceObject is TECBid)
                        {
                            MiscCostsSummaryVM.AddMiscCost(cost);
                        }
                        else if (referenceObject is TECSystem)
                        {
                            foreach (TECSystem instance in (referenceObject as TECSystem).SystemInstances)
                            {
                                MiscCostsSummaryVM.AddMiscCost(cost);
                            }
                        }
                    }
                }
                else if (args.PropertyName == "Remove" || args.PropertyName == "RemoveCatalog")
                {
                    if (targetObject is TECMisc)
                    {
                        TECMisc cost = targetObject as TECMisc;
                        if (referenceObject is TECBid)
                        {
                            MiscCostsSummaryVM.RemoveMiscCost(cost);
                        }
                        else if (referenceObject is TECSystem)
                        {
                            foreach (TECSystem instance in (referenceObject as TECSystem).SystemInstances)
                            {
                                MiscCostsSummaryVM.RemoveMiscCost(cost);
                            }
                        }
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
                RaisePropertyChanged("TotalCost");
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
                RaisePropertyChanged("TotalCost");
            }
        }

        private void MiscCostsSummaryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MiscCostSubTotalCost")
            {
                RaisePropertyChanged("TotalMiscCost");
                RaisePropertyChanged("TotalCost");
            }
            else if (e.PropertyName == "MiscCostSubTotalLabor")
            {
                RaisePropertyChanged("TotalMiscLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Add/Remove
        static public Tuple<double, double> AddCost(TECCost cost, Dictionary<Guid, AssociatedCostSummaryItem> dictionary, ObservableCollection<AssociatedCostSummaryItem> collection)
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
                AssociatedCostSummaryItem costItem = new AssociatedCostSummaryItem(cost);
                dictionary.Add(cost.Guid, costItem);
                collection.Add(costItem);
                costChange += dictionary[cost.Guid].TotalCost;
                laborChange += dictionary[cost.Guid].TotalLabor;
            }
            return Tuple.Create(costChange, laborChange);
        }

        static public Tuple<double, double> RemoveCost(TECCost cost, Dictionary<Guid, AssociatedCostSummaryItem> dictionary, ObservableCollection<AssociatedCostSummaryItem> collection)
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

        private void addController(TECController controller)
        {
            WireSummaryVM.AddController(controller);
            ConduitSummaryVM.AddController(controller);
        }

        private void removeController(TECController controller)
        {
            WireSummaryVM.RemoveController(controller);
            ConduitSummaryVM.RemoveController(controller);
        }

        private void addSystem(TECSystem system)
        {
            WireSummaryVM.AddSystem(system);
            ConduitSummaryVM.AddSystem(system);
        }

        private void removeSystem(TECSystem system)
        {
            WireSummaryVM.RemoveSystem(system);
            ConduitSummaryVM.RemoveSystem(system);
        }
        #endregion
    }
}