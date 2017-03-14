using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using TECUserControlLibrary.ViewModelExtensions;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ControlledScopeViewModel : ViewModelBase, IDropTarget
    {

        #region VM Extenstions
        public ScopeDataGridExtension ScopeDataGrid { get; set; }
        #endregion

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;
        #endregion

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

        public ControlledScopeViewModel(TECTemplates templates)
        {
            Templates = templates;
        }

        private void setupVMs()
        {
            ScopeDataGrid = new ScopeDataGridExtension(Templates);
            ScopeDataGrid.DragHandler += DragOver;
            ScopeDataGrid.DropHandler += Drop;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler(dropInfo);
        }

        public void Drop(IDropInfo dropInfo)
        {
            DropHandler(dropInfo);
        }

    }
}