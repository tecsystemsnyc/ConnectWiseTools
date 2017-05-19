using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimateBuilder.Model
{
    public class PanelTypeSummaryItem : TECObject
    {
        private TECPanelType _panelType;
        public TECPanelType PanelType
        {
            get { return _panelType; }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                RaisePropertyChanged("Quantity");
                updateTotal();
            }
        }

        private double _total;
        public double Total
        {
            get
            {
                return _total;
            }
            set
            {
                double old = _total;
                _total = value;
                NotifyPropertyChanged("Total", old, _total);
            }
        }

        public PanelTypeSummaryItem(TECPanelType panelType)
        {
            _panelType = panelType;
            _quantity = 1;
            panelType.PropertyChanged += PanelType_PropertyChanged;
            updateTotal();
        }

        private void updateTotal()
        {
            Total = (PanelType.Cost * Quantity);
        }

        private void PanelType_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost")
            {
                updateTotal();
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
