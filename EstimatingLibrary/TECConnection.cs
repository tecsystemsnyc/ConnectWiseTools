using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{

    public abstract class TECConnection : TECObject, CostComponent
    {
        #region Properties
        protected Guid _guid;
        protected double _length;
        protected double _conduitLength;
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
        public double ConduitLength
        {
            get { return _conduitLength; }
            set
            {
                var temp = this.Copy();
                _conduitLength = value;
                NotifyPropertyChanged("ConduitLength", temp, this);
            }
        }
        public TECController ParentController
        {
            get { return _parentController; }
            set
            {
                _parentController = value;
                RaisePropertyChanged("ParentController");
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
                NotifyPropertyChanged("ObjectPropertyChanged", temp, oldNew, typeof(TECConnection), typeof(TECConduitType));
            }
        }

        public double MaterialCost
        {
            get
            {
                return 0;
            }
        }

        public double LaborCost
        {
            get
            {
                return 0;
            }
        }

        public double ElectricalCost
        {
            get
            {
                return getElectricalCost();
            }
        }

        public double ElectricalLabor
        {
            get
            {
                return getElectricalLabor();
            }
        }
        #endregion //Properties

        #region Constructors 
        public TECConnection(Guid guid)
        {
            _guid = guid;
            _length = 0;
            _conduitLength = 0;
        }
        public TECConnection() : this(Guid.NewGuid()) { }
        public TECConnection(TECConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = connectionSource.Guid; }

            _length = connectionSource.Length;
            _conduitLength = connectionSource.ConduitLength;
            if (connectionSource.ConduitType != null)
            { _conduitType = connectionSource.ConduitType.Copy() as TECConduitType; }
        }
        #endregion //Constructors

        abstract protected double getElectricalCost();
        abstract protected double getElectricalLabor();
    }
}
