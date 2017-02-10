using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECSubScope : TECScope
    {
        #region Properties
        private ObservableCollection<TECDevice> _devices;
        public ObservableCollection<TECDevice> Devices
        {
            get { return _devices; }
            set
            {
                var temp = this.Copy();
                _devices = value;
                NotifyPropertyChanged("Devices", temp, this);
            }
        }

        private ObservableCollection<TECPoint> _points;
        public ObservableCollection<TECPoint> Points
        {
            get { return _points; }
            set
            {
                var temp = this.Copy();
                _points = value;
                NotifyPropertyChanged("Points", temp, this);
            }
        }


        private TECConnection _connection { get; set; }
        public TECConnection Connection {
            get { return _connection; }
            set
            {
                var temp = this.Copy();
                _connection = value;
                NotifyPropertyChanged("Connection", temp, this);
            }
        }

        public double MaterialCost
        {
            get { return getMaterialCost(); }
        }
        public double LaborCost
        {
            get { return getLaborCost(); }
        }

        public ObservableCollection<ConnectionType> ConnectionTypes
        {
            get { return getConnectionTypes(); }
        }

        public List<ConnectionType> AvailableConnections
        {
            get { return getAvailableConnectionTypes(); }
        }

        public ObservableCollection<PointTypes> AllPointTypes
        {
            get { return getAllPointTypes(); }
        }

        public ObservableCollection<IOType> AllIOTypes
        {
            get { return getAllIOTypes(); }
        }

        private double _length;
        public double Length
        {
            get
            {
                return _length;
            }
            set
            {
                var temp = this.Copy();
                _length = value;
                NotifyPropertyChanged("Length", temp, this);
            }
        }


        #endregion //Properties

        #region Constructors
        public TECSubScope(string name, string description, ObservableCollection<TECDevice> devices, ObservableCollection<TECPoint> points, Guid guid) : base(name, description, guid)
        {
            _devices = devices;
            _points = points;
            Points.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PointsCollectionChanged);
            subscribeToDevices();
            Devices.CollectionChanged += Devices_CollectionChanged;
        }
        
        public TECSubScope(string name, string description, ObservableCollection<TECDevice> devices, ObservableCollection<TECPoint> points) : this(name, description, devices, points, Guid.NewGuid()) { }
        public TECSubScope() : this("", "", new ObservableCollection<TECDevice>(), new ObservableCollection<TECPoint>()) { }

        //Copy Constructor
        public TECSubScope(TECSubScope sourceSubScope) : this(sourceSubScope.Name, sourceSubScope.Description, new ObservableCollection<TECDevice>(), new ObservableCollection<TECPoint>())
        {
            foreach(TECDevice device in sourceSubScope.Devices)
            {
                Devices.Add(new TECDevice(device));
            }
            foreach(TECPoint point in sourceSubScope.Points)
            {
                Points.Add(new TECPoint(point));
            }

            _location = sourceSubScope.Location;
            _quantity = sourceSubScope.Quantity;
            _tags = new ObservableCollection<TECTag>(sourceSubScope.Tags);
        }
        #endregion //Constructors

        #region Num Point Types
        private int _ai;
        private int _ao;
        private int _bi;
        private int _bo;
        private int _serial;

        public int AI { get { return _ai; } }
        public int AO { get { return _ao; } }
        public int BI { get { return _bi; } }
        public int BO { get { return _bo; } }
        public int Serial { get { return _serial; } }
        #endregion //Num Point Types

        #region Event Handlers

        private void PointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _ai = 0;
            _ao = 0;
            _bi = 0;
            _bo = 0;
            _serial = 0;
            foreach(TECPoint point in Points)
            {
                if      (point.Type == PointTypes.AI) { _ai++; }
                else if (point.Type == PointTypes.AO) { _ao++; }
                else if (point.Type == PointTypes.BI) { _bi++; }
                else if (point.Type == PointTypes.BO) { _bo++; }
                else if (point.Type == PointTypes.Serial) { _serial++; }
                else
                {
                    string message = "Invalid Point Type in PointsColllectionChanged in TECSubScope";
                    throw new Exception(message);
                }
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                }
                RaisePropertyChanged("TotalPoints");
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
                RaisePropertyChanged("TotalPoints");
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender);
            }
        }

        private void Devices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                    ((TECDevice)item).PropertyChanged += DeviceChanged;
                    RaisePropertyChanged("TotalDevices");
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                    ((TECDevice)item).PropertyChanged -= DeviceChanged;
                    RaisePropertyChanged("TotalDevices");
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender);
            }
        }

        private void DeviceChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
            if (e.PropertyName == "Quantity")
            {
                NotifyPropertyChanged("ChildChanged", (object)this, (object)args.NewValue);
            }
        }


        #endregion

        #region Methods

        public override Object Copy()
        {
            TECSubScope outScope = new TECSubScope(this);
            outScope._guid = Guid;
            return outScope;
        }

        public override object DragDropCopy()
        {
            TECSubScope outScope = new TECSubScope(this);
            return outScope;
        }

        private ObservableCollection<ConnectionType> getConnectionTypes()
        {
            var outTypes = new ObservableCollection<ConnectionType>();
            foreach(TECDevice device in Devices)
            {
                outTypes.Add(device.ConnectionType);
            }
            return outTypes;
        }

        private double getMaterialCost()
        {
            double matCost = 0;

            foreach(TECDevice device in this.Devices)
            {
                matCost += device.Cost * device.Manufacturer.Multiplier;
            }

            return matCost;
        }

        private double getLaborCost()
        {
            double labCost = 0;

            foreach (TECPoint point in this.Points)
            {
                labCost += 1 * point.Quantity;
            }

            return labCost;
        }

        private void subscribeToDevices()
        {
            foreach(TECDevice item in this._devices)
            {
                item.PropertyChanged += DeviceChanged;
            }
        }

        private void unSubscribeToDevices()
        {
            foreach (TECDevice item in this._devices)
            {
                item.PropertyChanged -= DeviceChanged;
            }
        }

        private List<ConnectionType> getAvailableConnectionTypes()
        {
            var availableConnections = new List<ConnectionType>();
            foreach (ConnectionType conType in this.ConnectionTypes)
            {
                availableConnections.Add(conType);
            }

            return availableConnections;
        }

        private ObservableCollection<PointTypes> getAllPointTypes()
        {
            var allPointTypes = new ObservableCollection<PointTypes>();

            foreach(TECPoint point in Points)
            {
                allPointTypes.Add(point.Type);
            }
            
            return allPointTypes;
        }

        private ObservableCollection<IOType> getAllIOTypes()
        {
            var allIOTypes = new ObservableCollection<IOType>();

            foreach(TECDevice device in Devices)
            {
                allIOTypes.Add(device.IOType);
            }
            return allIOTypes;
        }
        #endregion
    }
}
