using EstimatingLibrary;
using EstimatingUtilitiesLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingUtilitiesLibraryTests
{
    public class EULTestHelper
    {
        public static TECBid LoadTestBid(string path)
        {
            TECBid testBid = DatabaseLoader.Load(path) as TECBid;
            return testBid;
        }

        public static TECTemplates LoadTestTemplates(string path)
        {
            return DatabaseLoader.Load(path) as TECTemplates;
        }
    }
}
