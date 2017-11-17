﻿using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddSystemVM : ViewModelBase, IAddVM
    {
        private TECScopeManager parent;
        private TECSystem toAdd;
        private int quantity;

        public TECSystem ToAdd
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

        public AddSystemVM(TECScopeManager scopeManager)
        {
            Quantity = 1;
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
            for (int x = 0; x < Quantity; x++)
            {
                if (parent is TECBid bid)
                {
                    TECTypical typical = new TECTypical(toAdd, bid);
                    bid.Systems.Add(typical);
                    Added?.Invoke(typical);
                }
                else if (parent is TECTemplates templates)
                {
                    var system = new TECSystem(ToAdd, ToAdd.IsTypical, templates);
                    templates.SystemTemplates.Add(system);
                    Added?.Invoke(system);
                }
            }
            
        }
    }
}
