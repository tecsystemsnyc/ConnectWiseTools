using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TemplateBuilder.MVVM
{
    public class TemplatesEditorVM : ViewModelBase, EditorVM, IDropTarget
    {
        private TECTemplates _templates;
        private object _selected;

        public ScopeCollectionsTabVM ScopeCollection { get; }
        public MaterialVM MaterialsTab { get; }
        public ControllersPanelsVM ControllersPanelsVM { get; }
        public SystemHierarchyVM SystemHierarchyVM { get; }
        public EquipmentHierarchyVM EquipmentHierarchyVM { get; }
        public SubScopeHierarchyVM SubScopeHierarchyVM { get; }
        public PropertiesVM PropertiesVM { get; }
        public MiscCostsVM MiscVM { get; }

        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }
        public object Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged("Selected");
                SelectionChanged?.Invoke(value);
                PropertiesVM.Selected = value as TECObject;
            }
        }
        public ICommand AddParameterCommand { get; private set; }

        public TemplatesEditorVM(TECTemplates templates)
        {
            Templates = templates;
            ScopeCollection = new ScopeCollectionsTabVM(templates, templates.Catalogs);
            MaterialsTab = new MaterialVM(templates);
            MaterialsTab.SelectionChanged += obj => {
                Selected = obj;
            };
            ControllersPanelsVM = new ControllersPanelsVM(templates);
            ControllersPanelsVM.SelectionChanged += obj => { Selected = obj; };
            SystemHierarchyVM = new SystemHierarchyVM(templates, true);
            SystemHierarchyVM.Selected += obj => { Selected = obj; };
            EquipmentHierarchyVM = new EquipmentHierarchyVM(templates);
            EquipmentHierarchyVM.Selected += obj => { Selected = obj; };
            SubScopeHierarchyVM = new SubScopeHierarchyVM(templates);
            SubScopeHierarchyVM.Selected += obj => { Selected = obj; };

            MiscVM = new MiscCostsVM(templates);
            MiscVM.SelectionChanged += obj => { Selected = obj; };
            
            PropertiesVM = new PropertiesVM(templates.Catalogs, templates);

            AddParameterCommand = new RelayCommand(AddParametersExecute);

        }

        public event Action<Object> SelectionChanged;

        public void Refresh(TECTemplates templates)
        {
            Templates = templates;
            ScopeCollection.Refresh(templates, templates.Catalogs);
            MaterialsTab.Refresh(templates);
            ControllersPanelsVM.Refresh(templates);
            SystemHierarchyVM.Refresh(templates);
            EquipmentHierarchyVM.Refresh(templates);
            SubScopeHierarchyVM.Refresh(templates);
            PropertiesVM.Refresh(templates.Catalogs, templates);
            MiscVM.Refresh(templates);
        }

        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            UIHelpers.StandardDrop(dropInfo, Templates);
        }

        private void AddParametersExecute()
        {
            Templates.Parameters.Add(new TECParameters(Guid.NewGuid()));
        }
    }
}
