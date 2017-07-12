using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
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
            TECBid bid = new TECBid();
            ChangeStack stack;

            var path = Path.GetTempFileName();
            DatabaseHelper.SaveNew(path, bid);

            bid = DatabaseHelper.Load(path) as TECBid;
            stack = new ChangeStack(bid);
            bid.Systems.Add(new TECSystem());
            DatabaseHelper.Update(path, stack, false);

            bid = DatabaseHelper.Load(path) as TECBid;
            stack = new ChangeStack(bid);
            bid.Systems[0].AddInstance(bid);
            DatabaseHelper.Update(path, stack, false);

            bid = DatabaseHelper.Load(path) as TECBid;
            stack = new ChangeStack(bid);
            bid.Systems[0].Equipment.Add(new TECEquipment());
            DatabaseHelper.Update(path, stack, false);

            bid = DatabaseHelper.Load(path) as TECBid;
            stack = new ChangeStack(bid);
            bid.Systems[0].Equipment[0].SubScope.Add(new TECSubScope());
            DatabaseHelper.Update(path, stack, false);

            bid = DatabaseHelper.Load(path) as TECBid;
        }
    }
}
