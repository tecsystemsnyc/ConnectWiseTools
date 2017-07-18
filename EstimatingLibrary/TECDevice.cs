using EstimatingLibrary.Interfaces;
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
                    ConnectionTypes.CollectionChanged -= ConnectionTypes_CollectionChanged;
                }
                var temp = this.Copy();
                _connectionTypes = value; if (ConnectionTypes != null)
                {
                    ConnectionTypes.CollectionChanged += ConnectionTypes_CollectionChanged;
                }
                NotifyPropertyChanged("ConnectionTypes", temp, this);
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
        #endregion//Properties

        #region Constructors
        public TECDevice(Guid guid, ObservableCollection<TECElectricalMaterial> connectionTypes, TECManufacturer manufacturer) : base(guid, manufacturer)
        {
            _cost = 0;
            _connectionTypes = connectionTypes;
            _type = CostType.TEC;
            ConnectionTypes.CollectionChanged += ConnectionTypes_CollectionChanged;
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

        private void ConnectionTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TECConnectionType type in e.NewItems)
                {
                    NotifyPropertyChanged<object>("AddCatalog", this, type);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECConnectionType type in e.OldItems)
                {
                    NotifyPropertyChanged<object>("RemoveCatalog", this, type);
                }
            }
        }

        #endregion
    }
}
