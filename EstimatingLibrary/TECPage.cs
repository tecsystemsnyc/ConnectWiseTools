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
                PageScope.CollectionChanged -= PageScope_CollectionChanged;
                _pageScope = value;
                NotifyPropertyChanged("PageScope", temp, this);
                PageScope.CollectionChanged += PageScope_CollectionChanged;
            }
        }
        private ObservableCollection<TECVisualScope> _pageScope;
        #endregion
        public TECPage(string path, int pageNum)
        {
            PageNum = pageNum;
            Guid = Guid.NewGuid();
            _path = path;
            _pageScope = new ObservableCollection<TECVisualScope>();
            PageScope.CollectionChanged += PageScope_CollectionChanged;
        }
        
        public TECPage(int pageNum, Guid guid)
        {
            PageNum = pageNum;
            Guid = guid;
            _path = null;
            _pageScope = new ObservableCollection<TECVisualScope>();
            PageScope.CollectionChanged += PageScope_CollectionChanged;
        }

        public TECPage(TECPage page)
        {
            Guid = page.Guid;
            _path = page.Path;
            _pageScope = new ObservableCollection<TECVisualScope>();
            foreach (TECVisualScope vs in page.PageScope)
            {
                _pageScope.Add(new TECVisualScope(vs));
            }
            _pageScope.CollectionChanged += PageScope_CollectionChanged;
        }

        public override Object Copy()
        {
            TECPage outPage = new TECPage(this);
            return outPage;
        }

        private void PageScope_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("PageScope collection changed.");
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
