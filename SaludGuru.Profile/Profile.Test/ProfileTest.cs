using System;
using Profile.Manager.Models.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Profile.Test
{
    [TestClass]
    public class ProfileTest
    {
        [TestMethod]
        public void ProfileSearch()
        {
            Profile.Manager.Controller.Profile profile = new Manager.Controller.Profile();
            List<ProfileModel> oReturn = profile.ProfileSearch("da mo", 10, 0);
        }

        [TestMethod]
        public void GetProfileOptions()
        {
            List<Profile.Manager.Models.General.ItemModel> oResult =
                Profile.Manager.Controller.Profile.GetProfileOptions();
            Assert.AreEqual(oResult.Count > 0, true);
        }
    }
}
