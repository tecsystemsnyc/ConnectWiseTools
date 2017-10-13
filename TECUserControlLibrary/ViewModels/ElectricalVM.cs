using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ElectricalVM: ViewModelBase, IDropTarget
    {
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }
        
        public SystemsVM SystemsVM { get; set; }

        private TECSystem _selectedSystem;
        public TECSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
            }
        }

        public ElectricalVM(TECBid bid)
        {
            Bid = bid;
            SystemsVM = new SystemsVM(bid);
            SystemsVM.SelectionChanged += selectionChanged;
            SystemsVM.DataGridVisibilty.SystemExpander = System.Windows.Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemModifierPrice = System.Windows.Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemPointNumber = System.Windows.Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemQuantity = System.Windows.Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemSubScopeCount = System.Windows.Visibility.Collapsed; 
            SystemsVM.DataGridVisibilty.SystemTotalPrice = System.Windows.Visibility.Collapsed;
            SystemsVM.DataGridVisibilty.SystemUnitPrice = System.Windows.Visibility.Collapsed;
        }

        public void Refresh(TECBid bid)
        {
            Bid = bid;
            SystemsVM.Refresh(bid);
        }

        private void selectionChanged(object item)
        {
            if(item is TECSystem)
            {
                SelectedSystem = item as TECSystem;
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
        }
        public void Drop(IDropInfo dropInfo)
        {
        }
    }
}