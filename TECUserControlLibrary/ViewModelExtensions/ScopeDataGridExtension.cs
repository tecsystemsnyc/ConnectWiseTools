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

namespace TECUserControlLibrary.ViewModelExtensions
{
    public class ScopeDataGridExtension : ViewModelBase, IDropTarget
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
                _bid = value;
                // Call OnPropertyChanged whenever the property is updated
                RaisePropertyChanged("Bid");
                populateLocationSelections();
                Bid.Locations.CollectionChanged += Locations_CollectionChanged;
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

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion

        #region Selected ScopeProperties
        private TECSystem _selectedSystem;
        public TECSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
                SelectionChanged?.Invoke(value);

            }
        }
        private TECEquipment _selectedEquipment;
        public TECEquipment SelectedEquipment
        {
            get { return _selectedEquipment; }
            set
            {
                _selectedEquipment = value;
                RaisePropertyChanged("SelectedEquipment");
                SelectionChanged?.Invoke(value);
            }
        }
        private TECSubScope _selectedSubScope;
        public TECSubScope SelectedSubScope
        {
            get { return _selectedSubScope; }
            set
            {
                _selectedSubScope = value;
                RaisePropertyChanged("SelectedSubScope");
                SelectionChanged?.Invoke(value);
            }
        }
        private TECDevice _selectedDevice;
        public TECDevice SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
                RaisePropertyChanged("SelectedDevice");
                SelectionChanged?.Invoke(value);
            }
        }
        private TECPoint _selectedPoint;
        public TECPoint SelectedPoint
        {
            get { return _selectedPoint; }
            set
            {
                _selectedPoint = value;
                RaisePropertyChanged("SelectedPoint");
                SelectionChanged?.Invoke(value);
            }
        }

        #endregion

        #region Point Interface Properties
        public string PointName
        {
            get { return _pointName; }
            set
            {
                _pointName = value;
                RaisePropertyChanged("PointName");
            }
        }
        private string _pointName;

        public string PointDescription
        {
            get { return _pointDescription; }
            set
            {
                _pointDescription = value;
                RaisePropertyChanged("PointDescription");
            }
        }
        private string _pointDescription;

        public PointTypes PointType
        {
            get { return _pointType; }
            set
            {
                _pointType = value;
                RaisePropertyChanged("PointType");
            }
        }
        private PointTypes _pointType;

        public int PointQuantity
        {
            get { return _pointQuantity; }
            set
            {
                _pointQuantity = value;
                RaisePropertyChanged("PointQuantity");
            }
        }
        private int _pointQuantity;
        #endregion //Point Interface Properties

        #region Command Properties
        public ICommand AddPointCommand { get; private set; }

        public RelayCommand<AddingNewItemEventArgs> AddNewEquipment { get; private set; }

        #endregion

        #endregion

        #region Intializers
        public ScopeDataGridExtension(TECBid bid)
        {
            Bid = bid;
            DataGridVisibilty = new VisibilityModel();

            AddPointCommand = new RelayCommand(AddPointExecute, AddPointCanExecute);
            AddNewEquipment = new RelayCommand<AddingNewItemEventArgs>(e => AddNewEquipmentExecute(e));
        }
        #endregion

        #region Methods
        #region Commands
        private void AddPointExecute()
        {
            TECPoint newPoint = new TECPoint();
            newPoint.Name = PointName;
            newPoint.Description = PointDescription;
            newPoint.Type = PointType;
            newPoint.Quantity = PointQuantity;
            if (PointType != 0)
            {
                SelectedSubScope.Points.Add(newPoint);
            }
        }
        private bool AddPointCanExecute()
        {
            if ((PointType != 0) && (PointName != ""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private void AddNewEquipmentExecute(AddingNewItemEventArgs e)
        {
            //e.NewItem = new TECEquipment("here","this", 12, new ObservableCollection<TECSubScope>());
            //((TECEquipment)e.NewItem).Location = SelectedSystem.Location;
        }
        #endregion //Commands

        public void Locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            populateLocationSelections();
        }
        
        public void populateLocationSelections()
        {
            LocationSelections = new ObservableCollection<TECLocation>();

            LocationSelections.Add(new TECLocation("None"));
            foreach (TECLocation location in Bid.Locations)
            {
                LocationSelections.Add(location);
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
