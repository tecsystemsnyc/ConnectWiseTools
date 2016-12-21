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
        private ConnectionType _type;
        private TECController _controller;
        private ObservableCollection<TECScope> _scope;

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
        public ConnectionType Type
        {
            get { return _type; }
            set
            {
                var temp = this.Copy();
                _type = value;
                NotifyPropertyChanged("Type", temp, this);
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
                RaisePropertyChanged("Scope");
            }
        }

        #endregion //Properties

        #region Constructors 
        public TECConnection(double length, ConnectionType type, Guid guid)
        {
            this._guid = guid;
            this._length = length;
            this._type = type;
            this._scope = new ObservableCollection<TECScope>();
        }
        public TECConnection()
        {
            _guid = Guid.NewGuid();
            _length = 0;
            _type = ConnectionType.TwoC18;
            _controller = new TECController();
            _scope = new ObservableCollection<TECScope>();
        }

        public TECConnection(TECConnection connectionSource) : this(connectionSource.Length, connectionSource.Type, connectionSource.Guid)
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
