using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
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
        private bool _readOnly;
        private TECObject _selected;
        private string _templateText;

        public TECCatalogs Catalogs
        {
            get { return _catalogs; }
            set
            {
                _catalogs = value;
                RaisePropertyChanged("Catalogs");
            }
        }
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                RaisePropertyChanged("ReadOnly");
            }

        }
        public TECObject Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                setReadOnly(value);
                RaisePropertyChanged("Selected");
                if (IsTemplates)
                {
                    TemplateText = getTemplateText(value);
                }
            }
        }

        private string getTemplateText(TECObject item)
        {
            TECTemplates templates = scopeManager as TECTemplates;
            if(item is TECSubScope subScope)
            {
                if (templates.SubScopeTemplates.Contains(subScope))
                {
                    return "Reference Template";
                }
            } else if(item is TECEquipment equipment)
            {
                if (templates.EquipmentTemplates.Contains(equipment))
                {
                    return "Reference Template";
                }
            }
            return "Instance Template";
        }

        public bool IsTemplates { get; }
        public string TemplateText
        {
            get { return _templateText; }
            set
            {
                _templateText = value;
                RaisePropertyChanged("TemplateText");
            }
        }

        private void setReadOnly(TECObject value)
        {
            ReadOnly = false;
            if(scopeManager is TECBid)
            {
                if(value is TECHardware)
                {
                    ReadOnly = true;
                }
            }
            
        }

        public string TestString
        {
            get { return "test"; }
        }

        public PropertiesVM(TECCatalogs catalogs, TECScopeManager scopeManager)
        {
            IsTemplates = scopeManager is TECTemplates;
            TemplateText = "Instance Template";
            Refresh(catalogs, scopeManager);
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
