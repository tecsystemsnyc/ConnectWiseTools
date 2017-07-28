using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECDevice : TECHardware, DragDropComponent
    {
        #region Properties
        private ObservableCollection<TECElectricalMaterial> _connectionTypes;
        private IOType _ioType;
        
        public ObservableCollection<TECElectricalMaterial> ConnectionTypes
        {
            get { return _connectionTypes; }
            set
            {
                if(ConnectionTypes != null)
                {
                    ConnectionTypes.CollectionChanged -= (sender, args) => ConnectionTypes_CollectionChanged(sender, args, "ConnectionTypes");
                }
                var old = ConnectionTypes;
                _connectionTypes = value; if (ConnectionTypes != null)
                {
                    ConnectionTypes.CollectionChanged += (sender, args) => ConnectionTypes_CollectionChanged(sender, args, "ConnectionTypes");
                }
                NotifyPropertyChanged(Change.Edit, "ConnectionTypes", this, value, old);
                //NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        public IOType IOType
        {
            get { return _ioType; }
            set
            {
                var old = IOType;
                _ioType = value;
                NotifyPropertyChanged(Change.Add, "IOType", this, value, old);
                //NotifyPropertyChanged("ChildChanged", (object)this, (object)value);
            }
        }
        #endregion//Properties

        #region Constructors
        public TECDevice(Guid guid, ObservableCollection<TECElectricalMaterial> connectionTypes, TECManufacturer manufacturer) : base(guid, manufacturer)
        {
            _cost = 0;
            _connectionTypes = connectionTypes;
            _type = CostType.TEC;
            ConnectionTypes.CollectionChanged += (sender, args) => ConnectionTypes_CollectionChanged(sender, args, "ConnectionTypes");
        }
        public TECDevice(ObservableCollection<TECElectricalMaterial> connectionTypes, TECManufacturer manufacturer) : this(Guid.NewGuid(), connectionTypes, manufacturer) { }

        //Copy Constructor
        public TECDevice(TECDevice deviceSource)
            : this(deviceSource.Guid, deviceSource.ConnectionTypes, deviceSource.Manufacturer)
        {
            this.copyPropertiesFromHardware(deviceSource);
            _ioType = deviceSource.IOType;
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECDevice outDevice = new TECDevice(this);
            return outDevice;
        }

        public new Object DragDropCopy(TECScopeManager scopeManager)
        {
            foreach(TECDevice device in scopeManager.Catalogs.Devices)
            {
                if(device.Guid == this.Guid)
                {
                    return device;
                }
            }
            throw new Exception();
        }

        private void ConnectionTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TECElectricalMaterial type in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, propertyName, this, type);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECElectricalMaterial type in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, propertyName, this, type);
                }
            }
        }

        #endregion
    }
}
