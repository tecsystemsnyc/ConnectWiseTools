using EstimatingLibrary;
using EstimatingUtilitiesLibrary.Database;

namespace EstimatingUtilitiesLibraryTests
{
    public class EULTestHelper
    {
        public static TECBid LoadTestBid(string path)
        {
            (TECScopeManager testBid, bool needsSaveNew) = DatabaseLoader.Load(path);
            return testBid as TECBid;
        }

        public static TECTemplates LoadTestTemplates(string path)
        {
            (TECScopeManager testTemplates, bool needsSaveNew) = DatabaseLoader.Load(path);
            return testTemplates as TECTemplates;
        }
    }
}
