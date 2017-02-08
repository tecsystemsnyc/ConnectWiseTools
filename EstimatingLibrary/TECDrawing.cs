using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECDrawing : TECObject
    {
        public string Name
        {
            get { return _name; }
            set
            {
                var temp = this.Copy();
                _name = value;
                NotifyPropertyChanged("Name", temp, this);
            }
        }
        private string _name;

        public string Description
        {
            get { return _description; }
            set
            {
                var temp = this.Copy();
                _description = value;
                NotifyPropertyChanged("Description", temp, this);
            }
        }
        private string _description;

        public double FeetPerPoint
        {
            get { return _feetPerPoint; }
            set
            {
                var temp = Copy();
                _feetPerPoint = value;
                NotifyPropertyChanged("FeetPerPoint", temp, this);
            }
        }
        private double _feetPerPoint;

        public Guid Guid;

        public ObservableCollection<TECPage> Pages
        {
            get { return _pages; }
            set
            {
                _pages = value;
                RaisePropertyChanged("Pages");
            }
        }
        private ObservableCollection<TECPage> _pages;

        public TECDrawing(string name)
        {
            _name = name;
            _description = "No description";
            Guid = Guid.NewGuid();
            _pages = new ObservableCollection<TECPage>();

            Pages.CollectionChanged += Pages_CollectionChanged;
        }

        private void Pages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

        public TECDrawing(string name, string description, Guid guid, ObservableCollection<TECPage> pages) : this(name)
        {
            _description = description;
            Guid = guid;
            _pages = pages;
        }

        public override object Copy()
        {
            return new TECDrawing(Name, Description, Guid, Pages);
        }
    }
}
