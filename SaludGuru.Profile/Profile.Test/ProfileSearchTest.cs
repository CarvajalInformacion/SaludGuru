using System;
using Profile.Manager.Models.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Profile.Test
{
    [TestClass]
    public class ProfileSearchTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Profile.Manager.Controller.Profile profile = new Manager.Controller.Profile();
            List<ProfileModel> oReturn = profile.ProfileSearch("da mo", 10, 0);
        }
    }
}
