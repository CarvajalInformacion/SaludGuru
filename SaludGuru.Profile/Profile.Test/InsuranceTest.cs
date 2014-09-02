using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaludGuruProfile.Manager;

namespace Profile.Test
{
    [TestClass]
    public class InsuranceTest
    {
        [TestMethod]
        public void InsuranceGetAllAdmin()
        {
            List<SaludGuruProfile.Manager.Models.General.InsuranceModel> oSpList =
                SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin
                    (null);

            Assert.AreEqual(true, oSpList.Count > 0);
        }
    }
}
