using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class CostBatchPropertiesItem : ViewModelBase
    {
        private INotifyCostChanged item; 

        public List<TECCost> AllCosts
        {
            get { return item.CostBatch.AllCosts; }
        }

        public CostBatchPropertiesItem(INotifyCostChanged item)
        {
            this.item = item;
            item.CostChanged += item_CostChanged;
        }

        private void item_CostChanged(EstimatingLibrary.Utilities.CostBatch obj)
        {
            RaisePropertyChanged("AllCosts");
        }
    }
}
