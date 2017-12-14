using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
                var old = SubScope;
                _subScope = value;
                raisePropertyChanged("SubScope");
                notifyCombinedChanged(Change.Edit, "SubScope", this, value, old);
            }
        }

        //---Derived---
        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get
            {
                var outConnectionTypes = new ObservableCollection<TECConnectionType>();
                if (SubScope != null)
                {
                    foreach (IEndDevice dev in SubScope.Devices)
                    {
                        foreach(TECConnectionType type in dev.ConnectionTypes)
                        {
                            outConnectionTypes.Add(type);
                        }
                    }
                }

                return outConnectionTypes;
            }
        }
        public override IOCollection IO
        {
            get
            {
                IOCollection io = new IOCollection();
                foreach(TECPoint point in SubScope.Points)
                {
                    for(int i = 0; i < point.Quantity; i++)
                    {
                        io.AddIO(point.Type);
                    }
                }
                return io;
            }
        }
        #endregion

        #region Constructors
        public TECSubScopeConnection(Guid guid, bool isTypical) : base(guid, isTypical) { }
        public TECSubScopeConnection(bool isTypical) : this(Guid.NewGuid(), isTypical) { }
        public TECSubScopeConnection(TECSubScopeConnection connectionSource, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, isTypical, guidDictionary)
        {
            if (connectionSource._subScope != null)
            {
                _subScope = new TECSubScope(connectionSource.SubScope, isTypical, guidDictionary);
                _subScope.Connection = this;
            }
        }
        public TECSubScopeConnection(TECSubScopeConnection linkingSource, TECSubScope actualSubScope, bool isTypical) : base(linkingSource, isTypical)
        {
            _subScope = actualSubScope;
            _guid = linkingSource.Guid;
        }
        #endregion Constructors

        #region Methods
        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            saveList.Add(this.SubScope, "SubScope");
            return saveList;
        }
        protected override SaveableMap linkedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.linkedObjects());
            saveList.Add(this.SubScope, "SubScope");
            return saveList;
        }
        public override ObservableCollection<TECConnectionType> GetConnectionTypes()
        {
            return ConnectionTypes;
        }
        #endregion
    }
}
