using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddPointVM : ViewModelBase, IAddVM
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
        public ICommand AddCommand { get; private set; }

        public AddPointVM(TECSubScope parentSubScope)
        {
            parent = parentSubScope;
            isTypical = parentSubScope.IsTypical;
            toAdd = new TECPoint(parentSubScope.IsTypical);
            ToAdd.Quantity = 1;
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        public Action<object> Added { get; }

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
