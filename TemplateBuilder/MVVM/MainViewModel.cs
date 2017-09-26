using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System.IO;
using System.Windows;
using System.Windows.Input;
using DebugLibrary;
using TECUserControlLibrary.ViewModels;
using TECUserControlLibrary.Utilities;
using EstimatingUtilitiesLibrary.Database;
using System;

namespace TemplateBuilder.MVVM
{
    public class MainViewModel : BuilderViewModel, IDropTarget
    {
        #region Constants
        const string APPDATA_FOLDER = @"TECSystems\";
        const string TEMPLATES_FILE_NAME = @"TECTemplates.tdb";
        const string SPLASH_TITLE = "Welcome to Template Builder";
        const string SPLASH_SUBTITLE = "Please select object templates or create new templates";
        #endregion
        #region Fields
        private TemplateGridIndex _DGTabIndex;
        private Visibility _templatesVisibility;

        #endregion
        #region Constructors
        public MainViewModel() : base(SPLASH_TITLE, SPLASH_SUBTITLE, false)
        {

        }

        #endregion
        #region Properties
        public override Visibility TemplatesVisibility
        {
            get
            { return _templatesVisibility; }
            set
            {
                _templatesVisibility = value;
                RaisePropertyChanged("TemplatesVisibility");
            }
        }
        public ScopeCollectionsTabVM ScopeCollection { get; set; }
        public EquipmentVM ScopeDataGrid { get; set; }
        public EditTabVM EditTab { get; set; }
        public MaterialVM MaterialsTab { get; set; }
        public TypicalSystemVM TypicalSystemsTab { get; set; }
        public ControllersPanelsVM ControllersPanelsVM { get; set; }
        public TECTemplates Templates
        {
            get { return workingScopeManager as TECTemplates; }
            set
            {
                workingScopeManager = value;
            }
        }
        public TemplateGridIndex DGTabIndex
        {
            get
            {
                return _DGTabIndex;
            }
            set
            {
                _DGTabIndex = value;
                setVisibility(_DGTabIndex);
                RaisePropertyChanged("DGTabIndex");
            }
        }
        public ICommand RefreshCommand { get; private set; }
        
