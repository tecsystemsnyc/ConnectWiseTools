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

        public TECSubScope ToAdd
        {
            get { return toAdd; }
            set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
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
            parent.SubScope.Add(ToAdd);
            Added?.Invoke(ToAdd);
        }

    }
}
