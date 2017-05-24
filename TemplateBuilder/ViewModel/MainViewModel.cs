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
using TECUserControlLibrary.ViewModelExtensions;
using DebugLibrary;
using TECUserControlLibrary.ViewModels;

namespace TemplateBuilder.ViewModel
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
            DGTabIndex = TemplateGridIndex.ControlledScope;

            ResetStatus();
        }

        #region Resources Paths
        const string APPDATA_FOLDER = @"TECSystems\";
        const string TEMPLATES_FILE_NAME = @"TECTemplates.tdb";
        #endregion //Resources Paths

        #region Properties
        #region VM Extensions
        public ScopeCollectionExtension ScopeCollection { get; set; }
        public ScopeDataGridExtension ScopeDataGrid { get; set; }
        public EditTabExtension EditTab { get; set; }
        public MaterialsCostsExtension MaterialsTab { get; set; }
        public ControlledScopeViewModel ControlledScopeVM { get; set; }
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
                ControlledScopeVM.Refresh(Templates);
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
                { Templates = EstimatingLibraryDatabase.Load(TemplatesFilePath) as TECTemplates; }
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
                            Templates = EstimatingLibraryDatabase.Load(TemplatesFilePath) as TECTemplates;
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
            setupControlledScopeTab();
        }

        private void setupScopeCollecion()
        {
            ScopeCollection = new ScopeCollectionExtension(Templates);
            ScopeCollection.DragHandler += DragOver;
            ScopeCollection.DropHandler += Drop;
        }
        private void setupScopeDataGrid()
        {
            ScopeDataGrid = new ScopeDataGridExtension(Templates);
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
            ScopeDataGrid.SelectionChanged += EditTab.updateSelection;
        }
        private void setupEditTab()
        {
            EditTab = new EditTabExtension(Templates);
            EditTab.DragHandler += DragOver;
            EditTab.DropHandler += Drop;
        }
        private void setupMaterialsTab()
        {
            MaterialsTab = new MaterialsCostsExtension(Templates);
            MaterialsTab.DragHandler += DragOver;
            MaterialsTab.DropHandler += Drop;
            MaterialsTab.SelectionChanged += EditTab.updateSelection;
        }
        private void setupControlledScopeTab()
        {
            ControlledScopeVM = new ControlledScopeViewModel(Templates);
            ControlledScopeVM.DragHandler += DragOver;
            ControlledScopeVM.DropHandler += Drop;
            ControlledScopeVM.SelectionChanged += EditTab.updateSelection;
            ControlledScopeVM.ScopeDataGrid.SelectionChanged += EditTab.updateSelection;
        }
        #endregion
        #region Commands Methods
        protected override void NewExecute()
        {
            if (stack.SaveStack.Count > 0)
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
            if (stack.SaveStack.Count > 0)
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
                    load(true);
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
            UIHelpers.StandardDrop(dropInfo, newDevice);
        }
        #endregion
        #region Helper Methods
        private void setVisibility(TemplateGridIndex gridIndex)
        {
            ScopeDataGrid.NullifySelected();

            switch (gridIndex)
            {
                case TemplateGridIndex.Systems:
                    ScopeCollection.SystemsVisibility = Visibility.Visible;
                    ScopeCollection.EquipmentVisibility = Visibility.Visible;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                    ScopeCollection.TagsVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.PanelsVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

                    ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeLocation = Visibility.Collapsed;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.System;
                    break;
                case TemplateGridIndex.Equipment:
                    ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                    ScopeCollection.EquipmentVisibility = Visibility.Visible;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                    ScopeCollection.TagsVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.PanelsVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

                    ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.Equipment;

                    break;
                case TemplateGridIndex.SubScope:


                    ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                    ScopeCollection.TagsVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.PanelsVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.SubScope;

                    break;
                case TemplateGridIndex.Devices:

                    ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.DevicesEditVisibility = Visibility.Visible;
                    ScopeCollection.ManufacturerVisibility = Visibility.Visible;
                    ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                    ScopeCollection.TagsVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.PanelsVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.Devices;


                    break;
                case TemplateGridIndex.DDC:
                    ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerEditVisibility = Visibility.Visible;
                    ScopeCollection.ControllerVisibility = Visibility.Visible;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Visible;
                    ScopeCollection.TagsVisibility = Visibility.Collapsed;
                    ScopeCollection.AddPanelVisibility = Visibility.Visible;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.PanelsVisibility = Visibility.Visible;
                    ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.Controllers;

                    break;
                case TemplateGridIndex.Materials:
                    ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Visible;
                    ScopeCollection.TagsVisibility = Visibility.Collapsed;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.PanelsVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscCostVisibility = Visibility.Visible;
                    ScopeCollection.MiscWiringVisibility = Visibility.Visible;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.AssociatedCosts;
                    break;
                case TemplateGridIndex.Constants:
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

                    ScopeCollection.TabIndex = ScopeCollectionIndex.None;

                    break;
                case TemplateGridIndex.ControlledScope:

                    ScopeCollection.SystemsVisibility = Visibility.Visible;
                    ScopeCollection.EquipmentVisibility = Visibility.Visible;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Visible;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Visible;
                    ScopeCollection.TagsVisibility = Visibility.Collapsed;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                    ScopeCollection.PanelsVisibility = Visibility.Visible;
                    ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

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
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.TagsVisibility = Visibility.Visible;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.PanelsVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscCostVisibility = Visibility.Collapsed;
                    ScopeCollection.MiscWiringVisibility = Visibility.Collapsed;

                    ScopeCollection.TabIndex = ScopeCollectionIndex.None;

                    break;
            }
        }
        
        protected override void setupMenu()
        {
            MenuVM = new MenuViewModel(MenuType.TB);

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