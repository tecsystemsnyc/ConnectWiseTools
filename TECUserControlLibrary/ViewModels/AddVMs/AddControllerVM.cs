using EstimatingLibrary;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddControllerVM :  AddVM
    {
        private TECSystem parent;
        private TECController toAdd;
        private int quantity;
        private Action<TECController> add;
        private TECControllerType noneControllerType;
        private string _hintText;
        private TECControllerType _selectedType;
        private bool isTypical = false;

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
        public string HintText
        {
            get { return _hintText; }
            set
            {
                _hintText = value;
                RaisePropertyChanged("HintText");
            }
        }
        public TECControllerType SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                RaisePropertyChanged("SelectedType");
                if(value != null)
                {
                    ToAdd.Type = SelectedType;
                }
            }
        }

        public List<TECControllerType> ControllerTypes { get; private set; }

        public AddControllerVM(TECSystem parentSystem, IEnumerable<TECControllerType> controllerTypes, TECScopeManager scopeManager) : base(scopeManager)
        {
            setup(controllerTypes, parentSystem.IsTypical);
            parent = parentSystem;
            isTypical = parent.IsTypical;
            add = controller =>
            {
                parent.AddController(controller);
            };
            
        }

        public AddControllerVM(Action<TECController> addMethod, IEnumerable<TECControllerType> controllerTypes, TECScopeManager scopeManager) : base(scopeManager)
        {
            setup(controllerTypes, false);
            add = addMethod;
        }

        private void setup(IEnumerable<TECControllerType> controllerTypes, bool isTypical)
        {
            Quantity = 1;
            noneControllerType = new TECControllerType(new TECManufacturer());
            noneControllerType.Name = "Select Controller Type";
            toAdd = new TECController(noneControllerType, isTypical);
            ControllerTypes = new List<TECControllerType>(controllerTypes);
            ControllerTypes.Insert(0, noneControllerType);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
            SelectedType = noneControllerType;
            //PropertiesVM = new PropertiesVM()

        }
        
        private bool addCanExecute()
        {
            bool canAdd = toAdd.Type != noneControllerType;
            if (canAdd)
            {
                HintText = null;
                return true;
            } else
            {
                HintText = "Select a Controller Type";
                return false;
            }
        }
        private void addExecute()
        {
            for(int x = 0; x < Quantity; x++)
            {
                var controller = AsReference ? ToAdd : new TECController(ToAdd, isTypical);
                add(controller);
                Added?.Invoke(controller);
            }
        }
    }
}
