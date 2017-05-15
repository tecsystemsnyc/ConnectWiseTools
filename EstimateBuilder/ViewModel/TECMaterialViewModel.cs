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
    public enum TECMaterialIndex
    {
        Devices, Controllers, Panels, MiscCosts
    }

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

        private Dictionary<Guid, DeviceSummaryItem> _deviceDictionary;

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

        private double _totalDevicePrice;
        public double TotalDevicePrice
        {
            get
            {
                return _totalDevicePrice;
            }
            set
            {
                _totalDevicePrice = value;
                RaisePropertyChanged("TotalDevicePrice");
                raiseTotals();
            }
        }

        private double _deviceAssociatedCostsCost;
        public double DeviceAssociatedCostsCost
        {
            get { return _deviceAssociatedCostsCost; }
            set
            {
                _deviceAssociatedCostsCost = value;
                RaisePropertyChanged("DevicesAssociatedCostsCost");
                raiseTotals();
            }
        }

        private double _deviceAssociatedCostsLabor;
        public double DeviceAssociatedCostsLabor
        {
            get { return _deviceAssociatedCostsLabor; }
            set
            {
                _deviceAssociatedCostsLabor = value;
                RaisePropertyChanged("DeviceAssociatedCostsLabor");
                raiseTotals();
            }
        }

        private double _controllerCosts;
        public double ControllerCosts
        {
            get { return _controllerCosts; }
            set
            {
                _controllerCosts = value;
                RaisePropertyChanged("ControllerCosts");
                raiseTotals();
            }
        }

        private double _controllerLabor;
        public double ControllerLabor
        {
            get { return _controllerLabor; }
            set
            {
                _controllerLabor = value;
                RaisePropertyChanged("ControllerLabor");
                raiseTotals();
            }
        }

        private double _panelCosts;
        public double PanelCosts
        {
            get { return _panelCosts; }
            set
            {
                _panelCosts = value;
                RaisePropertyChanged("PanelCosts");
                raiseTotals();
            }
        }

        private double _panelLabor;
        public double PanelLabor
        {
            get { return _panelLabor; }
            set
            {
                _panelLabor = value;
                RaisePropertyChanged("PanelLabor");
                raiseTotals();
            }
        }

        private double _miscCostsCost;
        public double MiscCostsCost
        {
            get { return _miscCostsCost; }
            set
            {
                _miscCostsCost = value;
                RaisePropertyChanged("MiscCostsCost");
                raiseTotals();
            }
        }

        public double TotalCost
        {
            get
            {
                return (TotalDevicePrice + DeviceAssociatedCostsCost + ControllerCosts + PanelCosts + MiscCostsCost);
            }
        }

        public double TotalLabor
        {
            get
            {
                return (DeviceAssociatedCostsLabor + ControllerLabor + PanelLabor);
            }
        }
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
            _deviceDictionary = new Dictionary<Guid, DeviceSummaryItem>();

            TotalDevicePrice = 0;
            DeviceAssociatedCostsCost = 0;
            DeviceAssociatedCostsLabor = 0;
            ControllerCosts = 0;
            ControllerLabor = 0;
            PanelCosts = 0;
            PanelLabor = 0;
            MiscCostsCost = 0;

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
                    TotalDevicePrice -= (double)args.OldValue;
                    TotalDevicePrice += (double)args.NewValue;
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
            bool containsDevice = _deviceDictionary.ContainsKey(device.Guid);
            if (containsDevice)
            {
                TotalDevicePrice -= _deviceDictionary[device.Guid].Total;
                _deviceDictionary[device.Guid].Quantity++;
                TotalDevicePrice += _deviceDictionary[device.Guid].Total;
            }
            else
            {
                DeviceSummaryItem deviceItem = new DeviceSummaryItem(device);
                deviceItem.PropertyChanged += DeviceItem_PropertyChanged;
                _deviceDictionary.Add(device.Guid, deviceItem);
                DeviceSummaryItems.Add(deviceItem);
                TotalDevicePrice += deviceItem.Total;
            }
            foreach (TECAssociatedCost cost in device.AssociatedCosts)
            {
                DeviceAssociatedCostsCost += cost.Cost;
                DeviceAssociatedCostsLabor += cost.Labor;
            }
        }

        private void removeDevice(TECDevice device)
        {
            if (_deviceDictionary.ContainsKey(device.Guid))
            {
                TotalDevicePrice -= _deviceDictionary[device.Guid].Total;
                _deviceDictionary[device.Guid].Quantity--;
                TotalDevicePrice += _deviceDictionary[device.Guid].Total;
                
                if (_deviceDictionary[device.Guid].Quantity < 1)
                {
                    _deviceDictionary[device.Guid].PropertyChanged -= DeviceItem_PropertyChanged;
                    DeviceSummaryItems.Remove(_deviceDictionary[device.Guid]);
                    _deviceDictionary.Remove(device.Guid);
                }

                foreach (TECAssociatedCost cost in device.AssociatedCosts)
                {
                    DeviceAssociatedCostsCost -= cost.Cost;
                    DeviceAssociatedCostsLabor -= cost.Labor;
                }
            }
            else
            {
                throw new InvalidOperationException("Device not found in device dictionary.");
            }
        }

        private void addCostToDevices(TECAssociatedCost cost, TECDevice device)
        {
            if (_deviceDictionary.ContainsKey(device.Guid))
            {
                for (int i = 0; i < _deviceDictionary[device.Guid].Quantity; i++)
                {
                    DeviceAssociatedCostsCost += cost.Cost;
                    DeviceAssociatedCostsLabor += cost.Labor;
                }
            }
        }

        private void removeCostFromDevices(TECAssociatedCost cost, TECDevice device)
        {
            if (_deviceDictionary.ContainsKey(device.Guid))
            {
                for (int i = 0; i < _deviceDictionary[device.Guid].Quantity; i++)
                {
                    DeviceAssociatedCostsCost -= cost.Cost;
                    DeviceAssociatedCostsLabor -= cost.Labor;
                }
            }
        }

        private void addController(TECController controller)
        {
            ControllerCosts += controller.Cost * controller.Manufacturer.Multiplier;
            foreach(TECAssociatedCost cost in controller.AssociatedCosts)
            {
                addCostToController(cost);
            }
        }

        private void removeController(TECController controller)
        {
            ControllerCosts -= controller.Cost * controller.Manufacturer.Multiplier;
            foreach(TECAssociatedCost cost in controller.AssociatedCosts)
            {
                removeCostFromController(cost);
            }
        }

        private void addCostToController(TECAssociatedCost cost)
        {
            ControllerCosts += cost.Cost;
            ControllerLabor += cost.Labor;
        }

        private void removeCostFromController(TECAssociatedCost cost)
        {
            ControllerCosts -= cost.Cost;
            ControllerLabor -= cost.Labor;
        }

        private void addPanel(TECPanel panel)
        {
            PanelCosts += panel.Type.Cost;
            foreach(TECAssociatedCost cost in panel.AssociatedCosts)
            {
                addCostToPanel(cost);
            }
        }

        private void removePanel(TECPanel panel)
        {
            PanelCosts -= panel.Type.Cost;
            foreach (TECAssociatedCost cost in panel.AssociatedCosts)
            {
                removeCostFromPanel(cost);
            }
        }

        private void addCostToPanel(TECAssociatedCost cost)
        {
            PanelCosts += cost.Cost;
            PanelLabor += cost.Labor;
        }

        private void removeCostFromPanel(TECAssociatedCost cost)
        {
            PanelCosts -= cost.Cost;
            PanelLabor -= cost.Labor;
        }

        private void addMiscCost(TECMiscCost cost)
        {
            MiscCostsCost += cost.Cost * cost.Quantity;
        }

        private void removeMiscCost(TECMiscCost cost)
        {
            MiscCostsCost -= cost.Cost * cost.Quantity;
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