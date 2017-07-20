using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class SystemsVM : ViewModelBase, IDropTarget
    {
        #region Properties
        private VisibilityModel _dataGridVisibilty;
        public VisibilityModel DataGridVisibilty
        {
            get { return _dataGridVisibilty; }
            set
            {
                _dataGridVisibilty = value;
                RaisePropertyChanged("DataGridVisibilty");
            }
        }

        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                Bid.Locations.CollectionChanged -= Locations_CollectionChanged;
                _bid = value;
                RaisePropertyChanged("Bid");
                Bid.Locations.CollectionChanged += Locations_CollectionChanged;
                populateLocationSelections();
            }
        }

        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }
        private TECTemplates _templates;

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion

        #region Selected Scope Properties
        private TECSystem _selectedSystem;
        public TECSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private TECController _selectedController;
        public TECController SelectedController
        {
            get { return _selectedController; }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private TECPanel _selectedPanel;
        public TECPanel SelectedPanel
        {
            get { return _selectedPanel; }
            set
            {
                _selectedPanel = value;
                RaisePropertyChanged("SelectedPanel");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private ObservableCollection<TECLabeled> _locationSelections;
        public ObservableCollection<TECLabeled> LocationSelections
        {
            get { return _locationSelections; }
            set
            {
                _locationSelections = value;
                RaisePropertyChanged("LocationSelections");
            }
        }
        #endregion
        
        #region VMs
        public EquipmentVM ChildVM { get; set; }
        #endregion

        #endregion

        #region Intializers
        public SystemsVM(TECScopeManager scopeManager)
        {
            if(scopeManager is TECBid)
            {
                _bid = scopeManager as TECBid;
                populateLocationSelections();
            }
            else
            {
                Templates = scopeManager as TECTemplates;
            }
            
            DataGridVisibilty = new VisibilityModel();
            
            setupVMs(scopeManager);
        }
        #endregion

        #region Methods

        private void setupVMs(TECScopeManager scopeManager)
        {
            ChildVM = new EquipmentVM(scopeManager);
        }

        public void AssignChildDelegates()
        {
            ChildVM.DragHandler += DragHandler;
            ChildVM.DropHandler += DropHandler;
            ChildVM.SelectionChanged += SelectionChanged;
            ChildVM.AssignChildDelegates();
        }
        
        public void Refresh(TECScopeManager scopeManager)
        {
            if (scopeManager is TECBid)
            {
                var bid = scopeManager as TECBid;
                Bid = bid;
            }
            else if (scopeManager is TECTemplates)
            {
                var templates = scopeManager as TECTemplates;
                Templates = templates;
            }
            ChildVM.Refresh(scopeManager);
        }
        
        private void NullifySelections(object obj)
        {
            //if(obj != null)
            //{
            //    if (!(obj is TECSystem))
            //    {
            //        SelectedSystem = null;
            //    }
            //    if (!(obj is TECEquipment))
            //    {
            //        SelectedEquipment = null;
            //    }
            //    if (!(obj is TECSubScope))
            //    {
            //        SelectedSubScope = null;
            //    }
            //    if (!(obj is TECDevice))
            //    {
            //        SelectedDevice = null;
            //    }
            //    if (!(obj is TECPoint))
            //    {
            //        SelectedPoint = null;
            //    }
            //    if (!(obj is TECController))
            //    {
            //        SelectedController = null;
            //    }
            //    if (!(obj is TECPanel))
            //    {
            //        SelectedPanel = null;
            //    }
            //}

        }

        public void NullifySelected()
        {
            SelectedSystem = null;
            ChildVM.NullifySelected();
        }

        private void populateLocationSelections()
        {
            LocationSelections = new ObservableCollection<TECLabeled>();
            var noneLocation = new TECLabeled();
            noneLocation.Label = "None";
            LocationSelections.Add(noneLocation);
            foreach (TECLabeled location in Bid.Locations)
            {
                LocationSelections.Add(location);
            }
        }
        private void Locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object location in e.NewItems)
                { LocationSelections.Add(location as TECLabeled); }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object location in e.OldItems)
                { LocationSelections.Remove(location as TECLabeled); }
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler?.Invoke(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            DropHandler?.Invoke(dropInfo);
        }
        #endregion
        
    }
}
