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

        public ScopeCollectionsTabVM ScopeCollection { get; set; }
        public MaterialVM MaterialsTab { get; set; }
        public ControllersPanelsVM ControllersPanelsVM { get; set; }
        public SystemHierarchyVM SystemHierarchyVM { get; set; }
        public PropertiesVM PropertiesVM { get; set; }

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
            }
        }
        public ICommand AddParameterCommand { get; private set; }

        public TemplatesEditorVM(TECTemplates templates)
        {
            Templates = templates;
            ScopeCollection = new ScopeCollectionsTabVM(templates);
            MaterialsTab = new MaterialVM(templates);
            MaterialsTab.SelectionChanged += obj => {
                Selected = obj;
            };
            MaterialsTab.DragHandler += DragOver;
            MaterialsTab.DropHandler += Drop;
            ControllersPanelsVM = new ControllersPanelsVM(templates);
            ControllersPanelsVM.SelectionChanged += obj => { Selected = obj; };
            SystemHierarchyVM = new SystemHierarchyVM(templates);
            SystemHierarchyVM.Selected += obj => { Selected = obj; };

            PropertiesVM = new PropertiesVM(templates.Catalogs, templates);

            AddParameterCommand = new RelayCommand(AddParametersExecute);

        }

        public event Action<Object> SelectionChanged;

        public void Refresh(TECTemplates templates)
        {
            Templates = templates;
            ScopeCollection.Refresh(templates);
            MaterialsTab.Refresh(templates);
            ControllersPanelsVM.Refresh(templates);
            SystemHierarchyVM.Refresh(templates);
            PropertiesVM.Refresh(templates.Catalogs, templates);
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
