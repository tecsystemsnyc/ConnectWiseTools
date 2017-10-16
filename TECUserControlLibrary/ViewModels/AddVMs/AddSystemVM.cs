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
    public class AddSystemVM : ViewModelBase, IAddVM
    {
        private TECScopeManager parent;
        private TECSystem toAdd;

        public TECSystem ToAdd
        {
            get { return toAdd; }
            set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }
        public ICommand AddCommand { get; private set; }

        public AddSystemVM(TECScopeManager scopeManager)
        {
            parent = scopeManager;
            toAdd = new TECSystem(false);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        public Action<object> Added { get; }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            if(parent is TECBid bid)
            {
                TECTypical typical = new TECTypical(ToAdd);
                bid.Systems.Add(typical);
                Added?.Invoke(ToAdd);
            }
            else if (parent is TECTemplates templates)
            {
                templates.SystemTemplates.Add(ToAdd);
                Added?.Invoke(ToAdd);
            }
            
        }
    }
}
