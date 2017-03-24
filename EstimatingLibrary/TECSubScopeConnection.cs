using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    class TECSubScopeConnection : TECConnection
    {
        private ObservableCollection<TECSubScope> _scope;
        public ObservableCollection<TECSubScope> Scope
        {
            get { return _scope; }
            set
            {
                var temp = this.Copy();
                Scope.CollectionChanged -= Scope_CollectionChanged;
                _scope = value;
                NotifyPropertyChanged("Scope", temp, this);
                Scope.CollectionChanged += Scope_CollectionChanged;
            }
        }

        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get { return getConnectionTypes(); }

        }
        public ObservableCollection<IOType> IOTypes
        {
            get { return getIOTypes(); }
        }

        #region Constructors
        public TECSubScopeConnection(Guid guid) : base(guid)
        {
            _scope = new ObservableCollection<TECSubScope>();
            Scope.CollectionChanged += Scope_CollectionChanged;
        }
        public TECSubScopeConnection() : base (Guid.NewGuid()) { }
        public TECSubScopeConnection(TECConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, guidDictionary)
        {
            foreach (TECScope scope in connectionSource.Scope)
            {
                 _scope.Add(new TECSubScope((scope as TECSubScope), guidDictionary));
            }
        }
        #endregion Constructors
        public override Object Copy()
        {
            TECSubScopeConnection connection = new TECSubScopeConnection(this);
            connection._guid = this._guid;
            return connection;
        }
        
        private ObservableCollection<TECConnectionType> getConnectionTypes()
        {
            var outConnectionTypes = new ObservableCollection<TECConnectionType>();

            foreach (TECSubScope scope in Scope)
            {
                var sub = scope as TECSubScope;
                foreach (TECDevice dev in sub.Devices)
                {
                    outConnectionTypes.Add(dev.ConnectionType);
                }
            }
            return outConnectionTypes;
        }
        private ObservableCollection<IOType> getIOTypes()
        {
            var outIOTypes = new ObservableCollection<IOType>();

            foreach (TECScope scope in Scope)
            {
                var sub = scope as TECSubScope;
                foreach (TECDevice dev in sub.Devices)
                {
                    outIOTypes.Add(dev.IOType);
                }
               
            }
            return outIOTypes;
        }

        private void Scope_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECController)
                    {
                        (item as TECController).Connections.Add(this);
                    }
                    else if (item is TECSubScope)
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
                    if (item is TECController)
                    {
                        (item as TECController).Connections.Remove(this);
                    }
                    else if (item is TECSubScope)
                    {
                        (item as TECSubScope).Connection = null;
                    }
                    NotifyPropertyChanged("RemoveRelationship", this, item);
                }
            }
        }

    }
}
