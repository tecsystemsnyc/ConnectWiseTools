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
    /// 
    
    public class SystemsViewModel : ViewModelBase, IDropTarget
    {
        /// <summary>
        /// Initializes a new instance of the SystemsViewModel class.
        /// </summary>
        /// 
        public delegate void DragHandler(IDropInfo dropInfo);

        public SystemsViewModel()
        {
            

        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler dragHandler = DragOver;
        }

        public void Drop(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }
    }
}