using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.ViewModelExtensions
{
    public class LocationDataGridExtension : ScopeDataGridExtension
    {
        #region Properties
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                // Call OnPropertyChanged whenever the property is updated
                RaisePropertyChanged("Bid");
                populateLocationSelections();
                Bid.Locations.CollectionChanged += Locations_CollectionChanged;
            }
        }


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

        private ObservableCollection<TECLocation> _locationSelections;
        public ObservableCollection<TECLocation> LocationSelections
        {
            get { return _locationSelections; }
            set
            {
                _locationSelections = value;
                RaisePropertyChanged("LocationSelections");
            }
        }
        #endregion
        public LocationDataGridExtension()
        {

        }
        
        #region Methods
        private void Locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            populateLocationSelections();
        }

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

        private void populateLocationSelections()
        {
            LocationSelections = new ObservableCollection<TECLocation>();

            LocationSelections.Add(new TECLocation("None"));
            foreach (TECLocation location in Bid.Locations)
            {
                LocationSelections.Add(location);
            }
        }
        #endregion
    }
}
