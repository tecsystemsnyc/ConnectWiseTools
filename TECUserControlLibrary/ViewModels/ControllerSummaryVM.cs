using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class ControllerSummaryVM : ViewModelBase
    {
        #region Properties
        private TECBid _bid;
        public TECBid Bid
        {
            get
            {
                return _bid;
            }
            private set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }

        private ObservableCollection<TECController> _controllers;
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                _controllers = value;
                RaisePropertyChanged("Controllers");
            }
        }

        private Dictionary<Guid, CostSummaryItem> controllerAssCostDictionary;

        private ObservableCollection<CostSummaryItem> _controllerAssCostSummaryItems;
        public ObservableCollection<CostSummaryItem> ControllerAssCostSummaryItems
        {
            get
            {
                return _controllerAssCostSummaryItems;
            }
            set
            {
                _controllerAssCostSummaryItems = value;
                RaisePropertyChanged("ControllerAssCostSummaryItems");
            }
        }

        private double _controllerSubTotal;
        public double ControllerSubTotal
        {
            get { return _controllerSubTotal; }
            set
            {
                _controllerSubTotal = value;
                RaisePropertyChanged("ControllerSubTotal");
                RaisePropertyChanged("TotalControllerCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _controllerAssCostSubTotalCost;
        public double ControllerAssCostSubTotalCost
        {
            get { return _controllerAssCostSubTotalCost; }
            set
            {
                _controllerAssCostSubTotalCost = value;
                RaisePropertyChanged("ControllerAssCostSubTotalCost");
                RaisePropertyChanged("TotalControllerCost");
                RaisePropertyChanged("TotalCost");
            }
        }

        private double _controllerAssCostSubTotalLabor;
        public double ControllerAssCostSubTotalLabor
        {
            get { return _controllerAssCostSubTotalLabor; }
            set
            {
                _controllerAssCostSubTotalLabor = value;
                RaisePropertyChanged("ControllerAssCostSubTotalLabor");
                RaisePropertyChanged("TotalControllerLabor");
                RaisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        public ControllerSummaryVM(TECBid bid)
        {
            reinitialize(bid);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            Bid = bid;

            Controllers = new ObservableCollection<TECController>();

            controllerAssCostDictionary = new Dictionary<Guid, CostSummaryItem>();
            ControllerAssCostSummaryItems = new ObservableCollection<CostSummaryItem>();

            ControllerSubTotal = 0;
            ControllerAssCostSubTotalCost = 0;
            ControllerAssCostSubTotalLabor = 0;

            foreach (TECController controller in bid.Controllers)
            {
                AddController(controller);
            }
            foreach (TECSystem system in bid.Systems)
            {
                foreach(TECSystem sys in system.SystemInstances)
                {
                    foreach(TECController controller in sys.Controllers)
                    {
                        AddController(controller);
                    }
                }
            }
        }

        #region Add/Remove
        public void AddController(TECController controller)
        {
            Controllers.Add(controller);
            ControllerSubTotal += controller.Cost * controller.Manufacturer.Multiplier;
            foreach (TECCost cost in controller.AssociatedCosts)
            {
                AddCostToController(cost);
            }
        }

        public void RemoveController(TECController controller)
        {
            Controllers.Remove(controller);
            ControllerSubTotal -= controller.Cost * controller.Manufacturer.Multiplier;
            foreach (TECCost cost in controller.AssociatedCosts)
            {
                RemoveCostFromController(cost);
            }
        }

        public void AddCostToController(TECCost cost)
        {
            Tuple<double, double> delta = TECMaterialSummaryVM.AddCost(cost, controllerAssCostDictionary, ControllerAssCostSummaryItems);
            ControllerAssCostSubTotalCost += delta.Item1;
            ControllerAssCostSubTotalLabor += delta.Item2;
        }

        public void RemoveCostFromController(TECCost cost)
        {
            Tuple<double, double> delta = TECMaterialSummaryVM.AddCost(cost, controllerAssCostDictionary, ControllerAssCostSummaryItems);
            ControllerAssCostSubTotalCost += delta.Item1;
            ControllerAssCostSubTotalLabor += delta.Item2;
        }
        #endregion
    }
}
