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

        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
        #endregion //Commands Properties

        string defaultTemplatesPath;
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
        }

        private void getTemplates()
        {
            Templates = new TECTemplates();
            
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string resourcesFolder = Path.Combine(appData, APPDATA_FOLDER);

            if (!Directory.Exists(resourcesFolder))
            { Directory.CreateDirectory(resourcesFolder); }
            defaultTemplatesPath = Path.Combine(resourcesFolder, TEMPLATES_FILE_NAME);

            if (File.Exists(defaultTemplatesPath))
            {
                if (!UtilitiesMethods.IsFileLocked(defaultTemplatesPath))
                { Templates = EstimatingLibraryDatabase.LoadDBToTemplates(defaultTemplatesPath); }
                else
                {
                    string message = "TECTemplates file is open elsewhere. Could not load default templates. Please close TECTemplates.tdb and restart program.";
                    MessageBox.Show(message);
                }
            }
            else
            {
                string message = "No template database found. Reload templates to add one.";
                MessageBox.Show(message);
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
        #endregion

        #region Commands Methods
        private void LoadExecute()
        {
            //User choose path
            string path = getLoadPath();

            if (path != null)
            {
                Properties.Settings.Default.TemplateDirectoryPath = path;

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    File.Copy(path, defaultTemplatesPath, true);
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(defaultTemplatesPath);
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished loading SQL Database.");
            }
        }
        private void SaveExecute()
        {

            if (!UtilitiesMethods.IsFileLocked(defaultTemplatesPath))
            {
                
                EstimatingLibraryDatabase.UpdateTemplatesToDB(defaultTemplatesPath, Stack);

                Stack.ClearStacks();

                Console.WriteLine("Finished saving SQL Database.");
            }
            else
            {
                string message = "File is open elsewhere";
                MessageBox.Show(message);
            }
        }
        private void SaveToExecute()
        {
            //User choose path
            string path = getSavePath();
            if (path != null)
            {
                Properties.Settings.Default.TemplateDirectoryPath = path;

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    EstimatingLibraryDatabase.SaveTemplatesToNewDB(path, Templates);

                    Stack.ClearStacks();

                    Console.WriteLine("Finished saving SQL Database.");
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
            }
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
                    Console.WriteLine("Closing");
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
        #endregion //Commands Methods

        #region Get Path Methods
        private string getLoadPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (Properties.Settings.Default.TemplateDirectoryPath != null)
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.TemplateDirectoryPath;
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
                try
                {
                    path = openFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Cannot load this file. Original error: " + ex.Message);
                }
            }

            return path;
        }
        private string getSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (Properties.Settings.Default.TemplateDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = Properties.Settings.Default.TemplateDirectoryPath;
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
                try
                {
                    path = saveFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Cannot save in this location. Original error: " + ex.Message);
                }
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

                    break;
                case 4:
                    ScopeCollection.TabIndex = 7;
                    ScopeCollection.SystemsVisibility = Visibility.Collapsed;
                    ScopeCollection.EquipmentVisibility = Visibility.Collapsed;
                    ScopeCollection.SubScopeVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesVisibility = Visibility.Collapsed;
                    ScopeCollection.DevicesEditVisibility = Visibility.Visible;
                    ScopeCollection.ManufacturerVisibility = Visibility.Visible;
                    ScopeCollection.ControllerEditVisibility = Visibility.Visible;
                    
                    break;
                default:
                    ScopeCollection.TabIndex = 0;

                    ScopeCollection.SystemsVisibility = Visibility.Visible;
                    ScopeCollection.EquipmentVisibility = Visibility.Visible;
                    ScopeCollection.SubScopeVisibility = Visibility.Visible;
                    ScopeCollection.DevicesVisibility = Visibility.Visible;
                    ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
                    ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;
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

        #endregion //Helper Methods
        #endregion //Methods
    }
}