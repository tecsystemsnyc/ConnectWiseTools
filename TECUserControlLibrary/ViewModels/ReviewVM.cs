using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ReviewVM : ViewModelBase
    {
        private TECEstimator _estimate;
        public TECEstimator Estimate
        {
            get { return _estimate; }
            set
            {
                _estimate = value;
                RaisePropertyChanged("Estimate");
            }
        }

        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }

        private double _userPrice;
        public double UserPrice
        {
            get { return _userPrice; }
            set
            {
                _userPrice = value;
                var newProfit = (value - Estimate.SubcontractorSubtotal) / (Estimate.TECCost * (1 + Bid.Parameters.Overhead / 100)) - 1;
                Bid.Parameters.Profit = newProfit * 100;
                RaisePropertyChanged("UserPrice");
            }
        }
        
        public ReviewVM()
        {
            _bid = new TECBid();
            _estimate = new TECEstimator(_bid, new EstimatingLibrary.Utilities.ChangeWatcher(_bid));
        }
        
        public void Refresh(TECEstimator estimate, TECBid bid)
        {
            Estimate = estimate;
            Bid = bid;
        }
         
        
    }

    //class CostContainer
    //{
    //    CostContainer(string name, )
    //}
}