using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECSubScopeConnection : TECConnection
    {
        #region Properties
        //---Stored---
        private TECSubScope _subScope;
        
        public TECSubScope SubScope
        {
            get { return _subScope; }
            set
            {
                var oldNew = Tuple.Create<Object, Object>(_subScope, value);
                _subScope = value;
                RaisePropertyChanged("SubScope");
                var temp = Copy();
                NotifyPropertyChanged("RelationshipPropertyChanged", temp, oldNew);
            }
        }

        //---Derived---
        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get
            {
                var outConnectionTypes = new ObservableCollection<TECConnectionType>();
                
                foreach (TECDevice dev in SubScope.Devices)
                {
                    outConnectionTypes.Add(dev.ConnectionType);
                }

                return outConnectionTypes;
            }
        }
        public ObservableCollection<IOType> IOTypes
        {
            get
            {
                var outIOTypes = new ObservableCollection<IOType>();
                
                foreach (TECDevice dev in SubScope.Devices)
                {
                    outIOTypes.Add(dev.IOType);
                }

                return outIOTypes;
            }
        }
        #endregion

        #region Constructors
        public TECSubScopeConnection(Guid guid) : base(guid) {}
        public TECSubScopeConnection() : base (Guid.NewGuid()) { }
        public TECSubScopeConnection(TECSubScopeConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, guidDictionary)
        {
            if(connectionSource._subScope != null)
            { _subScope = new TECSubScope(connectionSource.SubScope, guidDictionary); }
            
        }
        #endregion Constructors

        #region Methods
        public override Object Copy()
        {
            TECSubScopeConnection connection = new TECSubScopeConnection(this);
            connection._guid = this._guid;
            if (_parentController != null)
            { connection._parentController = _parentController.Copy() as TECController; }
            if (_subScope != null)
            { connection._subScope = _subScope.Copy() as TECSubScope; }
            return connection;
        }
        #endregion

    }
}
