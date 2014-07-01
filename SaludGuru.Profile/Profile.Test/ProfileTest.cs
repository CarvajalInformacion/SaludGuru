using System;
using SaludGuruProfile.Manager.Models.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Profile.Test
{
    [TestClass]
    public class ProfileTest
    {
        #region Profile

        [TestMethod]
        public void ProfileGetFullAdmin()
        {
            string oProfilePublicId = "2C1D2510";

            ProfileModel oProfile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(oProfilePublicId);

            Assert.AreEqual(oProfile.ProfilePublicId, oProfilePublicId);
        }

        #endregion

        #region Profile Search
        [TestMethod]
        public void ProfileSearch()
        {
            int TotalRows;
            List<ProfileModel> oReturn = SaludGuruProfile.Manager.Controller.Profile.ProfileSearch(" da mo", 0, 10, out TotalRows);
            Assert.AreEqual(oReturn.Count > 0, true);
        }
        #endregion

        #region Get Profile Options
        [TestMethod]
        public void GetProfileOptions()
        {
            List<SaludGuruProfile.Manager.Models.General.ItemModel> oResult =
                SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions();
            Assert.AreEqual(oResult.Count > 0, true);
        }
        #endregion

        #region Specialty Profile
        //[TestMethod]
        //public void SpecialtyProfileUpsert()
        //{
        //    ProfileModel profile = new ProfileModel();
        //    profile.ProfilePublicId = "1234";
        //    profile.DefaultSpecialty.CategoryId = 1;
        //    profile.DefaultSpecialty.CategoryId = 1;
        //    SaludGuruProfile.Manager.Controller.Profile.SpecialtyProfileUpsert(profile);
        //}

        //[TestMethod]
        //public void SpecialtyProfileDelete()
        //{
        //    SaludGuruProfile.Manager.Controller.Profile.SpecialtyProfileDelete("4321", 27);
        //}
        #endregion

        #region Insurance Profile

        //[TestMethod]
        //public void InsuranceProfileUpsert()
        //{
        //    ProfileModel profile = new ProfileModel();
        //    profile.ProfilePublicId = "4321";
        //    profile.DefaultSpecialty.CategoryId = 1;
        //    profile.DefaultSpecialty.CategoryId = 1;
        //    SaludGuruProfile.Manager.Controller.Profile.SpecialtyProfileUpsert(profile);
        //}

        //[TestMethod]
        //public void InsuranceProfileDelete()
        //{
        //    SaludGuruProfile.Manager.Controller.Profile.InsuranceProfileDelete("4321", 1);
        //}
        #endregion

    }
}
