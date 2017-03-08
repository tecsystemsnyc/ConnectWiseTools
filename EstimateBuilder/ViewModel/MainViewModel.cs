using GalaSoft.MvvmLight;
using EstimateBuilder.Model;
using EstimatingLibrary;
using System.IO;
using System;
using EstimatingUtilitiesLibrary;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Deployment.Application;
using System.ComponentModel;
using TECUserControlLibrary.ViewModels;
using DebugLibrary;

namespace EstimateBuilder.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : BidEditorBase
    {
        public MainViewModel()
        {
            programName = "Estimate Builder";

            if (ApplicationDeployment.IsNetworkDeployed)
            { Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { Version = "Undeployed Version"; }

            LoadDrawingCommand = new RelayCommand(LoadDrawingExecute);
            
            scopeDirectoryPath = Properties.Settings.Default.ScopeDirectoryPath;

            BidSet += () =>
            {
                refreshAllBids();
            };

            setupAll();

            base.PropertyChanged += BidEditorBase_PropertyChanged;

        }

        

        #region Properties

        #region SettingsProperties
        public bool TemplatesHidden
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
                }
            }
        }
        #endregion

        #region ViewModels
        public ScopeEditorViewModel ScopeEditorVM { get; set; }
        public DrawingViewModel DrawingVM { get; set; }
        public LaborViewModel LaborVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public ReviewViewModel ReviewVM { get; set; }
        public ProposalViewModel ProposalVM { get; set; }
        public ElectricalViewModel ElectricalVM { get; set; }
        #endregion

        #region Command Properties
        public ICommand LoadDrawingCommand { get; private set; }
        #endregion Command Properties
        #endregion Properties

        #region Methods

        #region VM Setup Methods
        private void setupScopeEditorVM(TECBid bid, TECTemplates templates)
        {
            ScopeEditorVM = new ScopeEditorViewModel(bid, templates);
            ScopeEditorVM.PropertyChanged += ScopeEditorVM_PropertyChanged;
        }
        private void setupDrawingVM(TECBid bid)
        {
            DebugHandler.LogDebugMessage("Setting up drawing VM");
            DrawingVM = new DrawingViewModel();
            DrawingVM.Bid = bid;
            DrawingVM.Templates = Templates;
        }
        private void setupLaborVM(TECBid bid, TECTemplates templates)
        {
            LaborVM = new LaborViewModel();
            LaborVM.Bid = bid;
            LaborVM.Templates = templates;
            LaborVM.LoadTemplates += LoadTemplatesExecute;
            LaborVM.TemplatesLoaded = templatesLoaded;
            TemplatesLoadedSet += () => {
                LaborVM.TemplatesLoaded = templatesLoaded;
            };
        }
        private void setupSettingsVM()
        {
            SettingsVM = new SettingsViewModel();
            SettingsVM.PropertyChanged += SettingsVM_PropertyChanged;
            SettingsVM.TemplatesLoadPath = TemplatesFilePath;
            SettingsVM.ReloadTemplates += LoadTemplatesExecute;
        }
        private void setupReviewVM(TECBid bid)
        {
            ReviewVM = new ReviewViewModel();
            ReviewVM.Bid = bid;
        }
        private void setupProposalVM(TECBid bid)
        {
            ProposalVM = new ProposalViewModel(bid);
        }
        private void setupElectricalVM(TECBid bid)
        {
            ElectricalVM = new ElectricalViewModel(bid);
        }
        #endregion

        #region Commands Methods
        private void LoadDrawingExecute()
        {
            string path = getLoadDrawingsPath();

            if (path != null)
            {
                if (!isReady)
                {
                    MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                    return;
                }
                Properties.Settings.Default.ScopeDirectoryPath = Path.GetDirectoryName(path);

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    SetBusyStatus("Loading drawings from file: " + path);
                    var worker = new BackgroundWorker();

                    worker.DoWork += (s, e) => {
                        TECDrawing drawing = PDFConverter.convertPDFToDrawing(path);
                        e.Result = drawing;
                    };
                    worker.RunWorkerCompleted += (s, e) =>
                    {
                        if (e.Result is TECDrawing)
                        {
                            Bid.Drawings.Add((TECDrawing)e.Result);
                            ResetStatus();
                            MessageBox.Show("Drawings have finished loading.");
                        }
                        else
                        {
                            DebugHandler.LogError("Load Drawings Failed");
                        }
                    };
                    worker.RunWorkerAsync();
                }
                else
                {
                    DebugHandler.LogError("File " + path + " could not be opened. File is open elsewhere.");
                }
            }
        }
        #endregion Commands Methods

        #region Helper Methods
        private void setupAll()
        {
            setupScopeEditorVM(Bid, Templates);
            setupDrawingVM(Bid);
            setupLaborVM(Bid, Templates);
            setupReviewVM(Bid);
            setupSettingsVM();
            setupProposalVM(Bid);
            setupElectricalVM(Bid);
        }

        private void refreshAllBids()
        {
            ScopeEditorVM.Bid = Bid;
            DrawingVM.Bid = Bid;
            LaborVM.Bid = Bid;
            ReviewVM.Bid = Bid;
            //SettingsVM.Bid = Bid;
            ProposalVM.Bid = Bid;
            ElectricalVM.refresh(Bid);
        }

        private void refreshAllTemplates()
        {
            ScopeEditorVM.Templates = Templates;
            LaborVM.Templates = Templates;
        }

        private void getVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            { Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { Version = "Undeployed Version"; }
        }
        private string getLoadDrawingsPath()
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

            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            openFileDialog.DefaultExt = "pdf";
            openFileDialog.AddExtension = true;

            string path = null;

            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
            }

            return path;
        }
        #endregion

        #endregion

        #region Event Handlers
        private void TemplatesHiddenChanged()
        {
            SettingsVM.TemplatesHidden = TemplatesHidden;
            if (TemplatesHidden)
            {
                ScopeEditorVM.TemplatesVisibility = Visibility.Hidden;
            }
            else
            {
                ScopeEditorVM.TemplatesVisibility = Visibility.Visible;
            }
        }
        private void BidEditorBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TemplatesFilePath")
            {
                SettingsVM.TemplatesLoadPath = TemplatesFilePath;
            }
            else if (e.PropertyName == "Templates")
            {
                refreshAllTemplates();
            }
        }
        private void SettingsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TemplatesHidden")
            {
                TemplatesHidden = SettingsVM.TemplatesHidden;
            }
            else if (e.PropertyName == "TemplatesLoadPath")
            {
                TemplatesFilePath = SettingsVM.TemplatesLoadPath;
            }
        }

        private void ScopeEditorVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TemplatesVisibility")
            {
                if (ScopeEditorVM.TemplatesVisibility == Visibility.Visible)
                {
                    TemplatesHidden = false;
                }
                else if (ScopeEditorVM.TemplatesVisibility == Visibility.Hidden)
                {
                    TemplatesHidden = true;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        #endregion
    }
}