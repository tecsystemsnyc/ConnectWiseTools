using System;
using System.Collections.Generic;
using ConnectWiseDotNetSDK.ConnectWise.Client;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Api;
using ConnectWiseDotNetSDK.ConnectWise.Client.Sales.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConnectWiseIntegrationTests
{
    [TestClass]
    public class ConnectionTests
    {
        private const string APP_ID = "TECSystemsInc";      //Cookie Value
        private const string SITE = "na.myconnectwise.net"; //ConnectWise Site (Login Info)
        private const string COMPANY_NAME = "tecsystems";   //Company Name (Login Info)
        private const string PUBLIC_KEY = "";               //Public Key for GHanson (from ConnectWise)
        private const string PRIVATE_KEY = "";              //Private Key for GHanson (from ConnectWise)

        [TestMethod]
        public void TestAPIClient()
        {
            ApiClient connectWiseClient = new ApiClient(APP_ID, SITE, COMPANY_NAME).SetPublicPrivateKey(PUBLIC_KEY, PRIVATE_KEY);
        }

        [TestMethod]
        public void TestOpportunities()
        {
            ApiClient connectWiseClient = new ApiClient(APP_ID, SITE, COMPANY_NAME).SetPublicPrivateKey(PUBLIC_KEY, PRIVATE_KEY);
            OpportunitiesApi oppApi = new OpportunitiesApi(connectWiseClient);
            List<Opportunity> opps = oppApi.GetOpportunities().GetResult<List<Opportunity>>();
            Assert.IsTrue(opps.Count > 0, "No opportunities found.");
        }

        [TestMethod]
        public void TestOpportunityTypes()
        {
            ApiClient connectWiseClient = new ApiClient(APP_ID, SITE, COMPANY_NAME).SetPublicPrivateKey(PUBLIC_KEY, PRIVATE_KEY);
            OpportunityTypesApi oppTypesApi = new OpportunityTypesApi(connectWiseClient);
            List<OpportunityType> oppTypes = oppTypesApi.GetTypes().GetResult<List<OpportunityType>>();
            Assert.IsTrue(oppTypes.Count > 0, "No opportunities found.");
        }
    }
}
