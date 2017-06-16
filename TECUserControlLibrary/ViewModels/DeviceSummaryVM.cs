using EstimatingLibrary;
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
    public class DeviceSummaryVM : ViewModelBase
    {
        #region Properties
        public TECBid Bid { get; private set; }

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

        private Dictionary<Guid, CostSummaryItem> deviceAssCostDictionary;

        private ObservableCollection<CostSummaryItem> _deviceAssCostSummaryItems;
        public ObservableCollection<CostSummaryItem> DeviceAssCostSummaryItems
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
            }
        }

        #region Device Add/Remove/Replace
        private DeviceSummaryItem _selectedDevice;
        public DeviceSummaryItem SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                RaisePropertyChanged("SelectedDevice");
            }
        }

        private TECDevice _newSelectedDevice;
        public TECDevice NewSelectedDevice
        {
            get { return _newSelectedDevice; }
            set
            {
                _newSelectedDevice = value;
                RaisePropertyChanged("NewSelectedDevice");
            }
        }

        public ICommand AddDeviceCommand { get; private set; }
        public ICommand RemoveDeviceCommand { get; private set; }
        public ICommand ReplaceDeviceCommand { get; private set; }

        private ICommand _confirmCommand;
        public ICommand ConfirmCommand
        {
            get { return _confirmCommand; }
            set
            {
                _confirmCommand = value;
                RaisePropertyChanged("Confirm");
            }
        }
        public ICommand CancelCommand { get; private set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        private bool _hasNew;
        public bool HasNew
        {
            get { return _hasNew; }
            private set
            {
                _hasNew = value;
                RaisePropertyChanged("HasNew");
            }
        }

        private string _firstString;
        public string FirstString
        {
            get { return _firstString; }
            private set
            {
                _firstString = value;
                RaisePropertyChanged("FirstString");
            }
        }

        private string _secondString;
        public string SecondString
        {
            get { return _secondString; }
            set
            {
                _secondString = value;
                RaisePropertyChanged("SecondString");
            }
        }
        #endregion
        #endregion

        public DeviceSummaryVM(TECBid bid)
        {
            reinitialize(bid);
            AddDeviceCommand = new RelayCommand(AddDeviceExecute);
            RemoveDeviceCommand = new RelayCommand(RemoveDeviceExecute);
            ReplaceDeviceCommand = new RelayCommand(ReplaceDeviceExecute);
            CancelCommand = new RelayCommand(CancelExecute);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            Bid = bid;

            deviceDictionary = new Dictionary<Guid, DeviceSummaryItem>();
            DeviceSummaryItems = new ObservableCollection<DeviceSummaryItem>();
            deviceAssCostDictionary = new Dictionary<Guid, CostSummaryItem>();
            DeviceAssCostSummaryItems = new ObservableCollection<CostSummaryItem>();

            DeviceSubTotal = 0;
            DeviceAssCostSubTotalCost = 0;
            DeviceAssCostSubTotalLabor = 0;

            foreach(TECSystem system in bid.Systems)
            {
                foreach(TECSystem sys in system.SystemInstances)
                {
                    AddSystem(sys);
                }
            }
        }

        #region Add/Remove
        public void AddDevice(TECDevice device)
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
            foreach (TECCost cost in device.AssociatedCosts)
            {
                Tuple<double, double> delta = TECMaterialSummaryVM.AddCost(cost, deviceAssCostDictionary, DeviceAssCostSummaryItems);
                DeviceAssCostSubTotalCost += delta.Item1;
                DeviceAssCostSubTotalLabor += delta.Item2;
            }
        }

        public void RemoveDevice(TECDevice device)
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

                foreach (TECCost cost in device.AssociatedCosts)
                {
                    Tuple<double, double> delta = TECMaterialSummaryVM.RemoveCost(cost, deviceAssCostDictionary, DeviceAssCostSummaryItems);
                    DeviceAssCostSubTotalCost += delta.Item1;
                    DeviceAssCostSubTotalLabor += delta.Item2;
                }
            }
            else
            {
                throw new InvalidOperationException("Device not found in device dictionary.");
            }
        }

        public void AddCostToDevices(TECCost cost, TECDevice device)
        {
            if (deviceDictionary.ContainsKey(device.Guid))
            {
                for (int i = 0; i < deviceDictionary[device.Guid].Quantity; i++)
                {
                    Tuple<double, double> delta = TECMaterialSummaryVM.AddCost(cost, deviceAssCostDictionary, DeviceAssCostSummaryItems);
                    DeviceAssCostSubTotalCost += delta.Item1;
                    DeviceAssCostSubTotalLabor += delta.Item2;
                }
            }
        }

        public void RemoveCostFromDevices(TECCost cost, TECDevice device)
        {
            if (deviceDictionary.ContainsKey(device.Guid))
            {
                for (int i = 0; i < deviceDictionary[device.Guid].Quantity; i++)
                {
                    Tuple<double, double> delta = TECMaterialSummaryVM.RemoveCost(cost, deviceAssCostDictionary, DeviceAssCostSummaryItems);
                    DeviceAssCostSubTotalCost += delta.Item1;
                    DeviceAssCostSubTotalLabor += delta.Item2;
                }
            }
        }

        public void AddSubScope(TECSubScope ss)
        {
            foreach (TECDevice dev in ss.Devices)
            {
                AddDevice(dev);
            }
        }

        public void RemoveSubScope(TECSubScope ss)
        {
            foreach (TECDevice dev in ss.Devices)
            {
                RemoveDevice(dev);
            }
        }

        public void AddEquipment(TECEquipment equip)
        {
            foreach (TECSubScope ss in equip.SubScope)
            {
                AddSubScope(ss);
            }
        }

        public void RemoveEquipment(TECEquipment equip)
        {
            foreach (TECSubScope ss in equip.SubScope)
            {
                RemoveSubScope(ss);
            }
        }

        public void AddSystem(TECSystem sys)
        {
            foreach (TECEquipment equip in sys.Equipment)
            {
                AddEquipment(equip);
            }
        }

        public void RemoveSystem(TECSystem sys)
        {
            foreach (TECEquipment equip in sys.Equipment)
            {
                RemoveEquipment(equip);
            }
        }
        #endregion

        #region Commands Methods
        private void AddDeviceExecute()
        {
            ConfirmCommand = new RelayCommand(ConfirmAddDeviceExecute, AddDeviceCanConfirm);
            FirstString = "For all";
            SecondString = "add";
            HasNew = true;
            IsExpanded = true;
        }
        private void RemoveDeviceExecute()
        {
            ConfirmCommand = new RelayCommand(ConfirmRemoveDeviceExecute, RemoveDeviceCanConfirm);
            FirstString = "Remove all";
            SecondString = "from Bid";
            HasNew = false;
            IsExpanded = true;
        }
        private void ReplaceDeviceExecute()
        {
            ConfirmCommand = new RelayCommand(ConfirmReplaceDeviceExecute, ReplaceDeviceCanConfirm);
            FirstString = "Replace all";
            SecondString = "with";
            HasNew = true;
            IsExpanded = true;
        }

        private void ConfirmAddDeviceExecute()
        {
            foreach (TECSystem system in Bid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        int devicesToAdd = 0;
                        foreach (TECDevice dev in ss.Devices)
                        {
                            if (dev.Guid == SelectedDevice.Device.Guid)
                            {
                                devicesToAdd += 1;
                            }
                        }
                        for (int i = 0; i < devicesToAdd; i++)
                        {
                            ss.Devices.Add(NewSelectedDevice);
                        }
                    }
                }
            }
            IsExpanded = false;
        }
        private bool AddDeviceCanConfirm()
        {
            return ((SelectedDevice != null) && (NewSelectedDevice != null));
        }

        private void ConfirmRemoveDeviceExecute()
        {
            foreach (TECSystem system in Bid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        List<TECDevice> devicesToRemove = new List<TECDevice>();
                        foreach (TECDevice dev in ss.Devices)
                        {
                            if (dev.Guid == SelectedDevice.Device.Guid)
                            {
                                devicesToRemove.Add(dev);
                            }
                        }
                        foreach (TECDevice dev in devicesToRemove)
                        {
                            ss.Devices.Remove(dev);
                        }
                        if (SelectedDevice == null)
                        {
                            IsExpanded = false;
                            return;
                        }
                    }
                }
            }
            IsExpanded = false;
        }
        private bool RemoveDeviceCanConfirm()
        {
            return (SelectedDevice != null);
        }

        private void ConfirmReplaceDeviceExecute()
        {
            foreach (TECSystem system in Bid.Systems)
            {
                foreach (TECEquipment equip in system.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        List<TECDevice> devicesToRemove = new List<TECDevice>();
                        foreach (TECDevice dev in ss.Devices)
                        {
                            if (dev.Guid == SelectedDevice.Device.Guid)
                            {
                                devicesToRemove.Add(dev);
                            }
                        }
                        foreach (TECDevice dev in devicesToRemove)
                        {
                            ss.Devices.Remove(dev);
                            ss.Devices.Add(NewSelectedDevice);
                        }
                        if (SelectedDevice == null)
                        {
                            IsExpanded = false;
                            return;
                        }
                    }
                }
            }
            IsExpanded = false;
        }
        private bool ReplaceDeviceCanConfirm()
        {
            return ((NewSelectedDevice != null) && (SelectedDevice != null));
        }

        private void CancelExecute()
        {
            IsExpanded = false;
        }
        #endregion

        #region Event Handlers
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
        #endregion
    }
}
