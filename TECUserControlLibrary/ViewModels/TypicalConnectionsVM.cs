using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class TypicalConnectionsVM : ViewModelBase, IDropTarget
    {
        #region Fields
        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<TypicalSubScope> _subScope;
        private ObservableCollection<TECSubScope> _unconnectedSubScope;
        private TECController _selectedController;
        #endregion

        #region Properties
        public ObservableCollection<TECController> Controllers
        {
            get
            {
                return _controllers;
            }
            set
            {
                _controllers = value;
                RaisePropertyChanged("Controllers");
            }
        }
        public ObservableCollection<TypicalSubScope> SubScope
        {
            get { return _subScope; }
            set
            {
                _subScope = value;
                RaisePropertyChanged("SubScope");
            }
        }
        public ObservableCollection<TECSubScope> UnconnectedSubScope
        {
            get
            {
                return _unconnectedSubScope;
            }
            set
            {
                _unconnectedSubScope = value;
                RaisePropertyChanged("UnconnectedSubScope");
            }
        }
        public TECController SelectedController
        {
            get
            {
                return _selectedController;
            }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
            }
        }

        public ICommand UpdateAllCommand;
        #endregion

        public TypicalConnectionsVM(TECTypical typical)
        {
            initializeCollections();
            foreach (TECSubScope ss in typical.GetAllSubScope())
            {
                if (ss.ParentConnection == null)
                {
                    UnconnectedSubScope.Add(ss);
                }else
                {
                    SubScope.Add(new TypicalSubScope(ss, typical.TypicalInstanceDictionary.GetInstances(ss).ConvertAll((x) => (TECSubScope)x)));
                }
            }
            foreach (TECController controller in typical.Controllers)
            {
                Controllers.Add(controller);
            }
        }

        public event Action<UpdateConnectionVM> Update;

        private void initializeCollections()
        {
            _controllers = new ObservableCollection<TECController>();
            _subScope = new ObservableCollection<TypicalSubScope>();
            _unconnectedSubScope = new ObservableCollection<TECSubScope>();
        }

        private void updateAllExecute()
        {
            foreach(TypicalSubScope typSS in SubScope)
            {
                throw new NotImplementedException();
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            if(subScope != null && SelectedController != null && SelectedController.CanConnectSubScope(subScope))
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Copy;
            } 
        }

        public void Drop(IDropInfo dropInfo)
        {
            TECSubScope subScope = dropInfo.Data as TECSubScope;
            SelectedController.AddSubScope(subScope);
        }
    }
}
