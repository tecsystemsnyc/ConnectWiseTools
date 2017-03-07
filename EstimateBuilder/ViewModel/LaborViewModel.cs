using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Input;

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
                RaisePropertyChanged("Bid");
            }
        }

        private TECTemplates _templates;
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }

        public bool TemplatesLoaded;

        /// <summary>
        /// Initializes a new instance of the LaborViewModel class.
        /// </summary>
        /// 
        public LaborViewModel()
        {
            Bid = new TECBid();
            Templates = new TECTemplates();
        }

        public ICommand ReloadCommand { get; private set; }

        public Action LoadTemplates;

        private void ReloadExecute()
        {
            if (!TemplatesLoaded)
            {
                LoadTemplates();
            }

            //Check again to see if templates are properly loaded after reloading. Should not be else.
            if (TemplatesLoaded)
            {
                Bid.Labor.UpdateConstants(Templates.Labor);
            }
        }

    }
}