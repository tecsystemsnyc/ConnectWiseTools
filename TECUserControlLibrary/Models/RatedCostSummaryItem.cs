using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class RatedCostSummaryItem : TECObject
    {
        private TECCost _ratedCost;
        public TECCost RatedCost
        {
            get { return _ratedCost; }
        }

        private double _length;
        public double Length
        {
            get { return _length; }
            set
            {
                _length = value;
                RaisePropertyChanged("Length");
                updateTotals();
            }
        }

        private double _totalCost;
        public double TotalCost
        {
            get { return _totalCost; }
            set
            {
                double old = _totalCost;
                _totalCost = value;
                NotifyPropertyChanged("TotalCost", old, _totalCost);
            }
        }

        private double _totalLabor;
        public double TotalLabor
        {
            get
            {
                return _totalLabor;
            }
            set
            {
                double old = _totalLabor;
                _totalLabor = value;
                NotifyPropertyChanged("TotalLabor", old, _totalLabor);
            }
        }

        public RatedCostSummaryItem(TECCost ratedCost, double length)
        {
            _ratedCost = ratedCost;
            _length = length;
            RatedCost.PropertyChanged += RatedCost_PropertyChanged;
            updateTotals();
        }

        private void updateTotals()
        {
            TotalCost = (RatedCost.Cost * Length);
            TotalLabor = (RatedCost.Labor * Length);
        }

        private void RatedCost_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost" || e.PropertyName == "Labor")
            {
                updateTotals();
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
