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
    public class TECDrawing : TECObject
    {
        public string Name
        {
            get { return _name; }
            set
            {
                var old = Name;
                _name = value;
                NotifyPropertyChanged(Change.Edit, "Name", this, value, old);
            }
        }
        private string _name;

        public string Description
        {
            get { return _description; }
            set
            {
                var old = Description;
                _description = value;
                NotifyPropertyChanged(Change.Edit, "Description", this, value, old);
            }
        }
        private string _description;

        public double FeetPerPoint
        {
            get { return _feetPerPoint; }
            set
            {
                var old = FeetPerPoint;
                _feetPerPoint = value;
                NotifyPropertyChanged(Change.Edit, "FeetPerPoint", this, value, old);
            }
        }
        private double _feetPerPoint;

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

        public TECDrawing(string name) : base(Guid.NewGuid())
        {
            _name = name;
            _description = "No description";
            _pages = new ObservableCollection<TECPage>();

            Pages.CollectionChanged += (sender, args) => Pages_CollectionChanged(sender, args, "Pages");
        }

        private void Pages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, propertyName, this, item);
                }
            }
        }

        public TECDrawing(string name, string description, Guid guid, ObservableCollection<TECPage> pages) : this(name)
        {
            _description = description;
            _guid = guid;
            _pages = pages;
        }
        public TECDrawing(TECDrawing drawingSource) : this(drawingSource.Name)
        {
            _description = drawingSource.Description;
            _guid = drawingSource.Guid;
            foreach (TECPage page in drawingSource.Pages)
            {
                _pages.Add(new TECPage(page));
            }
        }


        public override object Copy()
        {
            return new TECDrawing(Name, Description, Guid, Pages);
        }
    }
}
