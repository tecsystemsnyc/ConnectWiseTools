using ConnectWiseDotNetSDK.ConnectWise.Client;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Api;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Model;
using ConnectWiseInformationInterface.Export;
using ConnectWiseInformationInterface.Models;
using ConnectWiseInformationInterface.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ConnectWiseInformationInterface.ViewModel
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
    public class MainViewModel : ViewModelBase
    {
        private readonly OppTypeBool allTypeBool;

        private OppFilterManager _oppManager;
        private readonly ObservableCollection<OppTypeBool> _oppTypeBools;
        private string _status;
        private int _numOppTypes;
        private bool _includeNoType;

        public OppFilterManager OppManager
        {
            get { return _oppManager; }
            private set
            {
                if (OppManager != value)
                {
                    _oppManager = value;
                    RaisePropertyChanged("OppManager");
                }
            }
        }
        public ReadOnlyObservableCollection<OppTypeBool> OppTypeBools
        {
            get
            {
                return new ReadOnlyObservableCollection<OppTypeBool>(_oppTypeBools);
            }
        }
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }
        public int NumOppTypes
        {
            get { return _numOppTypes; }
            set
            {
                if (_numOppTypes != value)
                {
                    _numOppTypes = value;
                    RaisePropertyChanged("NumOppTypes");
                }
            }
        }
        public bool IncludeNoType
        {
            get { return _includeNoType; }
            set
            {
                if (_includeNoType != value)
                {
                    _includeNoType = value;
                    OppManager.IncludeNoType = value;
                    RaisePropertyChanged("IncludeNoType");
                }
            }
        }

        public ICommand LoadOpportunitiesCommand { get; private set; }
        public ICommand RefreshOpportunitiesCommand { get; private set; }
        public ICommand ExportOpportunitiesCommand { get; private set; }
        public ICommand ClearDatesCommand { get; private set; }
        public ICommand ClearTypesCommand { get; private set; }

        public SettingsVM SettingsVM { get; private set; }

        public MainViewModel()
        {
            _oppManager = new OppFilterManager(new List<Opportunity>(), new List<SalesProbability>(), new List<OpportunityType>());

            OpportunityType allType = new OpportunityType();
            allType.Description = "All";
            allTypeBool = new OppTypeBool(allType);
            allTypeBool.PropertyChanged += oppTypeBoolChanged;

            _oppTypeBools = new ObservableCollection<OppTypeBool>();
            resetOppTypes();

            Status = "Not Loaded";
            NumOppTypes = 0;
            IncludeNoType = false;

            LoadOpportunitiesCommand = new RelayCommand(loadOpportunitiesExecute, loadOpportunitiesCanExecute);
            RefreshOpportunitiesCommand = new RelayCommand(loadOpportunitiesExecute, refreshOpportunitiesCanExecute);
            ExportOpportunitiesCommand = new RelayCommand(exportOpportunitiesExecute, exportOpportunitiesCanExecute);
            ClearDatesCommand = new RelayCommand(clearDatesExecute, clearDatesCanExecute);
            ClearTypesCommand = new RelayCommand(clearTypesExecute, clearTypesCanExecute);

            SettingsVM = new SettingsVM();
        }

        private void resetOppTypes()
        {
            _oppTypeBools.ObservablyClear();
            _oppTypeBools.Add(allTypeBool);
        }

        private void loadOpportunitiesExecute()
        {
            Status = "Loading...";
            //Using Keys
            ApiClient connectWiseClient = new ApiClient(SettingsVM.AppID, SettingsVM.Site, SettingsVM.CompanyName)
                .SetPublicPrivateKey(SettingsVM.PublicKey, SettingsVM.PrivateKey);

            //Load Opportunity Types
            List<OpportunityType> oppTypes = new OpportunityTypesApi(connectWiseClient)
                .GetTypes(pageSize: 1000)
                .GetResult<List<OpportunityType>>();

            NumOppTypes = oppTypes.Count;

            if (oppTypes == null)
            {
                showCantConnect();
                Status = "Load Failed";
                return;
            }
            else
            {
                oppTypes.Sort(oppTypeAlphaComparer);

                int oppTypeAlphaComparer(OpportunityType type1, OpportunityType type2)
                {
                    return string.Compare(type1.Description, type2.Description);
                }
            }

            resetOppTypes();
            foreach(OpportunityType oppType in oppTypes)
            {
                addOppType(oppType);
            }

            //Load Opportunities
            List<Opportunity> opps = new OpportunitiesApi(connectWiseClient)
                .GetOpportunities(pageSize: 1000)
                .GetResult<List<Opportunity>>();

            if (opps == null)
            {
                showCantConnect();
                Status = "Load Failed";
                return;
            }

            //Load Probabilities
            List<SalesProbability> probabilities = new SalesProbabilitiesApi(connectWiseClient)
                .GetProbabilities()
                .GetResult<List<SalesProbability>>();

            if (probabilities == null)
            {
                showCantConnect();
                return;
            }

            //Reset OppFilterManager
            OppManager = new OppFilterManager(opps, probabilities, oppTypes);

            Status = "Loaded Successfully";
        }
        private bool loadOpportunitiesCanExecute()
        {
            return SettingsVM.CanLoad() && OppManager.AllOpportunities.Count == 0;
        }
        private bool refreshOpportunitiesCanExecute()
        {
            return SettingsVM.CanLoad() && OppManager.AllOpportunities.Count > 0;
        }

        private void exportOpportunitiesExecute()
        {
            string userRoot = Environment.GetEnvironmentVariable("USERPROFILE");
            string downloadFolder = Path.Combine(userRoot, "Downloads");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = downloadFolder;
            saveFileDialog.FileName = "SalesLog";
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.AddExtension = true;

            string savePath = null;
            if (saveFileDialog.ShowDialog() == true)
            {
                savePath = saveFileDialog.FileName;
            }

            OpportunitiesExport.ExportOpportunities(savePath, OppManager);
        }
        private bool exportOpportunitiesCanExecute()
        {
            return (OppManager.FilteredOpportunities.Count > 0);
        }

        private void clearDatesExecute()
        {
            OppManager.StartDate = null;
            OppManager.EndDate = null;
        }
        private bool clearDatesCanExecute()
        {
            return (OppManager.StartDate.HasValue || OppManager.EndDate.HasValue);
        }

        private void clearTypesExecute()
        {
            foreach(OppTypeBool oppTypeBool in OppTypeBools)
            {
                oppTypeBool.Include = false;
            }
        }
        private bool clearTypesCanExecute()
        {
            foreach(OppTypeBool oppTypeBool in OppTypeBools)
            {
                if (oppTypeBool.Include)
                {
                    return true;
                }
            }
            return false;
        }

        private void addOppType(OpportunityType type)
        {
            OppTypeBool oppTypeBool = new OppTypeBool(type);
            oppTypeBool.PropertyChanged += oppTypeBoolChanged;
            _oppTypeBools.Add(oppTypeBool);
        }

        private void oppTypeBoolChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Include")
            {
                OppTypeBool changed = sender as OppTypeBool;
                //Set all to include if allTypeBool is included.
                if (changed == allTypeBool)
                {
                    if (allTypeBool.Include)
                    {
                        foreach (OppTypeBool oppTypeBool in OppTypeBools)
                        {
                            oppTypeBool.Include = true;
                        }
                    }
                }
                //Add or remove type from OppFilterManager
                else
                {
                    if (changed.Include)
                    {
                        if (!OppManager.OpportunityTypes.Contains(changed.Type))
                        {
                            OppManager.AddOpportunityType(changed.Type);
                        }
                    }
                    else
                    {
                        if (OppManager.OpportunityTypes.Contains(changed.Type))
                        {
                            OppManager.RemoveOpportunityType(changed.Type);
                        }

                        //Set allTypeBool to not included if any are not included
                        allTypeBool.Include = false;
                    }
                }
            }
        }

        private void showCantConnect()
        {
            MessageBox.Show("Could not connect to ConnectWise. Check your settings and try again.", "Can't connect!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}