using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using System.IO;
using EstimatingUtilitiesLibrary;

namespace Tests
{
    /// <summary>
    /// Summary description for EstimateBuilderTests
    /// </summary>
    [TestClass]
    public class EstimateBuilderTests
    {
        static EstimateBuilder.ViewModel.MainViewModel mainVM; 

        public EstimateBuilderTests()
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            mainVM = new EstimateBuilder.ViewModel.MainViewModel();
        }
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
        public void EstimateBuilderVM()
        {
            var testVM = new EstimateBuilder.ViewModel.MainViewModel();
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void TimeTest()
        {
            TECBid bid = TestHelper.CreateTestBid();
            TECTemplates templates = TestHelper.CreateTestTemplates();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            string bidPath = Path.GetTempFileName();
            EstimatingLibraryDatabase.SaveBidToNewDB(bidPath, bid);

            watch.Stop();

            Console.WriteLine("SaveBidToNewDB: " + watch.ElapsedMilliseconds);

            watch = System.Diagnostics.Stopwatch.StartNew();

            string copyBidPath = Path.GetTempFileName();

            File.Copy(bidPath, copyBidPath, true);

            watch.Stop();

            Console.WriteLine("Copy bid file: " + watch.ElapsedMilliseconds);

            watch = System.Diagnostics.Stopwatch.StartNew();

            string templatesPath = Path.GetTempFileName();
            EstimatingLibraryDatabase.SaveTemplatesToNewDB(templatesPath, templates);

            watch.Stop();

            Console.WriteLine("SaveTemplatesToNewDB: " + watch.ElapsedMilliseconds);

            watch = System.Diagnostics.Stopwatch.StartNew();

            string copyTemplatesPath = Path.GetTempFileName();

            File.Copy(templatesPath, copyTemplatesPath, true);

            watch.Stop();

            Console.WriteLine("Copy templates file: " + watch.ElapsedMilliseconds);

        }
    }
}
