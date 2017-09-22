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

namespace EstimateBuilder.MVVM
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

            ToggleTemplatesCommand = new RelayCommand(toggleTemplatesExecute);
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
        public ScopeEditorVM ScopeEditorVM { get; set; }
        public LaborVM LaborVM { get; set; }
        public ReviewVM ReviewVM { get; set; }
        public ProposalVM ProposalVM { get; set; }
        public ElectricalVM ElectricalVM { get; set; }
        #endregion

        #region Command Properties
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
            ScopeEditorVM = new ScopeEditorVM(bid, templates);
            ScopeEditorVM.PropertyChanged += scopeEditorVM_PropertyChanged;
            if (TemplatesHidden)
            {
                ScopeEditorVM.TemplatesVisibility = Visibility.Hidden;
            }
            else
            {
                ScopeEditorVM.TemplatesVisibility = Visibility.Visible;
            }
        }
        private void setupLaborVM(TECBid bid, TECTemplates templates)
        {
            LaborVM = new LaborVM();
            LaborVM.Bid = bid;
            LaborVM.Templates = templates;
            LaborVM.LoadTemplates += LoadTemplatesExecute;
            LaborVM.TemplatesLoaded = templatesLoaded;
        }
        private void setupReviewVM(TECBid bid)
        {
            ReviewVM = new ReviewVM();
            ReviewVM.Bid = bid;
        }
        private void setupProposalVM(TECBid bid)
        {
            ProposalVM = new ProposalVM(bid);
        }
        private void setupElectricalVM(TECBid bid)
        {
            ElectricalVM = new ElectricalVM(bid);
        }
        private void setupMenuVM()
        {
            MenuVM.TemplatesHidden = TemplatesHidden;
            MenuVM.ToggleTemplatesCommand = ToggleTemplatesCommand;
        }
        #endregion

        #region Commands Methods
        private void toggleTemplatesExecute()
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
            setupLaborVM(new TECBid(), new TECTemplates());
            setupReviewVM(new TECBid());
            setupProposalVM(new TECBid());
            setupElectricalVM(new TECBid());
            setupMenuVM();
        }
        override protected void refresh()
        {
            if (Bid != null && Templates != null)
            {
                ScopeEditorVM.Refresh(Bid, Templates);
                LaborVM.Refresh(Bid, Templates);
                //ReviewVM.Refresh(Bid);
                ProposalVM.Refresh(Bid);
                ElectricalVM.Refresh(Bid);
            }
        }
        #endregion

        #endregion

        #region Event Handlers
        private void settingsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
        private void scopeEditorVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
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