        protected override TECScopeManager workingScopeManager
        {
            get
            { return base.workingScopeManager; }
            set
            {
                base.workingScopeManager = value;
                RaisePropertyChanged("Templates");
                refresh();
            }
        }
        protected override string defaultSaveFileName
        {
            get
            {
                return "Templates";
            }
        }
        protected override string TemplatesFilePath
        {
            get { return Properties.Settings.Default.TemplatesFilePath; }
            set
            {
                if (Properties.Settings.Default.TemplatesFilePath != value)
                {
                    Properties.Settings.Default.TemplatesFilePath = value;
                    Properties.Settings.Default.Save();
                    SettingsVM.TemplatesLoadPath = TemplatesFilePath;
                }
            }
        }
        protected override string saveFilePath
        {
            get
            {
                return TemplatesFilePath;
            }

            set
            {
                TemplatesFilePath = value;
            }
        }
        protected override bool TemplatesHidden
        {
            get
            {
                return Properties.Settings.Default.TemplatesHidden;
            }
            set
            {
                if (Properties.Settings.Default.TemplatesHidden != value)
                {
                    Properties.Settings.Default.TemplatesHidden = value;
                    RaisePropertyChanged("TemplatesHidden");
                    TemplatesHiddenChanged();
                    Properties.Settings.Default.Save();
                }
            }
        }
        protected override string ScopeDirectoryPath
        {
            get { return Properties.Settings.Default.ScopeDirectoryPath; }
            set
            {
                Properties.Settings.Default.ScopeDirectoryPath = value;
                Properties.Settings.Default.Save();
            }
        }
        protected override string startupFilePath
        {
            get
            {
                return Properties.Settings.Default.StartupFile;
            }

            set
            {
                Properties.Settings.Default.StartupFile = value;
                Properties.Settings.Default.Save();
            }
        }
        protected override string defaultDirectory
        {
            get
            {
                return (Properties.Settings.Default.DefaultDirectory);
            }

            set
            {
                Properties.Settings.Default.DefaultDirectory = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion
        #region Methods
        protected override void setupCommands()
        {
            base.setupCommands();
            RefreshCommand = new RelayCommand(RefreshTemplatesExecute, RefreshCanExecute);
        }
        protected override void setupExtensions(MenuType menuType)
        {
            base.setupExtensions(menuType);
            setupScopeCollecion();
            setupEditTab();
            setupScopeDataGrid();
            setupMaterialsTab();
            setupTypicalSystemseTab();
            setupControllersPanelsVM();
        }
        protected override void setupMenu(MenuType menuType)
        {
            base.setupMenu(menuType);
            MenuVM.RefreshTemplatesCommand = RefreshCommand;
            MenuVM.LoadTemplatesCommand = LoadCommand;
        }
        protected override TECScopeManager NewScopeManager()
        {
            return new TECTemplates();
        }
        protected override void startUp(string mainPath, string templatesPath)
        {
            if (mainPath == "")
            {
                isNew = true;
            }
            else
            {
                isNew = false;
            }

            getTemplates();

            buildTitleString();
            setupCommands();
            setupExtensions(MenuType.TB);

            DGTabIndex = TemplateGridIndex.Systems;

            workingFileParameters = UIHelpers.EstimateFileParameters;
            CurrentVM = this;
        }
        protected override void startFromFile()
        {
            throw new NotImplementedException();
        }

        private void refresh()
        {
            if (ScopeCollection != null)
            {
                ScopeCollection.Refresh(Templates);
                ScopeDataGrid.Refresh(Templates);
                EditTab.Refresh(Templates);
                MaterialsTab.Refresh(Templates);
                //TypicalSystemsTab.Refresh(Templates);
                ControllersPanelsVM.Refresh(Templates);
            }
        }
        private void getTemplates()
        {
            Templates = new TECTemplates();

            if ((TemplatesFilePath != "") && (File.Exists(TemplatesFilePath)))
            {
                if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                {
                    DatabaseManager manager = new DatabaseManager(TemplatesFilePath);
                    Templates = manager.Load() as TECTemplates;
                }
                else
                {
                    DebugHandler.LogError("TECTemplates file is open elsewhere. Could not load templates. Please close the templates file and load again.");
                }
            }
            else
            {
                string message = "No templates file loaded. Would you like to load templates?";
                MessageBoxResult result = MessageBox.Show(message, "Load Templates?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    //User choose path
                    TemplatesFilePath = UIHelpers.GetLoadPath(UIHelpers.TemplatesFileParameters, defaultDirectory);

                    if (TemplatesFilePath != null)
                    {
                        if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                        {
                            DatabaseManager manager = new DatabaseManager(TemplatesFilePath);
                            Templates = manager.Load() as TECTemplates;
                        }
                        else
                        {
                            DebugHandler.LogError("Could not open file " + TemplatesFilePath + " File is open elsewhere.");
                        }
                        DebugHandler.LogDebugMessage("Finished loading templates");
                    }
                }
            }
            ResetStatus();
        }
        private void setupScopeCollecion()
        {
            ScopeCollection = new ScopeCollectionsTabVM(Templates);
            ScopeCollection.DragHandler += DragOver;
            ScopeCollection.DropHandler += Drop;
        }
        private void setupScopeDataGrid()
        {
            ScopeDataGrid = new EquipmentVM(Templates);
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            ScopeDataGrid.SelectionChanged += EditTab.updateSelection;
            ScopeDataGrid.AssignChildDelegates();
        }
        private void setupEditTab()
        {
            EditTab = new EditTabVM(Templates);
            EditTab.DragHandler += DragOver;
            EditTab.DropHandler += Drop;
        }
        private void setupMaterialsTab()
        {
            MaterialsTab = new MaterialVM(Templates);
            MaterialsTab.DragHandler += DragOver;
            MaterialsTab.DropHandler += Drop;
            MaterialsTab.SelectionChanged += EditTab.updateSelection;
        }
        private void setupTypicalSystemseTab()
        {
            //TypicalSystemsTab = new TypicalSystemVM(Templates);
            TypicalSystemsTab.DragHandler += DragOver;
            TypicalSystemsTab.DropHandler += Drop;
            TypicalSystemsTab.SelectionChanged += EditTab.updateSelection;
            TypicalSystemsTab.ComponentVM.SelectionChanged += EditTab.updateSelection;
            TypicalSystemsTab.AssignChildDelegates();
        }
        private void setupControllersPanelsVM()
        {
            ControllersPanelsVM = new ControllersPanelsVM(Templates);
            ControllersPanelsVM.DragHandler += DragOver;
            ControllersPanelsVM.DropHandler += Drop;
            ControllersPanelsVM.SelectionChanged += EditTab.updateSelection;
        }
        private void RefreshTemplatesExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (loadedStackLength > 0)
            {
                string message = "Would you like to save your changes before loading?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    if (!saveDelta(false))
                    {
                        MessageBox.Show("Save unsuccessful.");
                        return;
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            string path = TemplatesFilePath;
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    load(true, path);
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private bool RefreshCanExecute()
        {
            return !(TemplatesFilePath == "");
        }
        private void buildTitleString()
        {
            TitleString = "Template Builder";
        }
        private void setVisibility(TemplateGridIndex gridIndex)
        {
            ScopeDataGrid.NullifySelected();

            ScopeCollection.SystemsVisibility = Visibility.Collapsed;
            ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
            ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
            ScopeCollection.DevicesVisibility = Visibility.Collapsed;
            ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
            ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
            ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
            ScopeCollection.ControllerVisibility = Visibility.Collapsed;
            ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
            ScopeCollection.TagsVisibility = Visibility.Collapsed;
            ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
            ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
            ScopeCollection.PanelsVisibility = Visibility.Collapsed;
            ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
            ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

            switch (gridIndex)
            {
                case TemplateGridIndex.Equipment:
                    ScopeCollection.EquipmentVisibility = Visibility.Visible;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.TagsVisibility = Visibility.Visible;


                    ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.Equipment;

                    break;
                case TemplateGridIndex.SubScope:

                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.TagsVisibility = Visibility.Visible;

                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.SubScope;

                    break;
                case TemplateGridIndex.Devices:

                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.DevicesEditVisibility = Visibility.Visible;
                    ScopeCollection.ManufacturerVisibility = Visibility.Visible;
                    ScopeCollection.TagsVisibility = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.Devices;

                    break;
                case TemplateGridIndex.DDC:

                    ScopeCollection.ControllerEditVisibility = Visibility.Visible;
                    ScopeCollection.ControllerVisibility = Visibility.Visible;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Visible;
                    ScopeCollection.PanelsVisibility = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.Controllers;

                    break;
                case TemplateGridIndex.Materials:

                    ScopeCollection.AssociatedCostsVisibility = Visibility.Visible;
                    ScopeCollection.MiscCostVisibility = Visibility.Visible;
                    ScopeCollection.MiscWiringVisibility = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.AssociatedCosts;
                    break;
                case TemplateGridIndex.Constants:

                    ScopeCollection.TabIndex = ScopeCollectionIndex.None;

                    break;
                case TemplateGridIndex.Systems:

                    ScopeCollection.SystemsVisibility = Visibility.Visible;
                    ScopeCollection.EquipmentVisibility = Visibility.Visible;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.ControllerVisibility = Visibility.Visible;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Visible;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Visible;
                    ScopeCollection.PanelsVisibility = Visibility.Visible;

                    ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeLocation = Visibility.Collapsed;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.System;

                    break;
                default:

                    ScopeCollection.SystemsVisibility = Visibility.Visible;
                    ScopeCollection.EquipmentVisibility = Visibility.Visible;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.TagsVisibility = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.None;

                    break;
            }
        }
        #endregion
    }
}