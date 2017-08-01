using EstimateBuilder.Model;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class TECMaterialSummaryVM : ViewModelBase
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
        public DeviceSummaryVM DeviceSummaryVM { get; private set; }
        public ControllerSummaryVM ControllerSummaryVM { get; private set; }
        public PanelTypeSummaryVM PanelTypeSummaryVM { get; private set; }
        public MiscCostsSummaryVM MiscCostsSummaryVM { get; private set; }
        #endregion

        #region Summary Totals
        public double TotalDeviceCost
        {
            get
            {
                return (DeviceSummaryVM.DeviceSubTotal + DeviceSummaryVM.DeviceAssCostSubTotalCost);
            }
        }

        public double TotalDeviceLabor
        {
            get
            {
                return (DeviceSummaryVM.DeviceAssCostSubTotalLabor);
            }
        }

        public double TotalControllerCost
        {
            get
            {
                return (ControllerSummaryVM.ControllerSubTotal + ControllerSummaryVM.ControllerAssCostSubTotalCost);
            }
        }

        public double TotalControllerLabor
        {
            get { return ControllerSummaryVM.ControllerAssCostSubTotalLabor; }
        }

        public double TotalPanelCost
        {
            get { return (PanelTypeSummaryVM.PanelTypeSubTotal + PanelTypeSummaryVM.PanelAssCostSubTotalCost); }
        }

        public double TotalPanelLabor
        {
            get { return (PanelTypeSummaryVM.PanelTypeLaborSubTotal + PanelTypeSummaryVM.PanelAssCostSubTotalLabor); }
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
            get { return (TotalDeviceCost + TotalControllerCost + TotalPanelCost + TotalMiscCost); }
        }

        public double TotalLabor
        {
            get { return (TotalDeviceLabor + TotalControllerLabor + TotalPanelLabor + TotalMiscLabor); }
        }
        #endregion

        #endregion

        public TECMaterialSummaryVM(TECBid bid)
        {
            initializeVMs(bid);
            reinitialize(bid);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
            DeviceSummaryVM.Refresh(bid);
            ControllerSummaryVM.Refresh(bid);
            PanelTypeSummaryVM.Refresh(bid);
            MiscCostsSummaryVM.Refresh(bid);
        }

        private void reinitialize(TECBid bid)
        {
            changeWatcher = new ChangeWatcher(bid);
        }

        private void initializeVMs(TECBid bid)
        {
            DeviceSummaryVM = new DeviceSummaryVM(bid);
            ControllerSummaryVM = new ControllerSummaryVM(bid);
            PanelTypeSummaryVM = new PanelTypeSummaryVM(bid);
            MiscCostsSummaryVM = new MiscCostsSummaryVM(bid, CostType.TEC);

            DeviceSummaryVM.PropertyChanged += DeviceSummaryVM_PropertyChanged;
            ControllerSummaryVM.PropertyChanged += ControllerSummaryVM_PropertyChanged;
            PanelTypeSummaryVM.PropertyChanged += PanelTypeSummaryVM_PropertyChanged;
            MiscCostsSummaryVM.PropertyChanged += MiscCostsSummaryVM_PropertyChanged;
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
                else if (child is TECMisc)
                {
                    if (parent is TECBid)
                    {
                        addMiscCost(child as TECMisc);
                    }
                    else if (parent is TECSystem)
                    {
                        foreach(TECSystem instance in (parent as TECSystem).SystemInstances)
                        {
                            addMiscCost(child as TECMisc);
                        }
                    }
                }
                else if (child is TECCost)
                {
                    if (parent is TECController)
                    {
                        ControllerSummaryVM.AddCostToController(child as TECCost);
                    }
                    else if (parent is TECPanel)
                    {
                        PanelTypeSummaryVM.AddCostToPanel(child as TECCost);
                    }
                    else if (parent is TECDevice)
                    {
                        DeviceSummaryVM.AddCostToDevices(child as TECCost, parent as TECDevice);
                    }
                    else
                    {
                        addAssCost(child as TECCost);
                    }
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
                else if (child is TECMisc)
                {
                    if (parent is TECBid)
                    {
                        removeMiscCost(child as TECMisc);
                    }
                    else if (parent is TECSystem)
                    {
                        foreach (TECSystem instance in (parent as TECSystem).SystemInstances)
                        {
                            removeMiscCost(child as TECMisc);
                        }
                    }
                }
                else if (child is TECCost)
                {
                    if (parent is TECController)
                    {
                        ControllerSummaryVM.RemoveCostFromController(child as TECCost);
                    }
                    else if (parent is TECPanel)
                    {
                        PanelTypeSummaryVM.RemoveCostFromPanel(child as TECCost);
                    }
                    else if (parent is TECDevice)
                    {
                        DeviceSummaryVM.RemoveCostFromDevices(child as TECCost, parent as TECDevice);
                    }
                    else
                    {
                        removeAssCost(child as TECCost);
                    }
                }
            }
        }

        private void DeviceSummaryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DeviceSubTotal" || e.PropertyName == "DeviceAssCostSubTotalCost")
            {
                RaisePropertyChanged("TotalDeviceCost");
                RaisePropertyChanged("TotalCost");
            }
            else if (e.PropertyName == "DeviceAssCostSubTotalLabor")
            {
                RaisePropertyChanged("TotalDeviceLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }

        private void ControllerSummaryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ControllerSubTotal" || e.PropertyName == "ControllerAssCostSubTotalCost")
            {
                RaisePropertyChanged("TotalControllerCost");
                RaisePropertyChanged("TotalCost");
            }
            else if (e.PropertyName == "ControllerAssCostSubTotalLabor")
            {
                RaisePropertyChanged("TotalControllerLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }

        private void PanelTypeSummaryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PanelTypeSubTotal" || e.PropertyName == "PanelAssCostSubTotalCost")
            {
                RaisePropertyChanged("TotalPanelCost");
                RaisePropertyChanged("TotalCost");
            }
            else if (e.PropertyName == "PanelTypeLaborSubTotal" || e.PropertyName == "PanelAssCostSubTotalLabor")
            {
                RaisePropertyChanged("TotalPanelLabor");
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
            DeviceSummaryVM.AddInstanceSystem(instance);
            ControllerSummaryVM.AddInstanceSystem(instance);
            PanelTypeSummaryVM.AddInstanceSystem(instance);
            MiscCostsSummaryVM.AddInstanceSystem(instance, typical);
        }
        private void removeInstanceSystem(TECSystem instance, TECSystem typical)
        {
            DeviceSummaryVM.RemoveInstanceSystem(instance);
            ControllerSummaryVM.RemoveInstanceSystem(instance);
            PanelTypeSummaryVM.RemoveInstanceSystem(instance);
            MiscCostsSummaryVM.RemoveInstanceSystem(instance, typical);
        }

        private void addEquipment(TECEquipment equipment)
        {
            DeviceSummaryVM.AddEquipment(equipment);
            MiscCostsSummaryVM.AddEquipment(equipment);
        }
        private void removeEquipment(TECEquipment equipment)
        {
            DeviceSummaryVM.RemoveEquipment(equipment);
            MiscCostsSummaryVM.RemoveEquipment(equipment);
        }

        private void addSubScope(TECSubScope subscope)
        {
            DeviceSummaryVM.AddSubScope(subscope);
            MiscCostsSummaryVM.AddSubScope(subscope);
        }
        private void removeSubScope(TECSubScope subscope)
        {
            DeviceSummaryVM.RemoveSubScope(subscope);
            MiscCostsSummaryVM.RemoveSubScope(subscope);
        }

        private void addDevice(TECDevice device)
        {
            DeviceSummaryVM.AddDevice(device);
        }
        private void removeDevice(TECDevice device)
        {
            DeviceSummaryVM.RemoveDevice(device);
        }
        
        private void addController(TECController controller)
        {
            ControllerSummaryVM.AddController(controller);
        }
        private void removeController(TECController controller)
        {
            ControllerSummaryVM.RemoveController(controller);
        }

        private void addConnection(TECConnection connection)
        {
            MiscCostsSummaryVM.AddConnection(connection);
        }
        private void removeConnection(TECConnection connection)
        {
            MiscCostsSummaryVM.RemoveConnection(connection);
        }

        private void addPanel(TECPanel panel)
        {
            PanelTypeSummaryVM.AddPanel(panel);
        }
        private void removePanel(TECPanel panel)
        {
            PanelTypeSummaryVM.RemovePanel(panel);
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