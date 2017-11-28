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
    public class WorkBoxVM : ViewModelBase, IDropTarget
    {
        private TECScopeManager manager;

        public ObservableCollection<TECObject> BoxItems { get; }

        public WorkBoxVM(TECScopeManager manager)
        {
            this.manager = manager;
            BoxItems = new ObservableCollection<TECObject>();
        }

        public void Refresh(TECScopeManager manager)
        {
            this.manager = manager;
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is TECTypical typ)
            {
                BoxItems.Add(new TECSystem((typ as TECSystem), false, manager));
            }
            else
            {
                UIHelpers.StandardDrop(dropInfo, manager);
            }
        }
    }
}
