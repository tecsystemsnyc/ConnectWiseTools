using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    class WorkBoxVM : ViewModelBase, IDropTarget
    {
        private readonly TECScopeManager manager;

        public ObservableCollection<TECObject> BoxItems { get; }

        public WorkBoxVM(TECScopeManager manager)
        {
            this.manager = manager;
            BoxItems = new ObservableCollection<TECObject>();
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            UIHelpers.StandardDrop(dropInfo, manager);
        }
    }
}
