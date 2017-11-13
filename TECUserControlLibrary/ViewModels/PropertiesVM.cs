using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class PropertiesVM : ViewModelBase, IDropTarget
    {
        private TECCatalogs _catalogs;
        private TECScopeManager scopeManager;

        public TECCatalogs Catalogs
        {
            get { return _catalogs; }
            set
            {
                _catalogs = value;
                RaisePropertyChanged("Catalogs");
            }
        }

        public PropertiesVM(TECCatalogs catalogs, TECScopeManager scopeManager)
        {
            Catalogs = catalogs;
            this.scopeManager = scopeManager;
        }

        public void Refresh(TECCatalogs catalogs, TECScopeManager scopeManager)
        {
            Catalogs = catalogs;
            this.scopeManager = scopeManager;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }

        public void Drop(IDropInfo dropInfo)
        {
            UIHelpers.StandardDrop(dropInfo, scopeManager);
        }
    }
}
