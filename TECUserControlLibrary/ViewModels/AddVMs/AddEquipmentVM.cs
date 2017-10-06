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

        public TECEquipment ToAdd
        {
            get { return toAdd; }
            set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }
        public ICommand AddCommand { get; private set; }

        public AddEquipmentVM(TECSystem parentSystem)
        {
            parent = parentSystem;
            toAdd = new TECEquipment(parentSystem.IsTypical);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        public Action<object> Added { get; }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            parent.Equipment.Add(ToAdd);
            Added?.Invoke(ToAdd);
        }

    }
}
