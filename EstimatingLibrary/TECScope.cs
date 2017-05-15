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

    public abstract class TECScope : TECObject, GuidObject
    {
        #region Properties

        protected string _name;
        protected string _description;
        protected Guid _guid;
        protected int _quantity;
        protected TECLocation _location;

        protected ObservableCollection<TECTag> _tags;
        protected ObservableCollection<TECAssociatedCost> _associatedCosts;

        public string Name
        {
            get { return _name; }
            set
            {
                var temp = this.Copy();
                _name = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Name", temp, this);
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                var temp = this.Copy();
                _description = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Description", temp, this);

            }
        }
        public Guid Guid
        {
            get { return _guid; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                var temp = this.Copy();
                _quantity = value;
                NotifyPropertyChanged("Quantity", temp, this);

            }
        }
        public ObservableCollection<TECTag> Tags
        {
            get { return _tags; }
            set
            {
                var temp = this.Copy();
                Tags.CollectionChanged -= collectionChanged;
                _tags = value;
                NotifyPropertyChanged("Tags", temp, this);
                Tags.CollectionChanged += collectionChanged;
            }
        }
        public ObservableCollection<TECAssociatedCost> AssociatedCosts
        {
            get { return _associatedCosts; }
            set
            {
                var temp = this.Copy();
                AssociatedCosts.CollectionChanged -= AssociatedCosts_CollectionChanged;
                _associatedCosts = value;
                NotifyPropertyChanged("AssociatedCosts", temp, this);
                AssociatedCosts.CollectionChanged += AssociatedCosts_CollectionChanged;
            }
        }

        public TECLocation Location
        {
            get { return _location; }
            set
            {
                var oldNew = Tuple.Create<Object, Object>(_location, value);
                var temp = Copy();
                _location = value;
                NotifyPropertyChanged("Location", temp, this);
                temp = Copy();
                NotifyPropertyChanged("ObjectPropertyChanged", temp, oldNew, typeof(TECScope), typeof(TECLocation));
            }
        }

        //public abstract double MaterialCost { get; }
        //public abstract double LaborCost { get; }

        public void SetLocationFromParent(TECLocation location)
        {
            var oldNew = Tuple.Create<Object, Object>(_location, location);
            _location = location;
            RaisePropertyChanged("Location");
            var temp = Copy();
            NotifyPropertyChanged("ObjectPropertyChanged", temp, oldNew, typeof(TECScope), typeof(TECLocation));
        }

        #endregion //Properties

        #region Constructors
        public TECScope(Guid guid)
        {
            _name = "";
            _description = "";
            _guid = guid;

            _quantity = 1;
            _tags = new ObservableCollection<TECTag>();
            _associatedCosts = new ObservableCollection<TECAssociatedCost>();
            Tags.CollectionChanged += collectionChanged;
            AssociatedCosts.CollectionChanged += collectionChanged;
        }

        abstract public Object DragDropCopy();
        #endregion //Constructors

        #region Methods
        protected void copyPropertiesFromScope(TECScope scope)
        {
            _name = scope.Name;
            _description = scope.Description;
            _quantity = scope.Quantity;
            if (scope.Location != null)
            { _location = scope.Location.Copy() as TECLocation; }
            foreach (TECTag tag in scope.Tags)
            { _tags.Add(tag.Copy() as TECTag); }
            foreach (TECAssociatedCost cost in scope.AssociatedCosts)
            { _associatedCosts.Add(cost.Copy() as TECAssociatedCost); }
        }
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("AddCatalog", this, item, typeof(TECScope), item.GetType());
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("RemoveCatalog", this, item, typeof(TECScope), item.GetType());
                }
            }
        }

        private void AssociatedCosts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            collectionChanged(sender, e);
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    var assCost = item as TECAssociatedCost;
                    assCost.PropertyChanged += AssCost_PropertyChanged;
                    var old = this.Copy() as TECScope;
                    old.AssociatedCosts.Remove(item as TECAssociatedCost);
                    NotifyPropertyChanged("CostComponentChanged", old, this);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    var assCost = item as TECAssociatedCost;
                    assCost.PropertyChanged -= AssCost_PropertyChanged;
                    var old = this.Copy() as TECScope;
                    old.AssociatedCosts.Add(item as TECAssociatedCost);
                    NotifyPropertyChanged("CostComponentChanged", old, this);
                }
            }
        }

        private void AssCost_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs<object>)
            {
                var args = e as PropertyChangedExtendedEventArgs<object>;
                if ((args.PropertyName == "Cost") || (args.PropertyName == "Labor"))
                {
                    var old = this.Copy() as TECScope;
                    old.AssociatedCosts.Remove(args.NewValue as TECAssociatedCost);
                    old.AssociatedCosts.Add(args.OldValue as TECAssociatedCost);
                    NotifyPropertyChanged("CostComponentChanged", old, this);
                }
            }
        }
        #endregion Methods
    }
}
