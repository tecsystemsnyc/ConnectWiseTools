using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;

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
            get { return _electricalCostCollection; }
            set
            {
                _electricalCostCollection = value;
                RaisePropertyChanged("ElectricalCostCollection");
            }
        }

        private string _miscName;
        public string MiscName
        {
            get { return _miscName; }
            set
            {
                _miscName = value;
                RaisePropertyChanged("MiscName");
            }
        }

        private CostType _miscType;
        public CostType MiscType
        {
            get { return _miscType; }
            set
            {
                _miscType = value;
                RaisePropertyChanged("MiscType");
            }
        }

        private TECBid _bid;

        public ICommand AddNewCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MiscCostsVM class.
        /// </summary>
        public MiscCostsVM(TECBid bid)
        {
            Refresh(bid);
            AddNewCommand = new RelayCommand(addNewExecute, addNewCanExecute);
            MiscType = CostType.TEC;
        }

        private bool addNewCanExecute()
        {
            if(MiscName != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void addNewExecute()
        {
            TECMisc newMisc = new TECMisc();
            newMisc.Name = MiscName;
            newMisc.Type = MiscType;

            _bid.MiscCosts.Add(newMisc);
        }

        public void Refresh(TECBid bid)
        {
            if(_bid != null)
            {
                _bid.MiscCosts.CollectionChanged -= MiscCosts_CollectionChanged;
            }
            populateCollections(bid);
            bid.MiscCosts.CollectionChanged += MiscCosts_CollectionChanged;
            _bid = bid;
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
                TECMisc old = args.OldValue as TECMisc;
                TECMisc current = sender as TECMisc;
                if (old.Type == CostType.TEC){
                    TECCostCollection.Remove(sender as TECMisc);
                } else if (old.Type == CostType.TEC)
                {
                    TECCostCollection.Remove(sender as TECMisc);
                }

                if(current.Type == CostType.TEC)
                {
                    TECCostCollection.Add(current);
                }
                else if (current.Type == CostType.Electrical)
                {
                    ElectricalCostCollection.Add(current);
                }
            }
        }

        private void populateCollections(TECBid bid)
        {
            TECCostCollection = new ObservableCollection<TECMisc>();
            ElectricalCostCollection = new ObservableCollection<TECMisc>();
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