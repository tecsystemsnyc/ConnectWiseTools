using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MiscCostsVM : ViewModelBase
    {
        private ObservableCollection<TECMisc> _tecCostCollection;
        public ObservableCollection<TECMisc> TECCostCollection
        {
            get { return _tecCostCollection; }
            set
            {
                _tecCostCollection = value;
                RaisePropertyChanged("TECCostCollection");
            }
        }

        private ObservableCollection<TECMisc> _electricalCostCollection;
        public ObservableCollection<TECMisc> ElectricalCostCollection
        {
            get { return _tecCostCollection; }
            set
            {
                _tecCostCollection = value;
                RaisePropertyChanged("TECCostCollection");
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the MiscCostsVM class.
        /// </summary>
        public MiscCostsVM(TECBid bid)
        {
            populateCollections(bid);
            bid.MiscCosts.CollectionChanged += MiscCosts_CollectionChanged;
        }

        private void MiscCosts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(TECMisc misc in e.NewItems)
                {
                    handleAddMisc(misc);
                }
            } else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECMisc misc in e.OldItems)
                { 
                    handleRemoveMisc(misc);
                }
            }
        }

        private void Misc_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var args = e as PropertyChangedExtendedEventArgs<object>;
            if(e.PropertyName == "Type")
            {
                //TECMisc old = new 
                if((args.OldValue as TECMisc).Type == CostType.TEC){
                    TECCostCollection.Remove(sender as TECMisc);
                } else if ((args.OldValue as TECMisc).Type == CostType.TEC)
                {
                    TECCostCollection.Remove(sender as TECMisc);
                }
            }
        }

        private void populateCollections(TECBid bid)
        {
            ObservableCollection<TECMisc> tecCosts = new ObservableCollection<TECMisc>();
            ObservableCollection<TECMisc> electricalCosts = new ObservableCollection<TECMisc>();
            foreach(TECMisc misc in bid.MiscCosts)
            {
                handleAddMisc(misc);
            }
        }

        private void handleAddMisc(TECMisc misc)
        {
            misc.PropertyChanged += Misc_PropertyChanged;
            if (misc.Type == CostType.TEC)
            {
                TECCostCollection.Add(misc);
            }
            else if (misc.Type == CostType.Electrical)
            {
                ElectricalCostCollection.Add(misc);
            }
        }
        private void handleRemoveMisc(TECMisc misc)
        {
            misc.PropertyChanged -= Misc_PropertyChanged;
            if (misc.Type == CostType.TEC)
            {
                TECCostCollection.Remove(misc);
            }
            else if (misc.Type == CostType.Electrical)
            {
                ElectricalCostCollection.Remove(misc);
            }
        }
    }
}