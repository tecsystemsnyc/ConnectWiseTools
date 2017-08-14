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

    public abstract class TECScope : TECObject, INotifyCostChanged
    {
        #region Properties

        protected string _name;
        protected string _description;

        protected ObservableCollection<TECLabeled> _tags;
        protected ObservableCollection<TECCost> _associatedCosts;

        public event Action<List<TECCost>> CostChanged;

        public string Name
        {
            get { return _name; }
            set
            {
                var old = Name;
                _name = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyCombinedChanged(Change.Edit, "Name", this, value, old);
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
                NotifyCombinedChanged(Change.Edit, "Description", this, value, old);

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
                NotifyCombinedChanged(Change.Edit, "Tags", this, value, old);
                Tags.CollectionChanged += (sender, args) => collectionChanged(sender, args, "Tags");
            }
        }
        public ObservableCollection<TECCost> AssociatedCosts
        {
            get { return _associatedCosts; }
            set
            {
                var old = AssociatedCosts;
                _associatedCosts = value;
                NotifyCombinedChanged(Change.Edit, "AssociatedCosts", this, value, old);
            }
        }

        virtual public List<TECCost> Costs { get { return this.AssociatedCosts.ToList(); } }

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
                List<TECCost> costs = new List<TECCost>();
                foreach (object item in e.NewItems)
                {
                    if(item is TECCost cost) { costs.AddRange(costs); }
                    NotifyCombinedChanged(Change.Add, propertyName, this, item);
                }
                NotifyCostChanged(costs);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                List<TECCost> costs = new List<TECCost>();
                foreach (object item in e.OldItems)
                {
                    if (item is TECCost cost) { costs.AddRange(costs); }
                    NotifyCombinedChanged(Change.Remove, propertyName, this, item);
                }
                NotifyCostChanged(CostHelper.NegativeCosts(costs));
            }
        }
        
        public void NotifyCostChanged(List<TECCost> costs)
        {
            CostChanged?.Invoke(costs);
        }

        #endregion Methods
    }
}
