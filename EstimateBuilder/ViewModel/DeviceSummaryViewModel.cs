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
        private ChangeWatcher _changeWatcher;

        private Dictionary<TECDevice, DeviceSummaryItem> _deviceDictionary;

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
            _deviceDictionary = new Dictionary<TECDevice, DeviceSummaryItem>();

            foreach(TECSystem sys in bid.Systems)
            {
                addSystem(sys);
            }

            _changeWatcher = new ChangeWatcher(bid);
            _changeWatcher.Changed += bidChanged;
        }

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

        private void addDevice(TECDevice device)
        {
            if (_deviceDictionary.ContainsKey(device))
            {
                _deviceDictionary[device].Quantity++;
            }
            else
            {
                _deviceDictionary.Add(device, new DeviceSummaryItem(device));
            }
        }

        private void removeDevice(TECDevice device)
        {
            if (_deviceDictionary.ContainsKey(device))
            {
                _deviceDictionary[device].Quantity--;
                if (_deviceDictionary[device].Quantity < 1)
                {
                    _deviceDictionary.Remove(device);
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
    }
}