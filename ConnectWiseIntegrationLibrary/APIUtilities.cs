using ConnectWiseDotNetSDK.ConnectWise.Client;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Api;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectWiseIntegrationLibrary
{
    public static class APIUtilities
    {
        private const string APP_ID = "TECSystemsInc";      //Cookie Value
        private const string SITE = "na.myconnectwise.net"; //ConnectWise Site (Login Info)
        private const string COMPANY_NAME = "tecsystems";   //Company Name (Login Info)
        private const string PUBLIC_KEY = "";
        private const string PRIVATE_KEY = "";

        public static List<OpportunityType> GetAllOpportunityTypes()
        {
            ApiClient connectWiseClient = new ApiClient(APP_ID, SITE, COMPANY_NAME).SetPublicPrivateKey(PUBLIC_KEY, PRIVATE_KEY);
            OpportunityTypesApi oppTypeApi = new OpportunityTypesApi(connectWiseClient);
            return oppTypeApi.GetTypes().GetResult<List<OpportunityType>>();
        }

        public static List<Opportunity> GetAllOpportunities()
        {
            ApiClient connectWiseClient = new ApiClient(APP_ID, SITE, COMPANY_NAME).SetPublicPrivateKey(PUBLIC_KEY, PRIVATE_KEY);
            OpportunitiesApi oppApi = new OpportunitiesApi(connectWiseClient);
            return oppApi.GetOpportunities().GetResult<List<Opportunity>>();
        }

        public static void PrintOpportunities(IEnumerable<Opportunity> opps)
        {
            foreach(Opportunity opp in opps)
            {
                //opp.
            }
        }
    }
}
