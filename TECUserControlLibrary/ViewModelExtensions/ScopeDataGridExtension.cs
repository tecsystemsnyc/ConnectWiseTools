using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModelExtensions
{
    public class ScopeDataGridExtension : ViewModelBase
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

        #region Selected ScopeProperties
        private TECSystem _selectedSystem;
        public TECSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
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
        #endregion

        #endregion
        
        #region Intializers
        public ScopeDataGridExtension()
        {
            DataGridVisibilty = new VisibilityModel();

            AddPointCommand = new RelayCommand(AddPointExecute, AddPointCanExecute);
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
        #endregion //Commands

        #endregion
    }
}
