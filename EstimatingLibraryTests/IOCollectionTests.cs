using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using System.Linq;

namespace EstimatingLibraryTests
{
    /// <summary>
    /// Summary description for IOCollectionTests
    /// </summary>
    [TestClass]
    public class IOCollectionTests
    {
        public IOCollectionTests()
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
        public void AddCollection()
        {
            List<TECIO> io = new List<TECIO>
            {
                new TECIO(IOType.AI),
                new TECIO(IOType.AO),
                new TECIO(IOType.DI),
                new TECIO(IOType.DO)
            };
            IOCollection firstColletion = new IOCollection(io);
            IOCollection secondCollection = new IOCollection(io);

            IOCollection resultCollection = firstColletion + secondCollection;
            
            Assert.IsTrue(resultCollection.Contains(IOType.AI));
            Assert.AreEqual(2, resultCollection.IONumber(IOType.AI));

            Assert.IsTrue(resultCollection.Contains(IOType.AO));
            Assert.AreEqual(2, resultCollection.IONumber(IOType.AO));

            Assert.IsTrue(resultCollection.Contains(IOType.DI));
            Assert.AreEqual(2, resultCollection.IONumber(IOType.DI));

            Assert.IsTrue(resultCollection.Contains(IOType.DO));
            Assert.AreEqual(2, resultCollection.IONumber(IOType.DO));

        }

        [TestMethod]
        public void RemoveCollection()
        {
            List<TECIO> io = new List<TECIO>
            {
                new TECIO(IOType.AI),
                new TECIO(IOType.AO),
                new TECIO(IOType.DI),
                new TECIO(IOType.DO)
            };
            List<TECIO> otherIO = new List<TECIO>
            {
                new TECIO(IOType.AI),
                new TECIO(IOType.AO),
                new TECIO(IOType.DI),
                new TECIO(IOType.DO),
                new TECIO(IOType.AI),
                new TECIO(IOType.AO),
                new TECIO(IOType.DI),
                new TECIO(IOType.DO)
            };
            IOCollection firstColletion = new IOCollection(io);
            IOCollection secondCollection = new IOCollection(otherIO);

            IOCollection resultCollection = secondCollection - firstColletion;

            Assert.IsTrue(resultCollection.Contains(IOType.AI));
            Assert.AreEqual(1, resultCollection.IONumber(IOType.AI));

            Assert.IsTrue(resultCollection.Contains(IOType.AO));
            Assert.AreEqual(1, resultCollection.IONumber(IOType.AO));

            Assert.IsTrue(resultCollection.Contains(IOType.DI));
            Assert.AreEqual(1, resultCollection.IONumber(IOType.DI));

            Assert.IsTrue(resultCollection.Contains(IOType.DO));
            Assert.AreEqual(1, resultCollection.IONumber(IOType.DO));

        }

    }

    public static class IOCollectionExtensions
    {
        public static int IONumber(this IOCollection collection, IOType type)
        {
            int ioNum = 0;
            foreach (TECIO io in collection.ListIO())
            {
                if (io.Type == type)
                {
                    ioNum += io.Quantity;
                }
            }
            return ioNum;
        }
    }
}
