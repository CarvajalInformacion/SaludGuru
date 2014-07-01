using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaludGuruProfile.Manager.Models.Office;

namespace Profile.Test
{
    /// <summary>
    /// Summary description for OfficeTest
    /// </summary>
    [TestClass]
    public class OfficeTest
    {
        [TestMethod]
        public void OfficeGetFullAdmin()
        {
            string oOfficePublicId = "69C08DEA";

            OfficeModel oOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(oOfficePublicId);

            Assert.AreEqual(oOffice.OfficePublicId, oOfficePublicId);
        }
    }
}
