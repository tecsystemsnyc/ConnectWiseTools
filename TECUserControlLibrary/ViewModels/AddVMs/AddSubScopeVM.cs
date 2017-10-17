using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddSubScopeVM : ViewModelBase, IAddVM
    {
        private TECEquipment parent;
        private TECSubScope toAdd;
        private int quantity;

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
            parent = parentEquipment;
            toAdd = new TECSubScope(parentEquipment.IsTypical);
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
                parent.SubScope.Add(subScope);
                Added?.Invoke(subScope);
            }
        }

    }
}
