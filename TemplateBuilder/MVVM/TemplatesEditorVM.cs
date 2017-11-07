using EstimatingLibrary;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.ViewModels;

namespace TemplateBuilder.MVVM
{
    public class TemplatesEditorVM : EditorVM
    {
        public ScopeCollectionsTabVM ScopeCollection { get; set; }
        public EquipmentVM ScopeDataGrid { get; set; }
        public EditTabVM EditTab { get; set; }
        public MaterialVM MaterialsTab { get; set; }
        public ControllersPanelsVM ControllersPanelsVM { get; set; }
        public SystemHierarchyVM SystemHierarchyVM { get; set; }

        public TemplatesEditorVM(TECTemplates templates)
        {
            ScopeCollection = new ScopeCollectionsTabVM(templates);
            ScopeDataGrid = new EquipmentVM(templates);
            EditTab = new EditTabVM(templates);
            MaterialsTab = new MaterialVM(templates);
            ControllersPanelsVM = new ControllersPanelsVM(templates);
            SystemHierarchyVM = new SystemHierarchyVM(templates);
        }

        public void Refresh(TECTemplates templates)
        {
            ScopeCollection.Refresh(templates);
            ScopeDataGrid.Refresh(templates);
            EditTab.Refresh(templates);
            MaterialsTab.Refresh(templates);
            ControllersPanelsVM.Refresh(templates);
            SystemHierarchyVM.Refresh(templates);
        }
    }
}
