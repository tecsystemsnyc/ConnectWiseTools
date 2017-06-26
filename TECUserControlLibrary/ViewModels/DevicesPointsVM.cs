using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DevicesPointsVM : ViewModelBase, IDropTarget
    {
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

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion

        public ICommand AddPointCommand { get; private set; }

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

        /// <summary>
        /// Initializes a new instance of the DevicesPointsVM class.
        /// </summary>
        #region Intializers
        public DevicesPointsVM()
        {
            DataGridVisibilty = new VisibilityModel();
            setupCommands();
            PointName = "";
            PointDescription = "";
        }
        #endregion

        #region Commands
        private void setupCommands()
        {
            AddPointCommand = new RelayCommand(AddPointExecute, AddPointCanExecute);
        }

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
        #endregion //Commands

        public void NullifySelected()
        {
            SelectedDevice = null;
            SelectedPoint = null;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler?.Invoke(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            DropHandler?.Invoke(dropInfo);
        }
    }
}