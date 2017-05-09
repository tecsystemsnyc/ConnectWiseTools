using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary;

namespace EstimateBuilder.Model
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
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
