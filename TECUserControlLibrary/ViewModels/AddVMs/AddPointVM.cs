using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddPointVM : AddVM
    {
        private TECSubScope parent;
        private TECPoint toAdd;
        private bool isTypical;

        public TECPoint ToAdd
        {
            get { return toAdd; }
            set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }

        public AddPointVM(TECSubScope parentSubScope, TECScopeManager scopeManager) : base(scopeManager)
        {
            parent = parentSubScope;
            isTypical = parentSubScope.IsTypical;
            toAdd = new TECPoint(parentSubScope.IsTypical);
            ToAdd.Quantity = 1;
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            var newPoint = new TECPoint(ToAdd, isTypical);
            parent.Points.Add(newPoint);
            Added?.Invoke(newPoint);
        }
        

    }
}
