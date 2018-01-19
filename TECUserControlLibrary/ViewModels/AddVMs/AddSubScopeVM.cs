using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddSubScopeVM :  AddVM
    {
        private TECEquipment parent;
        private TECSubScope toAdd;
        private int quantity;
        private Action<TECSubScope> add;
        private bool isTypical = false;
        private string _pointName;
        private int _pointQuantity;
        private IOType _pointType;
        private TECSubScope underlyingTemplate;
        private List<TECPoint> originalPoints = new List<TECPoint>();
        private List<IEndDevice> originalDevices = new List<IEndDevice>();
        private bool _displayReferenceProperty = false;

        public TECSubScope ToAdd
        {
            get { return toAdd; }
            private set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }
        public string PointName
        {
            get { return _pointName; }
            set
            {
                _pointName = value;
                RaisePropertyChanged("PointName");
            }
        }
        public int PointQuantity
        {
            get { return _pointQuantity; }
            set
            {
                _pointQuantity = value;
                RaisePropertyChanged("PointQuantity");
            }
        }
        public IOType PointType
        {
            get { return _pointType; }
            set
            {
                _pointType = value;
                RaisePropertyChanged("PointType");
            }
        }
        public ICommand AddPointCommand { get; private set; }
        public RelayCommand<IEndDevice> DeleteDeviceCommand { get; private set; }
        public RelayCommand<TECPoint> DeletePointCommand { get; private set; }
        public bool DisplayReferenceProperty
        {
            get { return _displayReferenceProperty; }
            private set
            {
                _displayReferenceProperty = value;
                RaisePropertyChanged("DisplayReferenceProperty");
            }
        }

        public AddSubScopeVM(TECEquipment parentEquipment, TECScopeManager scopeManager) : base(scopeManager)
        {
            parent = parentEquipment;
            isTypical = parent.IsTypical;
            toAdd = new TECSubScope(parentEquipment.IsTypical);
            add = subScope =>
            {
                parent.SubScope.Add(subScope);

            };
            setup();
        }

        public AddSubScopeVM(Action<TECSubScope> addMethod, TECScopeManager scopeManager): base(scopeManager)
        {
            add = addMethod;
            toAdd = new TECSubScope(false);
            setup();
            PropertiesVM.DisplayReferenceProperty = false;
        }
        private void setup()
        {
            Quantity = 1;
            PointQuantity = 1;
            PointName = "";
            PointType = IOType.AI;
            AddCommand = new RelayCommand(addExecute, addCanExecute);
            AddPointCommand = new RelayCommand(addPointExecute, canAddPoint);
            DeletePointCommand = new RelayCommand<TECPoint>(deletePointExecute);
            DeleteDeviceCommand = new RelayCommand<IEndDevice>(deleteDeviceExecute);
        }
        
        private void addPointExecute()
        {
            TECPoint newPoint = new TECPoint(isTypical);
            newPoint.Type = PointType;
            newPoint.Quantity = PointQuantity;
            newPoint.Label = PointName;
            ToAdd.Points.Add(newPoint);
            PointName = "";
        }
        private bool canAddPoint()
        {
            if(PointQuantity != 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void deletePointExecute(TECPoint point)
        {
            toAdd.Points.Remove(point);
        }

        private void deleteDeviceExecute(IEndDevice device)
        {
            toAdd.Devices.Remove(device);
        }
        
        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            for (int x = 0; x < Quantity; x++)
            {
                TECSubScope subScope = null;
                if(underlyingTemplate != null)
                {
                    subScope = AsReference ? templates.SubScopeSynchronizer.NewItem(underlyingTemplate) : new TECSubScope(underlyingTemplate, isTypical);
                    subScope.CopyPropertiesFromScope(ToAdd);
                    foreach (IEndDevice device in ToAdd.Devices.Where(item => !originalDevices.Contains(item)))
                    {
                        subScope.Devices.Add(device);
                    }
                    foreach (TECPoint point in ToAdd.Points.Where(item => !originalPoints.Contains(item)))
                    {
                        subScope.Points.Add(point);
                    }
                }
                else
                {
                    subScope = new TECSubScope(ToAdd, isTypical);
                }
                
                add(subScope);
                Added?.Invoke(subScope);
            }
        }

        internal void SetTemplate(TECSubScope subScope)
        {
            ToAdd = new TECSubScope(subScope, isTypical);
            originalDevices = new List<IEndDevice>(ToAdd.Devices);
            originalPoints = new List<TECPoint>(ToAdd.Points);
            underlyingTemplate = subScope;
            if (IsTemplates)
            {
                DisplayReferenceProperty = true;
            }
        }
    }
}
