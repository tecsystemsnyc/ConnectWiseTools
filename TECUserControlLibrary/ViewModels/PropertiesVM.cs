using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
        private bool _displayReferenceProperty = false;

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
                    DisplayReferenceProperty = value is TECEquipment || value is TECSubScope ? true : false;
                    TemplateText = getTemplateText(value);
                }
            }
        }
        public bool DisplayReferenceProperty
        {
            get { return _displayReferenceProperty; }
            set
            {
                _displayReferenceProperty = value;
                RaisePropertyChanged("DisplayReferenceProperty");
            }
        }
        public RelayCommand<TECConnectionType> DeleteConnectionTypeCommand { get; private set; }

        private string getTemplateText(TECObject item)
        {
            TECTemplates templates = scopeManager as TECTemplates;
            if(templates.IsTemplateObject(item))
            {
                string parentString = "";
                if(item is TECSubScope subScope)
                {
                    TECSubScope parent = templates.SubScopeSynchronizer.GetTemplate(subScope);
                    if (item != parent)
                    {
                        parentString = String.Format(" of {0}",
                            parent.Name);
                    }
                } else if(item is TECEquipment equipment)
                {
                    TECEquipment parent = templates.EquipmentSynchronizer.GetTemplate(equipment);
                    if (item != parent)
                    {
                        parentString = String.Format(" of {0}",
                            parent.Name);
                    }
                }

                return String.Format("Reference{0}", 
                    parentString);
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
            DeleteConnectionTypeCommand = new RelayCommand<TECConnectionType>(deleteConnectionTypeExecute, canDeleteConnectionType);
            Refresh(catalogs, scopeManager);
        }

        private void deleteConnectionTypeExecute(TECConnectionType obj)
        {
            (Selected as TECDevice).ConnectionTypes.Remove(obj);
        }

        private bool canDeleteConnectionType(TECConnectionType arg)
        {
            return Selected is TECDevice;
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
