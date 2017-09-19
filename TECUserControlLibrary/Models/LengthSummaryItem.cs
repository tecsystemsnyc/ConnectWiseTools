using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary;
using EstimatingLibrary.Utilities;

namespace TECUserControlLibrary.Models
{
    public class LengthSummaryItem : TECObject
    {
        #region Fields
        private TECElectricalMaterial _material;

        private double _length;

        private double _totalCost;
        private double _totalLabor;
        #endregion

        //Cosntructor
        public LengthSummaryItem(TECElectricalMaterial material, double length) : base(Guid.NewGuid())
        {
            _material = material;
            _length = length;
            updateTotals();
        }

        #region Properties
        public TECElectricalMaterial Material
        {
            get { return _material; }
        }

        public double Length
        {
            get { return _length; }
            private set
            {
                _length = value;
                raisePropertyChanged("Length");
            }
        }

        public double TotalCost
        {
            get { return _totalCost; }
            private set
            {
                _totalCost = value;
                raisePropertyChanged("TotalCost");
            }
        }
        public double TotalLabor
        {
            get { return _totalLabor; }
            set
            {
                double old = _totalLabor;
                _totalLabor = value;
                raisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Methods
        public CostBatch AddLength(double length)
        {
            Length += length;
            return updateTotals();
        }
        public CostBatch RemoveLength(double length)
        {
            Length -= length;
            return updateTotals();
        }

        private CostBatch updateTotals()
        {
            double newCost = (Material.Cost * Length);
            double newLabor = (Material.Labor * Length);

            double deltaCost = newCost - TotalCost;
            double deltaLabor = newLabor - TotalLabor;

            TotalCost = newCost;
            TotalLabor = newLabor;

            return new CostBatch(deltaCost, deltaLabor, CostType.Electrical);
        }
        #endregion
    }
}
