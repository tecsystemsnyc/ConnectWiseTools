using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECConduitType : TECCost, ElectricalMaterialComponent
    {
        #region Properties
        private ObservableCollection<TECCost> _ratedCosts;
        public ObservableCollection<TECCost> RatedCosts
        {
            get
            {
                return _ratedCosts;
            }
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

        public TECConduitType(Guid guid) : base(guid)
        {
            _cost = 0;
            _labor = 0;
            _ratedCosts = new ObservableCollection<TECCost>();
            _type = CostType.Electrical;
            RatedCosts.CollectionChanged += RatedCosts_CollectionChanged;
        }

        public TECConduitType() : this(Guid.NewGuid()) { }
        public TECConduitType(TECConduitType conduitSource) : this()
        {
            copyPropertiesFromCost(conduitSource);
            _labor = conduitSource.Labor;
            _ratedCosts = conduitSource._ratedCosts;
        }
        public override object Copy()
        {
            var outType = new TECConduitType();
            outType.copyPropertiesFromCost(this);
            outType._guid = this._guid;
            outType._labor = this._labor;
            outType._ratedCosts = this._ratedCosts;

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
                    NotifyPropertyChanged("Add", this, item, typeof(ElectricalMaterialComponent), typeof(TECCost));
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECCost item in e.NewItems)
                {
                    NotifyPropertyChanged("Remove", this, item, typeof(ElectricalMaterialComponent), typeof(TECCost));
                }
            }
        }
    }
}
