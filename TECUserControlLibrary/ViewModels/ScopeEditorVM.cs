using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ScopeEditorVM : ViewModelBase, IDropTarget
    {
        //Initializer
        public ScopeEditorVM(TECBid bid, TECTemplates templates, ChangeWatcher watcher)
        {
            Bid = bid;
            Templates = templates;

            setupScopeCollection();
            setupControllersPanelsTab();
            setupMiscVM();
            TypicalEditVM = new SystemHierarchyVM(bid);
            TypicalEditVM.Selected += item =>
            {
                Selected = item;
            };
            InstanceEditVM = new TypicalHierarchyVM(bid);
            InstanceEditVM.Selected += item => { Selected = item; };
            NetworkVM = new NetworkVM(bid, watcher);
            DGTabIndex = GridIndex.Systems;
            TemplatesVisibility = Visibility.Visible;
        }

        #region Properties
        private TECObject selected;
        private GridIndex _dGTabIndex;

        public TECObject Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                RaisePropertyChanged("Selected");
            }
        }
        public GridIndex DGTabIndex
        {
            get { return _dGTabIndex; }
            set
            {
                _dGTabIndex = value;
                RaisePropertyChanged("DGTabIndex");
            }
        }

        #region Extensions
        public ScopeCollectionsTabVM ScopeCollection { get; set; }
        public ControllersPanelsVM ControllersPanelsTab { get; set; }
        public MiscCostsVM MiscVM { get; set; }
        public SystemHierarchyVM TypicalEditVM { get; set; }
        public TypicalHierarchyVM InstanceEditVM { get; set; }
        public NetworkVM NetworkVM { get; }

        #endregion

        #region Interface Properties

        #region Scope Properties
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }
        private TECTemplates _templates;

        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                RaisePropertyChanged("Bid");
            }
        }
        private TECBid _bid;
        #endregion Scope Properties

        #endregion //Interface Properties

        #region Visibility Properties
        private Visibility _templatesVisibility;
        public Visibility TemplatesVisibility
        {
            get { return _templatesVisibility; }
            set
            {
                if (value != _templatesVisibility)
                {
                    _templatesVisibility = value;
                    RaisePropertyChanged("TemplatesVisibility");
                }
            }
        }
        #endregion Visibility Properties
        #endregion //Properties

        #region Methods
        public void Refresh(TECBid bid, TECTemplates templates, ChangeWatcher watcher)
        {
            Bid = bid;
            Templates = templates;

            ScopeCollection.Refresh(Templates);
            ControllersPanelsTab.Refresh(Bid);
            MiscVM.Refresh(Bid);
            TypicalEditVM.Refresh(Bid);
            NetworkVM.Refresh(bid, watcher);

        }

        #region Setup Extensions

        private void setupScopeCollection()
        {
            ScopeCollection = new ScopeCollectionsTabVM(Templates);
            ScopeCollection.DragHandler += DragOver;
            ScopeCollection.DropHandler += Drop;
        }
        private void setupControllersPanelsTab()
        {
            ControllersPanelsTab = new ControllersPanelsVM(Bid);
            ControllersPanelsTab.SelectionChanged += obj =>
            {
                Selected = obj as TECObject;

            };
        }
        private void setupMiscVM()
        {
            MiscVM = new MiscCostsVM(Bid);
            MiscVM.SelectionChanged += misc =>
            {
                Selected = misc;
            };
        }
        #endregion

        #region Drag Drop
        public void DragOver(IDropInfo dropInfo)
        {
            if(dropInfo.Data is TECSystem)
            {
                UIHelpers.SystemToTypicalDragOver(dropInfo);
            }
            else
            {
                UIHelpers.StandardDragOver(dropInfo);
            }
        }
        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is TECSystem)
            {
                UIHelpers.SystemToTypicalDrop(dropInfo, Bid);
            }
            else
            {
                UIHelpers.StandardDrop(dropInfo, Bid);
            }
        }
        #endregion

        #region Helper Methods
        #endregion //Helper Methods

        #region Event Handlers
        #endregion
        #endregion //Methods
    }
}