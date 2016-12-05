using System;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DevicesPointsViewModel : ViewModelBase, IDropTarget
    {
        public delegate void DragHandler(IDropInfo dropInfo);
        public delegate void DropHandler(IDropInfo dropInfo);

        /// <summary>
        /// Initializes a new instance of the DevicesPointsViewModel class.
        /// </summary>
        public DevicesPointsViewModel()
        {

        }

        public void DragOver(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public void Drop(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }
        
    }
}