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
        public List<IOType> PossibleTypes { get; }

        public AddPointVM(TECSubScope parentSubScope, TECScopeManager scopeManager) : base(scopeManager)
        {
            parent = parentSubScope;
            isTypical = parentSubScope.IsTypical;
            toAdd = new TECPoint(parentSubScope.IsTypical);
            ToAdd.Quantity = 1;
            PossibleTypes = possibleTypes(parentSubScope);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        private bool addCanExecute()
        {
            TECConnection connection = parent.Connection;
            if(connection == null)
            {
                return true;
            }
            else
            {
                TECIO proposedIO = new TECIO(ToAdd.Type);
                proposedIO.Quantity = ToAdd.Quantity;
                bool containsIO = connection.ParentController.AvailableIO.Contains(proposedIO);
                bool isNetworkIO = connection is TECNetworkConnection netConnect && netConnect.IOType == ToAdd.Type;
                return containsIO || isNetworkIO;
            }
        }
        private void addExecute()
        {
            var newPoint = new TECPoint(ToAdd, isTypical);
            parent.Points.Add(newPoint);
            Added?.Invoke(newPoint);
        }
        
        private List<IOType> possibleTypes(TECSubScope subScope)
        {
            List<IOType> resultList = new List<IOType>();
            if (subScope.Points.Count == 0)
            {
                resultList.AddRange(TECIO.NetworkIO);
                resultList.AddRange(TECIO.PointIO);
            }
            else if (subScope.IsNetwork)
            {
                IOType type = subScope.Points[0].Type;
                ToAdd.Type = type;
                resultList.Add(type);
            }
            else
            {
                resultList.AddRange(TECIO.PointIO);
            }
            return resultList;
        }

    }
}
