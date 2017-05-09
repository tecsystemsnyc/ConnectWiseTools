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
    public class DeviceSummaryViewModel : ViewModelBase
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
            }
        }
        #endregion

        public DeviceSummaryViewModel(TECBid bid)
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

            foreach (TECSystem sys in bid.Systems)
            {
                addSystem(sys);
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

                if(args.PropertyName == "Add")
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
                }
                else if (args.PropertyName == "Remove")
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
            }
            else
            {
                throw new InvalidOperationException("Device not found in device dictionary.");
            }
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