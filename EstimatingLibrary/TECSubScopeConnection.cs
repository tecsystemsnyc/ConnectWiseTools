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
        private ObservableCollection<TECSubScope> _subScope;
        
        public ObservableCollection<TECSubScope> SubScope
        {
            get { return _subScope; }
            set
            {
                var temp = this.Copy();
                SubScope.CollectionChanged -= SubScope_CollectionChanged;
                _subScope = value;
                NotifyPropertyChanged("SubScope", temp, this);
                SubScope.CollectionChanged += SubScope_CollectionChanged;
            }
        }

        //---Derived---
        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get
            {
                var outConnectionTypes = new ObservableCollection<TECConnectionType>();

                foreach (TECSubScope ss in SubScope)
                {
                    foreach (TECDevice dev in ss.Devices)
                    {
                        outConnectionTypes.Add(dev.ConnectionType);
                    }
                }
                return outConnectionTypes;
            }
        }
        public ObservableCollection<IOType> IOTypes
        {
            get
            {
                var outIOTypes = new ObservableCollection<IOType>();

                foreach (TECSubScope ss in SubScope)
                {
                    foreach (TECDevice dev in ss.Devices)
                    {
                        outIOTypes.Add(dev.IOType);
                    }
                }
                return outIOTypes;
            }
        }
        #endregion

        #region Constructors
        public TECSubScopeConnection(Guid guid) : base(guid)
        {
            _subScope = new ObservableCollection<TECSubScope>();
            SubScope.CollectionChanged += SubScope_CollectionChanged;
        }
        public TECSubScopeConnection() : base (Guid.NewGuid()) { }
        public TECSubScopeConnection(TECSubScopeConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, guidDictionary)
        {
            foreach (TECSubScope ss in connectionSource.SubScope)
            {
                 _subScope.Add(new TECSubScope(ss, guidDictionary));
            }
        }
        #endregion Constructors

        #region Methods
        public override Object Copy()
        {
            TECSubScopeConnection connection = new TECSubScopeConnection(this);
            connection._guid = this._guid;
            return connection;
        }
        #endregion

        #region Event Handlers
        private void SubScope_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECSubScope)
                    {
                        (item as TECSubScope).Connection = this;
                    }
                    NotifyPropertyChanged("AddRelationship", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECSubScope)
                    {
                        (item as TECSubScope).Connection = null;
                    }
                    NotifyPropertyChanged("RemoveRelationship", this, item);
                }
            }
        }
        #endregion

    }
}
