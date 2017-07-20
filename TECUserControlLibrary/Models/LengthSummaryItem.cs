using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary;

namespace TECUserControlLibrary.Models
{
    public class LengthSummaryItem : TECObject
    {
        private ElectricalMaterialComponent _material;
        public ElectricalMaterialComponent Material
        {
            get { return _material; }
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
            get { return _totalLabor; }
            set
            {
                double old = _totalLabor;
                _totalLabor = value;
                NotifyPropertyChanged("TotalLabor", old, _totalLabor);
            }
        }

        public LengthSummaryItem(ElectricalMaterialComponent material) : base(Guid.NewGuid())
        {
            _material = material;
            _length = 0;
            material.PropertyChanged += Material_PropertyChanged;
            updateTotals();
        }

        private void updateTotals()
        {
            TotalCost = (Material.Cost * Length);
            TotalLabor = (Material.Labor * Length);
        }

        private void Material_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
