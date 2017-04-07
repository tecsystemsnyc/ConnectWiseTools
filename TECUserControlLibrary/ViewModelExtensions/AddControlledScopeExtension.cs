using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System.Windows.Input;
using System;
using System.Reflection;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;

namespace TECUserControlLibrary.ViewModelExtensions
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddControlledScopeExtension : ViewModelBase, IDropTarget
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

        private TECControlledScope _controlledScope;
        public TECControlledScope ControlledScope
        {
            get { return _controlledScope; }
            set
            {
                _controlledScope = value;
                RaisePropertyChanged("ControlledScope");
            }
        }

        private int _controlledScopeQuantity;
        public int ControlledScopeQuantity
        {
            get { return _controlledScopeQuantity; }
            set
            {
                _controlledScopeQuantity = value;
                RaisePropertyChanged("ControlledScopeQuantity");
            }
        }

        private ObservableCollection<TECControlledScope> _scopeSource;
        public ObservableCollection<TECControlledScope> ScopeSource
        {
            get { return _scopeSource; }
            set
            {
                _scopeSource = value;
                RaisePropertyChanged("ScopeSource");
            }
        }

        public ICommand AddControlledScopeCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the AddControlledScopeExtension class.
        /// </summary>
        public AddControlledScopeExtension(TECBid bid)
        {
            _bid = bid;
            AddControlledScopeCommand = new RelayCommand(addControlledScopeExecute, addControlledScopeCanExecute);
        }

        public void Refresh(TECBid bid)
        {
            Bid = bid;
            ScopeSource = new ObservableCollection<TECControlledScope>();
        }

        private void addControlledScopeExecute()
        {
            for(int x = 0; x < ControlledScopeQuantity; x++)
            {
                Bid.addControlledScope(ControlledScope);
            }
        }
        private bool addControlledScopeCanExecute()
        {
            if(ControlledScope != null && ControlledScopeQuantity > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data;
            Type sourceType = sourceItem.GetType();

            if (sourceType == typeof(TECControlledScope))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            Type sourceType = dropInfo.Data.GetType();

            if (dropInfo.Data is TECControlledScope)
            {
                ControlledScope = (dropInfo.Data as TECControlledScope).Copy() as TECControlledScope;
            }
        }
    }
}