using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibraryTests
{
    [TestClass]
    public class EquipmentModelTests
    {
        [TestMethod]
        public void CopyTemplateWithReferences()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECEquipment tempEquip = new TECEquipment(false);
            templates.EquipmentTemplates.Add(tempEquip);

            TECSubScope tempSS = new TECSubScope(false);
            tempSS.Name = "Template SS";
            templates.SubScopeTemplates.Add(tempSS);
            tempEquip.SubScope.Add(templates.SubScopeSynchronizer.NewItem(tempSS));

            TECSubScope equipSS = new TECSubScope(false);
            equipSS.Name = "Equipment SS";
            tempEquip.SubScope.Add(equipSS);

            //Act
            TECEquipment equipCopy = new TECEquipment(tempEquip, false, ssSynchronizer: templates.SubScopeSynchronizer);

            //Assert
            TECSubScope newTempSS = null, newEquipSS = null;
            foreach(TECSubScope ss in equipCopy.SubScope)
            {
                if (ss.Name == "Template SS")
                {
                    newTempSS = ss;
                }
                else if (ss.Name == "Equipment SS")
                {
                    newEquipSS = ss;
                }
                else
                {
                    Assert.Fail("Different subScope than expected in equipment copy.");
                }
            }
            Assert.IsNotNull(newTempSS, "Template SubScope didn't copy properly.");
            Assert.IsNotNull(newEquipSS, "Equipment SubScope didn't copy properly.");

            TemplateSynchronizer<TECSubScope> ssSync = templates.SubScopeSynchronizer;

            Assert.IsTrue(ssSync.Contains(newTempSS));
            Assert.IsTrue(ssSync.GetFullDictionary()[tempSS].Contains(newTempSS));
        }
    }
}
