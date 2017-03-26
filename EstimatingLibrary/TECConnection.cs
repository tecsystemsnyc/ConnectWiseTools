using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{

    public abstract class TECConnection : TECObject
    {
        #region Properties
        protected Guid _guid;
        protected double _length;
        protected TECController _parentController;
        protected TECConduitType _conduitType;

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
        public TECController ParentController
        {
            get { return _parentController; }
            set
            {
                var oldNew = Tuple.Create<Object, Object>(_parentController, value);
                var temp = Copy();
                _parentController = value;
                RaisePropertyChanged("ParentController");
                NotifyPropertyChanged("RelationshipPropertyChanged", temp, oldNew);
            }
        }
        public TECConduitType ConduitType
        {
            get { return _conduitType; }
            set
            {
                var oldNew = Tuple.Create<Object, Object>(_conduitType, value); 
                var temp = Copy();
                _conduitType = value;
                NotifyPropertyChanged("ConduitType", temp, this);
                temp = Copy();
                NotifyPropertyChanged("ObjectPropertyChanged", temp, oldNew);
            }
        }
        #endregion //Properties

        #region Constructors 
        public TECConnection(Guid guid)
        {
            _guid = guid;
            _length = 0;
        }
        public TECConnection() : this(Guid.NewGuid()) { }
        public TECConnection(TECConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = connectionSource.Guid; }

            _length = connectionSource.Length;
            //if(connectionSource.)
            if(connectionSource._parentController != null)
            { _parentController = new TECController(connectionSource.ParentController, guidDictionary); }
            if (connectionSource.ConduitType != null)
            { _conduitType = connectionSource.ConduitType.Copy() as TECConduitType; }
        }
        #endregion //Constructors
    }
}
