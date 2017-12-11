using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Tests
{
    /// <summary>
    /// Summary description for RandomTests
    /// </summary>
    [TestClass]
    public class RandomTests
    {
        public RandomTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void AddSubScopeToLoadedSystem()
        {
            throw new Exception();
            //TECBid bid = TestHelper.CreateTestBid();

            //var path = Path.GetTempFileName();

            //DatabaseManager<TECBid> manager = new DatabaseManager<TECBid>(path);
            //manager.New(bid);
            //bid = manager.Load() as TECBid;
            //var watcher = new ChangeWatcher(bid);

            //DeltaStacker stack = new DeltaStacker(watcher);
            //bid.Systems[0].Equipment[0].SubScope.Add(new TECSubScope(true));
            //manager.Save(stack.CleansedStack());

            //bid = manager.Load() as TECBid;
        }
    }
}
