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
    public class AddMiscVM : ViewModelBase, IAddVM
    {
        private TECSystem parent;
        private TECMisc toAdd;

        public TECMisc ToAdd
        {
            get { return toAdd; }
            set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }
        public ICommand AddCommand { get; private set; }

        public AddMiscVM(TECSystem parentSystem)
        {
            parent = parentSystem;
            toAdd = new TECMisc(CostType.TEC, parentSystem.IsTypical);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        public Action<object> Added { get; }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            parent.MiscCosts.Add(ToAdd);
            Added?.Invoke(ToAdd);
        }

    }
}
