using ConnectWiseDotNetSDK.ConnectWise.Client.Common.Model;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectWiseInformationInterface.Models
{
    public class OppFilterManager : INotifyPropertyChanged
    {
        #region Fields
        private readonly List<Opportunity> _allOpportunities;
        private readonly List<SalesProbability> _probabilities;
        private readonly ObservableCollection<OpportunityType> _opportunityTypes;
        private readonly ObservableCollection<Opportunity> _filteredOpportunities;

        private DateTime? _startDate;
        private DateTime? _endDate;
        #endregion

        #region Properties
        public ReadOnlyCollection<Opportunity> AllOpportunities
        {
            get
            {
                return new ReadOnlyCollection<Opportunity>(_allOpportunities);
            }
        }
        public ReadOnlyCollection<SalesProbability> Probabilities
        {
            get
            {
                return new ReadOnlyCollection<SalesProbability>(_probabilities);
            }
        }
        public ReadOnlyObservableCollection<OpportunityType> OpportunityTypes
        {
            get { return new ReadOnlyObservableCollection<OpportunityType>(_opportunityTypes); }
        }
        public ReadOnlyObservableCollection<Opportunity> FilteredOpportunities
        {
            get
            {
                return new ReadOnlyObservableCollection<Opportunity>(_filteredOpportunities);
            }
        }

        public DateTime? StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (StartDate != value)
                {
                    _startDate = value;
                    raisePropertyChanged("StartDate");
                    refilter();
                }
            }
        }
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                if (EndDate != value)
                {
                    _endDate = value;
                    raisePropertyChanged("EndDate");
                    refilter();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public OppFilterManager(
            IEnumerable<Opportunity> allOpps,
            IEnumerable<SalesProbability> probs)
        {
            _allOpportunities = new List<Opportunity>(allOpps);
            _probabilities = new List<SalesProbability>(probs);

            _opportunityTypes = new ObservableCollection<OpportunityType>();
            _filteredOpportunities = new ObservableCollection<Opportunity>();

            StartDate = null;
            EndDate = null;

            refilter();
        }

        public void AddOpportunityType(OpportunityType type)
        {
            _opportunityTypes.Add(type);
            refilter();
        }
        public void RemoveOpportunityType(OpportunityType type)
        {
            _opportunityTypes.Remove(type);
            refilter();
        }

        private void refilter()
        {
            //Remove Opportunities not in range
            List<Opportunity> oppsToRemove = new List<Opportunity>();
            foreach(Opportunity opp in _filteredOpportunities)
            {
                bool removeOpp = (!closeDateInRange(opp.ExpectedCloseDate))
                    || (!typeInRange(opp.Type));
                if (removeOpp)
                {
                    oppsToRemove.Add(opp);
                }
            }
            foreach(Opportunity opp in oppsToRemove)
            {
                _filteredOpportunities.Remove(opp);
            }

            //Add Opportunities in range
            foreach(Opportunity opp in AllOpportunities)
            {
                bool addOpp = (!FilteredOpportunities.Contains(opp))
                    && closeDateInRange(opp.ExpectedCloseDate)
                    && typeInRange(opp.Type);
                if (addOpp)
                {
                    _filteredOpportunities.Add(opp);
                }
            }

            bool closeDateInRange(DateTime? date)
            {
                if (!date.HasValue) return true;
                bool afterStart = (!StartDate.HasValue) || (DateTime.Compare(date.Value, StartDate.Value) >= 0);
                bool beforeEnd = (!EndDate.HasValue) || (DateTime.Compare(date.Value, EndDate.Value) <= 0);
                return (afterStart && beforeEnd);
            }

            bool typeInRange(OpportunityTypeReference typeRef)
            {
                if (typeRef == null) return true;
                foreach(OpportunityType oppType in OpportunityTypes)
                {
                    if (typeRef.Name == oppType.Description)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private void raisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class OpportunityExtensions
    {
        private const string ENGINEERING    = "FC: Engineer";
        private const string PROGRAMMING    = "FC: Software";
        private const string GRAPHICS       = "FC: Graphics";
        private const string TECHNICIAN     = "FC:TechLabor";
        private const string PM             = "FC:PrjctMgmt";

        public static int GetProbability(this Opportunity opp, IEnumerable<SalesProbability> probs)
        {
            int probability = 0;
            foreach (SalesProbability prob in probs)
            {
                if (prob.Id == opp.Probability.Id)
                {
                    probability = (int)prob.Probability;
                }
            }
            return probability;
        }

        public static int GetEngineeringHours(this Opportunity opp)
        {
            foreach(CustomFieldValue custom in opp.CustomFields)
            {
                if (custom.Caption == ENGINEERING)
                {
                    return Convert.ToInt32(custom.Value);
                }
            }
            return 0;
        }

        public static int GetProgrammingHours(this Opportunity opp)
        {
            foreach (CustomFieldValue custom in opp.CustomFields)
            {
                if (custom.Caption == PROGRAMMING)
                {
                    return Convert.ToInt32(custom.Value);
                }
            }
            return 0;
        }

        public static int GetGraphicsHours(this Opportunity opp)
        {
            foreach (CustomFieldValue custom in opp.CustomFields)
            {
                if (custom.Caption == GRAPHICS)
                {
                    return Convert.ToInt32(custom.Value);
                }
            }
            return 0;
        }

        public static int GetTechnicianHours(this Opportunity opp)
        {
            foreach (CustomFieldValue custom in opp.CustomFields)
            {
                if (custom.Caption == TECHNICIAN)
                {
                    return Convert.ToInt32(custom.Value);
                }
            }
            return 0;
        }

        public static int GetPMHours(this Opportunity opp)
        {
            foreach (CustomFieldValue custom in opp.CustomFields)
            {
                if (custom.Caption == PM)
                {
                    return Convert.ToInt32(custom.Value);
                }
            }
            return 0;
        }
    }
}
