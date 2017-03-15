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
    public class MainViewModel : ViewModelBase, IDropTarget
    {
        //Initializer
        public MainViewModel()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            { Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { Version = "Undeployed Version"; }
            getTemplates();

            TECLogo = Path.GetTempFileName();
            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);

            Stack = new ChangeStack(Templates);

            CurrentStatusText = "Done.";
            TitleString = "Template Builder";

            setupCommands();
            setupScopeCollecion();
            setupEditTab();
            setupScopeDataGrid();
            setupMaterialsTab();
            setupVMs();

            setVisibility(0);
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

        #region ViewModels
        public MenuViewModel MenuVM { get; set; }
        #endregion

        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                Stack = new ChangeStack(value);
                RaisePropertyChanged("Templates");
            }
        }
        private TECTemplates _templates;
        public ChangeStack Stack { get; set; }
        public string TECLogo { get; set; }
        public string Version { get; set; }

        public string CurrentStatusText
        {
            get { return _currentStatusText; }
            set
            {
                _currentStatusText = value;
                RaisePropertyChanged("CurrentStatusText");
            }
        }
        private string _currentStatusText;
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
        public int DGTabIndex
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
        private int _DGTabIndex;
        
        #endregion //Tab Indexes

        #endregion //Interface Properties

        #region Commands Properties
        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveToCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
        #endregion //Commands Properties
        #endregion //Properties

        #region Methods
        #region Setup Methods
        private void setupCommands()
        {
            LoadCommand = new RelayCommand(LoadExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            SaveToCommand = new RelayCommand(SaveToExecute);
            ClosingCommand = new RelayCommand<CancelEventArgs>(e => ClosingExecute(e));

            UndoCommand = new RelayCommand(UndoExecute, UndoCanExecute);
            RedoCommand = new RelayCommand(RedoExecute, RedoCanExecute);

            RefreshCommand = new RelayCommand(RefreshTemplatesExecute);
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
        }
        private void setupMaterialsTab()
        {
            MaterialsTab = new MaterialsCostsExtension(Templates);
            MaterialsTab.DragHandler += DragOver;
            MaterialsTab.DropHandler += Drop;
        }
        private void setupVMs()
        {
            MenuVM = new MenuViewModel(MenuType.TB);

            MenuVM.LoadCommand = LoadCommand;
            MenuVM.SaveCommand = SaveCommand;
            MenuVM.SaveAsCommand = SaveToCommand;
            MenuVM.UndoCommand = UndoCommand;
            MenuVM.RedoCommand = RedoCommand;
            MenuVM.RefreshTemplatesCommand = RefreshCommand;

            ControlledScopeVM = new ControlledScopeViewModel(Templates);
            ControlledScopeVM.DragHandler += DragOver;
            ControlledScopeVM.DropHandler += Drop;
        }
        #endregion

        #region Commands Methods
        private void LoadExecute()
        {
            //User choose path
            string path = getLoadPath();

            if (path != null)
            {
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
            }
        }
        private void SaveExecute()
        {
            saveTemplates();
        }
        private void SaveToExecute()
        {
            saveTemplatesAs();
        }
        
        private void ClosingExecute(CancelEventArgs e)
        {
            bool changes = (Stack.SaveStack.Count > 0);
            if (changes)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveExecute();
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
        
        private void UndoExecute()
        {
            Stack.Undo();
        }
        private bool UndoCanExecute()
        {
            if (Stack.UndoStack.Count > 0)
                return true;
            else
                return false;
        }

        private void RedoExecute()
        {
            Stack.Redo();
        }
        private bool RedoCanExecute()
        {
            if (Stack.RedoStack.Count > 0)
                return true;
            else
                return false;
        }

        private void RefreshTemplatesExecute()
        {
            string path = Properties.Settings.Default.TemplatesFilePath;
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(path);
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        #endregion //Commands Methods

        #region Get Path Methods
        private string getLoadPath()
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
        private string getSavePath()
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
            var sourceItem = dropInfo.Data;
            var targetCollection = dropInfo.TargetCollection;
            Type sourceType = sourceItem.GetType();
            Type targetType = targetCollection.GetType().GetTypeInfo().GenericTypeArguments[0];

            if (sourceItem != null && sourceType == targetType)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }
        public void Drop(IDropInfo dropInfo)
        {
            Console.WriteLine("Main Drop");
            Object sourceItem;
            if (dropInfo.VisualTarget != dropInfo.DragInfo.VisualSource)
            { sourceItem = ((TECScope)dropInfo.Data).DragDropCopy(); }
            else
            { sourceItem = dropInfo.Data; }

            if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
            {
                ((IList)dropInfo.TargetCollection).Add(sourceItem);
                if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
                {
                    ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem);
                }
            }
            else
            {
                if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)
                {
                    ((IList)dropInfo.DragInfo.SourceCollection).Remove(sourceItem);
                }
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    ((IList)dropInfo.TargetCollection).Add(sourceItem);
                }
                else
                {
                    ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                }
            }
        }
        #endregion

        #region Helper Methods
        private void setVisibility(int gridIndex)
        {
            nullifySelected();

            switch (gridIndex)
            {
                case 0:
                    ScopeCollection.TabIndex = 0;

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

                    ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeLocation = Visibility.Collapsed;
                    break;
                case 1:
                    ScopeCollection.TabIndex = 1;

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



                    ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    break;
                case 2:
                    ScopeCollection.TabIndex = 2;

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

                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Collapsed;
                    break;
                case 3:
                    ScopeCollection.TabIndex = 6;

                    ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesEditVisibility = Visibility.Visible;
                    ScopeCollection.ManufacturerVisibility = Visibility.Visible;
                    ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                    ScopeCollection.TagsVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;

                    break;
                case 4:
                    ScopeCollection.TabIndex = 7;
                    ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Visible;
                    ScopeCollection.ControllerEditVisibility = Visibility.Visible;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                    ScopeCollection.TagsVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;

                    break;
                case 5:
                    ScopeCollection.TabIndex = 9;
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

                    break;
                case 6:
                    ScopeCollection.TabIndex = 10;
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

                    break;
                case 7:
                    ScopeCollection.TabIndex = 11;

                    ScopeCollection.SystemsVisibility = Visibility.Visible;
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Visible;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Collapsed;
                    ScopeCollection.TagsVisibility = Visibility.Collapsed;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Collapsed;

                    ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeLocation = Visibility.Collapsed;
                    break;
                case 8:
                    ScopeCollection.TabIndex = 12;

                    ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.AssociatedCostsVisibility = Visibility.Visible;
                    ScopeCollection.TagsVisibility = Visibility.Visible;
                    ScopeCollection.ControlledScopeVisibility = Visibility.Visible;
                    ScopeCollection.AddPanelVisibility = Visibility.Visible;

                    ScopeDataGrid.DataGridVisibilty.SystemQuantity = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemTotalPrice = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeQuantity = Visibility.Visible;
                    ScopeDataGrid.DataGridVisibilty.SystemLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.EquipmentLocation = Visibility.Collapsed;
                    ScopeDataGrid.DataGridVisibilty.SubScopeLocation = Visibility.Collapsed;
                    break;
                default:
                    ScopeCollection.TabIndex = 0;

                    ScopeCollection.SystemsVisibility = Visibility.Visible;
                    ScopeCollection.EquipmentVisibility = Visibility.Visible;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ControllerVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
                    ScopeCollection.TagsVisibility = Visibility.Visible;
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
                //SetBusyStatus("Saving to path: " + path);
                ChangeStack stackToSave = Stack.Copy();
                Stack.ClearStacks();

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
                    //ResetStatus();
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
                
                Stack.ClearStacks();
                //SetBusyStatus("Saving file: " + path);

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
                    //ResetStatus();
                };

                worker.RunWorkerAsync();

            }
        }

        #endregion //Helper Methods
        #endregion //Methods
    }
}