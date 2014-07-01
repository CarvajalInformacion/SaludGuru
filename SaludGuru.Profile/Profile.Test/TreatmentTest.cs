using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaludGuruProfile.Manager;

namespace Profile.Test
{
    [TestClass]
    public class TreatmentTest
    {
        [TestMethod]
        public void TreatmentGetAllAdmin()
        {
            List<SaludGuruProfile.Manager.Models.General.TreatmentModel> oSpList =
                SaludGuruProfile.Manager.Controller.Treatment.GetAllAdmin
                    (null);

            Assert.AreEqual(true, oSpList.Count > 0);
        }
    }
}
