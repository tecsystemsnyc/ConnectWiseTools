using ConnectWiseDotNetSDK.ConnectWise.Client;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Api;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Model;
using ConnectWiseInformationInterface.Export;
using ConnectWiseInformationInterface.Models;
using EstimatingLibrary.Utilities;
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
        private const string APP_ID = "TECSystemsInc";      //Cookie Value
        private const string SITE = "na.myconnectwise.net"; //ConnectWise Site (Login Info)
        private const string COMPANY_NAME = "tecsystems";   //Company Name (Login Info)
        private const string PUBLIC_KEY = "8vAUgWONMsBxf89Z";               //Public Key for GHanson (from ConnectWise)
        private const string PRIVATE_KEY = "iTvZUXzksv1BNj6u";              //Private Key for GHanson (from ConnectWise)

        private DateTime _startCloseDate;
        private DateTime _endCloseDate;

        private bool updatingOppTypeBools;
        private readonly OppTypeBool allBool;
        private readonly ObservableCollection<OppTypeBool> _oppTypes;
        private readonly ObservableCollection<Opportunity> _loadedOpportunities;
        private readonly ObservableCollection<Opportunity> _applicableOpportunities;

        private List<SalesProbability> probabilities;

        private string _username;

        public DateTime StartCloseDate
        {
            get { return _startCloseDate; }
            set
            {
                _startCloseDate = value;
                RaisePropertyChanged("StartCloseDate");
                updateApplicableOpportunities();
            }
        }
        public DateTime EndCloseDate
        {
            get { return _endCloseDate; }
            set
            {
                _endCloseDate = value;
                RaisePropertyChanged("EndCloseDate");
                updateApplicableOpportunities();
            }
        }

        public ReadOnlyObservableCollection<OppTypeBool> OppTypes
        {
            get
            {
                return new ReadOnlyObservableCollection<OppTypeBool>(_oppTypes);
            }
        }
        public ReadOnlyObservableCollection<Opportunity> LoadedOpportunities
        {
            get { return new ReadOnlyObservableCollection<Opportunity>(_loadedOpportunities); }
        }
        public ReadOnlyObservableCollection<Opportunity> ApplicableOpportunities
        {
            get { return new ReadOnlyObservableCollection<Opportunity>(_applicableOpportunities); }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChanged("Username");
            }
        }

        public ICommand LoadOpportunitiesCommand { get; private set; }
        public ICommand ExportOpportunitiesCommand { get; private set; }

        public MainViewModel()
        {
            _oppTypes = new ObservableCollection<OppTypeBool>();
            _loadedOpportunities = new ObservableCollection<Opportunity>();
            _applicableOpportunities = new ObservableCollection<Opportunity>();

            _startCloseDate = DateTime.Now;
            _endCloseDate = DateTime.Now;

            _username = "";

            allBool = new OppTypeBool("All");
            addOppType(allBool);
            updatingOppTypeBools = false;

            LoadOpportunitiesCommand = new RelayCommand<object>(loadOpportunitiesExecute, loadOpportunitiesCanExecute);
            ExportOpportunitiesCommand = new RelayCommand(exportOpportunitiesExecute, exportOpportunitiesCanExecute);
        }

        private void loadOpportunitiesExecute(object passwordBox)
        {
            //PasswordBox box = passwordBox as PasswordBox;
            //string password = box.Password;

            //Using Keys
            ApiClient connectWiseClient = new ApiClient(APP_ID, SITE, COMPANY_NAME).SetPublicPrivateKey(PUBLIC_KEY, PRIVATE_KEY);

            //Using Username/Password
            //ApiClient connectWiseClient = new ApiClient(APP_ID, SITE, COMPANY_NAME).SetCookieAuthenticatieon(Username, password);

            List<OpportunityType> oppTypes = new OpportunityTypesApi(connectWiseClient)
                .GetTypes()
                .GetResult<List<OpportunityType>>();

            if (oppTypes == null)
            {
                MessageBox.Show("Could not connect to ConnectWise.", "Can't connect!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            _oppTypes.ObservablyClear();
            addOppType(allBool);
            foreach(OpportunityType oppType in oppTypes)
            {
                addOppType(new OppTypeBool(oppType.Description));
            }

            List<Opportunity> opps = new OpportunitiesApi(connectWiseClient)
                .GetOpportunities()
                .GetResult<List<Opportunity>>();

            if (opps == null)
            {
                MessageBox.Show("Could not connect to ConnectWise.", "Can't connect!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            _loadedOpportunities.ObservablyClear();
            foreach(Opportunity opp in opps)
            {
                _loadedOpportunities.Add(opp);
            }

            probabilities = new SalesProbabilitiesApi(connectWiseClient)
                .GetProbabilities()
                .GetResult<List<SalesProbability>>();

            if (probabilities == null)
            {
                MessageBox.Show("Could not connect to ConnectWise.", "Can't connect!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }
        private bool loadOpportunitiesCanExecute(object passwordBox)
        {
            return true;
            //PasswordBox box = passwordBox as PasswordBox;
            //if (box == null) return false;
            //string password = box.Password;
            //return (Username != "" && password != "");
        }

        private void exportOpportunitiesExecute()
        {
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
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

            OpportunitiesExport.ExportOpportunities(savePath, ApplicableOpportunities, probabilities);
        }
        private bool exportOpportunitiesCanExecute()
        {
            return (ApplicableOpportunities.Count > 0 && probabilities != null);
        }

        private void addOppType(OppTypeBool oppType)
        {
            oppType.PropertyChanged += oppTypeBoolChanged;
            _oppTypes.Add(oppType);
        }
        private void oppTypeBoolChanged(object sender, PropertyChangedEventArgs e)
        {
            OppTypeBool typeBool = sender as OppTypeBool;
            if (!updatingOppTypeBools)
            {
                updatingOppTypeBools = true;
                if (typeBool == allBool)
                {
                    //Set all bools equal to allBool value.
                    foreach(OppTypeBool oppType in _oppTypes)
                    {
                        if (oppType != allBool)
                        {
                            oppType.Include = allBool.Include;
                        }
                    }
                }
                else
                {
                    //Set allBool to false if any bool gets set false;
                    if (!typeBool.Include)
                    {
                        allBool.Include = false;
                    }
                }
                updatingOppTypeBools = false;
                updateApplicableOpportunities();
            }
        }

        private void updateApplicableOpportunities()
        {
            _applicableOpportunities.ObservablyClear();
            foreach(Opportunity opp in LoadedOpportunities)
            {
                if (opp.Type != null && oppTypeSelected(opp.Type.Name) && closeDateInRange(opp.ExpectedCloseDate.Value))
                {
                    _applicableOpportunities.Add(opp);
                }
            }
        }

        private bool oppTypeSelected(string name)
        {
            foreach(OppTypeBool oppType in OppTypes)
            {
                if (oppType.Name == name && oppType.Include)
                {
                    return true;
                }
            }
            return false;
        }
        private bool closeDateInRange(DateTime date)
        {
            bool afterStart = DateTime.Compare(date, StartCloseDate) >= 0;
            bool beforeEnd = DateTime.Compare(date, EndCloseDate) <= 0;
            return (afterStart && beforeEnd);
        }
    }
}