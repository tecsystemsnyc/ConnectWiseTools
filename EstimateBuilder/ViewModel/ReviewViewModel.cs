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

        private double _marginSliderValue;
        //public double MarginSliderValue
        //{
        //    get { return _marginSliderValue; }
        //    set
        //    {
        //        _marginSliderValue = value;
        //        RaisePropertyChanged("MarginSliderValue");
        //    }
        //}
        public double MarginSliderValue
        {
            get { return Bid.Margin; }
            set
            {
                var profit = (value / 100) / Bid.TotalCost - Bid.Parameters.Overhead / 100;
                Bid.Parameters.Profit = profit * 100; 
                RaisePropertyChanged("MarginSliderValue");
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