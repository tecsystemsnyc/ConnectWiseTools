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
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
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
                NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
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
        public TECDevice(Guid guid) : base(guid)
        {
            _cost = 0;
            _connectionType = new TECConnectionType();
        }
        public TECDevice() : this(Guid.NewGuid()) { }
        
        //Copy Constructor
        public TECDevice(TECDevice deviceSource) 
            : this(deviceSource.Guid)
        {
            _name = deviceSource.Name;
            _description = deviceSource.Description;
            _cost = deviceSource.Cost;
            _manufacturer = deviceSource.Manufacturer;
            _connectionType = deviceSource.ConnectionType;
            _ioType = deviceSource.IOType;
            _quantity = deviceSource.Quantity;
            _associatedCosts = new ObservableCollection<TECAssociatedCost>(deviceSource.AssociatedCosts);
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
