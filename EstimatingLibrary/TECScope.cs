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

    public abstract class TECScope : TECObject
    {
        #region Properties

        protected string _name;
        protected string _description;

        protected ObservableCollection<TECLabeled> _tags;
        protected ObservableCollection<TECCost> _associatedCosts;

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

        public ObservableCollection<TECLabeled> Tags
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
        public ObservableCollection<TECCost> AssociatedCosts
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
        
        #endregion

        #region Constructors
        public TECScope(Guid guid) : base(guid)
        {
            _name = "";
            _description = "";
            _guid = guid;

            _tags = new ObservableCollection<TECLabeled>();
            _associatedCosts = new ObservableCollection<TECCost>();
            Tags.CollectionChanged += collectionChanged;
            AssociatedCosts.CollectionChanged += AssociatedCosts_CollectionChanged;
        }

        #endregion 

        #region Methods
        protected void copyPropertiesFromScope(TECScope scope)
        {
            _name = scope.Name;
            _description = scope.Description;
            var tags = new ObservableCollection<TECLabeled>();
            foreach (TECLabeled tag in scope.Tags)
            { tags.Add(tag as TECLabeled); }
            _tags = tags;
            var associatedCosts = new ObservableCollection<TECCost>();
            foreach (TECCost cost in scope.AssociatedCosts)
            { associatedCosts.Add(cost as TECCost); }
            _associatedCosts = associatedCosts;
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
                    var assCost = item as TECCost;
                    assCost.PropertyChanged += AssCost_PropertyChanged;
                    var old = this.Copy() as TECScope;
                    old.AssociatedCosts.Remove(item as TECCost);
                    NotifyPropertyChanged("CostComponentChanged", old as object, this as object);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    var assCost = item as TECCost;
                    assCost.PropertyChanged -= AssCost_PropertyChanged;
                    var old = this.Copy() as TECScope;
                    old.AssociatedCosts.Add(item as TECCost);
                    NotifyPropertyChanged("CostComponentChanged", old as object, this as object);
                }
            }
        }

        private void AssCost_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e is PropertyChangedExtendedEventArgs)
            {
                var args = e as PropertyChangedExtendedEventArgs;
                if ((args.PropertyName == "Cost") || (args.PropertyName == "Labor"))
                {
                    var old = this.Copy() as TECScope;
                    old.AssociatedCosts.Remove(args.NewValue as TECCost);
                    old.AssociatedCosts.Add(args.OldValue as TECCost);
                    NotifyPropertyChanged("CostComponentChanged", old, this);
                }
            }
        }
        #endregion Methods
    }
}
