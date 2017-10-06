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
    public class AddPanelVM : ViewModelBase, IAddVM
    {
        private TECSystem parent;
        private TECPanel toAdd;

        public TECPanel ToAdd
        {
            get { return toAdd; }
            set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }
        public List<TECPanelType> PanelTypes { get; private set; }
        public ICommand AddCommand { get; private set; }

        public AddPanelVM(TECSystem parentSystem, IEnumerable<TECPanelType> panelTypes)
        {
            parent = parentSystem;
            PanelTypes = new List<TECPanelType>(panelTypes);
            toAdd = new TECPanel(PanelTypes[0], parentSystem.IsTypical);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        public Action<object> Added { get; }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            parent.Panels.Add(ToAdd);
            Added?.Invoke(ToAdd);
        }

    }
}
