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
                NotifyCombinedChanged(Change.Edit, "RatedCosts", this, value, old);
            }
        }
        #endregion

        public TECElectricalMaterial(Guid guid) : base(guid, CostType.Electrical)
        {
            _ratedCosts = new ObservableCollection<TECCost>();
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

        public CostBatch GetCosts(double length)
        {
            CostBatch costBatch = new CostBatch(Cost, Labor, Type);
            foreach (TECCost ratedCost in RatedCosts)
            {
                costBatch.AddCost(ratedCost);
            }
            costBatch *= length;
            foreach (TECCost assocCost in AssociatedCosts)
            {
                costBatch.AddCost(assocCost);
            }
            return costBatch;
        }
        protected override List<TECObject> saveObjects()
        {
            List<TECObject> saveList = new List<TECObject>();
            saveList.AddRange(base.saveObjects());
            saveList.AddRange(this.RatedCosts);
            return saveList;
        }
        private void RatedCosts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TECCost item in e.NewItems)
                {
                    NotifyCombinedChanged(Change.Add, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECCost item in e.OldItems)
                {
                    NotifyCombinedChanged(Change.Remove, propertyName, this, item);
                }
            }
        }
    }
}
