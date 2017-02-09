﻿using EstimatingLibrary;
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

        #region Constants

        string DEFAULT_STATUS_TEXT = "Ready :)";

        #endregion

        #region Properties
        protected bool isReady
        {
            get;
            private set;
        }
        private string _programName;
        protected string programName
        {
            get { return _programName; }
            set
            {
                _programName = value;
                buildTitleString();
            }
        }

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
                BidSet?.Invoke();
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
            private set
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
        private string bidDBFilePath;
        public string startupFile;
        public string scopeDirectoryPath;
        #endregion

        #region Delgates
        public Action BidSet;
        #endregion

        #endregion
        public BidEditorBase()
        {
            SetBusyStatus("Initializing Program...");

            setupCommands();
            setupTemplates();
            getLogo();
            setupTemplates();
            setupBid();
            setupStack();

            ResetStatus();
        }

        #region Methods

        #region Setup
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
        private void setupTemplates()
        {
            Templates = new TECTemplates();
            
            if ((Properties.Settings.Default.TemplatesFilePath != "") && (File.Exists(Properties.Settings.Default.TemplatesFilePath)))
            {
                if (!UtilitiesMethods.IsFileLocked(Properties.Settings.Default.TemplatesFilePath))
                { Templates = EstimatingLibraryDatabase.LoadDBToTemplates(Properties.Settings.Default.TemplatesFilePath); }
                else
                {
                    string message = "TECTemplates file is open elsewhere. Could not load templates. Please close the templates file and load again.";
                    MessageBox.Show(message);
                }
            }
            else
            {
                string message = "No templates file loaded. Would you like to load templates?";
                MessageBoxResult result = MessageBox.Show(message, "Load Templates?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    //User choose path
                    Properties.Settings.Default.TemplatesFilePath = getLoadTemplatesPath();

                    if (Properties.Settings.Default.TemplatesFilePath != null)
                    {
                        if (!UtilitiesMethods.IsFileLocked(Properties.Settings.Default.TemplatesFilePath))
                        {
                            Templates = EstimatingLibraryDatabase.LoadDBToTemplates(Properties.Settings.Default.TemplatesFilePath);
                        }
                        else
                        {
                            message = "File is open elsewhere";
                            MessageBox.Show(message);
                        }
                        Console.WriteLine("Finished loading templates");
                    }
                }
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
        #endregion

        #region Helper Functions

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

            if (scopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = scopeDirectoryPath;
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
            if (scopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = scopeDirectoryPath;
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
            openFileDialog.Title = "Please choose a template database.";
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
                    Properties.Settings.Default.TemplatesFilePath = path;
                    Properties.Settings.Default.Save();
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
            TitleString = Bid.Name + " - " + programName;
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

        protected void SetBusyStatus(string statusText)
        {
            CurrentStatusText = statusText;
            isReady = false;
        }
        protected void ResetStatus()
        {
            CurrentStatusText = DEFAULT_STATUS_TEXT;
            isReady = true;
        }

        protected bool IsReady()
        {
            return isReady;
        }

        #endregion //Helper Functions

        #region Commands
        private void NewExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (stack.SaveStack.Count > 0)
            {
                string message = "Would you like to save your changes before creating a new scope?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {

                    if (saveSynchronously())
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
                    Console.WriteLine("Creating new bid.");
                    bidDBFilePath = null;
                    Bid = new TECBid();
                }
            }
            else
            {
                Console.WriteLine("Creating new bid.");
                bidDBFilePath = null;
                Bid = new TECBid();
            }
            
        }
        private void LoadExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }

            //User choose path
            string path = getLoadPath();
            if (path != null)
            {
                SetBusyStatus("Loading File: " + path);
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
                ResetStatus();
            }
        }
        private void SaveExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }

            saveBid();
            
        }
        private void SaveAsExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            saveBidAs();
        }
        private void DocumentExecute()
        {
            string path = getDocumentSavePath();

            if (path != null)
            {
                scopeDirectoryPath = Path.GetDirectoryName(path);

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
                scopeDirectoryPath = Path.GetDirectoryName(path);

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
            //new View.BudgetWindow();
            //MessengerInstance.Send<GenericMessage<ObservableCollection<TECSystem>>>(new GenericMessage<ObservableCollection<TECSystem>>(Bid.Systems));
        }
        private void LoadTemplatesExecute()
        {
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            //User choose path
            Properties.Settings.Default.TemplatesFilePath = getLoadTemplatesPath();

            if (Properties.Settings.Default.TemplatesFilePath != null)
            {
                SetBusyStatus("Loading templates from file: " + Properties.Settings.Default.TemplatesFilePath);
                if (!UtilitiesMethods.IsFileLocked(Properties.Settings.Default.TemplatesFilePath))
                {
                    
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(Properties.Settings.Default.TemplatesFilePath);
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
            ResetStatus();
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
            if (!isReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                e.Cancel = true;
                return;
            }
            bool changes = (stack.SaveStack.Count > 0);
            if (changes)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Would you like to save before quitting?", "Save?", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    if (!saveSynchronously())
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

        private void saveBid()
        {
            if (bidDBFilePath != null)
            {
                SetBusyStatus("Saving to path: " + bidDBFilePath);
                ChangeStack stackToSave = stack.Copy();
                stack.ClearStacks();

                BackgroundWorker worker = new BackgroundWorker();

                worker.DoWork += (s, e) =>
                {
                    if (!UtilitiesMethods.IsFileLocked(bidDBFilePath))
                    {
                        try
                        {
                            EstimatingLibraryDatabase.UpdateBidToDB(bidDBFilePath, stackToSave);
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Save delta failed. Saving to new file. Error: " + ex.Message);
                            EstimatingLibraryDatabase.SaveBidToNewDB(bidDBFilePath, Bid);
                        }
                    }
                    else
                    {
                        string message = "File is open elsewhere";
                        MessageBox.Show(message);
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
                saveBidAs();
            }
        }

        private void saveBidAs()
        {
            //User choose path
            string path = getSavePath();
            if (path != null)
            {
                bidDBFilePath = path;
                scopeDirectoryPath = Path.GetDirectoryName(path);

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
                        EstimatingLibraryDatabase.SaveBidToNewDB(path, Bid);
                    }
                    else
                    {
                        string message = "File is open elsewhere";
                        MessageBox.Show(message);
                    }
                };
                worker.RunWorkerCompleted += (s, e) =>
                {
                    Console.WriteLine("Finished saving SQL Database.");
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
                bidDBFilePath = path;
                scopeDirectoryPath = Path.GetDirectoryName(path);

                stack.ClearStacks();
                SetBusyStatus("Saving file: " + path);
                
                    if (!UtilitiesMethods.IsFileLocked(path))
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        //Create new database
                        EstimatingLibraryDatabase.SaveBidToNewDB(path, Bid);
                    saveSuccessful = true;
                    }
                    else
                    {
                        string message = "File is open elsewhere";
                        MessageBox.Show(message);
                    saveSuccessful = false;
                }
               

            }

            return saveSuccessful;
        }

        private bool saveSynchronously()
        {
            bool saveSuccessful = false;

            if (bidDBFilePath != null)
            {
                SetBusyStatus("Saving to path: " + bidDBFilePath);
                ChangeStack stackToSave = stack.Copy();
                stack.ClearStacks();
                
                    if (!UtilitiesMethods.IsFileLocked(bidDBFilePath))
                    {
                        try
                        {
                            EstimatingLibraryDatabase.UpdateBidToDB(bidDBFilePath, stackToSave);
                        saveSuccessful = true;
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Save delta failed. Saving to new file. Error: " + ex.Message);
                            EstimatingLibraryDatabase.SaveBidToNewDB(bidDBFilePath, Bid);
                        }
                    }
                    else
                    {
                        string message = "File is open elsewhere";
                        MessageBox.Show(message);
                    }
               
            }
            else
            {
                if (saveAsSynchronously())
                {
                    saveSuccessful = true;
                }else
                {
                    saveSuccessful = false;
                }
            }

            return saveSuccessful;
        }
        #endregion
        
        #endregion
    }
}