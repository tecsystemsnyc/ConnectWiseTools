using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECPage : TECObject
    {
        #region Poperties
        private string _path;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                RaisePropertyChanged("Path");
            }
        }
        public int PageNum { get; set; }
        public ObservableCollection<TECVisualScope> PageScope
        {
            get { return _pageScope; }
            set
            {
                var old = PageScope;
                PageScope.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "PageScope");
                _pageScope = value;
                NotifyPropertyChanged(Change.Edit, "PageScope", this, value, old);
                PageScope.CollectionChanged += (sender, args) => collectionChanged(sender, args, "PageScope");
            }
        }
        private ObservableCollection<TECVisualScope> _pageScope;
        public ObservableCollection<TECVisualConnection> Connections
        {
            get { return _connections; }
            set
            {
                var old = Connections;
                Connections.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Connections");
                _connections = value;
                NotifyPropertyChanged(Change.Edit, "Connections", this, value, old);
                Connections.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Connections");
            }
        }
        private ObservableCollection<TECVisualConnection> _connections;
        #endregion

        public TECPage(Guid guid) : base(guid)
        {
            _guid = guid;
            PageNum = 0;
            _path = "";
            _pageScope = new ObservableCollection<TECVisualScope>();
            _connections = new ObservableCollection<TECVisualConnection>();
            PageScope.CollectionChanged += (sender, args) => collectionChanged(sender, args, "PageScope");
            Connections.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Connections");
        }

        public TECPage() : this(Guid.NewGuid()) { }

        public TECPage(TECPage page) : base(page.Guid)
        {
            _path = page.Path;
            _pageScope = new ObservableCollection<TECVisualScope>();
            _connections = new ObservableCollection<TECVisualConnection>();
            foreach (TECVisualScope vs in page.PageScope)
            {
                _pageScope.Add(vs.Copy() as TECVisualScope);
            }
            PageScope.CollectionChanged += (sender, args) => collectionChanged(sender, args, "PageScope");
            Connections.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Connections");
        }

        public override Object Copy()
        {
            TECPage outPage = new TECPage(this);
            return outPage;
        }

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyChanged)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, propertyChanged, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, propertyChanged, this, item);
                }
            }
        }

    }
}
