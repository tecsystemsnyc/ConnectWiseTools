using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{

    public enum ConnectionType { TwoC18, ThreeC18, FourC18, SixC18,
        WireTHHN14, WireTHHN12,
        PlenTwoC18, PlenThree18, PlenFourC18, PlenSixC18,
        Cat6, Fiber, FiberArmor, BX};

    public class TECConnection : INotifyPropertyChanged
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
        public TECConnection(double length, ConnectionType type, TECController controller, ObservableCollection<TECScope> scope, Guid guid)
        {
            this._guid = guid;
            this.Length = length;
            this.Type = type;
            this.Controller = controller;
            this.Scope = scope;
        }
        public TECConnection()
        {
            _guid = Guid.NewGuid();
            Length = 0;
            Type = ConnectionType.TwoC18;
            Controller = new TECController();
            Scope = new ObservableCollection<TECScope>();
        }

        public TECConnection(TECConnection connectionSource) : this(connectionSource.Length, connectionSource.Type, connectionSource.Controller, connectionSource.Scope, connectionSource.Guid)
        { }
        #endregion //Constructors

        #region Methods
        public Object Copy()
        {
            TECConnection connection = new TECConnection(this);

            return connection;
        }
        #endregion

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        protected void NotifyPropertyChanged<T>(string propertyName, T oldvalue, T newvalue)
        {
            RaiseExtendedPropertyChanged(this, new PropertyChangedExtendedEventArgs<T>(propertyName, oldvalue, newvalue));
        }
        protected void RaiseExtendedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(sender, e);
        }
        #endregion //Property Changed
    }
}
