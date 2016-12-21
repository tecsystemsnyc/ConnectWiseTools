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
        public Guid Guid { get; set; }
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
                var temp = this.Copy();
                PageScope.CollectionChanged -= collectionChanged;
                _pageScope = value;
                NotifyPropertyChanged("PageScope", temp, this);
                PageScope.CollectionChanged += collectionChanged;
            }
        }
        private ObservableCollection<TECVisualScope> _pageScope;
        public ObservableCollection<TECVisualConnection> Connections
        {
            get { return _connections; }
            set
            {
                var temp = this.Copy();
                Connections.CollectionChanged -= collectionChanged;
                _connections = value;
                NotifyPropertyChanged("Connections", temp, this);
                Connections.CollectionChanged += collectionChanged;
            }
        }
        private ObservableCollection<TECVisualConnection> _connections;
        #endregion

        public TECPage(string path, int pageNum)
        {
            PageNum = pageNum;
            Guid = Guid.NewGuid();
            _path = path;
            _pageScope = new ObservableCollection<TECVisualScope>();
            _connections = new ObservableCollection<TECVisualConnection>();
            PageScope.CollectionChanged += collectionChanged;
            Connections.CollectionChanged += collectionChanged;
        }
        
        public TECPage(int pageNum, Guid guid)
        {
            PageNum = pageNum;
            Guid = guid;
            _path = null;
            _pageScope = new ObservableCollection<TECVisualScope>();
            _connections = new ObservableCollection<TECVisualConnection>();
            PageScope.CollectionChanged += collectionChanged;
            Connections.CollectionChanged += collectionChanged;
        }

        public TECPage(TECPage page)
        {
            Guid = page.Guid;
            _path = page.Path;
            _pageScope = new ObservableCollection<TECVisualScope>();
            _connections = new ObservableCollection<TECVisualConnection>();
            foreach (TECVisualScope vs in page.PageScope)
            {
                _pageScope.Add(new TECVisualScope(vs));
            }
            PageScope.CollectionChanged += collectionChanged;
            Connections.CollectionChanged += collectionChanged;
        }

        public override Object Copy()
        {
            TECPage outPage = new TECPage(this);
            return outPage;
        }

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
            }
        }

    }
}
