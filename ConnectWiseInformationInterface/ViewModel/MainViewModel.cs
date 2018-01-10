using ConnectWiseDotNetSDK.ConnectWise.Client;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Api;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Model;
using ConnectWiseInformationInterface.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private const string PUBLIC_KEY = "";               //Public Key for GHanson (from ConnectWise)
        private const string PRIVATE_KEY = "";              //Private Key for GHanson (from ConnectWise)


        public enum Quarter { Q1 = 1, Q2, Q3, Q4 }

        private Quarter _startCloseDate;
        private Quarter _endCloseDate;

        private bool updatingOppTypeBools;
        private readonly OppTypeBool allBool;
        private readonly ObservableCollection<OppTypeBool> _oppTypes;
        private readonly ObservableCollection<Opportunity> _loadedOpportunities;

        private int _applicableOpportunities;

        public Quarter StartCloseDate
        {
            get { return _startCloseDate; }
            set
            {
                _startCloseDate = value;
                RaisePropertyChanged("StartCloseDate");
            }
        }
        public Quarter EndCloseDate
        {
            get { return _endCloseDate; }
            set
            {
                _endCloseDate = value;
                RaisePropertyChanged("EndCloseDate");
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

        public int ApplicableOpportunities
        {
            get { return _applicableOpportunities; }
            set
            {
                _applicableOpportunities = value;
                RaisePropertyChanged("ApplicableOpportunities");
            }
        }

        public ICommand LoadOpportunitiesCommand { get; private set; }
        public ICommand ExportOpportunitiesCommand { get; private set; }

        public MainViewModel()
        {
            _oppTypes = new ObservableCollection<OppTypeBool>();
            _loadedOpportunities = new ObservableCollection<Opportunity>();

            allBool = new OppTypeBool("All");
            addOppType(allBool);
            updatingOppTypeBools = false;

            LoadOpportunitiesCommand = new RelayCommand(loadOpportunitiesExecute);
            ExportOpportunitiesCommand = new RelayCommand(exportOpportunitiesExecute, exportOpportunitiesCanExecute);
        }

        private void loadOpportunitiesExecute()
        {
            ApiClient connectWiseClient = new ApiClient(APP_ID, SITE, COMPANY_NAME).SetPublicPrivateKey(PUBLIC_KEY, PRIVATE_KEY);

            List<OpportunityType> oppTypes = new OpportunityTypesApi(connectWiseClient)
                .GetTypes()
                .GetResult<List<OpportunityType>>();
            foreach(OpportunityType oppType in oppTypes)
            {
                addOppType(new OppTypeBool(oppType.Description));
            }

            List<Opportunity> opps = new OpportunitiesApi(connectWiseClient)
                .GetOpportunities()
                .GetResult<List<Opportunity>>();
            foreach(Opportunity opp in opps)
            {
                _loadedOpportunities.Add(opp);
            }
        }

        private void exportOpportunitiesExecute()
        {
            throw new NotImplementedException();
        }
        private bool exportOpportunitiesCanExecute()
        {
            return ApplicableOpportunities > 0;
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
            }
        }

        private void updateApplicableOpportunities()
        {
            throw new NotImplementedException();
        }
    }
}