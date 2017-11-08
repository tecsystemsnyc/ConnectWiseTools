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
        public EquipmentVM ScopeDataGrid { get; set; }
        public EditTabVM EditTab { get; set; }
        public MaterialVM MaterialsTab { get; set; }
        public ControllersPanelsVM ControllersPanelsVM { get; set; }
        public SystemHierarchyVM SystemHierarchyVM { get; set; }

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
                SelectionChanged?.Invoke(value);
            }
        }
        public ICommand AddParameterCommand { get; private set; }

        public TemplatesEditorVM(TECTemplates templates)
        {
            Templates = templates;
            EditTab = new EditTabVM(templates);
            SelectionChanged += EditTab.updateSelection;
            EditTab.DragHandler += DragOver;
            EditTab.DropHandler += Drop;
            ScopeCollection = new ScopeCollectionsTabVM(templates);
            ScopeDataGrid = new EquipmentVM(templates);
            ScopeDataGrid.SelectionChanged += EditTab.updateSelection;
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            MaterialsTab = new MaterialVM(templates);
            MaterialsTab.SelectionChanged += EditTab.updateSelection;
            MaterialsTab.DragHandler += DragOver;
            MaterialsTab.DropHandler += Drop;
            ControllersPanelsVM = new ControllersPanelsVM(templates);
            ControllersPanelsVM.SelectionChanged += EditTab.updateSelection;
            SystemHierarchyVM = new SystemHierarchyVM(templates);
            SystemHierarchyVM.Selected += EditTab.updateSelection;

            AddParameterCommand = new RelayCommand(AddParametersExecute);

        }

        public event Action<Object> SelectionChanged;

        public void Refresh(TECTemplates templates)
        {
            Templates = templates;
            ScopeCollection.Refresh(templates);
            ScopeDataGrid.Refresh(templates);
            EditTab.Refresh(templates);
            MaterialsTab.Refresh(templates);
            ControllersPanelsVM.Refresh(templates);
            SystemHierarchyVM.Refresh(templates);
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
