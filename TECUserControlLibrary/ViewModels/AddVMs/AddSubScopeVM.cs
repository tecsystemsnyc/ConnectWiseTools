using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
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

        public TECSubScope ToAdd
        {
            get { return toAdd; }
            set
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
                var subScope = new TECSubScope(ToAdd, isTypical);
                add(subScope);
                Added?.Invoke(subScope);
            }
        }
        
    }
}
