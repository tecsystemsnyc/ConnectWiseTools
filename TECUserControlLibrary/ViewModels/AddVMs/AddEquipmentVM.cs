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
    public class AddEquipmentVM : ViewModelBase, IAddVM
    {
        private TECSystem parent;
        private TECEquipment toAdd;
        private int quantity;

        public TECEquipment ToAdd
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

        public AddEquipmentVM(TECSystem parentSystem)
        {
            parent = parentSystem;
            toAdd = new TECEquipment(parentSystem.IsTypical);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
            Quantity = 1;
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
                var equipment = new TECEquipment(ToAdd, ToAdd.IsTypical);
                parent.Equipment.Add(equipment);
                Added?.Invoke(equipment);
            }
        }

    }
}
