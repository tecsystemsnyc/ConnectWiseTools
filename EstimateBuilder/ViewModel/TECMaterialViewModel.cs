using EstimateBuilder.Model;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TECUserControlLibrary.Models;

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
        public ObservableCollection<AssociatedCostSummaryItem> DeviceAssCostSummaryItems
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
                RaisePropertyChanged("DeviceSubTotal");
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
            deviceDictionary = new Dictionary<Guid, DeviceSummaryItem>();
            DeviceSummaryItems = new ObservableCollection<DeviceSummaryItem>();
            deviceAssCostDictionary = new Dictionary<Guid, AssociatedCostSummaryItem>();
            DeviceAssCostSummaryItems = new ObservableCollection<AssociatedCostSummaryItem>();

            controllerAssCostDictionary = new Dictionary<Guid, AssociatedCostSummaryItem>();
            ControllerAssCostSummaryItems = new ObservableCollection<AssociatedCostSummaryItem>();

            panelTypeDictionary = new Dictionary<Guid, PanelTypeSummaryItem>();
            PanelTypeSummaryItems = new ObservableCollection<PanelTypeSummaryItem>();
            panelAssCostDictionary = new Dictionary<Guid, AssociatedCostSummaryItem>();
            PanelAssCostSummaryItems = new ObservableCollection<AssociatedCostSummaryItem>();

            MiscCosts = new ObservableCollection<TECMiscCost>();

            _deviceSubTotal = 0;
            _deviceAssCostSubTotalCost = 0;
            _deviceAssCostSubTotalLabor = 0;
            _controllerAssCostSubTotalCost = 0;
            _controllerAssCostSubTotalLabor = 0;
            _panelTypeSubTotal = 0;
            _panelAssCostSubTotalCost = 0;
            _panelAssCostSubTotalLabor = 0;
            _miscCostSubTotalCost = 0;
            _miscCostSubTotalLabor = 0;

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
                Tuple<double, double> delta = addCost(cost, deviceAssCostDictionary, DeviceAssCostSummaryItems);
                DeviceAssCostSubTotalCost += delta.Item1;
                DeviceAssCostSubTotalLabor += delta.Item2;
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
                    Tuple<double, double> delta = removeCost(cost, deviceAssCostDictionary, DeviceAssCostSummaryItems);
                    DeviceAssCostSubTotalCost += delta.Item1;
                    DeviceAssCostSubTotalLabor += delta.Item2;
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
                    Tuple<double, double> delta = addCost(cost, deviceAssCostDictionary, DeviceAssCostSummaryItems);
                    DeviceAssCostSubTotalCost += delta.Item1;
                    DeviceAssCostSubTotalLabor += delta.Item2;
                }
            }
        }

        private void removeCostFromDevices(TECAssociatedCost cost, TECDevice device)
        {
            if (deviceDictionary.ContainsKey(device.Guid))
            {
                for (int i = 0; i < deviceDictionary[device.Guid].Quantity; i++)
                {
                    Tuple<double, double> delta = removeCost(cost, deviceAssCostDictionary, DeviceAssCostSummaryItems);
                    DeviceAssCostSubTotalCost += delta.Item1;
                    DeviceAssCostSubTotalLabor += delta.Item2;
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
            Tuple<double, double> delta = addCost(cost, controllerAssCostDictionary, ControllerAssCostSummaryItems);
            ControllerAssCostSubTotalCost += delta.Item1;
            ControllerAssCostSubTotalLabor += delta.Item2;
        }

        private void removeCostFromController(TECAssociatedCost cost)
        {
            Tuple<double, double> delta = removeCost(cost, controllerAssCostDictionary, ControllerAssCostSummaryItems);
            ControllerAssCostSubTotalCost += delta.Item1;
            ControllerAssCostSubTotalLabor += delta.Item2;
        }

        private void addPanel(TECPanel panel)
        {
            bool containsPanelType = panelTypeDictionary.ContainsKey(panel.Type.Guid);
            if (containsPanelType)
            {
                PanelTypeSubTotal -= panelTypeDictionary[panel.Type.Guid].Total;
                panelTypeDictionary[panel.Type.Guid].Quantity++;
                PanelTypeSubTotal += panelTypeDictionary[panel.Type.Guid].Total;
            }
            else
            {
                PanelTypeSummaryItem panelTypeItem = new PanelTypeSummaryItem(panel.Type);
                panelTypeDictionary.Add(panel.Type.Guid, panelTypeItem);
                PanelTypeSummaryItems.Add(panelTypeItem);
                PanelTypeSubTotal += panelTypeItem.Total;
            }
            foreach (TECAssociatedCost cost in panel.AssociatedCosts)
            {
                Tuple<double, double> delta = addCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
                PanelAssCostSubTotalCost += delta.Item1;
                PanelAssCostSubTotalLabor += delta.Item2;
            }
        }

        private void removePanel(TECPanel panel)
        {
            bool containsPanelType = panelTypeDictionary.ContainsKey(panel.Type.Guid);
            if (containsPanelType)
            {
                PanelTypeSubTotal -= panelTypeDictionary[panel.Type.Guid].Total;
                panelTypeDictionary[panel.Type.Guid].Quantity--;
                PanelTypeSubTotal += panelTypeDictionary[panel.Type.Guid].Total;

                if (panelTypeDictionary[panel.Type.Guid].Quantity < 1)
                {
                    PanelTypeSummaryItems.Remove(panelTypeDictionary[panel.Type.Guid]);
                    panelTypeDictionary.Remove(panel.Type.Guid);
                }

                foreach (TECAssociatedCost cost in panel.AssociatedCosts)
                {
                    Tuple<double, double> delta = removeCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
                    PanelAssCostSubTotalCost += delta.Item1;
                    PanelAssCostSubTotalLabor += delta.Item2;
                }
            }
            else
            {
                throw new InvalidOperationException("Panel type not found in panel type dictionary.");
            }
        }

        private void addCostToPanel(TECAssociatedCost cost)
        {
            Tuple<double, double> delta = addCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
            PanelAssCostSubTotalCost += delta.Item1;
            PanelAssCostSubTotalLabor += delta.Item2;
        }

        private void removeCostFromPanel(TECAssociatedCost cost)
        {
            Tuple<double, double> delta = removeCost(cost, panelAssCostDictionary, PanelAssCostSummaryItems);
            PanelAssCostSubTotalCost += delta.Item1;
            PanelAssCostSubTotalLabor += delta.Item2;
        }

        private void addMiscCost(TECMiscCost cost)
        {
            MiscCosts.Add(cost);
            MiscCostSubTotalCost += cost.Cost * cost.Quantity;
            MiscCostSubTotalLabor += cost.Labor * cost.Quantity;
        }

        private void removeMiscCost(TECMiscCost cost)
        {
            MiscCosts.Remove(cost);
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

        private Tuple<double, double> addCost(TECAssociatedCost cost, Dictionary<Guid, AssociatedCostSummaryItem> dictionary, ObservableCollection<AssociatedCostSummaryItem> collection)
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

        private Tuple<double, double> removeCost(TECAssociatedCost cost, Dictionary<Guid, AssociatedCostSummaryItem> dictionary, ObservableCollection<AssociatedCostSummaryItem> collection)
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