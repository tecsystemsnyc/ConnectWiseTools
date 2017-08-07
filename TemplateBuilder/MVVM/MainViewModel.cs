using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TemplateBuilder.Model;
using TECUserControlLibrary;
using System.Collections;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Reflection;
using System.Deployment.Application;
using System.ComponentModel;
using System.Windows.Controls;
using DebugLibrary;
using TECUserControlLibrary.ViewModels;
using TECUserControlLibrary.Utilities;

namespace TemplateBuilder.MVVM
{
    public class MainViewModel : BuilderViewModel, IDropTarget
    {
        //Initializer
        public MainViewModel() : base()
        {
            getTemplates();
            buildTitleString();
            setupCommands();
            setupVMs();
            DGTabIndex = TemplateGridIndex.Systems;

            ResetStatus();
        }

        #region Resources Paths
        const string APPDATA_FOLDER = @"TECSystems\";
        const string TEMPLATES_FILE_NAME = @"TECTemplates.tdb";
        #endregion //Resources Paths

        #region Properties
        #region VM Extensions
        public ScopeCollectionsTabVM ScopeCollection { get; set; }
        public EquipmentVM ScopeDataGrid { get; set; }
        public EditTabVM EditTab { get; set; }
        public MaterialVM MaterialsTab { get; set; }
        public TypicalSystemVM TypicalSystemsTab { get; set; }
        public ConstantsVM ConstantsVM { get; set; }
        public ControllersPanelsVM ControllersPanelsVM { get; set; }
        #endregion
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
        public TECTemplates Templates
        {
            get { return workingScopeManager as TECTemplates; }
            set
            {
                workingScopeManager = value;
            }
        }
        protected override string defaultSaveFileName
        {
            get
            {
                return "Templates";
            }
        }

        override protected string TemplatesFilePath
        {
            get { return Properties.Settings.Default.TemplatesFilePath; }
            set
            {
                if (Properties.Settings.Default.TemplatesFilePath != value)
                {
                    Properties.Settings.Default.TemplatesFilePath = value;
                    Properties.Settings.Default.Save();
                    TemplatesFilePathChanged();
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

        #region Interface Properties

        #region Tab Indexes
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
        private TemplateGridIndex _DGTabIndex;
        #endregion //Tab Indexes

        #endregion //Interface Properties

        #region Commands Properties
        public ICommand RefreshCommand { get; private set; }
        #endregion //Commands Properties

        #region Visibility Properties
        private Visibility _templatesVisibility;
        override public Visibility TemplatesVisibility
        {
            get
            { return _templatesVisibility; }
            set
            {
                _templatesVisibility = value;
                RaisePropertyChanged("TemplatesVisibility");
            }
        }
        #endregion Visibility Properties

        #region SettingsProperties
        override protected bool TemplatesHidden
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
        override protected string ScopeDirectoryPath
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
        #endregion //Properties

        #region Methods
        private void refresh()
        {
            if (ScopeCollection != null)
            {
                ScopeCollection.Refresh(Templates);
                ScopeDataGrid.Refresh(Templates);
                EditTab.Refresh(Templates);
                MaterialsTab.Refresh(Templates);
                TypicalSystemsTab.Refresh(Templates);
                ConstantsVM.Refresh(Templates.Labor);
                ControllersPanelsVM.Refresh(Templates);
            }
        }
        #region Setup Methods
        override protected void setupCommands()
        {
            base.setupCommands();
            RefreshCommand = new RelayCommand(RefreshTemplatesExecute, RefreshCanExecute);
        }
        private void getTemplates()
        {
            Templates = new TECTemplates();

            if ((TemplatesFilePath != "") && (File.Exists(TemplatesFilePath)))
            {
                if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                { Templates = DatabaseHelper.Load(TemplatesFilePath) as TECTemplates; }
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
                    TemplatesFilePath = getLoadPath(TemplatesFileParameters);

                    if (TemplatesFilePath != null)
                    {
                        if (!UtilitiesMethods.IsFileLocked(TemplatesFilePath))
                        {
                            Templates = DatabaseHelper.Load(TemplatesFilePath) as TECTemplates;
                        }
                        else
                        {
                            DebugHandler.LogError("Could not open file " + TemplatesFilePath + " File is open elsewhere.");
                        }
                        DebugHandler.LogDebugMessage("Finished loading templates");
                    }
                }
            }
        }
        private void setupVMs()
        {
            setupScopeCollecion();
            setupEditTab();
            setupScopeDataGrid();
            setupMaterialsTab();
            setupTypicalSystemseTab();
            setupConstantsVM();
            setupControllersPanelsVM();
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
            TypicalSystemsTab = new TypicalSystemVM(Templates);
            TypicalSystemsTab.DragHandler += DragOver;
            TypicalSystemsTab.DropHandler += Drop;
            TypicalSystemsTab.SelectionChanged += EditTab.updateSelection;
            TypicalSystemsTab.ComponentVM.SelectionChanged += EditTab.updateSelection;
            TypicalSystemsTab.AssignChildDelegates();
        }
        private void setupConstantsVM()
        {
            ConstantsVM = new ConstantsVM(Templates.Labor);
        }
        private void setupControllersPanelsVM()
        {
            ControllersPanelsVM = new ControllersPanelsVM(Templates);
            ControllersPanelsVM.DragHandler += DragOver;
            ControllersPanelsVM.DropHandler += Drop;
            ControllersPanelsVM.SelectionChanged += EditTab.updateSelection;
        }
        #endregion
        #region Commands Methods
        protected override void NewExecute()
        {
            if (deltaStack.CleansedStack().Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before creating new templates?", "Save?", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    if (!saveDelta(false))
                    {
                        MessageBox.Show("Save unsuccessful.");
                        return;
                    }
                    else
                    {
                        Templates = new TECTemplates();
                    }
                }
                else if (result == MessageBoxResult.No)
                {
                    Templates = new TECTemplates();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Templates = new TECTemplates();
            }
            refresh();
            TemplatesFilePath = "";
        }
        private void RefreshTemplatesExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (deltaStack.CleansedStack().Count > 0)
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
        #endregion //Commands Methods
        #region Drag Drop
        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            bool newDevice = false;
            if(DGTabIndex == TemplateGridIndex.Devices)
            {
                newDevice = true;
            }
            UIHelpers.StandardDrop(dropInfo, Templates, newDevice);
        }
        #endregion
        #region Helper Methods
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
        
        protected override void setupMenu()
        {
            MenuVM = new MenuVM(MenuType.TB);

            MenuVM.NewCommand = NewCommand;
            MenuVM.LoadCommand = LoadCommand;
            MenuVM.SaveCommand = SaveCommand;
            MenuVM.SaveAsCommand = SaveAsCommand;
            MenuVM.UndoCommand = UndoCommand;
            MenuVM.RedoCommand = RedoCommand;
            MenuVM.RefreshTemplatesCommand = RefreshCommand;
            MenuVM.LoadTemplatesCommand = LoadCommand;
        }

        protected override void buildTitleString()
        {
            TitleString = "Template Builder";
        }
        #endregion //Helper Methods
        #endregion //Methods
    }
}