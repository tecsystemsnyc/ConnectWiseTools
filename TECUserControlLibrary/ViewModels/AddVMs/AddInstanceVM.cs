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
    public class AddInstanceVM : ViewModelBase, IAddVM
    {
        private TECTypical parent;
        private TECBid bid;
        private int quantity;
        private TECSystem toAdd;

        public Action<object> Added { get; }

        public ICommand AddCommand { get; private set; }
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }
        public TECSystem ToAdd
        {
            get { return toAdd; }
            set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }

        public AddInstanceVM(TECTypical typical, TECBid bid)
        {
            toAdd = new TECSystem(false);
            Quantity = 1;
            parent = typical;
            this.bid = bid; 
            AddCommand = new RelayCommand(addExecute, canAdd);
        }

        private void addExecute()
        {
            for(int x = 0; x < quantity; x++)
            {
                TECSystem newSystem = parent.AddInstance(bid);
                newSystem.Name = ToAdd.Name;
            }
        }

        private bool canAdd()
        {
            if(Quantity > 0)
            {
                return true;
            }
            return false;
        }
    }
}
