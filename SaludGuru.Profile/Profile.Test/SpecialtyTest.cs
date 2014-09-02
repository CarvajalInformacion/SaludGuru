using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaludGuruProfile.Manager;

namespace Profile.Test
{
    [TestClass]
    public class SpecialtyTest
    {
        [TestMethod]
        public void SpecialtyGetAllAdmin()
        {
            List<SaludGuruProfile.Manager.Models.General.SpecialtyModel> oSpList =
                SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin
                    (null);

            Assert.AreEqual(true, oSpList.Count > 0);
        }
    }
}
