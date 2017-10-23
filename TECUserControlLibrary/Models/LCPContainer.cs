using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary.Utilities;

namespace TECUserControlLibrary.Models
{
    public class LCPContainer : ViewModelBase, INotifyCostChanged, INotifyPointChanged, IRelatable
    {
        private List<TECController> controllers;
        private List<TECSubScope> subScope;

        public CostBatch CostBatch => throw new NotImplementedException();

        public int PointNumber => throw new NotImplementedException();

        public SaveableMap PropertyObjects => throw new NotImplementedException();

        public SaveableMap LinkedObjects => throw new NotImplementedException();

        public event Action<CostBatch> CostChanged;
        public event Action<int> PointChanged;

        public LCPContainer(TECPanel panel)
        {
            foreach(TECController controller in panel.Controllers)
            {

            }
        }

    }
}
