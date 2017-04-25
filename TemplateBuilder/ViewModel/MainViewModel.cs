﻿using EstimatingLibrary;
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
            _templates = new TECTemplates();
           
            TECLogo = Path.GetTempFileName();
            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);

            TitleString = "Template Builder";
            
            setupScopeCollecion();
            setupEditTab();
            setupScopeDataGrid();
            setupMaterialsTab();
            setupCommands();
            setupVMs();

            getTemplates();

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
        
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                stack = new ChangeStack(value);
                RaisePropertyChanged("Templates");
                refresh();
            }
        }
        private TECTemplates _templates;
        public string Version { get; set; }
        public string TitleString
        {
            get { return _titleString; }
            set
            {
                _titleString = value;
                RaisePropertyChanged("TitleString");
            }
        }
        private string _titleString;

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
        #endregion //Properties

        #region Methods
        private void refresh()
        {
            ScopeCollection.Refresh(Templates);
            ScopeDataGrid.Refresh(Templates);
            EditTab.Refresh(Templates);
            MaterialsTab.Refresh(Templates);
            ControlledScopeVM.Refresh(Templates);
        }
        #region Setup Methods
        private void setupCommands()
        {
            RefreshCommand = new RelayCommand(RefreshTemplatesExecute, RefreshCanExecute);
        }

        private void getTemplates()
        {
            Templates = new TECTemplates();

            if ((Properties.Settings.Default.TemplatesFilePath != "") && (File.Exists(Properties.Settings.Default.TemplatesFilePath)))
            {
                if (!UtilitiesMethods.IsFileLocked(Properties.Settings.Default.TemplatesFilePath))
                { Templates = EstimatingLibraryDatabase.LoadDBToTemplates(Properties.Settings.Default.TemplatesFilePath); }
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
                    Properties.Settings.Default.TemplatesFilePath = getLoadPath();

                    if (Properties.Settings.Default.TemplatesFilePath != null)
                    {
                        if (!UtilitiesMethods.IsFileLocked(Properties.Settings.Default.TemplatesFilePath))
                        {
                            Templates = EstimatingLibraryDatabase.LoadDBToTemplates(Properties.Settings.Default.TemplatesFilePath);
                        }
                        else
                        {
                            DebugHandler.LogError("Could not open file " + Properties.Settings.Default.TemplatesFilePath + " File is open elsewhere.");
                        }
                        DebugHandler.LogDebugMessage("Finished loading templates");
                    }
                }
            }
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
        }
        private void setupVMs()
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
                    if (!saveSynchronously())
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
            Properties.Settings.Default.TemplatesFilePath = "";
        }
        protected override void LoadExecute()
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
                    if (!saveSynchronously())
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

            //User choose path
            string path = getLoadPath();

            if (path != null)
            {
                loadFromPath(path);
            }
        }
        protected override void SaveExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }

            saveTemplates();
        }
        protected override void SaveAsExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            saveTemplatesAs();
        }

        protected override void ClosingExecute(CancelEventArgs e)
        {
            bool changes = (stack.SaveStack.Count > 0);
            if (changes)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    if (!saveSynchronously())
                    {
                        MessageBox.Show("Save unsuccessful.");
                        return;
                    }
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                Properties.Settings.Default.Save();
            }
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
                    if (!saveSynchronously())
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

            string path = Properties.Settings.Default.TemplatesFilePath;
            if (path != null)
            {
                SetBusyStatus("Loading templates from file: " + path);
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(path);
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
            ResetStatus();
        }
        private bool RefreshCanExecute()
        {
            return !(Properties.Settings.Default.TemplatesFilePath == "");
        }
        #endregion //Commands Methods

        #region Get Path Methods
        protected override string getLoadPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (Properties.Settings.Default.TemplatesFilePath != null)
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.TemplatesFilePath;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            openFileDialog.Filter = "TEC Template Database Files (*.tdb)|*.tdb";
            openFileDialog.DefaultExt = "tdb";
            openFileDialog.AddExtension = true;

            string path = null;

            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
                Properties.Settings.Default.TemplatesFilePath = path;
                Properties.Settings.Default.Save();
            }

            return path;
        }
        protected override string getSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (Properties.Settings.Default.TemplatesFilePath != null)
            {
                saveFileDialog.InitialDirectory = Properties.Settings.Default.TemplatesFilePath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            saveFileDialog.Filter = "TEC Template Database Files (*.tdb)|*.tdb";
            saveFileDialog.DefaultExt = "tdb";
            saveFileDialog.AddExtension = true;

            string path = null;

            if (saveFileDialog.ShowDialog() == true)
            {
                path = saveFileDialog.FileName;
            }

            return path;
        }
        #endregion //Get Path Methods

        #region Drag Drop
        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo);
        }

        public void Drop(IDropInfo dropInfo)
        {
            UIHelpers.StandardDrop(dropInfo);
        }
        #endregion

        #region Helper Methods
        private void setVisibility(TemplateGridIndex gridIndex)
        {
            nullifySelected();

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

                    ScopeCollection.TabIndex = ScopeCollectionIndex.ControlledScope;

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
        private void nullifySelected()
        {
            ScopeDataGrid.SelectedDevice = null;
            ScopeDataGrid.SelectedPoint = null;
            ScopeDataGrid.SelectedSubScope = null;
            ScopeDataGrid.SelectedEquipment = null;
            ScopeDataGrid.SelectedSystem = null;
        }
        private void saveTemplates()
        {
            string path = Properties.Settings.Default.TemplatesFilePath;
            if (path != null)
            {
                SetBusyStatus("Saving to path: " + path);
                ChangeStack stackToSave = stack.Copy();
                stack.ClearStacks();

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(Properties.Settings.Default.TemplatesFilePath))
                    {
                        EstimatingLibraryDatabase.UpdateTemplatesToDB(path, stackToSave);
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                    }
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                };
                worker.RunWorkerAsync();
            }
            else
            {
                saveTemplatesAs();
            }
        }
        private void saveTemplatesAs()
        {
            //User choose path
            string path = getSavePath();

            if (path != null)
            {
                Properties.Settings.Default.TemplatesFilePath = path;

                stack.ClearStacks();
                SetBusyStatus("Saving file: " + path);

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(path))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        //Create new database
                        EstimatingLibraryDatabase.SaveTemplatesToNewDB(path, Templates);
                    }
                    else
                    {
                        DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                    }
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    ResetStatus();
                };

                worker.RunWorkerAsync();

            }
        }
        
        private bool saveAsSynchronously()
        {
            bool saveSuccessful = false;

            //User choose path
            string path = getSavePath();
            if (path != null)
            {
                stack.ClearStacks();
                SetBusyStatus("Saving file: " + path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    //Create new database
                    EstimatingLibraryDatabase.SaveTemplatesToNewDB(path, Templates);
                    saveSuccessful = true;
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                    saveSuccessful = false;
                }


            }

            return saveSuccessful;
        }

        private bool saveSynchronously()
        {
            bool saveSuccessful = false;

            if (Properties.Settings.Default.TemplatesFilePath != null)
            {
                SetBusyStatus("Saving to path: " + Properties.Settings.Default.TemplatesFilePath);
                ChangeStack stackToSave = stack.Copy();
                stack.ClearStacks();

                if (!UtilitiesMethods.IsFileLocked(Properties.Settings.Default.TemplatesFilePath))
                {
                    try
                    {
                        EstimatingLibraryDatabase.UpdateTemplatesToDB(Properties.Settings.Default.TemplatesFilePath, stackToSave);
                        saveSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        DebugHandler.LogError("Save delta failed. Saving to new file. Error: " + ex.Message);
                        EstimatingLibraryDatabase.SaveTemplatesToNewDB(Properties.Settings.Default.TemplatesFilePath, Templates);
                    }
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + Properties.Settings.Default.TemplatesFilePath + " File is open elsewhere.");
                }

            }
            else
            {
                if (saveAsSynchronously())
                {
                    saveSuccessful = true;
                }
                else
                {
                    saveSuccessful = false;
                }
            }

            return saveSuccessful;
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
        }
        
        protected override void loadFromPath(string path)
        {
            SetBusyStatus("Loading File: " + path);
            //Properties.Settings.Default.TemplateDirectoryPath = path;

            if (!UtilitiesMethods.IsFileLocked(path))
            {
                //File.Copy(path, defaultTemplatesPath, true);
                //Templates = EstimatingLibraryDatabase.LoadDBToTemplates(defaultTemplatesPath);
                Templates = EstimatingLibraryDatabase.LoadDBToTemplates(path);
            }
            else
            {
                DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
            }
            ResetStatus();
        }
        
        #endregion //Helper Methods
        #endregion //Methods
    }
}