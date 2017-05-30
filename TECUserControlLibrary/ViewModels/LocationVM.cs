using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModels
{
    public class LocationVM : SystemsVM
    {
        #region Properties
        private TECLocation _selectedLocation;
        public TECLocation SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                RaisePropertyChanged("SelectedLocation");
                organizeByLocation();
            }
        }

        private ObservableCollection<TECSystem> _systemsByLocation;
        public ObservableCollection<TECSystem> SystemsByLocation
        {
            get { return _systemsByLocation; }
            set
            {
                _systemsByLocation = value;
                RaisePropertyChanged("SystemsByLocation");
            }

        }

        private ObservableCollection<TECEquipment> _equipmentByLocation;
        public ObservableCollection<TECEquipment> EquipmentByLocation
        {
            get { return _equipmentByLocation; }
            set
            {
                _equipmentByLocation = value;
                RaisePropertyChanged("EquipmentByLocation");
            }
        }

        private ObservableCollection<TECSubScope> _subScopeByLocation;
        public ObservableCollection<TECSubScope> SubScopeByLocation
        {
            get { return _subScopeByLocation; }
            set
            {
                _subScopeByLocation = value;
                RaisePropertyChanged("SubScopeByLocation");
            }
        }

        private LocationScopeType _selectedScopeType;
        public LocationScopeType SelectedScopeType
        {
            get { return _selectedScopeType; }
            set
            {
                _selectedScopeType = value;
                RaisePropertyChanged("SelectedScopeType");
            }
        }

        #endregion

        public LocationVM(TECBid bid) : base(bid) { }

        #region Methods

        private void organizeByLocation()
        {
            SystemsByLocation = new ObservableCollection<TECSystem>();
            EquipmentByLocation = new ObservableCollection<TECEquipment>();
            SubScopeByLocation = new ObservableCollection<TECSubScope>();

            foreach (TECSystem system in Bid.Systems)
            {
                if (system.Location == SelectedLocation)
                {
                    SystemsByLocation.Add(system);
                }
                foreach (TECEquipment equipment in system.Equipment)
                {
                    if (equipment.Location == SelectedLocation)
                    {
                        EquipmentByLocation.Add(equipment);
                    }
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        if (subScope.Location == SelectedLocation)
                        {
                            SubScopeByLocation.Add(subScope);
                        }
                    }
                }
            }
        }

        public void Refresh(TECBid bid)
        {
            Bid = bid;
            SelectedLocation = null;
            organizeByLocation();
        }
        #endregion
    }
}
