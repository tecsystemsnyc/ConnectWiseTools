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
using GongSolutions.Wpf.DragDrop;
using System.Linq;
using TECUserControlLibrary;

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
        public MainViewModel() : base()
        {
            isEstimate = true;
            programName = "Estimate Builder";
            buildTitleString();
            workingFileParameters = EstimateFileParameters;

            LoadDrawingCommand = new RelayCommand(LoadDrawingExecute);
            ToggleTemplatesCommand = new RelayCommand(ToggleTemplatesExecute);
            setupMenuVM();
        }

        #region Properties

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
        protected override string TemplatesFilePath
        {
            get
            {
                return base.TemplatesFilePath;
            }

            set
            {
                base.TemplatesFilePath = value;
                SettingsVM.TemplatesLoadPath = TemplatesFilePath;
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

        #region ViewModels
        public ScopeEditorViewModel ScopeEditorVM { get; set; }
        public DrawingViewModel DrawingVM { get; set; }
        public LaborViewModel LaborVM { get; set; }
        public ReviewViewModel ReviewVM { get; set; }
        public ProposalViewModel ProposalVM { get; set; }
        public ElectricalViewModel ElectricalVM { get; set; }
        public NetworkViewModel NetworkVM { get; set; }
        public TECMaterialViewModel TECMaterialVM { get; set; }
        public ElectricalMaterialSummaryViewModel ElectricalMaterialVM { get; set; }
        #endregion

        #region Command Properties
        public ICommand LoadDrawingCommand { get; private set; }
        public ICommand ToggleTemplatesCommand { get; private set; }
        #endregion Command Properties

        public override Visibility TemplatesVisibility
        {
            get
            {
                return ScopeEditorVM.TemplatesVisibility;
            }

            set
            {
                ScopeEditorVM.TemplatesVisibility = value;
                RaisePropertyChanged("TemplatesVisibility");
            }
        }

        protected override bool templatesLoaded
        {
            get
            {
                return base.templatesLoaded;
            }

            set
            {
                base.templatesLoaded = value;
                if (LaborVM != null)
                {
                    LaborVM.TemplatesLoaded = templatesLoaded;
                }
            }
        }
        #endregion Properties

        #region Methods

        #region VM Setup Methods
        private void setupScopeEditorVM(TECBid bid, TECTemplates templates)
        {
            ScopeEditorVM = new ScopeEditorViewModel(bid, templates);
            ScopeEditorVM.PropertyChanged += ScopeEditorVM_PropertyChanged;
            if (TemplatesHidden)
            {
                ScopeEditorVM.TemplatesVisibility = Visibility.Hidden;
            }
            else
            {
                ScopeEditorVM.TemplatesVisibility = Visibility.Visible;
            }
        }
        private void setupDrawingVM(TECBid bid)
        {
            //DebugHandler.LogDebugMessage("Setting up drawing VM");
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
        private void setupMenuVM()
        {
            MenuVM.TemplatesHidden = TemplatesHidden;
            MenuVM.ToggleTemplatesCommand = ToggleTemplatesCommand;
        }
        private void setupNetworkVM(TECBid bid)
        {
            NetworkVM = new NetworkViewModel(bid);
        }
        private void setupDeviceVM(TECBid bid)
        {
            TECMaterialVM = new TECMaterialViewModel(bid);
        }
        private void setupElectricalMaterialVM(TECBid bid)
        {
            ElectricalMaterialVM = new ElectricalMaterialSummaryViewModel(bid);
        }
        #endregion

        #region Commands Methods
        private void LoadDrawingExecute()
        {
            string path = getLoadDrawingsPath();

            if (path != null)
            {
                if (!IsReady)
                {
                    MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                    return;
                }

                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    SetBusyStatus("Loading drawings from file: " + path, true);
                    var worker = new BackgroundWorker();

                    worker.DoWork += (s, e) =>
                    {
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
        private void ToggleTemplatesExecute()
        {
            if (TemplatesHidden)
            {
                TemplatesHidden = false;
            }
            else
            {
                TemplatesHidden = true;
            }
        }
        #endregion Commands Methods

        #region Helper Methods
        override protected void setupExtensions()
        {
            base.setupExtensions();
            setupScopeEditorVM(new TECBid(), new TECTemplates());
            setupDrawingVM(new TECBid());
            setupLaborVM(new TECBid(), new TECTemplates());
            setupReviewVM(new TECBid());
            setupProposalVM(new TECBid());
            setupElectricalVM(new TECBid());
            setupMenuVM();
            setupNetworkVM(new TECBid());
            setupDeviceVM(new TECBid());
            setupElectricalMaterialVM(new TECBid());
        }
        override protected void refresh()
        {
            if (Bid != null && Templates != null)
            {
                ScopeEditorVM.Refresh(Bid, Templates);
                DrawingVM.Bid = Bid;
                LaborVM.Refresh(Bid, Templates);
                ReviewVM.Refresh(Bid);
                ProposalVM.Refresh(Bid);
                ElectricalVM.Refresh(Bid);
                NetworkVM.Refresh(Bid);
                TECMaterialVM.Refresh(Bid);
                ElectricalMaterialVM.Refresh(Bid);
            }
        }
        private string getLoadDrawingsPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

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