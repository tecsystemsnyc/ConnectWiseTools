using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddPanelVM : AddVM
    {
        private TECSystem parent;
        private TECPanel toAdd;
        private int quantity;
        private Action<TECPanel> add;
        private bool isTypical = false;

        public TECPanel ToAdd
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
        public List<TECPanelType> PanelTypes { get; private set; }

        public AddPanelVM(TECSystem parentSystem, IEnumerable<TECPanelType> panelTypes, TECScopeManager scopeManager) : base(scopeManager)
        {
            Quantity = 1;
            parent = parentSystem;
            isTypical = parent.IsTypical;
            add = panel =>
            {
                parent.Panels.Add(panel);
            };
            PanelTypes = new List<TECPanelType>(panelTypes);
            toAdd = new TECPanel(PanelTypes[0], parentSystem.IsTypical);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }
        public AddPanelVM(Action<TECPanel> addMethod, IEnumerable<TECPanelType> panelTypes, TECScopeManager scopeManager) : base(scopeManager)
        {
            Quantity = 1;
            add = addMethod;
            PanelTypes = new List<TECPanelType>(panelTypes);
            toAdd = new TECPanel(PanelTypes[0], false);
            AddCommand = new RelayCommand(addExecute, addCanExecute);

        }


        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            for (int x = 0; x < Quantity; x++)
            {
                var panel = new TECPanel(ToAdd, isTypical);
                add(panel);
                Added?.Invoke(panel);
            }
        }
        

    }
}
