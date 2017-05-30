using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SubScopeVM : ViewModelBase, IDropTarget
    {
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

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion

        #region VMs
        public DevicesPointsVM ChildVM;
        #endregion

        private TECSubScope _selectedSubScope;
        public TECSubScope SelectedSubScope
        {
            get { return _selectedSubScope; }
            set
            {
                _selectedSubScope = value;
                RaisePropertyChanged("SelectedSubScope");
                SelectionChanged?.Invoke(value);
                ChildVM.SelectedSubScope = value;
            }
        }
        private ObservableCollection<TECLocation> _locationSelections;
        public ObservableCollection<TECLocation> LocationSelections
        {
            get { return _locationSelections; }
            set
            {
                _locationSelections = value;
                RaisePropertyChanged("LocationSelections");
            }
        }

        /// <summary>
        /// Initializes a new instance of the SubScopeView class.
        /// </summary>
        #region Intializers
        public SubScopeVM(TECScopeManager scopeManager)
        {
            if (scopeManager is TECBid)
            {
                _bid = scopeManager as TECBid;
            }
            else
            {
                Templates = scopeManager as TECTemplates;
            }
            setupVMs();
            populateLocationSelections();
            DataGridVisibilty = new VisibilityModel();
        }
        #endregion

        #region Methods
        private void setupVMs()
        {
            ChildVM = new DevicesPointsVM();
            ChildVM.DragHandler += DragHandler;
            ChildVM.DropHandler += DropHandler;
            ChildVM.SelectionChanged += SelectionChanged;
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
        }

        public void NullifySelected()
        {
            SelectedSubScope = null;
            ChildVM.NullifySelected();
        }

        private void populateLocationSelections()
        {
            LocationSelections = new ObservableCollection<TECLocation>();
            var noneLocation = new TECLocation();
            noneLocation.Name = "None";
            LocationSelections.Add(noneLocation);
            foreach (TECLocation location in Bid.Locations)
            {
                LocationSelections.Add(location);
            }
        }
        private void Locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object location in e.NewItems)
                { LocationSelections.Add(location as TECLocation); }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object location in e.OldItems)
                { LocationSelections.Remove(location as TECLocation); }
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            DropHandler(dropInfo);
        }
        

        #endregion
    }
}