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
    public class LaborViewModel : ViewModelBase
    {
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                _labor = Bid.Labor;
                RaisePropertyChanged("Bid");

            }
        }

        private TECLabor _labor;
        public TECLabor Labor
        {
            get { return _labor; }
            set
            {
                _labor = value;
                RaisePropertyChanged("Labor");
            }
        }

        /// <summary>
        /// Initializes a new instance of the LaborViewModel class.
        /// </summary>
        /// 
        public LaborViewModel()
        {
            Bid = new TECBid();
            Labor = new TECLabor();
            MessengerInstance.Register<GenericMessage<TECBid>>(this, PopulateBid);

            MessengerInstance.Send<NotificationMessage>(new NotificationMessage("LaborViewModelLoaded"));
        }

        #region Methods

        #region Message Methods

        public void PopulateBid(GenericMessage<TECBid> genericMessage)
        {
            Bid = genericMessage.Content;
        }
        
        #endregion Message Methods

        #endregion //Methods

    }
}