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

        //Tests for Contains(IEnumerable<TECIO>)
        [TestMethod]
        public void ContainsSameList()
        {
            //Ex. 5 AI, 3 DI contains 4 AI, 2 DI. Uses Contains(IEnumerable<TECIO>)
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

            TECIO aix = new TECIO(IOType.AI);
            aix.Quantity = 3;
            TECIO aox = new TECIO(IOType.AO);
            aox.Quantity = 2;
            TECIO dix = new TECIO(IOType.DI);
            dix.Quantity = 4;
            TECIO ioDOx = new TECIO(IOType.DO);
            ioDOx.Quantity = 5;
            List<TECIO> subjectIo = new List<TECIO>
            {
                aix,
                aox,
                dix,
                ioDOx
            };

            Assert.IsTrue(collection.Contains(subjectIo));

        }
        [TestMethod]
        public void UnivsersalContainsSpecificList()
        {
            //Ex. 5 UI, 5 UO contains 3 AI, 2 DI, 3 AO, 2 DO. Uses Contains(IEnumerable<TECIO>)
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

            TECIO ai = new TECIO(IOType.AI);
            ai.Quantity = 3;
            TECIO ao = new TECIO(IOType.AO);
            ao.Quantity = 2;
            TECIO di = new TECIO(IOType.DI);
            di.Quantity = 2;
            TECIO ioDO = new TECIO(IOType.DO);
            ioDO.Quantity = 3;
            List<TECIO> subjectIo = new List<TECIO>
            {
                ai,
                ao,
                di,
                ioDO
            };

            Assert.IsTrue(collection.Contains(subjectIo));
        }
        [TestMethod]
        public void SpecificDoesntContainUniversalList()
        {
            //Ex. 3 AI, 2 DI, 3 AO, 2 DO doesn't contain 2 UI, 2 UO. Uses Contains(IEnumerable<TECIO>)
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
            ui.Quantity = 2;
            TECIO uo = new TECIO(IOType.UO);
            uo.Quantity = 2;
            List<TECIO> subjectIO = new List<TECIO>
            {
                ui,
                uo
            };

            Assert.IsFalse(collection.Contains(subjectIO));
        }

        //Tests for Contains(IOCollection)
        [TestMethod]
        public void ContainsSameCollection()
        {
            //Ex. 5 AI, 3 DI contains 4 AI, 2 DI. Uses Contains(IOCollection)
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

            TECIO aix = new TECIO(IOType.AI);
            aix.Quantity = 3;
            TECIO aox = new TECIO(IOType.AO);
            aox.Quantity = 2;
            TECIO dix = new TECIO(IOType.DI);
            dix.Quantity = 4;
            TECIO ioDOx = new TECIO(IOType.DO);
            ioDOx.Quantity = 5;
            List<TECIO> subjectIo = new List<TECIO>
            {
                aix,
                aox,
                dix,
                ioDOx
            };
            IOCollection subjectCollection = new IOCollection(subjectIo);


            Assert.IsTrue(collection.Contains(subjectCollection));
        }
        [TestMethod]
        public void UniversalContainsSpecificCollection()
        {
            //Ex. 5 UI, 5 UO contains 3 AI, 2 DI, 3 AO, 2 DO. Uses Contains(IOCollection)
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

            TECIO ai = new TECIO(IOType.AI);
            ai.Quantity = 3;
            TECIO ao = new TECIO(IOType.AO);
            ao.Quantity = 2;
            TECIO di = new TECIO(IOType.DI);
            di.Quantity = 2;
            TECIO ioDO = new TECIO(IOType.DO);
            ioDO.Quantity = 3;
            List<TECIO> subjectIo = new List<TECIO>
            {
                ai,
                ao,
                di,
                ioDO
            };
            IOCollection subjectCollection = new IOCollection(subjectIo);

            Assert.IsTrue(collection.Contains(subjectCollection));
        }
        [TestMethod]
        public void SpecificDoesntContainUniversalCollection()
        {
            //Ex. 3 AI, 2 DI, 3 AO, 2 DO doesn't contain 2 UI, 2 UO. Uses Contains(IOCollection)
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
            ui.Quantity = 2;
            TECIO uo = new TECIO(IOType.UO);
            uo.Quantity = 2;
            List<TECIO> subjectIO = new List<TECIO>
            {
                ui,
                uo
            };
            IOCollection subjectCollection = new IOCollection(subjectIO);


            Assert.IsFalse(collection.Contains(subjectCollection));
        }
        #endregion

        #region AddIO Tests
        [TestMethod]
        public void AddType()
        {
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

            collection.AddIO(IOType.AI);

            Assert.AreEqual(6, collection.IONumber(IOType.AI));
        }

        [TestMethod]
        public void AddIO()
        {
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

            TECIO toAdd = new TECIO(IOType.AI);
            toAdd.Quantity = 2;

            collection.AddIO(toAdd);

            Assert.AreEqual(7, collection.IONumber(IOType.AI));
        }

        [TestMethod]
        public void AddList()
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

            IOCollection resultCollection = new IOCollection();
            resultCollection.AddIO(firstColletion.ListIO());
            resultCollection.AddIO(secondCollection.ListIO());

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
        public void RemoveType()
        {
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

            collection.RemoveIO(IOType.AI);

            Assert.AreEqual(4, collection.IONumber(IOType.AI));
        }
        [TestMethod]
        public void RemoveSpecificTypeFromUniversal()
        {
            TECIO ui = new TECIO(IOType.UI);
            ui.Quantity = 5;
            List<TECIO> io = new List<TECIO>
            {
                ui
            };
            IOCollection collection = new IOCollection(io);

            collection.RemoveIO(IOType.AI);

            Assert.AreEqual(4, collection.IONumber(IOType.UI));
        }

        [TestMethod]
        public void RemoveIO()
        {
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

            TECIO toRemove = new TECIO(IOType.AI);
            toRemove.Quantity = 2;

            collection.RemoveIO(toRemove);

            Assert.AreEqual(3, collection.IONumber(IOType.AI));
        }
        [TestMethod]
        public void RemoveSpecificIOFromUniversal()
        {
            TECIO ui = new TECIO(IOType.UI);
            ui.Quantity = 5;
            List<TECIO> io = new List<TECIO>
            {
                ui
            };
            IOCollection collection = new IOCollection(io);

            TECIO toRemove = new TECIO(IOType.DI);
            toRemove.Quantity = 2;

            collection.AddIO(toRemove);

            Assert.AreEqual(3, collection.IONumber(IOType.UI));
        }

        [TestMethod]
        public void RemoveList()
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

            IOCollection resultCollection = new IOCollection(secondCollection);
            resultCollection.RemoveIO(firstColletion.ListIO());

            Assert.IsTrue(resultCollection.Contains(IOType.AI));
            Assert.AreEqual(1, resultCollection.IONumber(IOType.AI));

            Assert.IsTrue(resultCollection.Contains(IOType.AO));
            Assert.AreEqual(1, resultCollection.IONumber(IOType.AO));

            Assert.IsTrue(resultCollection.Contains(IOType.DI));
            Assert.AreEqual(1, resultCollection.IONumber(IOType.DI));

            Assert.IsTrue(resultCollection.Contains(IOType.DO));
            Assert.AreEqual(1, resultCollection.IONumber(IOType.DO));
        }
        [TestMethod]
        public void RemoveSpecificListFromUnivsersal()
        {
            throw new NotImplementedException();
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
