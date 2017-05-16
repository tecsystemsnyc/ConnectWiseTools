using EstimateBuilder.Model;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EstimateBuilder.ViewModel
{
    public class TECMaterialViewModel : ViewModelBase
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
                }
                _changeWatcher = value;
                _changeWatcher.Changed += bidChanged;
            }
        }

        #region Device View Properties
        private Dictionary<Guid, DeviceSummaryItem> deviceDictionary;

        private ObservableCollection<DeviceSummaryItem> _deviceSummaryItems;
        public ObservableCollection<DeviceSummaryItem> DeviceSummaryItems
        {
            get { return _deviceSummaryItems; }
            set
            {
                _deviceSummaryItems = value;
                RaisePropertyChanged("DeviceSummaryItems");
            }
        }

        private Dictionary<Guid, AssociatedCostSummaryItem> deviceAssCostDictionary;

        private ObservableCollection<AssociatedCostSummaryItem> _deviceAssCostSummaryItems;
        private ObservableCollection<AssociatedCostSummaryItem> DeviceAssCostSummaryItems
        {
            get { return _deviceAssCostSummaryItems; }
            set
            {
                _deviceAssCostSummaryItems = value;
                RaisePropertyChanged("DeviceAssCostSummaryItems");
            }
        }

        private double _deviceSubTotal;
        public double DeviceSubTotal
        {
            get { return _deviceSubTotal; }
            set
            {
                _deviceSubTotal = value;
                RaisePropertyChanged("DeviceSubToal");
                RaisePropertyChanged("TotalDeviceCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _deviceAssCostSubTotalCost;
        public double DeviceAssCostSubTotalCost
        {
            get { return _deviceAssCostSubTotalCost; }
            set
            {
                _deviceAssCostSubTotalCost = value;
                RaisePropertyChanged("DeviceAssCostSubTotalCost");
                RaisePropertyChanged("TotalDeviceCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _deviceAssCostSubTotalLabor;
        public double DeviceAssCostSubTotalLabor
        {
            get { return _deviceAssCostSubTotalLabor; }
            set
            {
                _deviceAssCostSubTotalLabor = value;
                RaisePropertyChanged("DeviceAssCostSubTotalLabor");
                RaisePropertyChanged("TotalDeviceLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Controller View Properties
        private Dictionary<Guid, AssociatedCostSummaryItem> controllerAssCostDictionary;

        private ObservableCollection<AssociatedCostSummaryItem> _controllerAssCostSummaryItems;
        public ObservableCollection<AssociatedCostSummaryItem> ControllerAssCostSummaryItems
        {
            get
            {
                return _controllerAssCostSummaryItems;
            }
            set
            {
                _controllerAssCostSummaryItems = value;
                RaisePropertyChanged("ControllerAssCostSummaryItems");
            }
        }

        private double _controllerSubTotal;
        public double ControllerSubTotal
        {
            get { return _controllerSubTotal; }
            set
            {
                _controllerSubTotal = value;
                RaisePropertyChanged("ControllerSubTotal");
                RaisePropertyChanged("TotalControllerCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _controllerAssCostSubTotalCost;
        public double ControllerAssCostSubTotalCost
        {
            get { return _controllerAssCostSubTotalCost; }
            set
            {
                _controllerAssCostSubTotalCost = value;
                RaisePropertyChanged("ControllerAssCostSubTotalCost");
                RaisePropertyChanged("TotalControllerCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _controllerAssCostSubTotalLabor;
        public double ControllerAssCostSubTotalLabor
        {
            get { return _controllerAssCostSubTotalLabor; }
            set
            {
                _controllerAssCostSubTotalLabor = value;
                RaisePropertyChanged("ControllerAssCostSubTotalLabor");
                RaisePropertyChanged("TotalControllerLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Panel View Properties
        private Dictionary<Guid, PanelTypeSummaryItem> panelTypeDictionary;

        private ObservableCollection<PanelTypeSummaryItem> _panelTypeSummaryItems;
        public ObservableCollection<PanelTypeSummaryItem> PanelTypeSummaryItems
        {
            get
            {
                return _panelTypeSummaryItems;
            }
            set
            {
                _panelTypeSummaryItems = value;
                RaisePropertyChanged("PanelTypeSummaryItems");
            }
        }

        private Dictionary<Guid, AssociatedCostSummaryItem> panelAssCostDictionary;

        private ObservableCollection<AssociatedCostSummaryItem> _panelAssCostSummaryItems;
        public ObservableCollection<AssociatedCostSummaryItem> PanelAssCostSummaryItems
        {
            get { return _panelAssCostSummaryItems; }
            set
            {
                _panelAssCostSummaryItems = value;
                RaisePropertyChanged("PanelAssCostSummaryItems");
            }
        }

        private double _panelTypeSubTotal;
        public double PanelTypeSubTotal
        {
            get { return _panelTypeSubTotal; }
            set
            {
                _panelTypeSubTotal = value;
                RaisePropertyChanged("PanelTypeSubTotal");
                RaisePropertyChanged("TotalPanelCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _panelAssCostSubTotalCost;
        public double PanelAssCostSubTotalCost
        {
            get { return _panelAssCostSubTotalCost; }
            set
            {
                _panelAssCostSubTotalCost = value;
                RaisePropertyChanged("PanelAssCostSubTotalCost");
                RaisePropertyChanged("TotalPanelCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _panelAssCostSubTotalLabor;
        public double PanelAssCostSubTotalLabor
        {
            get { return _panelAssCostSubTotalLabor; }
            set
            {
                _panelAssCostSubTotalLabor = value;
                RaisePropertyChanged("PanelAssCostSubTotalLabor");
                RaisePropertyChanged("TotalPanelLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Misc Cost View Properties
        private ObservableCollection<TECMiscCost> _miscCosts;
        public ObservableCollection<TECMiscCost> MiscCosts
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
                return (DeviceSubTotal + DeviceAssCostSubTotalCost);
            }
        }

        public double TotalDeviceLabor
        {
            get
            {
                return (DeviceAssCostSubTotalLabor);
            }
        }

        public double TotalControllerCost
        {
            get
            {
                return (ControllerSubTotal + ControllerAssCostSubTotalCost);
            }
        }

        public double TotalControllerLabor
        {
            get { return ControllerAssCostSubTotalLabor; }
        }

        public double TotalPanelCost
        {
            get { return (PanelTypeSubTotal + PanelAssCostSubTotalCost); }
        }

        public double TotalPanelLabor
        {
            get { return PanelAssCostSubTotalLabor; }
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

        public TECMaterialViewModel(TECBid bid)
        {
            reinitialize(bid);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            DeviceSummaryItems = new ObservableCollection<DeviceSummaryItem>();
            deviceDictionary = new Dictionary<Guid, DeviceSummaryItem>();

            _deviceSubTotal = 0;
            _deviceAssCostSubTotalCost = 0;
            _deviceAssCostSubTotalLabor = 0;
            _controllerAssCostSubTotalCost = 0;
            _controllerAssCostSubTotalLabor = 0;

            foreach (TECSystem sys in bid.Systems)
            {
                addSystem(sys);
            }
            foreach (TECController controller in bid.Controllers)
            {
                addController(controller);
            }
            foreach(TECPanel panel in bid.Panels)
            {
                addPanel(panel);
            }
            foreach(TECMiscCost cost in bid.MiscCosts)
            {
                addMiscCost(cost);
            }

            changeWatcher = new ChangeWatcher(bid);
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
                    if (targetObject is TECSystem && referenceObject is TECBid)
                    {
                        addSystem(targetObject as TECSystem);
                    }
                    else if (targetObject is TECEquipment && referenceObject is TECSystem)
                    {
                        addEquipment(targetObject as TECEquipment);
                    }
                    else if (targetObject is TECSubScope && referenceObject is TECEquipment)
                    {
                        addSubScope(targetObject as TECSubScope);
                    }
                    else if (targetObject is TECDevice && referenceObject is TECSubScope)
                    {
                        addDevice(targetObject as TECDevice);
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECDevice)
                    {
                        addCostToDevices(targetObject as TECAssociatedCost, referenceObject as TECDevice);
                    }
                    else if (targetObject is TECController && referenceObject is TECBid)
                    {
                        addController(targetObject as TECController);
                    }
                    else if (targetObject is TECPanel && referenceObject is TECBid)
                    {
                        addPanel(targetObject as TECPanel);
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECController)
                    {
                        addCostToController(targetObject as TECAssociatedCost);
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECPanel)
                    {
                        addCostToPanel(targetObject as TECAssociatedCost);
                    }
                    else if (targetObject is TECMiscCost && referenceObject is TECBid)
                    {
                        addMiscCost(targetObject as TECMiscCost);
                    }
 
                }
                else if (args.PropertyName == "Remove" || args.PropertyName == "RemoveCatalog")
                {
                    if (targetObject is TECSystem && referenceObject is TECBid)
                    {
                        removeSystem(targetObject as TECSystem);
                    }
                    else if (targetObject is TECEquipment && referenceObject is TECSystem)
                    {
                        removeEquipment(targetObject as TECEquipment);
                    }
                    else if (targetObject is TECSubScope && referenceObject is TECEquipment)
                    {
                        removeSubScope(targetObject as TECSubScope);
                    }
                    else if (targetObject is TECDevice && referenceObject is TECSubScope)
                    {
                        removeDevice(targetObject as TECDevice);
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECDevice)
                    {
                        removeCostFromDevices(targetObject as TECAssociatedCost, referenceObject as TECDevice);
                    }
                    else if (targetObject is TECController && referenceObject is TECBid)
                    {
                        removeController(targetObject as TECController);
                    }
                    else if (targetObject is TECPanel && referenceObject is TECBid)
                    {
                        removePanel(targetObject as TECPanel);
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECController)
                    {
                        removeCostFromController(targetObject as TECAssociatedCost);
                    }
                    else if (targetObject is TECAssociatedCost && referenceObject is TECPanel)
                    {
                        removeCostFromPanel(targetObject as TECAssociatedCost);
                    }
                    else if (targetObject is TECMiscCost && referenceObject is TECBid)
                    {
                        removeMiscCost(targetObject as TECMiscCost);
                    }
                }
                else if (args.PropertyName == "Cost" || args.PropertyName == "Quantity")
                {
                    if (args.OldValue is TECMiscCost && args.NewValue is TECMiscCost)
                    {
                        removeMiscCost(args.OldValue as TECMiscCost);
                        addMiscCost(args.NewValue as TECMiscCost);
                    }
                }
            }
        }

        private void DeviceItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<Object>)
            {
                PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;

                if (args.PropertyName == "Total")
                {
                    DeviceSubTotal -= (double)args.OldValue;
                    DeviceSubTotal += (double)args.NewValue;
                }
            }
        }

        private void raiseTotals()
        {
            RaisePropertyChanged("TotalCost");
            RaisePropertyChanged("TotalLabor");
        }
        #endregion

        #region Add/Remove
        private void addDevice(TECDevice device)
        {
            bool containsDevice = deviceDictionary.ContainsKey(device.Guid);
            if (containsDevice)
            {
                DeviceSubTotal -= deviceDictionary[device.Guid].Total;
                deviceDictionary[device.Guid].Quantity++;
                DeviceSubTotal += deviceDictionary[device.Guid].Total;
            }
            else
            {
                DeviceSummaryItem deviceItem = new DeviceSummaryItem(device);
                deviceItem.PropertyChanged += DeviceItem_PropertyChanged;
                deviceDictionary.Add(device.Guid, deviceItem);
                DeviceSummaryItems.Add(deviceItem);
                DeviceSubTotal += deviceItem.Total;
            }
            foreach (TECAssociatedCost cost in device.AssociatedCosts)
            {
                DeviceAssCostSubTotalCost += cost.Cost;
                DeviceAssCostSubTotalLabor += cost.Labor;
            }
        }

        private void removeDevice(TECDevice device)
        {
            if (deviceDictionary.ContainsKey(device.Guid))
            {
                DeviceSubTotal -= deviceDictionary[device.Guid].Total;
                deviceDictionary[device.Guid].Quantity--;
                DeviceSubTotal += deviceDictionary[device.Guid].Total;
                
                if (deviceDictionary[device.Guid].Quantity < 1)
                {
                    deviceDictionary[device.Guid].PropertyChanged -= DeviceItem_PropertyChanged;
                    DeviceSummaryItems.Remove(deviceDictionary[device.Guid]);
                    deviceDictionary.Remove(device.Guid);
                }

                foreach (TECAssociatedCost cost in device.AssociatedCosts)
                {
                    DeviceAssCostSubTotalCost -= cost.Cost;
                    DeviceAssCostSubTotalLabor -= cost.Labor;
                }
            }
            else
            {
                throw new InvalidOperationException("Device not found in device dictionary.");
            }
        }

        private void addCostToDevices(TECAssociatedCost cost, TECDevice device)
        {
            if (deviceDictionary.ContainsKey(device.Guid))
            {
                for (int i = 0; i < deviceDictionary[device.Guid].Quantity; i++)
                {
                    DeviceAssCostSubTotalCost += cost.Cost;
                    DeviceAssCostSubTotalLabor += cost.Labor;
                }
            }
        }

        private void removeCostFromDevices(TECAssociatedCost cost, TECDevice device)
        {
            if (deviceDictionary.ContainsKey(device.Guid))
            {
                for (int i = 0; i < deviceDictionary[device.Guid].Quantity; i++)
                {
                    DeviceAssCostSubTotalCost -= cost.Cost;
                    DeviceAssCostSubTotalLabor -= cost.Labor;
                }
            }
        }

        private void addController(TECController controller)
        {
            ControllerSubTotal += controller.Cost * controller.Manufacturer.Multiplier;
            foreach(TECAssociatedCost cost in controller.AssociatedCosts)
            {
                addCostToController(cost);
            }
        }

        private void removeController(TECController controller)
        {
            ControllerSubTotal -= controller.Cost * controller.Manufacturer.Multiplier;
            foreach(TECAssociatedCost cost in controller.AssociatedCosts)
            {
                removeCostFromController(cost);
            }
        }

        private void addCostToController(TECAssociatedCost cost)
        {
            ControllerAssCostSubTotalCost += cost.Cost;
            ControllerAssCostSubTotalLabor += cost.Labor;
        }

        private void removeCostFromController(TECAssociatedCost cost)
        {
            ControllerAssCostSubTotalCost -= cost.Cost;
            ControllerAssCostSubTotalLabor -= cost.Labor;
        }

        private void addPanel(TECPanel panel)
        {
            PanelTypeSubTotal += panel.Type.Cost;
            foreach(TECAssociatedCost cost in panel.AssociatedCosts)
            {
                addCostToPanel(cost);
            }
        }

        private void removePanel(TECPanel panel)
        {
            PanelTypeSubTotal -= panel.Type.Cost;
            foreach (TECAssociatedCost cost in panel.AssociatedCosts)
            {
                removeCostFromPanel(cost);
            }
        }

        private void addCostToPanel(TECAssociatedCost cost)
        {
            PanelAssCostSubTotalCost += cost.Cost;
            PanelAssCostSubTotalLabor += cost.Labor;
        }

        private void removeCostFromPanel(TECAssociatedCost cost)
        {
            PanelAssCostSubTotalCost -= cost.Cost;
            PanelAssCostSubTotalLabor -= cost.Labor;
        }

        private void addMiscCost(TECMiscCost cost)
        {
            MiscCostSubTotalCost += cost.Cost * cost.Quantity;
            MiscCostSubTotalLabor += cost.Labor * cost.Quantity;
        }

        private void removeMiscCost(TECMiscCost cost)
        {
            MiscCostSubTotalCost -= cost.Cost * cost.Quantity;
            MiscCostSubTotalLabor -= cost.Labor * cost.Quantity;
        }

        private void addSubScope(TECSubScope ss)
        {
            foreach(TECDevice dev in ss.Devices)
            {
                addDevice(dev);
            }
        }

        private void removeSubScope(TECSubScope ss)
        {
            foreach(TECDevice dev in ss.Devices)
            {
                removeDevice(dev);
            }
        }

        private void addEquipment(TECEquipment equip)
        {
            foreach(TECSubScope ss in equip.SubScope)
            {
                addSubScope(ss);
            }
        }

        private void removeEquipment(TECEquipment equip)
        {
            foreach (TECSubScope ss in equip.SubScope)
            {
                removeSubScope(ss);
            }
        }

        private void addSystem(TECSystem sys)
        {
            foreach(TECEquipment equip in sys.Equipment)
            {
                addEquipment(equip);
            }
        }

        private void removeSystem(TECSystem sys)
        {
            foreach (TECEquipment equip in sys.Equipment)
            {
                removeEquipment(equip);
            }
        }
        #endregion
    }
}