using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using GongSolutions.Wpf.DragDrop;
using TECUserControlLibrary.Utilities;
using System.Windows;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MiscCostsVM : ViewModelBase, IDropTarget
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

        private ObservableCollection<TECMisc> sourceCollection;

        public Visibility QuantityVisibility { get; set; }

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

        private double _cost;
        public double Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                RaisePropertyChanged("Cost");
            }
        }

        private double _labor;
        public double Labor
        {
            get { return _labor; }
            set
            {
                _labor = value;
                RaisePropertyChanged("Labor");
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
        private TECTemplates _templates;
        private TECSystem _system;

        public ICommand AddNewCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MiscCostsVM class.
        /// </summary>
        public MiscCostsVM(TECScopeManager scopeManager)
        {
            Refresh(scopeManager);
            AddNewCommand = new RelayCommand(addNewExecute, addNewCanExecute);
            MiscType = CostType.TEC;
        }
        public MiscCostsVM(TECSystem system)
        {
            Refresh(system);
            AddNewCommand = new RelayCommand(addNewExecute, addNewCanExecute);
            MiscType = CostType.TEC;
        }

        public void Refresh(TECScopeManager scopeManager)
        {
            var bid = scopeManager as TECBid;
            var templates = scopeManager as TECTemplates;
            if (bid != null)
            {
                QuantityVisibility = Visibility.Visible;
                if (_bid != null)
                {
                    _bid.MiscCosts.CollectionChanged -= MiscCosts_CollectionChanged;
                }

                bid.MiscCosts.CollectionChanged += MiscCosts_CollectionChanged;
                _bid = bid;
                sourceCollection = bid.MiscCosts;
                populateCollections();
            }
            else if (templates != null)
            {
                QuantityVisibility = Visibility.Collapsed;
                if (templates != null)
                {
                    templates.MiscCostTemplates.CollectionChanged -= MiscCosts_CollectionChanged;
                }

                templates.MiscCostTemplates.CollectionChanged += MiscCosts_CollectionChanged;
                _templates = templates;
                sourceCollection = templates.MiscCostTemplates;
                populateCollections();
            }


        }
        public void Refresh(TECSystem system)
        {
            QuantityVisibility = Visibility.Visible;
            if (_bid != null)
            {
                system.MiscCosts.CollectionChanged -= MiscCosts_CollectionChanged;
            }

            system.MiscCosts.CollectionChanged += MiscCosts_CollectionChanged;
            _system = system;
            sourceCollection = system.MiscCosts;
            populateCollections();
            
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
            newMisc.Cost = Cost;
            newMisc.Labor = Labor;

            sourceCollection.Add(newMisc);
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
                } else if (old.Type == CostType.Electrical)
                {
                    ElectricalCostCollection.Remove(sender as TECMisc);
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

        private void populateCollections()
        {
            TECCostCollection = new ObservableCollection<TECMisc>();
            ElectricalCostCollection = new ObservableCollection<TECMisc>();
            foreach(TECMisc misc in sourceCollection)
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

        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }

        public void Drop(IDropInfo dropInfo)
        {
            TECScopeManager scopeManager;
            if (_templates != null)
            {
                scopeManager = _templates;
            }
            else
            {
                scopeManager = _bid;
            }
            var newMisc = (dropInfo.Data as TECMisc).DragDropCopy(scopeManager) as TECMisc;
            sourceCollection.Add(newMisc);
        }
    }
}