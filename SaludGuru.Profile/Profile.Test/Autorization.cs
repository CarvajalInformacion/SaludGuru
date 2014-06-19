using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Profile.Test
{
    [TestClass]
    public class Autorization
    {
        [TestMethod]
        public void GetEmailAutorization()
        {
            List<SessionController.Models.Profile.Autorization.AutorizationModel> AutorizationResponse =
                SaludGuruProfile.Manager.Controller.Autorization.GetEmailAutorization("jairo.guzman@carvajal.com");

            Assert.IsNotNull(AutorizationResponse);
            Assert.AreEqual((bool)(AutorizationResponse.Count > 0), true);
        }
    }
}
