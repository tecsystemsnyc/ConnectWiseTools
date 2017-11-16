using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddControllerVM : ViewModelBase, IAddVM
    {
        private TECSystem parent;
        private TECController toAdd;
        private int quantity;
        private Action<TECController> add;
        
        public TECController ToAdd
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

        public List<TECControllerType> ControllerTypes { get; private set; }
        public ICommand AddCommand { get; private set; }

        public AddControllerVM(TECSystem parentSystem, IEnumerable<TECControllerType> controllerTypes)
        {
            Quantity = 1;
            parent = parentSystem;
            add = controller =>
            {
                parent.AddController(controller);
            };
            ControllerTypes = new List<TECControllerType>(controllerTypes);
            toAdd = new TECController(ControllerTypes[0], parentSystem.IsTypical);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        public AddControllerVM(Action<TECController> addMethod, IEnumerable<TECControllerType> controllerTypes)
        {
            Quantity = 1;
            add = addMethod;
            ControllerTypes = new List<TECControllerType>(controllerTypes);
            toAdd = new TECController(ControllerTypes[0], false);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }
        
        public Action<object> Added { get; }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            for(int x = 0; x < Quantity; x++)
            {
                var controller = new TECController(ToAdd, ToAdd.IsTypical);
                add(controller);
                Added?.Invoke(controller);
            }
        }

    }
}
