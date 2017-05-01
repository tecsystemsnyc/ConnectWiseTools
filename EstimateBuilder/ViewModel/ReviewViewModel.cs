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
                Bid.PropertyChanged -= Bid_PropertyChanged;
                _bid = value;
                Bid.PropertyChanged += Bid_PropertyChanged;
                RaisePropertyChanged("Bid");
            }
        }

        //private double _userPrice;
        //public double UserPrice
        //{
        //    get { return _userPrice; }
        //    set
        //    {
        //        _userPrice = value;
        //        var newProfit = (value - Bid.SubcontractorSubtotal) / (EstimateCalculator.GetTECCost(Bid) * (1 + Bid.Parameters.Overhead / 100)) - 1;
        //        Bid.Parameters.Profit = newProfit * 100;
        //        RaisePropertyChanged("UserPrice");
        //    }
        //}

        //#region Margins

        //public double MarginM8
        //{
        //    get { return Bid.Margin - 8; }
        //}
        //public double MarginM6
        //{
        //    get { return Bid.Margin - 6; }
        //}
        //public double MarginM4
        //{
        //    get { return Bid.Margin - 4; }
        //}
        //public double MarginM2
        //{
        //    get { return Bid.Margin - 2; }
        //}
        //public double MarginP2
        //{
        //    get { return Bid.Margin +2; }
        //}
        //public double MarginP4
        //{
        //    get { return Bid.Margin + 4; }
        //}
        //public double MarginP6
        //{
        //    get { return Bid.Margin + 6; }
        //}
        //public double MarginP8
        //{
        //    get { return Bid.Margin + 8; }
        //}

        //#endregion
        //#region Prices
        //public double TotalPriceM8
        //{
        //    get { return Bid.TotalCost / (1 - MarginM8 / 100); }
        //}
        //public double TotalPriceM6
        //{
        //    get { return Bid.TotalCost / (1 - MarginM6 / 100); }
        //}
        //public double TotalPriceM4
        //{
        //    get { return Bid.TotalCost / (1 - MarginM4 / 100); }
        //}
        //public double TotalPriceM2
        //{
        //    get { return Bid.TotalCost / (1 - MarginM2 / 100); }
        //}
        //public double TotalPriceP2
        //{
        //    get { return Bid.TotalCost / (1 - MarginP2 / 100); }
        //}
        //public double TotalPriceP4
        //{
        //    get { return Bid.TotalCost / (1 - MarginP4 / 100); }
        //}
        //public double TotalPriceP6
        //{
        //    get { return Bid.TotalCost / (1 - MarginP6 / 100); }
        //}
        //public double TotalPriceP8
        //{
        //    get { return Bid.TotalCost / (1 - MarginP8 / 100); }
        //}
        //#endregion
        //#region Markups
        //public double Markup
        //{
        //    get { return Bid.TotalPrice - Bid.TotalCost; }
        //}

        //public double MarkupM8
        //{
        //    get { return TotalPriceM8 - Bid.TotalCost; }
        //}
        //public double MarkupM6
        //{
        //    get { return TotalPriceM6 - Bid.TotalCost; }
        //}
        //public double MarkupM4
        //{
        //    get { return TotalPriceM4 - Bid.TotalCost; }
        //}
        //public double MarkupM2
        //{
        //    get { return TotalPriceM2 - Bid.TotalCost; }
        //}
        //public double MarkupP2
        //{
        //    get { return TotalPriceP2 - Bid.TotalCost; }
        //}
        //public double MarkupP4
        //{
        //    get { return TotalPriceP4 - Bid.TotalCost; }
        //}
        //public double MarkupP6
        //{
        //    get { return TotalPriceP6 - Bid.TotalCost; }
        //}
        //public double MarkupP8
        //{
        //    get { return TotalPriceP8 - Bid.TotalCost; }
        //}
        //#endregion

        public ReviewViewModel()
        {
            _bid = new TECBid();
        }

        private void Bid_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalPrice")
            {
                raiseTable();
            }
        }

        public void Refresh(TECBid bid)
        {
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