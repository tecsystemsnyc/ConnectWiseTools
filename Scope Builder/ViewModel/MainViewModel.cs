using GalaSoft.MvvmLight;
using EstimatingLibrary;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System;
using Microsoft.Win32;
using EstimatingUtilitiesLibrary;
using System.Windows;
using System.IO;
using GongSolutions.Wpf.DragDrop;
using GalaSoft.MvvmLight.Messaging;
using TECUserControlLibrary;
using System.Collections;
using System.Drawing.Imaging;
using System.Deployment.Application;
using System.ComponentModel;
using System.Windows.Controls;
using System.Reflection;
using TECUserControlLibrary.ViewModelExtensions;
using TECUserControlLibrary.ViewModels;

namespace Scope_Builder.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : BidEditorBase, IDropTarget
    {
        #region Properties

        private int _dGTabIndex;
        public int DGTabIndex
        {
            get { return _dGTabIndex; }
            set
            {
                _dGTabIndex = value;
                RaisePropertyChanged("DGTabIndex");
            }
        }

        #region VMExtensions
        public ScopeDataGridExtension ScopeDataGrid { get; set; }
        public LocationDataGridExtension LocationDataGrid { get; set; }
        public ScopeCollectionExtension ScopeCollection { get; set; }

        public DocumentBuilderViewModel DocumentBuilderVM { get; set; }
        #endregion

        #region Commands Properties
        public ICommand ToggleTemplatesVisibilityCommand { get; private set; }
        #endregion //Commands Properties

        #region Visibility Properties
        private Visibility _templatesVisibility;
        public Visibility TemplatesVisibility
        {
            get
            {
                return _templatesVisibility;
            }
            set
            {
                _templatesVisibility = value;
                RaisePropertyChanged("TemplatesVisibility");
            }
        }
        #endregion Visibility Properties
        #endregion

        #region Fields

        #endregion

        #region Intitializer
        public MainViewModel()
        {
            programName = "Scope Builder";

            setupScopeDataGrid();
            setupLocationDataGrid();
            setupScopeCollection();
            getVersion();
            DGTabIndex = 0;

            setVisibility(0);

            ToggleTemplatesVisibilityCommand = new RelayCommand(ToggleTemplatesVisibilityExecute);
            TemplatesVisibility = Visibility.Visible;

            startupFile = Properties.Settings.Default.StartupFile;
            pointCSVDirectoryPath = Properties.Settings.Default.PointCSVDirectoryPath;
            scopeDirectoryPath = Properties.Settings.Default.ScopeDirectoryPath;
            documentDirectoryPath = Properties.Settings.Default.DocumentDirectoryPath;

            checkForOpenWith(Properties.Settings.Default.StartupFile);
        }
        #endregion 

        #region Commands Methods
        private void ToggleTemplatesVisibilityExecute()
        {
            if (TemplatesVisibility == Visibility.Visible)
            {
                TemplatesVisibility = Visibility.Hidden;
            }
            else if (TemplatesVisibility == Visibility.Hidden)
            {
                TemplatesVisibility = Visibility.Visible;
            }
        }

        #endregion //Commands Methods

        #region Helper Functions
        private void setVisibility(int tIndex)
        {
            ScopeCollection.ControllerEditVisibility = Visibility.Collapsed;
            ScopeCollection.DevicesEditVisibility = Visibility.Collapsed;
            ScopeCollection.TagsVisibility = Visibility.Collapsed;
            ScopeCollection.ManufacturerVisibility = Visibility.Collapsed;

            switch (tIndex)
            {
                case 0:
                    ScopeDataGrid.DataGridVisibilty.ExpandSubScope = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void getVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            { Version = "Version " + ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            else
            { Version = "Undeployed Version"; }
        }
        #endregion //Helper Functions

        #region Setup Extensions
        private void setupScopeDataGrid()
        {
            ScopeDataGrid = new ScopeDataGridExtension(Bid);
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
        }
        private void setupLocationDataGrid()
        {
            LocationDataGrid = new LocationDataGridExtension(Bid);
            LocationDataGrid.DragHandler += DragOver;
            LocationDataGrid.DropHandler += Drop;
        }
        private void setupScopeCollection()
        {
            ScopeCollection = new ScopeCollectionExtension(Templates);
            ScopeCollection.DragHandler += DragOver;
            ScopeCollection.DropHandler += Drop;
        }
        #endregion
        
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
            {
                sourceItem = ((TECScope)dropInfo.Data).DragDropCopy();
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    ((IList)dropInfo.TargetCollection).Add(sourceItem);
                }
                else
                {
                    ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                }
            }
            else
            {
                sourceItem = dropInfo.Data;
                int currentIndex = ((IList)dropInfo.TargetCollection).IndexOf(sourceItem);
                int removeIndex = currentIndex;
                if (dropInfo.InsertIndex < currentIndex)
                {
                    removeIndex += 1;
                }
                if (dropInfo.InsertIndex > ((IList)dropInfo.TargetCollection).Count)
                {
                    ((IList)dropInfo.TargetCollection).Add(sourceItem);
                }
                else
                {
                    ((IList)dropInfo.TargetCollection).Insert(dropInfo.InsertIndex, sourceItem);
                }
                ((IList)dropInfo.TargetCollection).RemoveAt(removeIndex);
            }
        }
        #endregion

    }
}