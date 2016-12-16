using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECDevice : TECScope
    {
        #region Properties
        private double _cost;
        private TECConnection _wire;
        private TECManufacturer _manufacturer;

        public double Cost {
            get { return _cost; }
            set
            {
                var temp = this.Copy();
                _cost = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Cost", temp, this);
            }
        }
        public TECConnection Wire
        {
            get { return _wire; }
            set
            {
                var temp = this.Copy();
                _wire = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Wire", temp, this);
            }
        }
        public TECManufacturer Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                var temp = this.Copy();
                _manufacturer = value;
                NotifyPropertyChanged("Manufacturer", temp, this);
            }
        }
        #endregion//Properties

        #region Constructors
        public TECDevice(string name, string description, double cost, TECConnection wire, TECManufacturer manufacturer, Guid guid) : base(name, description, guid)
        {
            _cost = cost;
            _wire = wire;
            _manufacturer = manufacturer;
        }
        public TECDevice(string name, string description, double cost, TECConnection wire, TECManufacturer manufacturer)
            : this(name, description, cost, wire, manufacturer, Guid.NewGuid()) { }
        public TECDevice() : this("", "", 0, new TECConnection(), new TECManufacturer()) { }
        
        //Copy Constructor
        public TECDevice(TECDevice deviceSource) 
            : this(deviceSource.Name, deviceSource.Description, deviceSource.Cost, deviceSource.Wire, deviceSource.Manufacturer, deviceSource.Guid)
        {
            _quantity = deviceSource.Quantity;
            _tags = new ObservableCollection<TECTag>(deviceSource.Tags);
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECDevice outDevice = new TECDevice(this);
            return outDevice;
        }

        public override Object DragDropCopy()
        {
            TECDevice outDevice = new TECDevice(this);
            return outDevice;
        }
        #endregion
    }
}
