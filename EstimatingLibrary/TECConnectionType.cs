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

        public TECConnectionType(Guid guid) : base(guid)
        {
            _cost = 0;
            _labor = 0;
            _ratedCosts = new ObservableCollection<TECCost>();
            RatedCosts.CollectionChanged += RatedCosts_CollectionChanged;
        }
        public TECConnectionType() : this(Guid.NewGuid()) { }
        public TECConnectionType(TECConnectionType connectionTypeSource) : this()
        {
            copyPropertiesFromScope(connectionTypeSource);
            _cost = connectionTypeSource.Cost;
            _labor = connectionTypeSource.Labor;
            _ratedCosts = connectionTypeSource._ratedCosts;
        }

        public override object Copy()
        {
            var outType = new TECConnectionType();
            outType._guid = this._guid;
            outType.copyPropertiesFromScope(this);
            outType._cost = this._cost;
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
