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
                _ratedCosts = value;
                RaisePropertyChanged("RatedCosts");
            }
        }
        #endregion

        public TECConduitType(Guid guid) : base(guid)
        {
            _cost = 0;
            _labor = 0;
        }
        public TECConduitType() : this(Guid.NewGuid()) { }
        public TECConduitType(TECConduitType conduitSource) : this()
        {
            copyPropertiesFromCost(conduitSource);
            _labor = conduitSource.Labor;
        }
        public override object Copy()
        {
            var outType = new TECConduitType();
            outType.copyPropertiesFromCost(this);
            outType._guid = this._guid;
            outType._labor = this._labor;

            return outType;
        }

        public override object DragDropCopy()
        {
            throw new NotImplementedException();
        }
    }
}
