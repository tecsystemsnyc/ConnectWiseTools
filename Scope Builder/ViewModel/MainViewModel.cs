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
        #region VMExtensions
        public ScopeDataGridExtension ScopeDataGrid { get; set; }
        public LocationDataGridExtension LocationDataGrid { get; set; }
        public ScopeCollectionExtension ScopeCollection { get; set; }
        #endregion
        
        public int DGTabIndex
        {
            get { return _dgTabIndex; }
            set
            {
                _dgTabIndex = value;
                RaisePropertyChanged("DGTabIndex");
            }
        }
        private int _dgTabIndex;

        #endregion

        #region Fields
       
        #endregion

        #region Intitializer
        public MainViewModel()
        {
            Console.WriteLine("Child");
            setupScopeDataGrid();
            setupLocationDataGrid();
            getVersion();

            setVisibility(0);
            startupFile = Properties.Settings.Default.StartupFile;
            pointCSVDirectoryPath = Properties.Settings.Default.PointCSVDirectoryPath;
            scopeDirectoryPath = Properties.Settings.Default.ScopeDirectoryPath;
            documentDirectoryPath = Properties.Settings.Default.DocumentDirectoryPath;

            checkForOpenWith(Properties.Settings.Default.StartupFile);

        }
        #endregion 
        
        #region Commands
        
       
        #endregion //Commands

        #region Helper Functions
        private void setVisibility(int tIndex)
        {
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
            { sourceItem =  ((TECScope)dropInfo.Data).DragDropCopy(); }
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

    }
}