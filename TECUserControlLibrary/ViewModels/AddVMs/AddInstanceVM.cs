using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddInstanceVM : AddVM
    {
        private TECTypical parent;
        private TECBid bid;
        private int quantity;
        private TECSystem toAdd;

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

        public AddInstanceVM(TECTypical typical, TECBid bid) : base(bid)
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
