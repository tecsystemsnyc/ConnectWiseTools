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
    public class AddControllerVM : ViewModelBase, IAddVM
    {
        private TECSystem parent;
        private TECController toAdd;

        public TECController ToAdd
        {
            get { return toAdd; }
            set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }
        public List<TECControllerType> ControllerTypes { get; private set; }
        public ICommand AddCommand { get; private set; }

        public AddControllerVM(TECSystem parentSystem, IEnumerable<TECControllerType> controllerTypes)
        {
            parent = parentSystem;
            ControllerTypes = new List<TECControllerType>(controllerTypes);
            toAdd = new TECController(ControllerTypes[0], parentSystem.IsTypical);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }
        
        public Action<object> Added { get; }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            parent.AddController(ToAdd);
            Added?.Invoke(ToAdd);
        }

    }
}
