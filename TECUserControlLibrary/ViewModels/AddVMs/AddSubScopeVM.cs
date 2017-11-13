using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddSubScopeVM : ViewModelBase, IAddVM
    {
        private TECEquipment parent;
        private TECSubScope toAdd;
        private int quantity;
        private Action<TECSubScope> add;

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
        public ICommand AddCommand { get; private set; }

        public AddSubScopeVM(TECEquipment parentEquipment)
        {
            Quantity = 1;
            parent = parentEquipment;
            toAdd = new TECSubScope(parentEquipment.IsTypical);
            AddCommand = new RelayCommand(addExecute, addCanExecute);

            add = subScope =>
            {
                parent.SubScope.Add(subScope);

            };
        }

        public AddSubScopeVM(Action<TECSubScope> addMethod)
        {
            add = addMethod;
            Quantity = 1;
            toAdd = new TECSubScope(false);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        public Action<object> Added { get; }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            for (int x = 0; x < Quantity; x++)
            {
                var subScope = new TECSubScope(ToAdd, ToAdd.IsTypical);
                add(subScope);
                Added?.Invoke(subScope);
            }
        }

    }
}
