using EstimatingLibrary.Interfaces;
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
                var old = Name;
                _name = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged(Change.Edit, "Name", this, value, old);
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                var old = Description;
                _description = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged(Change.Edit, "Description", this, value, old);

            }
        }

        public ObservableCollection<TECLabeled> Tags
        {
            get { return _tags; }
            set
            {
                var old = Tags;
                Tags.CollectionChanged -= (sender, args) => collectionChanged(sender, args, "Tags");
                _tags = value;
                NotifyPropertyChanged(Change.Edit, "Tags", this, value, old);
                Tags.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Tags");
            }
        }
        public ObservableCollection<TECCost> AssociatedCosts
        {
            get { return _associatedCosts; }
            set
            {
                var old = AssociatedCosts;
                AssociatedCosts.CollectionChanged -= AssociatedCosts_CollectionChanged;
                _associatedCosts = value;
                NotifyPropertyChanged(Change.Edit, "AssociatedCosts", this, value, old);
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
            Tags.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Tags");
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
        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
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

        private void AssociatedCosts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            collectionChanged(sender, e, "AssociatedCosts");
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    var assCost = item as TECCost;
                    assCost.PropertyChanged += AssCost_PropertyChanged;
                    var old = this.Copy() as TECScope;
                    old.AssociatedCosts.Remove(item as TECCost);
                    //NotifyPropertyChanged("CostComponentChanged", old as object, this as object);
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
                    //NotifyPropertyChanged("CostComponentChanged", old as object, this as object);
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
                    //var old = this.Copy() as TECScope;
                    //old.AssociatedCosts.Remove(args.NewValue as TECCost);
                    //old.AssociatedCosts.Add(args.OldValue as TECCost);
                    //NotifyPropertyChanged("CostComponentChanged", old, this);
                }
            }
        }
        #endregion Methods
    }
}
