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
        //Tests for Contains(IOType)
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
        [TestMethod]
        public void SpecificDoesntContainUniversalType()
        {
            //Ex. AI and DI don't contain UI, uses Contains(IOType)
            List<TECIO> io = new List<TECIO>
            {
                new TECIO(IOType.AI),
                new TECIO(IOType.AO),
                new TECIO(IOType.DI),
                new TECIO(IOType.DO)
            };
            IOCollection collection = new IOCollection(io);

            Assert.IsFalse(collection.Contains(IOType.UI));
            Assert.IsFalse(collection.Contains(IOType.UO));
        }

        //Tests for Contains(TECIO)
        [TestMethod]
        public void ContainsSameIO()
        {
            //Ex. 5 AI contains 3 AI, uses Contains(TECIO)
            TECIO ai = new TECIO(IOType.AI);
            ai.Quantity = 5;
            TECIO ao = new TECIO(IOType.AO);
            ao.Quantity = 5;
            TECIO di = new TECIO(IOType.DI);
            di.Quantity = 5;
            TECIO ioDO = new TECIO(IOType.DO);
            ioDO.Quantity = 5;
            List<TECIO> io = new List<TECIO>
            {
                ai,
                ao,
                di,
                ioDO
            };
            IOCollection collection = new IOCollection(io);

            TECIO aiX = new TECIO(IOType.AI);
            aiX.Quantity = 3;
            TECIO aoX = new TECIO(IOType.AO);
            aoX.Quantity = 3;
            TECIO diX = new TECIO(IOType.DI);
            diX.Quantity = 3;
            TECIO ioDOX = new TECIO(IOType.DO);
            ioDOX.Quantity = 3;

            Assert.IsTrue(collection.Contains(aiX));
            Assert.IsTrue(collection.Contains(aoX));
            Assert.IsTrue(collection.Contains(diX));
            Assert.IsTrue(collection.Contains(ioDOX));

        }
        [TestMethod]
        public void UnivseralContainsSpecificIO()
        {
            //Ex. 5 UI contains 3 AI and 3 DI separately, uses Contains(TECIO)
            TECIO ui = new TECIO(IOType.UI);
            ui.Quantity = 5;
            TECIO uo = new TECIO(IOType.UO);
            uo.Quantity = 5;
            List<TECIO> io = new List<TECIO>
            {
                ui,
                uo
            };
            IOCollection collection = new IOCollection(io);

            TECIO aiX = new TECIO(IOType.AI);
            aiX.Quantity = 3;
            TECIO aoX = new TECIO(IOType.AO);
            aoX.Quantity = 3;
            TECIO diX = new TECIO(IOType.DI);
            diX.Quantity = 3;
            TECIO ioDOX = new TECIO(IOType.DO);
            ioDOX.Quantity = 3;

            Assert.IsTrue(collection.Contains(aiX));
            Assert.IsTrue(collection.Contains(aoX));
            Assert.IsTrue(collection.Contains(diX));
            Assert.IsTrue(collection.Contains(ioDOX));
        }
        [TestMethod]
        public void SpecificDoesntContainUniversalIO()
        {
            //Ex. 6 AI doesn't contain 5 UI, uses Contains(TECIO)
            TECIO ai = new TECIO(IOType.AI);
            ai.Quantity = 5;
            TECIO ao = new TECIO(IOType.AO);
            ao.Quantity = 5;
            TECIO di = new TECIO(IOType.DI);
            di.Quantity = 5;
            TECIO ioDO = new TECIO(IOType.DO);
            ioDO.Quantity = 5;
            List<TECIO> io = new List<TECIO>
            {
                ai,
                ao,
                di,
                ioDO
            };
            IOCollection collection = new IOCollection(io);

            TECIO ui = new TECIO(IOType.UI);
            ui.Quantity = 3;
            TECIO uo = new TECIO(IOType.UO);
            uo.Quantity = 3;

            Assert.IsFalse(collection.Contains(ui));
            Assert.IsFalse(collection.Contains(uo));
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
