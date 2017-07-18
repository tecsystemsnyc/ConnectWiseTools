using EstimatingLibrary.Interfaces;
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
                var temp = this.Copy();
                RatedCosts.CollectionChanged -= RatedCosts_CollectionChanged;
                _ratedCosts = value;
                RatedCosts.CollectionChanged += RatedCosts_CollectionChanged;
                NotifyPropertyChanged("RatedCosts", temp, this);
            }
        }
        #endregion

        public TECElectricalMaterial(Guid guid) : base(guid)
        {
            _ratedCosts = new ObservableCollection<TECCost>();
            _type = CostType.Electrical;
            RatedCosts.CollectionChanged += RatedCosts_CollectionChanged;
        }
        public TECElectricalMaterial() : this(Guid.NewGuid()) { }
        public TECElectricalMaterial(TECElectricalMaterial materialSource) : this()
        {
            copyPropertiesFromCost(materialSource);
            var ratedCosts = new ObservableCollection<TECCost>();
            foreach (TECCost cost in materialSource.RatedCosts)
            { ratedCosts.Add(cost as TECCost); }
            RatedCosts.CollectionChanged -= RatedCosts_CollectionChanged;
            _ratedCosts = ratedCosts;
            RatedCosts.CollectionChanged += RatedCosts_CollectionChanged;
        }

        public override object Copy()
        {
            var outType = new TECElectricalMaterial();
            outType._guid = this._guid;
            outType.copyPropertiesFromCost(this);
            var ratedCosts = new ObservableCollection<TECCost>();
            foreach (TECCost cost in RatedCosts)
            { ratedCosts.Add(cost as TECCost); }
            outType._ratedCosts = ratedCosts;
            outType.RatedCosts.CollectionChanged += outType.RatedCosts_CollectionChanged;

            return outType;
        }

        private void RatedCosts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TECCost item in e.NewItems)
                {
                    NotifyPropertyChanged("AddCatalog", this as object, item as object, typeof(ElectricalMaterialComponent), typeof(TECCost));
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECCost item in e.OldItems)
                {
                    NotifyPropertyChanged("RemoveCatalog", this as object, item as object, typeof(ElectricalMaterialComponent), typeof(TECCost));
                }
            }
        }
    }
}
