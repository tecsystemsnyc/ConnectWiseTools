using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class NetworkViewModel : ViewModelBase
    {
        #region Properties
        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                _bid = value;
                update();
            }
        }

        private ObservableCollection<NetworkControllerConnnection> _serverSource;
        public ObservableCollection<NetworkControllerConnnection> ServerSource
        {
            get { return _serverSource; }
            set
            {
                _serverSource = value;
                RaisePropertyChanged("ServerSource");
            }
        }

        private ObservableCollection<NetworkControllerConnnection> _bmsControllerSource;
        public ObservableCollection<NetworkControllerConnnection> BMSControllerSource
        {
            get { return _bmsControllerSource; }
            set
            {
                _bmsControllerSource = value;
                RaisePropertyChanged("BMSControllerSource");
            }
        }

        private ObservableCollection<NetworkControllerConnnection> _generalControllerSource;
        public ObservableCollection<NetworkControllerConnnection> GeneralControllerSource
        {
            get { return _generalControllerSource; }
            set
            {
                _generalControllerSource = value;
                RaisePropertyChanged("GeneralControllerSource");
            }
        }

        public NetworkControllerConnnection SelectedBMSController { get; set; }
        public NetworkControllerConnnection SelectedGeneralController { get; set; }
        #endregion

        #region Commands
        public ICommand AddServerCommand { get; private set; }
        public ICommand AddBMSControllerCommand { get; private set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the NetworkGridExtension class.
        /// </summary>
        public NetworkViewModel(TECBid bid)
        {
            _bid = bid;

            AddServerCommand = new RelayCommand(AddServerExecute);
            AddBMSControllerCommand = new RelayCommand(AddBMSControllerExecute);

            update();
        }

        private void Controllers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECController)
                    {
                        sortAndAddController(item as TECController);
                    }
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is TECController)
                    {
                        removeController(item as TECController);
                    }
                }
            }
        }

        #region Methods
        private void update()
        {
            Bid.Controllers.CollectionChanged += Controllers_CollectionChanged;

            ServerSource = new ObservableCollection<NetworkControllerConnnection>();
            BMSControllerSource = new ObservableCollection<NetworkControllerConnnection>();
            GeneralControllerSource = new ObservableCollection<NetworkControllerConnnection>();

            foreach (TECController controller in Bid.Controllers)
            {
                sortAndAddController(controller);
            }
        }

        private void sortAndAddController(TECController controller)
        {
            if (controller.IsServer)
            {
                ServerSource.Add(new NetworkControllerConnnection(controller));
            }
            else if (controller.IsBMS)
            {
                BMSControllerSource.Add(new NetworkControllerConnnection(controller));
            }
            else
            {
                GeneralControllerSource.Add(new NetworkControllerConnnection(controller));
            }
        }

        private void removeController(TECController controller)
        {
            NetworkControllerConnnection serverToRemove = null;
            foreach (NetworkControllerConnnection server in ServerSource)
            {
                if (server.Controller == controller)
                {
                    serverToRemove = server;
                }
                else if (server.ParentController == controller)
                {
                    server.ParentController = null;
                }
            }
            if (serverToRemove != null)
            {
                ServerSource.Remove(serverToRemove);
            }

            NetworkControllerConnnection bmsControllerToRemove = null;
            foreach (NetworkControllerConnnection bmsController in BMSControllerSource)
            {
                if (bmsController.Controller == controller)
                {
                    bmsControllerToRemove = bmsController;
                }
                else if (bmsController.ParentController == controller)
                {
                    bmsController.ParentController = null;
                }
            }
            if (bmsControllerToRemove != null)
            {
                BMSControllerSource.Remove(bmsControllerToRemove);
            }

            NetworkControllerConnnection generalControllerToRemove = null;
            foreach (NetworkControllerConnnection generalController in GeneralControllerSource)
            {
                if (generalController.Controller == controller)
                {
                    generalControllerToRemove = generalController;
                }
                else if (generalController.ParentController == controller)
                {
                    generalController.ParentController = null;
                }
            }
            if (generalControllerToRemove != null)
            {
                GeneralControllerSource.Remove(generalControllerToRemove);
            }
        }

        private void AddServerExecute()
        {
            ServerSource.Add(SelectedBMSController);
            SelectedBMSController.Controller.IsServer = true;
            BMSControllerSource.Remove(SelectedBMSController);
        }

        private void AddBMSControllerExecute()
        {
            BMSControllerSource.Add(SelectedGeneralController);
            SelectedGeneralController.Controller.IsBMS = true;
            GeneralControllerSource.Remove(SelectedGeneralController);
        }
        #endregion
    }
}