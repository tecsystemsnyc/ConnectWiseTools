using GalaSoft.MvvmLight;
using EstimateBuilder.Model;
using EstimatingLibrary;
using System.IO;
using System;
using EstimatingUtilitiesLibrary;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Deployment.Application;
using System.ComponentModel;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            else
            {
                Version = "Undeployed Version";
            }

            Bid = new TECBid();
            Templates = new TECTemplates();
           
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string resourcesFolder = Path.Combine(appDataFolder, Properties.Resources.ResourcesFolder);

            if (!Directory.Exists(resourcesFolder))
            {
                Directory.CreateDirectory(resourcesFolder);
            }

            defaultTemplatesPath = Path.Combine(resourcesFolder, Properties.Resources.TemplateFile);

            if (File.Exists(defaultTemplatesPath))
            {
                if (!UtilitiesMethods.IsFileLocked(defaultTemplatesPath))
                {
                    Templates = EstimatingLibraryDatabase.LoadDBToTemplates(defaultTemplatesPath);
                }
                else
                {
                    string message = "TECTemplates file is open elsewhere. Could not load default templates. Please close TECTemplates.tdb and restart program.";
                    MessageBox.Show(message);
                }
            }
            else
            {
                string message = "No template database found. Use Template Builder to create and save templates and reload them in Scope Builder.";
                MessageBox.Show(message);
            }
            
            Bid.DeviceCatalog = Templates.DeviceCatalog;
            Bid.ManufacturerCatalog = Templates.ManufacturerCatalog;
            Bid.Tags = Templates.Tags;

            bidDBFilePath = null;

            NewCommand = new RelayCommand(NewExecute);
            LoadCommand = new RelayCommand(LoadExecute);
            SaveCommand = new RelayCommand(SaveExecute);
            SaveAsCommand = new RelayCommand(SaveAsExecute);
            LoadTemplatesCommand = new RelayCommand(LoadTemplatesExecute);
            LoadDrawingCommand = new RelayCommand(LoadDrawingExecute);
            UndoCommand = new RelayCommand(UndoExecute, UndoCanExecute);
            RedoCommand = new RelayCommand(RedoExecute, RedoCanExecute);
            DocumentCommand = new RelayCommand(DocumentExecute);
            CSVExportCommand = new RelayCommand(CSVExportExecute);

            ClosingCommand = new RelayCommand<CancelEventArgs>(e => ClosingExecute(e));

            MessengerInstance.Register<NotificationMessage>(this, processNotification);
            MessengerInstance.Register<NotificationMessage<String>>(this, processNotificationInformation);

            TECLogo = Path.GetTempFileName();

            (Properties.Resources.TECLogo).Save(TECLogo, ImageFormat.Png);

            Stack = new ChangeStack(Bid);

            //Bid.Systems.CollectionChanged += Systems_CollectionChanged;
        }

        #region Properties
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

        public ChangeStack Stack { get; set; }

        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
                Bid.DeviceCatalog = Templates.DeviceCatalog;
                Bid.ManufacturerCatalog = Templates.ManufacturerCatalog;
                Bid.Tags = Templates.Tags;
                MessengerInstance.Send<GenericMessage<TECTemplates>>(new GenericMessage<TECTemplates>(Templates));
                MessengerInstance.Send<GenericMessage<TECBid>>(new GenericMessage<TECBid>(Bid));
            }
        }
        private TECTemplates _templates;

        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                Stack = new ChangeStack(Bid);
                MessengerInstance.Send(new GenericMessage<TECBid>(Bid));
                RaisePropertyChanged("Bid");
            }
        }
        private TECBid _bid;

        private string defaultTemplatesPath;
        private bool saveSuccessful;
        private string bidDBFilePath;

        public string TECLogo { get; set; }

        public string Version { get; set; }

        #region Command Properties

        public ICommand NewCommand { get; private set; }
        public ICommand LoadCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand LoadTemplatesCommand { get; private set; }
        public ICommand LoadDrawingCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand CSVExportCommand { get; private set; }
        public ICommand DocumentCommand { get; private set; }

        public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
        #endregion Command Properties
        #endregion Properties

        #region Methods

        #region Commands Methods
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
                bidDBFilePath = path;
                Properties.Settings.Default.ScopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    //Open database
                    Bid = EstimatingLibraryDatabase.LoadDBToBid(path, Templates);
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
            saveSuccessful = false;
            if (bidDBFilePath != null)
            {
                if (!UtilitiesMethods.IsFileLocked(bidDBFilePath))
                {
                    SaveWindow saveWindow = new SaveWindow();
                    try
                    {
                        EstimatingLibraryDatabase.UpdateBidToDB(bidDBFilePath, Stack);
                    }
                    catch (Exception e)
                    {
                        Console.Write("Save delta failed. Saving to new file. Error: " + e.Message);
                        EstimatingLibraryDatabase.SaveBidToNewDB(bidDBFilePath, Bid);
                    }
                    saveSuccessful = true;
                    Stack.ClearStacks();
                    saveWindow.Close();
                }
                else
                {
                    string message = "File is open elsewhere";
                    MessageBox.Show(message);
                }
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
                Properties.Settings.Default.ScopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    SaveWindow saveWindow = new SaveWindow();
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    EstimatingLibraryDatabase.SaveBidToNewDB(bidDBFilePath, Bid);
                    saveSuccessful = true;
                    Stack.ClearStacks();
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

        private void LoadTemplatesExecute()
        {
            //User choose path
            string path = getLoadTemplatesPath();

            if (path != null)
            {
                Properties.Settings.Default.TemplateDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    File.Copy(path, defaultTemplatesPath, true);
                    EstimatingLibraryDatabase.LoadDBToTemplates(path);
                }
                else
                {
                    string message = "File is open elsewhere.";
                    MessageBox.Show(message);
                }
                Console.WriteLine("Finished loading templates.");
            }
        }

        private void LoadDrawingExecute()
        {
            string path = getLoadDrawingsPath();

            if (path != null)
            {
                Properties.Settings.Default.DrawingDirectoryPath = Path.GetDirectoryName(path);
                
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CurrentStatusText = "Loading Drawings...";
                    var worker = new BackgroundWorker();

                    worker.DoWork += (s, e) => {
                        TECDrawing drawing = PDFConverter.convertPDFToDrawing(path);
                        e.Result = drawing;
                    };
                    worker.RunWorkerCompleted += (s, e) =>
                    {
                        if(e.Result is TECDrawing)
                        {
                            Bid.Drawings.Add((TECDrawing)e.Result);
                            CurrentStatusText = "Done.";
                            string message = "Drawings have finished loading.";
                            MessageBox.Show(message);
                        } else
                        {
                            Console.WriteLine("Load Drawings Failed");
                        }
                    };
                    worker.RunWorkerAsync();
                }
                else
                {
                    string message = "File is open elsewhere.";
                    MessageBox.Show(message);
                }
            }
        }

        private void UndoExecute()
        {
            Stack.Undo();
        }
        private bool UndoCanExecute()
        {
            return true;
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
            return true;
            if (Stack.RedoStack.Count > 0)
                return true;
            else
                return false;
        }

        private void DocumentExecute()
        {
            string path = getDocumentSavePath();

            if (path != null)
            {
                Properties.Settings.Default.DocumentDirectoryPath = Path.GetDirectoryName(path);

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
                Properties.Settings.Default.PointCSVDirectoryPath = Path.GetDirectoryName(path);

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

        private void ClosingExecute(CancelEventArgs e)
        {
            bool changes = (Stack.SaveStack.Count > 0);
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
        #endregion Commands Methods

        #region Helper Methods

        private string getLoadPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (Properties.Settings.Default.ScopeDirectoryPath != null)
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.ScopeDirectoryPath;
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

        private string getSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (Properties.Settings.Default.ScopeDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = Properties.Settings.Default.ScopeDirectoryPath;
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

        private string getLoadTemplatesPath()
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

        private string getLoadDrawingsPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (Properties.Settings.Default.TemplateDirectoryPath != null)
            {
                openFileDialog.InitialDirectory = Properties.Settings.Default.DrawingDirectoryPath;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            openFileDialog.DefaultExt = "pdf";
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

        private string getDocumentSavePath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (Properties.Settings.Default.DocumentDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = Properties.Settings.Default.DocumentDirectoryPath;
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
            if (Properties.Settings.Default.PointCSVDirectoryPath != null)
            {
                saveFileDialog.InitialDirectory = Properties.Settings.Default.PointCSVDirectoryPath;
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

        #endregion Helper Methods

        private void processNotification(NotificationMessage message)
        {
            if (message.Notification == "ScopeEditorViewModelLoaded")
            {
                Console.WriteLine("Scope Editor View Model Loaded");
                MessengerInstance.Send<GenericMessage<TECTemplates>>(new GenericMessage<TECTemplates>(Templates));
                MessengerInstance.Send<GenericMessage<TECBid>>(new GenericMessage<TECBid>(Bid));
            }
            else if(message.Notification == "LaborViewModelLoaded")
            {
                Console.WriteLine("Labor View Model Loaded");
                MessengerInstance.Send<GenericMessage<TECBid>>(new GenericMessage<TECBid>(Bid));
            }
            else if (message.Notification == "DrawingViewModelLoaded")
            {
                Console.WriteLine("Drawing View Model Loaded");
                MessengerInstance.Send<GenericMessage<TECBid>>(new GenericMessage<TECBid>(Bid));
            }
            else if (message.Notification == "ReviewViewModelLoaded")
            {
                Console.WriteLine("Review View Model Loaded");
                MessengerInstance.Send<GenericMessage<TECBid>>(new GenericMessage<TECBid>(Bid));
            }
        }

        private void processNotificationInformation(NotificationMessage<String> message)
        {
            if (message.Notification == "StatusUpdate")
            {
                CurrentStatusText = message.Content;
            }
        }

        #endregion Methods
    }
}