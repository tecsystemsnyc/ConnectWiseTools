using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class DeleteDeviceVM : ViewModelBase
    {
        private readonly TECTemplates templates;

        public TECDevice Device { get; }
        public List<TECDevice> PotentialReplacements { get; }

        private TECDevice _selectedReplacement;
        public TECDevice SelectedReplacement
        {
            get { return _selectedReplacement; }
            set
            {
                _selectedReplacement = value;
                RaisePropertyChanged("SelectedReplacement");
            }
        }

        public ICommand DeleteCommand { get; private set; }
        public ICommand DeleteAndReplaceCommand { get; private set; }

        public DeleteDeviceVM(TECDevice device, TECTemplates templates)
        {
            this.templates = templates;
            Device = device;
            PotentialReplacements = new List<TECDevice>();
            populatePotentialReplacements();

            DeleteCommand = new RelayCommand(deleteExecute);
            DeleteAndReplaceCommand = new RelayCommand(deleteAndReplaceExecute, deleteAndReplaceCanExecute);
        }

        private void deleteExecute()
        {
            MessageBoxResult result = MessageBox.Show(
                "Deleting a device will remove it from all template Systems, Equipment and Points. Are you sure?", 
                "Continue?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

            if (result == MessageBoxResult.Yes)
            {
                foreach (TECSubScope ss in templates.SubScopeTemplates)
                {
                    while (ss.Devices.Contains(Device))
                    {
                        ss.Devices.Remove(Device);
                    }
                }

                foreach (TECEquipment equip in templates.EquipmentTemplates)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        while (ss.Devices.Contains(Device))
                        {
                            ss.Devices.Remove(Device);
                        }
                    }
                }

                foreach (TECSystem sys in templates.SystemTemplates)
                {
                    foreach (TECEquipment equip in sys.Equipment)
                    {
                        foreach (TECSubScope ss in equip.SubScope)
                        {
                            while (ss.Devices.Contains(Device))
                            {
                                ss.Devices.Remove(Device);
                            }
                        }
                    }
                }

                templates.Catalogs.Devices.Remove(Device);
            }
        }
        private void deleteAndReplaceExecute()
        {
            foreach (TECSubScope ss in templates.SubScopeTemplates)
            {
                while (ss.Devices.Contains(Device))
                {
                    ss.Devices.Remove(Device);
                    ss.Devices.Add(SelectedReplacement);
                }
            }

            foreach (TECEquipment equip in templates.EquipmentTemplates)
            {
                foreach (TECSubScope ss in equip.SubScope)
                {
                    while (ss.Devices.Contains(Device))
                    {
                        ss.Devices.Remove(Device);
                        ss.Devices.Add(SelectedReplacement);
                    }
                }
            }

            foreach (TECSystem sys in templates.SystemTemplates)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        while (ss.Devices.Contains(Device))
                        {
                            ss.Devices.Remove(Device);
                            ss.Devices.Add(SelectedReplacement);
                        }
                    }
                }
            }

            templates.Catalogs.Devices.Remove(Device);
        }
        private bool deleteAndReplaceCanExecute()
        {
            return (SelectedReplacement != null);
        }

        private void populatePotentialReplacements()
        {
            bool hasNetworkConnection = false;
            foreach (TECSystem sys in templates.SystemTemplates)
            {
                foreach (TECEquipment equip in sys.Equipment)
                {
                    foreach (TECSubScope ss in equip.SubScope)
                    {
                        if (ss.IsNetwork && ss.Connection != null)
                        {
                            hasNetworkConnection = true;
                            break;
                        }
                    }
                    if (hasNetworkConnection) break;
                }
                if (hasNetworkConnection) break;
            }

            foreach (TECDevice dev in templates.Catalogs.Devices)
            {
                if (hasNetworkConnection)
                {
                    if (connectionTypesAreSame(Device, dev))
                    {
                        PotentialReplacements.Add(dev);
                    }
                }
                else
                {
                    PotentialReplacements.Add(dev);
                }
            }
            PotentialReplacements.Remove(Device);
        }

        private bool connectionTypesAreSame(TECDevice dev1, TECDevice dev2)
        {
            bool oneContainsTwo = true;
            foreach (TECConnectionType type in dev2.ConnectionTypes)
            {
                if (!dev1.ConnectionTypes.Contains(type))
                {
                    oneContainsTwo = false;
                    break;
                }
            }
            bool twoContainsOne = true;
            foreach (TECConnectionType type in dev1.ConnectionTypes)
            {
                if (!dev2.ConnectionTypes.Contains(type))
                {
                    twoContainsOne = false;
                    break;
                }
            }

            return (oneContainsTwo && twoContainsOne);
        }
    }
}
