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

        #region Contains Tests
        [TestMethod]
        public void ContainsSameType()
        {
            //Ex. AI contains AI, uses Contains(IOType)
            List<TECIO> io = new List<TECIO>
            {
                new TECIO(IOType.AI),
                new TECIO(IOType.AO),
                new TECIO(IOType.DI),
                new TECIO(IOType.DO)
            };
            IOCollection collection = new IOCollection(io);

            Assert.IsTrue(collection.Contains(IOType.AI));
            Assert.IsTrue(collection.Contains(IOType.AO));
            Assert.IsTrue(collection.Contains(IOType.DI));
            Assert.IsTrue(collection.Contains(IOType.DO));

        }

        [TestMethod]
        public void UnivseralContainsSpecificType()
        {
            //Ex. UI contains AI and DI, uses Contains(IOType)
            List<TECIO> io = new List<TECIO>
            {
                new TECIO(IOType.UI),
                new TECIO(IOType.UO)
            };
            IOCollection collection = new IOCollection(io);

            Assert.IsTrue(collection.Contains(IOType.AI));
            Assert.IsTrue(collection.Contains(IOType.AO));
            Assert.IsTrue(collection.Contains(IOType.DI));
            Assert.IsTrue(collection.Contains(IOType.DO));
        }

        #endregion

        #region AddIO Tests
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
        #endregion

        #region RemoveIO Tests
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
        #endregion
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
