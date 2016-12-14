using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BidEditorBase : ViewModelBase
    {
        #region Properties
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                stack = new ChangeStack(value);
                // Call OnPropertyChanged whenever the property is updated
                RaisePropertyChanged("Bid");
                buildTitleString();
                Bid.PropertyChanged += Bid_PropertyChanged;
            }
        }
        private TECTemplates _templates;
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }

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

        public string TECLogo { get; set; }
        public string Version { get; set; }
        #region Command Properties
        public ICommand NewCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand DocumentCommand { get; private set; }
        public ICommand LoadTemplatesCommand { get; private set; }
        public ICommand CSVExportCommand { get; private set; }
        public ICommand BudgetCommand { get; private set; }

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }

        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
        #endregion

        #region Fields
        private ChangeStack stack;
        private bool saveSuccessful;
        private string bidDBFilePath;
        private string defaultTemplatesPath;
        public string startupFile;
        public string pointCSVDirectoryPath;
        public string scopeDirectoryPath;
        public string documentDirectoryPath;
        #endregion

        #region Resources Paths
        const string APPDATA_FOLDER = @"TECSystems\";
        const string TEMPLATES_FILE_NAME = @"TECTemplates.tdb";
        #endregion //Resources Paths

        #endregion

        /// <summary>
        /// Initializes a new instance of the BidEditorBase class.
        /// </summary>
        public BidEditorBase()
        {
            Console.WriteLine("Parent");
            CurrentStatusText = "Loading...";

            setupCommands();
            setupTemplates();
            getLogo();
            setupTemplates();
            setupBid();
            setupStack();

            CurrentStatusText = "Done.";
        }

        #region Methods
        private void setupCommands()
        {
            NewCommand = new RelayCommand(NewExecute);
            LoadCommand = new RelayCommand(LoadExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            SaveAsCommand = new RelayCommand(SaveAsExecute);
            DocumentCommand = new RelayCommand(DocumentExecute);
            CSVExportCommand = new RelayCommand(CSVExportExecute);
            BudgetCommand = new RelayCommand(BudgetExecute);
            LoadTemplatesCommand = new RelayCommand(LoadTemplatesExecute);
            UndoCommand = new RelayCommand(UndoExecute, UndoCanExecute);
            RedoCommand = new RelayCommand(RedoExecute, RedoCanExecute);
            
            ClosingCommand = new RelayCommand<CancelEventArgs>(e => ClosingExecute(e));
        }
        #region Helper Functions
        private void setupTemplates()
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
                //string message = "No template database found. Use Template Builder to create and save templates and reload them in Scope Builder.";
                //MessageBox.Show(message);
            }
        }
        private void setupBid()
        {
            Bid = new TECBid();
            Bid.DeviceCatalog = Templates.DeviceCatalog;
            Bid.ManufacturerCatalog = Templates.ManufacturerCatalog;
            Bid.Tags = Templates.Tags;

            bidDBFilePath = null;
        }
        private void setupStack()
        {
            stack = new ChangeStack(Bid);
        }
        public void checkForOpenWith(string startupFile)
        {
            if (startupFile != "")
            {
                CurrentStatusText = "Loading...";
                LoadFromPath(startupFile);
                CurrentStatusText = "Done.";
            }
        }

        private void getLogo()
        {
            TECLogo = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);
        }

        private string getSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (scopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = scopeDirectoryPath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            saveFileDialog.FileName = Bid.BidNumber + " " + Bid.Name;
            saveFileDialog.Filter = "Bid Database Files (*.bdb)|*.bdb";
            saveFileDialog.DefaultExt = "bdb";
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

        private string getDocumentSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (documentDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = documentDirectoryPath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            saveFileDialog.Filter = "Rich Text Files (*.rtf)|*.rtf";
            saveFileDialog.DefaultExt = "rtf";
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

        private string getCSVSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (pointCSVDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = pointCSVDirectoryPath;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            saveFileDialog.Filter = "Comma Separated Values Files (*.csv)|*.csv";
            saveFileDialog.DefaultExt = "csv";
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

        private string getLoadPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (scopeDirectoryPath != null)
            {
                openFileDialog.InitialDirectory = scopeDirectoryPath;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            openFileDialog.Filter = "Bid Database Files (*.bdb)|*.bdb";
            openFileDialog.DefaultExt = "bdb";
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

        private string getLoadTemplatesPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Template Database Files (*.tdb)|*.tdb";
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

        private void buildTitleString()
        {
            TitleString = Bid.Name + " - Scope Builder";
        }

        private void Bid_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                buildTitleString();
            }
        }

        private void LoadFromPath(string path)
        {
            if (path != null)
            {
                CurrentStatusText = "Loading...";
                bidDBFilePath = path;
                scopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Bid = EstimatingLibraryDatabase.LoadDBToBid(path, Templates);
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished loading SQL Database.");
                CurrentStatusText = "Done.";
            }
        }

        #endregion //Helper Functions

        #region Commands
        private void NewExecute()
        {
            string message = "Would you like to save your changes before creating a new scope?";
            MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.Yes)
            {
                SaveExecute();

                if (saveSuccessful)
                {
                    bidDBFilePath = null;
                    Bid = new TECBid();
                }
                else
                {
                    MessageBox.Show("Save unsuccessful. New scope not created.");
                }
            }
            else if (result == MessageBoxResult.No)
            {
                bidDBFilePath = null;
                Bid = new TECBid();
            }
        }
        private void LoadExecute()
        {
            //User choose path
            string path = getLoadPath();
            if (path != null)
            {
                CurrentStatusText = "Loading...";
                bidDBFilePath = path;
                scopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Bid = EstimatingLibraryDatabase.LoadDBToBid(path, Templates);
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished loading SQL Database.");
                CurrentStatusText = "Done.";
            }
        }
        private void SaveExecute()
        {
            saveSuccessful = false;
            if (bidDBFilePath != null)
            {
                if (!UtilitiesMethods.IsFileLocked(bidDBFilePath))
                {
                    CurrentStatusText = "Saving...";
                    SaveWindow saveWindow = new SaveWindow();
                    try
                    {
                        EstimatingLibraryDatabase.UpdateBidToDB(bidDBFilePath, stack);
                    }
                    catch (Exception e)
                    {
                        Console.Write("Save delta failed. Saving to new file. Error: " + e.Message);
                        EstimatingLibraryDatabase.SaveBidToNewDB(bidDBFilePath, Bid);
                    }

                    stack.ClearStacks();
                    saveSuccessful = true;
                    saveWindow.Close();
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                CurrentStatusText = "Done.";
            }
            else
            {
                SaveAsExecute();
            }
        }
        private void SaveAsExecute()
        {
            //User choose path
            saveSuccessful = false;
            string path = getSavePath();
            if (path != null)
            {
                bidDBFilePath = path;
                scopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    SaveWindow saveWindow = new SaveWindow();
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    //Create new database
                    EstimatingLibraryDatabase.SaveBidToNewDB(path, Bid);
                    stack.ClearStacks();
                    saveSuccessful = true;
                    saveWindow.Close();
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished saving SQL Database.");
            }
        }
        private void DocumentExecute()
        {
            string path = getDocumentSavePath();

            if (path != null)
            {
                documentDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    EstimatingUtilitiesLibrary.ScopeDocumentBuilder.CreateScopeDocument(Bid, path);
                    Console.WriteLine("Scope saved to document.");
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
            }
        }
        private void CSVExportExecute()
        {
            //User choose path
            string path = getCSVSavePath();
            if (path != null)
            {
                pointCSVDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CSVWriter writer = new CSVWriter(path);
                    writer.BidPointsToCSV(Bid);
                    Console.WriteLine("Finished exporting points to a csv.");
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
            }
        }
        private void BudgetExecute()
        {
            new View.BudgetWindow();
            MessengerInstance.Send<GenericMessage<ObservableCollection<TECSystem>>>(new GenericMessage<ObservableCollection<TECSystem>>(Bid.Systems));
        }
        private void LoadTemplatesExecute()
        {
            //User choose path
            string path = getLoadTemplatesPath();

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    File.Copy(path, defaultTemplatesPath, true);
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(path);
                    Bid.DeviceCatalog = Templates.DeviceCatalog;
                    Bid.ManufacturerCatalog = Templates.ManufacturerCatalog;
                    Bid.Tags = Templates.Tags;
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished loading templates");
            }
        }

        private void UndoExecute()
        {
            stack.Undo();
        }
        private bool UndoCanExecute()
        {
            if (stack.UndoStack.Count > 0)
                return true;
            else
                return false;
        }

        private void RedoExecute()
        {
            stack.Redo();
        }
        private bool RedoCanExecute()
        {
            if (stack.RedoStack.Count > 0)
                return true;
            else
                return false;
        }

        private void ClosingExecute(CancelEventArgs e)
        {
            bool changes = (stack.SaveStack.Count > 0);
            if (changes)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveExecute();
                    if (!saveSuccessful)
                    {
                        e.Cancel = true;
                        MessageBox.Show("Save unsuccessful, cancelling quit.");
                    }
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
        #endregion
        
        #endregion
    }
}