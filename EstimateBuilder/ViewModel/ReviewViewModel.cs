using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ReviewViewModel : ViewModelBase
    {
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
                var oldProfit = Bid.Parameters.Profit;
                var oldPrice = Bid.TotalPrice;
                Bid.Parameters.Profit = oldProfit * oldPrice / value;
                RaisePropertyChanged("UserPrice");
            }
        }

        public ReviewViewModel()
        {
            Bid = new TECBid();
            Bid.PropertyChanged += Bid_PropertyChanged;
        }

        private void Bid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("Margin");
        }

        public void Refresh(TECBid bid)
        {
            Bid = bid;
            Bid.PropertyChanged += Bid_PropertyChanged;
        }
    }
}