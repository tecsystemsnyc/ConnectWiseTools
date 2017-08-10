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
                if(_estimate != null)
                {
                    Estimate.PropertyChanged -= Bid_PropertyChanged;
                }
                _estimate = value;
                Estimate.PropertyChanged += Bid_PropertyChanged;
                RaisePropertyChanged("Estimate");
            }
        }

        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                if (_bid != null)
                {
                    Bid.PropertyChanged -= Bid_PropertyChanged;
                }
                _bid = value;
                Bid.PropertyChanged += Bid_PropertyChanged;
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

        #region Margins

        public double MarginM8
        {
            get { return Estimate.Margin - 8; }
        }
        public double MarginM6
        {
            get { return Estimate.Margin - 6; }
        }
        public double MarginM4
        {
            get { return Estimate.Margin - 4; }
        }
        public double MarginM2
        {
            get { return Estimate.Margin - 2; }
        }
        public double MarginP2
        {
            get { return Estimate.Margin + 2; }
        }
        public double MarginP4
        {
            get { return Estimate.Margin + 4; }
        }
        public double MarginP6
        {
            get { return Estimate.Margin + 6; }
        }
        public double MarginP8
        {
            get { return Estimate.Margin + 8; }
        }

        #endregion
        #region Prices
        public double TotalPriceM8
        {
            get { return Estimate.TotalCost / (1 - MarginM8 / 100); }
        }
        public double TotalPriceM6
        {
            get { return Estimate.TotalCost / (1 - MarginM6 / 100); }
        }
        public double TotalPriceM4
        {
            get { return Estimate.TotalCost / (1 - MarginM4 / 100); }
        }
        public double TotalPriceM2
        {
            get { return Estimate.TotalCost / (1 - MarginM2 / 100); }
        }
        public double TotalPriceP2
        {
            get { return Estimate.TotalCost / (1 - MarginP2 / 100); }
        }
        public double TotalPriceP4
        {
            get { return Estimate.TotalCost / (1 - MarginP4 / 100); }
        }
        public double TotalPriceP6
        {
            get { return Estimate.TotalCost / (1 - MarginP6 / 100); }
        }
        public double TotalPriceP8
        {
            get { return Estimate.TotalCost / (1 - MarginP8 / 100); }
        }
        #endregion
        #region Markups
        public double Markup
        {
            get { return Estimate.TotalPrice - Estimate.TotalCost; }
        }

        public double MarkupM8
        {
            get { return TotalPriceM8 - Estimate.TotalCost; }
        }
        public double MarkupM6
        {
            get { return TotalPriceM6 - Estimate.TotalCost; }
        }
        public double MarkupM4
        {
            get { return TotalPriceM4 - Estimate.TotalCost; }
        }
        public double MarkupM2
        {
            get { return TotalPriceM2 - Estimate.TotalCost; }
        }
        public double MarkupP2
        {
            get { return TotalPriceP2 - Estimate.TotalCost; }
        }
        public double MarkupP4
        {
            get { return TotalPriceP4 - Estimate.TotalCost; }
        }
        public double MarkupP6
        {
            get { return TotalPriceP6 - Estimate.TotalCost; }
        }
        public double MarkupP8
        {
            get { return TotalPriceP8 - Estimate.TotalCost; }
        }
        #endregion

        public ReviewVM()
        {
            _bid = new TECBid();
            _estimate = new TECEstimator(_bid, new EstimatingLibrary.Utilities.ChangeWatcher(_bid));
        }

        private void Bid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalPrice")
            {
                raiseTable();
            }
        }

        public void Refresh(TECEstimator estimate, TECBid bid)
        {
            Estimate = estimate;
            Bid = bid;
            raiseTable();
        }

        private void raiseTable()
        {
            RaisePropertyChanged("MarginM8");
            RaisePropertyChanged("MarginM6");
            RaisePropertyChanged("MarginM4");
            RaisePropertyChanged("MarginM2");
            RaisePropertyChanged("MarginP2");
            RaisePropertyChanged("MarginP4");
            RaisePropertyChanged("MarginP6");
            RaisePropertyChanged("MarginP8");

            RaisePropertyChanged("TotalPriceM8");
            RaisePropertyChanged("TotalPriceM6");
            RaisePropertyChanged("TotalPriceM4");
            RaisePropertyChanged("TotalPriceM2");
            RaisePropertyChanged("TotalPriceP2");
            RaisePropertyChanged("TotalPriceP4");
            RaisePropertyChanged("TotalPriceP6");
            RaisePropertyChanged("TotalPriceP8");

            RaisePropertyChanged("Markup");
            RaisePropertyChanged("MarkupM8");
            RaisePropertyChanged("MarkupM6");
            RaisePropertyChanged("MarkupM4");
            RaisePropertyChanged("MarkupM2");
            RaisePropertyChanged("MarkupP2");
            RaisePropertyChanged("MarkupP4");
            RaisePropertyChanged("MarkupP6");
            RaisePropertyChanged("MarkupP8");
        }
    }
}