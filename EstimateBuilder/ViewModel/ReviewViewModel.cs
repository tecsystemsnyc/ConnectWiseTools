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
        public ReviewViewModel()
        {
            Bid = new TECBid();
            MessengerInstance.Register<GenericMessage<TECBid>>(this, PopulateBid);

            MessengerInstance.Send<NotificationMessage>(new NotificationMessage("ReviewViewModelLoaded"));
        }

        #region Methods

        #region Message Methods

        public void PopulateBid(GenericMessage<TECBid> genericMessage)
        {
            Console.WriteLine("Review tab populated");
            Bid = genericMessage.Content;
        }

        #endregion Message Methods

        #endregion //Methods
    }
}