using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddEquipmentVM : AddVM
    {
        private TECSystem parent;
        private TECEquipment toAdd;
        private int quantity;
        private Action<TECEquipment> add;
        private bool isTypical = false;

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

        public AddEquipmentVM(TECSystem parentSystem, TECScopeManager scopeManager) : base(scopeManager)
        {
            parent = parentSystem;
            isTypical = parent.IsTypical;
            toAdd = new TECEquipment(parentSystem.IsTypical);
            add = equip =>
            {
                parentSystem.Equipment.Add(equip);
            };
            AddCommand = new RelayCommand(addExecute, addCanExecute);
            Quantity = 1;
        }
        public AddEquipmentVM(Action<TECEquipment> addMethod, TECScopeManager scopeManager) : base(scopeManager)
        {
            toAdd = new TECEquipment(false);
            add = addMethod;
            AddCommand = new RelayCommand(addExecute, addCanExecute);
            Quantity = 1;
        }
        
        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            for (int x = 0; x < Quantity; x++)
            {
                var equipment = new TECEquipment(ToAdd, isTypical);
                add(equipment);
                Added?.Invoke(equipment);
            }
        }
        
    }
}
