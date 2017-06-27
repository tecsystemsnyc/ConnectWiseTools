using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECConnectionType : TECCost, ElectricalMaterialComponent
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

        public TECConnectionType(Guid guid) : base(guid)
        {
            _cost = 0;
            _labor = 0;
            _ratedCosts = new ObservableCollection<TECCost>();
            _type = CostType.Electrical;
            RatedCosts.CollectionChanged += RatedCosts_CollectionChanged;
        }
        public TECConnectionType() : this(Guid.NewGuid()) { }
        public TECConnectionType(TECConnectionType connectionTypeSource) : this()
        {
            copyPropertiesFromCost(connectionTypeSource);
            var ratedCosts = new ObservableCollection<TECCost>();
            foreach (TECCost cost in connectionTypeSource.RatedCosts)
            { ratedCosts.Add(cost as TECCost); }
            RatedCosts.CollectionChanged -= RatedCosts_CollectionChanged;
            _ratedCosts = ratedCosts;
            RatedCosts.CollectionChanged += RatedCosts_CollectionChanged;
        }

        public override object Copy()
        {
            var outType = new TECConnectionType();
            outType._guid = this._guid;
            outType.copyPropertiesFromCost(this);
            var ratedCosts = new ObservableCollection<TECCost>();
            foreach (TECCost cost in RatedCosts)
            { ratedCosts.Add(cost as TECCost); }
            outType._ratedCosts = ratedCosts;
            outType.RatedCosts.CollectionChanged += outType.RatedCosts_CollectionChanged;

            return outType;
        }
        public override object DragDropCopy()
        {
            throw new NotImplementedException();
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
                foreach (TECCost item in e.NewItems)
                {
                    NotifyPropertyChanged("RemoveCatalog", this as object, item as object, typeof(ElectricalMaterialComponent), typeof(TECCost));
                }
            }
        }
    }
}
