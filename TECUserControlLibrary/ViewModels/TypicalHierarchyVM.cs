using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace TECUserControlLibrary.ViewModels
{
    public class TypicalHierarchyVM : ViewModelBase
    {
        private TECTypical _selectedTypical;

        public SystemHierarchyVM SystemHierarchyVM { get; }
        public ReadOnlyObservableCollection<TECTypical> TypicalSystems { get; }
        public TECTypical SelectedTypical
        {
            get { return _selectedTypical; }
            set
            {
                _selectedTypical = value;
                RaisePropertyChanged("SelectedTypical");
                Selected?.Invoke(value);
            }
        }

        public TypicalHierarchyVM(TECBid bid)
        {
            SystemHierarchyVM = new SystemHierarchyVM(bid);
            SystemHierarchyVM.Selected += this.Selected;
            TypicalSystems = new ReadOnlyObservableCollection<TECTypical>(bid.Systems);
        }

        public event Action<TECObject> Selected;
    }
}
