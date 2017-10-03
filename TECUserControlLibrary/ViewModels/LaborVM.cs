using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LaborVM : ViewModelBase
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
        public bool TemplatesLoaded;
        public ICommand ReloadCommand { get; private set; }

        public Action LoadTemplates;

        /// <summary>
        /// Initializes a new instance of the LaborViewModel class.
        /// </summary>
        /// 
        public LaborVM()
        {
            Bid = new TECBid();
            Templates = new TECTemplates();
            Estimate = new TECEstimator(Bid, new ChangeWatcher(Bid));

            ReloadCommand = new RelayCommand(ReloadExecute);
        }

        public void Refresh(TECBid bid, TECEstimator estimate, TECTemplates templates)
        {
            Bid = bid;
            Templates = templates;
        }

        private void ReloadExecute()
        {
            if (!TemplatesLoaded)
            {
                LoadTemplates();
            }

            //Check again to see if templates are properly loaded after reloading. Should not be else.
            if (TemplatesLoaded)
            {
                throw new NotImplementedException();
                //Bid.Labor.UpdateConstants(Templates.Labor);
            }
        }

    }
}