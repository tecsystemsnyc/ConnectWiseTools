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
                    _changeWatcher.InstanceChanged -= bidChanged;
                }
                _changeWatcher = value;
                _changeWatcher.InstanceChanged += bidChanged;
            }
        }

        #region View Models
        public DeviceSummaryVM DeviceSummaryVM { get; private set; }
        public ControllerSummaryVM ControllerSummaryVM { get; private set; }
        public PanelTypeSummaryVM PanelTypeSummaryVM { get; private set; }
        #endregion

        #region Misc Cost View Properties
        private ObservableCollection<TECMisc> _miscCosts;
        public ObservableCollection<TECMisc> MiscCosts
        {
            get { return _miscCosts; }
            set
            {
                _miscCosts = value;
                RaisePropertyChanged("MiscCosts");
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
                RaisePropertyChanged("TotalMiscCost");
                RaisePropertyChanged("TotalCost");
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
                RaisePropertyChanged("TotalMiscLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
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
            get { return PanelTypeSummaryVM.PanelAssCostSubTotalLabor; }
        }

        public double TotalMiscCost
        {
            get { return MiscCostSubTotalCost; }
        }

        public double TotalMiscLabor
        {
            get { return MiscCostSubTotalLabor; }
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
        }

        private void reinitialize(TECBid bid)
        {
            MiscCosts = new ObservableCollection<TECMisc>();
            
            MiscCostSubTotalCost = 0;
            MiscCostSubTotalLabor = 0;
            
            foreach(TECMisc cost in bid.MiscCosts)
            {
                addMiscCost(cost);
            }

            changeWatcher = new ChangeWatcher(bid);
        }

        private void initializeVMs(TECBid bid)
        {
            DeviceSummaryVM = new DeviceSummaryVM(bid);
            ControllerSummaryVM = new ControllerSummaryVM(bid);
            PanelTypeSummaryVM = new PanelTypeSummaryVM(bid);

            DeviceSummaryVM.PropertyChanged += DeviceSummaryVM_PropertyChanged;
            ControllerSummaryVM.PropertyChanged += ControllerSummaryVM_PropertyChanged;
            PanelTypeSummaryVM.PropertyChanged += PanelTypeSummaryVM_PropertyChanged;
        }

        #region Event Handlers
        private void bidChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                var targetObject = args.NewValue;
                var referenceObject = args.OldValue;

                if (args.PropertyName == "Add" || args.PropertyName == "AddCatalog")
                {
                    if (targetObject is TECSystem && referenceObject is TECSystem)
                    {
                        DeviceSummaryVM.AddSystem(targetObject as TECSystem);
                        foreach(TECController control in (targetObject as TECSystem).Controllers)
                        {
                            ControllerSummaryVM.AddController(control);
                        }
                    }
                    else if (targetObject is TECEquipment && referenceObject is TECSystem)
                    {
                        DeviceSummaryVM.AddEquipment(targetObject as TECEquipment);
                    }
                    else if (targetObject is TECSubScope && referenceObject is TECEquipment)
                    {
                        DeviceSummaryVM.AddSubScope(targetObject as TECSubScope);
                    }
                    else if (targetObject is TECDevice && referenceObject is TECSubScope)
                    {
                        DeviceSummaryVM.AddDevice(targetObject as TECDevice);
                    }
                    else if (targetObject is TECCost && referenceObject is TECDevice)
                    {
                        DeviceSummaryVM.AddCostToDevices(targetObject as TECCost, referenceObject as TECDevice);
                    }
                    else if (targetObject is TECController && (referenceObject is TECBid || referenceObject is TECSystem))
                    {
                        ControllerSummaryVM.AddController(targetObject as TECController);
                    }
                    else if (targetObject is TECPanel && (referenceObject is TECBid || referenceObject is TECSystem))
                    {
                        PanelTypeSummaryVM.AddPanel(targetObject as TECPanel);
                    }
                    else if (targetObject is TECCost && referenceObject is TECController)
                    {
                        ControllerSummaryVM.AddCostToController(targetObject as TECCost);
                    }
                    else if (targetObject is TECCost && referenceObject is TECPanel)
                    {
                        PanelTypeSummaryVM.AddCostToPanel(targetObject as TECCost);
                    }
                    else if (targetObject is TECMisc && referenceObject is TECBid)
                    {
                        addMiscCost(targetObject as TECMisc);
                    }
 
                }
                else if (args.PropertyName == "Remove" || args.PropertyName == "RemoveCatalog")
                {
                    if (targetObject is TECSystem && referenceObject is TECBid)
                    {
                        DeviceSummaryVM.RemoveSystem(targetObject as TECSystem);
                        foreach(TECController control in (targetObject as TECSystem).Controllers)
                        {
                            ControllerSummaryVM.RemoveController(control);
                        }
                    }
                    else if (targetObject is TECEquipment && referenceObject is TECSystem)
                    {
                        DeviceSummaryVM.RemoveEquipment(targetObject as TECEquipment);
                    }
                    else if (targetObject is TECSubScope && referenceObject is TECEquipment)
                    {
                        DeviceSummaryVM.RemoveSubScope(targetObject as TECSubScope);
                    }
                    else if (targetObject is TECDevice && referenceObject is TECSubScope)
                    {
                        DeviceSummaryVM.RemoveDevice(targetObject as TECDevice);
                    }
                    else if (targetObject is TECCost && referenceObject is TECDevice)
                    {
                        DeviceSummaryVM.RemoveCostFromDevices(targetObject as TECCost, referenceObject as TECDevice);
                    }
                    else if (targetObject is TECController && (referenceObject is TECBid || referenceObject is TECSystem))
                    {
                        ControllerSummaryVM.RemoveController(targetObject as TECController);
                    }
                    else if (targetObject is TECPanel && (referenceObject is TECBid || referenceObject is TECSystem))
                    {
                        PanelTypeSummaryVM.RemovePanel(targetObject as TECPanel);
                    }
                    else if (targetObject is TECCost && referenceObject is TECController)
                    {
                        ControllerSummaryVM.RemoveCostFromController(targetObject as TECCost);
                    }
                    else if (targetObject is TECCost && referenceObject is TECPanel)
                    {
                        PanelTypeSummaryVM.RemoveCostFromPanel(targetObject as TECCost);
                    }
                    else if (targetObject is TECMisc && referenceObject is TECBid)
                    {
                        removeMiscCost(targetObject as TECMisc);
                    }
                }
                else if (args.PropertyName == "Cost" || args.PropertyName == "Quantity")
                {
                    if (args.OldValue is TECMisc && args.NewValue is TECMisc)
                    {
                        removeMiscCost(args.OldValue as TECMisc);
                        addMiscCost(args.NewValue as TECMisc);
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
            else if (e.PropertyName == "PanelAssCostSubTotalLabor")
            {
                RaisePropertyChanged("TotalPanelLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Add/Remove
        private void addMiscCost(TECMisc cost)
        {
            MiscCosts.Add(cost);
            MiscCostSubTotalCost += cost.Cost * cost.Quantity;
            MiscCostSubTotalLabor += cost.Labor * cost.Quantity;
        }

        private void removeMiscCost(TECMisc cost)
        {
            MiscCosts.Remove(cost);
            MiscCostSubTotalCost -= cost.Cost * cost.Quantity;
            MiscCostSubTotalLabor -= cost.Labor * cost.Quantity;
        }

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
        #endregion
    }
}