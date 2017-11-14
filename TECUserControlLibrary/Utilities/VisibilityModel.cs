using System.ComponentModel;
using System.Windows;

namespace TECUserControlLibrary.Utilities
{
    public class VisibilityModel : INotifyPropertyChanged
    {
        #region System Datagrid Visibilities
        private Visibility _systemName;
        public Visibility SystemName
        {
            get { return _systemName; }
            set
            {
                _systemName = value;
                RaisePropertyChanged("SystemName");
            }
        }

        private Visibility _systemDescription;
        public Visibility SystemDescription
        {
            get { return _systemDescription; }
            set
            {
                _systemDescription = value;
                RaisePropertyChanged("SystemDescription");
            }
        }

        private Visibility _systemLocation;
        public Visibility SystemLocation
        {
            get { return _systemLocation; }
            set
            {
                _systemLocation = value;
                RaisePropertyChanged("SystemLocation");
            }
        }

        private Visibility _systemQuantity;
        public Visibility SystemQuantity
        {
            get { return _systemQuantity; }
            set
            {
                _systemQuantity = value;
                RaisePropertyChanged("SystemQuantity");
            }
        }

        private Visibility _systemEquipmentCount;
        public Visibility SystemEquipmentCount
        {
            get { return _systemEquipmentCount; }
            set
            {
                _systemEquipmentCount = value;
                RaisePropertyChanged("SystemEquipmentCount");
            }
        }

        private Visibility _systemSubScopeCount;
        public Visibility SystemSubScopeCount
        {
            get { return _systemSubScopeCount; }
            set
            {
                _systemSubScopeCount = value;
                RaisePropertyChanged("SystemSubScopeCount");
            }
        }

        private Visibility _systemPointNumber;
        public Visibility SystemPointNumber
        {
            get { return _systemPointNumber; }
            set
            {
                _systemPointNumber = value;
                RaisePropertyChanged("SystemPointNumber");
            }
        }

        private Visibility _systemModifierPrice;
        public Visibility SystemModifierPrice
        {
            get { return _systemModifierPrice; }
            set
            {
                _systemModifierPrice = value;
                RaisePropertyChanged("SystemModifierPrice");
            }
        }

        private Visibility _systemUnitPrice;
        public Visibility SystemUnitPrice
        {
            get { return _systemUnitPrice; }
            set
            {
                _systemUnitPrice = value;
                RaisePropertyChanged("SystemUnitPrice");
            }
        }

        private Visibility _systemTotalPrice;
        public Visibility SystemTotalPrice
        {
            get { return _systemTotalPrice; }
            set
            {
                _systemTotalPrice = value;
                RaisePropertyChanged("SystemTotalPrice");
            }
        }

        private Visibility _systemExpander;
        public Visibility SystemExpander
        {
            get { return _systemExpander; }
            set
            {
                _systemExpander = value;
                RaisePropertyChanged("SystemExpander");
            }
        }
        #endregion

        #region Equipment Datagrid Visibilities
        private Visibility _equipmentName;
        public Visibility EquipmentName
        {
            get { return _equipmentName; }
            set
            {
                _equipmentName = value;
                RaisePropertyChanged("EquipmentName");
            }
        }

        private Visibility _equipmentDescription;
        public Visibility EquipmentDescription
        {
            get { return _equipmentDescription; }
            set
            {
                _equipmentDescription = value;
                RaisePropertyChanged("EquipmentDescription");
            }
        }

        private Visibility _equipmentLocation;
        public Visibility EquipmentLocation
        {
            get { return _equipmentLocation; }
            set
            {
                _equipmentLocation = value;
                RaisePropertyChanged("EquipmentLocation");
            }
        }

        private Visibility _equipmentQuantity;
        public Visibility EquipmentQuantity
        {
            get { return _equipmentQuantity; }
            set
            {
                _equipmentQuantity = value;
                RaisePropertyChanged("EquipmentQuantity");
            }
        }

        private Visibility _equipmentSubScopeCount;
        public Visibility EquipmentSubScopeCount
        {
            get { return _equipmentSubScopeCount; }
            set
            {
                _equipmentSubScopeCount = value;
                RaisePropertyChanged("EquipmentSubScopeCount");
            }
        }

        private Visibility _equipmentUnitPrice;
        public Visibility EquipmentUnitPrice
        {
            get { return _equipmentUnitPrice; }
            set
            {
                _equipmentUnitPrice = value;
                RaisePropertyChanged("EquipmentUnitPrice");
            }
        }

        private Visibility _equipmentTotalPrice;
        public Visibility EquipmentTotalPrice
        {
            get { return _equipmentTotalPrice; }
            set
            {
                _equipmentTotalPrice = value;
                RaisePropertyChanged("EquipmentTotalPrice");
            }
        }
        #endregion

        #region SubScope Datagrid Visibilities
        private Visibility _subScopeName;
        public Visibility SubScopeName
        {
            get { return _subScopeName; }
            set
            {
                _subScopeName = value;
                RaisePropertyChanged("SubScopeName");
            }
        }

        private Visibility _subScopeDescription;
        public Visibility SubScopeDescription
        {
            get { return _subScopeDescription; }
            set
            {
                _subScopeDescription = value;
                RaisePropertyChanged("SubScopeDescription");
            }
        }

        private Visibility _subScopeLocation;
        public Visibility SubScopeLocation
        {
            get { return _subScopeLocation; }
            set
            {
                _subScopeLocation = value;
                RaisePropertyChanged("SubScopeLocation");
            }
        }

        private Visibility _subScopeQuantity;
        public Visibility SubScopeQuantity
        {
            get { return _subScopeQuantity; }
            set
            {
                _subScopeQuantity = value;
                RaisePropertyChanged("SubScopeQuantity");
            }
        }

        private Visibility _expandSubScope;
        public Visibility ExpandSubScope
        {
            get { return _expandSubScope; }
            set
            {
                _expandSubScope = value;
                RaisePropertyChanged("ExpandSubScope");
            }
        }

        private Visibility _subScopeLength;
        public Visibility SubScopeLength
        {
            get { return _subScopeLength; }
            set
            {
                _subScopeLength = value;
                RaisePropertyChanged("SubScopeLength");
            }
        }
        #endregion

        public VisibilityModel()
        {
            SystemName = Visibility.Visible;
            SystemDescription = Visibility.Visible;
            SystemLocation = Visibility.Visible;
            SystemQuantity = Visibility.Visible;
            SystemEquipmentCount = Visibility.Visible;
            SystemSubScopeCount = Visibility.Visible;
            SystemUnitPrice = Visibility.Visible;
            SystemModifierPrice = Visibility.Visible;
            SystemTotalPrice = Visibility.Visible;
            SystemExpander = Visibility.Visible;

            EquipmentName = Visibility.Visible;
            EquipmentDescription = Visibility.Visible;
            EquipmentLocation = Visibility.Visible;
            EquipmentQuantity = Visibility.Visible;
            EquipmentSubScopeCount = Visibility.Visible;
            EquipmentUnitPrice = Visibility.Visible;
            EquipmentTotalPrice = Visibility.Visible;

            ExpandSubScope = Visibility.Visible;
            SubScopeName = Visibility.Visible;
            SubScopeDescription = Visibility.Visible;
            SubScopeLocation = Visibility.Visible;
            SubScopeQuantity = Visibility.Visible;
            SubScopeLength = Visibility.Visible;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
