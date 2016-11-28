using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tests.TestHelper;

namespace Tests
{
    [TestClass]
    public class SaveAsTests
    {
        [TestMethod]
        public void SaveAs_Bid_Info()
        {
            TECBid expectedBid = CreateTestBid();
        }
    }
}
