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
        private TECConnectionType _connectionType;
        private IOType _ioType;
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
        public TECConnectionType ConnectionType
        {
            get { return _connectionType; }
            set
            {
                var temp = this.Copy();
                _connectionType = value;
                NotifyPropertyChanged("ConnectionType", temp, this);
            }
        }
        public IOType IOType
        {
            get { return _ioType; }
            set
            {
                var temp = this.Copy();
                _ioType = value;
                NotifyPropertyChanged("IOType", temp, this);
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
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        #endregion//Properties

        #region Constructors
        public TECDevice(string name, string description, double cost, TECManufacturer manufacturer, Guid guid) : base(name, description, guid)
        {
            _cost = cost;
            _connectionType = new TECConnectionType();
            _manufacturer = manufacturer;
        }
        public TECDevice(string name, string description, double cost, TECConnectionType connectionType, TECManufacturer manufacturer)
            : this(name, description, cost, manufacturer, Guid.NewGuid()) { }
        public TECDevice() : this("", "", 0, new TECConnectionType(), new TECManufacturer()) { }
        
        //Copy Constructor
        public TECDevice(TECDevice deviceSource) 
            : this(deviceSource.Name, deviceSource.Description, deviceSource.Cost, deviceSource.Manufacturer, deviceSource.Guid)
        {
            _connectionType = deviceSource.ConnectionType;
            _ioType = deviceSource.IOType;
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
