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
        private ConnectionType _connectionType;
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
        public ConnectionType ConnectionType
        {
            get { return _connectionType; }
            set
            {
                var temp = this.Copy();
                _connectionType = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("ConnectionType", temp, this);
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
        public TECDevice(string name, string description, double cost, ConnectionType connectiontype, TECManufacturer manufacturer, Guid guid) : base(name, description, guid)
        {
            _cost = cost;
            _connectionType = connectiontype;
            _manufacturer = manufacturer;
        }
        public TECDevice(string name, string description, double cost, ConnectionType connectionType, TECManufacturer manufacturer)
            : this(name, description, cost, connectionType, manufacturer, Guid.NewGuid()) { }
        public TECDevice() : this("", "", 0, 0, new TECManufacturer()) { }
        
        //Copy Constructor
        public TECDevice(TECDevice deviceSource) 
            : this(deviceSource.Name, deviceSource.Description, deviceSource.Cost, deviceSource.ConnectionType, deviceSource.Manufacturer, deviceSource.Guid)
        {
            _connectionType = deviceSource.ConnectionType;
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
