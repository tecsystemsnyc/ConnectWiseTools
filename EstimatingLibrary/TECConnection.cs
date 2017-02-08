using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{

    public class TECConnection : TECObject
    {
        #region Properties
        private Guid _guid;
        private double _length;
        private TECController _controller;
        private ObservableCollection<TECScope> _scope;
        private ObservableCollection<ConnectionType> _connectionTypes;
        private ObservableCollection<IOType> _ioTypes;

        public Guid Guid
        {
            get { return _guid; }
        }
        public double Length
        {
            get { return _length; }
            set
            {
                var temp = this.Copy();
                _length = value;
                NotifyPropertyChanged("Length", temp, this);
            }
        }
        
        public TECController Controller
        {
            get { return _controller; }
            set
            {
                var temp = this.Copy();
                _controller = value;
                NotifyPropertyChanged("Controller", temp, this);
            }
        }
        public ObservableCollection<TECScope> Scope
        {
            get { return _scope; }
            set
            {
                var temp = this.Copy();
                _scope = value;
                NotifyPropertyChanged("Scope", temp, this);
            }
        }
        public ObservableCollection<ConnectionType> ConnectionTypes
        {
            get { return _connectionTypes; }
            set
            {
                var temp = this.Copy();
                _connectionTypes = value;
                NotifyPropertyChanged("ConnectionTypes", temp, this);
            }
        }
        public ObservableCollection<IOType> IOTypes
        {
            get { return _ioTypes; }
            set
            {
                var temp = this.Copy();
                _ioTypes = value;
                NotifyPropertyChanged("IOTypes", temp, this);
            }
        }

        #endregion //Properties

        #region Constructors 
        public TECConnection(double length, ObservableCollection<ConnectionType> types, Guid guid)
        {
            this._guid = guid;
            this._length = length;
            this._connectionTypes = types;
            this._scope = new ObservableCollection<TECScope>();
        }
        public TECConnection()
        {
            _guid = Guid.NewGuid();
            _length = 0;
            _connectionTypes = new ObservableCollection<ConnectionType>();
            _ioTypes = new ObservableCollection<IOType>();
            _controller = new TECController();
            _scope = new ObservableCollection<TECScope>();
        }

        public TECConnection(TECConnection connectionSource) : this(connectionSource.Length, connectionSource.ConnectionTypes, connectionSource.Guid)
        {
            _scope = connectionSource.Scope;
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECConnection connection = new TECConnection(this);

            return connection;
        }
        
        #endregion
        
    }
}
