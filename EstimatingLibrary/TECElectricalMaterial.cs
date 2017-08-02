using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECElectricalMaterial : TECCost
    {
        #region Properties
        private ObservableCollection<TECCost> _ratedCosts;
        public ObservableCollection<TECCost> RatedCosts
        {
            get { return _ratedCosts; }
            set
            {
                var old = RatedCosts;
                RatedCosts.CollectionChanged -= (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
                _ratedCosts = value;
                RatedCosts.CollectionChanged += (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
                NotifyPropertyChanged(Change.Edit, "RatedCosts", this, value, old);
            }
        }
        #endregion

        public TECElectricalMaterial(Guid guid) : base(guid)
        {
            _ratedCosts = new ObservableCollection<TECCost>();
            _type = CostType.Electrical;
            RatedCosts.CollectionChanged += (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
        }
        public TECElectricalMaterial() : this(Guid.NewGuid()) { }
        public TECElectricalMaterial(TECElectricalMaterial materialSource) : this()
        {
            copyPropertiesFromCost(materialSource);
            var ratedCosts = new ObservableCollection<TECCost>();
            foreach (TECCost cost in materialSource.RatedCosts)
            { ratedCosts.Add(cost as TECCost); }
            RatedCosts.CollectionChanged -= (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
            _ratedCosts = ratedCosts;
            RatedCosts.CollectionChanged += (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
        }
        
        private void RatedCosts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TECCost item in e.NewItems)
                {
                    NotifyPropertyChanged(Change.Add, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECCost item in e.OldItems)
                {
                    NotifyPropertyChanged(Change.Remove, propertyName, this, item);
                }
            }
        }
    }
}
