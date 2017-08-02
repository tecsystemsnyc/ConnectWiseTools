using EstimatingLibrary;
using EstimatingLibrary.Utilities;
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
                NotifyPropertyChanged(Change.Edit, "Total", this, _total, old);
            }
        }

        private double _laborTotal;
        public double LaborTotal
        {
            get
            {
                return _laborTotal;
            }
            set
            {
                double old = _laborTotal;
                _laborTotal = value;
                NotifyPropertyChanged(Change.Edit, "LaborTotal", this, _laborTotal, old);
            }
        }

        public PanelTypeSummaryItem(TECPanelType panelType) : base(Guid.NewGuid())
        {
            _panelType = panelType;
            _quantity = 1;
            panelType.PropertyChanged += PanelType_PropertyChanged;
            updateTotal();
        }

        private void updateTotal()
        {
            Total = (PanelType.Cost * Quantity);
            LaborTotal = (PanelType.Labor * Quantity);
        }

        private void PanelType_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Cost" || e.PropertyName == "Labor")
            {
                updateTotal();
            }
        }
    }
}
