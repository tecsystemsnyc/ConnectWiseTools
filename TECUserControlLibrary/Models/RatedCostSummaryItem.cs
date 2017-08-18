using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class RatedCostSummaryItem : TECObject
    {
        #region Fields
        private TECCost _ratedCost;

        private double _length;

        private double _totalCost;
        private double _totalLabor;
        #endregion

        //Constructor
        public RatedCostSummaryItem(TECCost ratedCost, double length) : base(Guid.NewGuid())
        {
            _ratedCost = ratedCost;
            _length = length;
            updateTotals();
        }

        #region Properties
        public TECCost RatedCost
        {
            get { return _ratedCost; }
        }

        public double Length
        {
            get { return _length; }
            private set
            {
                _length = value;
                RaisePropertyChanged("Length");
            }
        }

        public double TotalCost
        {
            get { return _totalCost; }
            private set
            {
                _totalCost = value;
                RaisePropertyChanged("TotalCost");
            }
        }
        public double TotalLabor
        {
            get { return _totalLabor; }
            private set
            {
                _totalLabor = value;
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Methods
        public CostObject AddLength(double length)
        {
            Length += length;
            return updateTotals();
        }
        public CostObject RemoveLength(double length)
        {
            Length -= length;
            return updateTotals();
        }

        private CostObject updateTotals()
        {
            double newCost = (RatedCost.Cost * Length);
            double newLabor = (RatedCost.Labor * Length);

            double deltaCost = newCost - TotalCost;
            double deltaLabor = newLabor - TotalLabor;

            TotalCost = newCost;
            TotalLabor = newLabor;

            return new CostObject(deltaCost, deltaLabor);
        }
        #endregion
    }
}
