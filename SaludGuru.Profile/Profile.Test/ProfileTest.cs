﻿using System;
using SaludGuruProfile.Manager.Models.Profile;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SaludGuruProfile.Manager.Models;

namespace Profile.Test
{
    [TestClass]
    public class ProfileTest
    {
        #region Profile

        [TestMethod]
        public void ProfileGetFullAdmin()
        {
            string oProfilePublicId = "38E35666";

            ProfileModel oProfile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(oProfilePublicId);

            Assert.AreEqual(oProfile.ProfilePublicId, oProfilePublicId);
        }

        [TestMethod]
        public void UpsertProfileDetailInfo()
        {
            ProfileModel model = new ProfileModel();
            ProfileInfoModel info = new ProfileInfoModel();
            ProfileInfoModel info2 = new ProfileInfoModel();
            ProfileInfoModel info3 = new ProfileInfoModel();

            model.ProfileInfo = new List<ProfileInfoModel>();

            info.ProfileInfoId = 136;
            info.ProfileInfoType = enumProfileInfoType.AsignacionCita;
            info.Value = "101";

            model.ProfileInfo.Add(info);

            info2.ProfileInfoId = 137;
            info2.ProfileInfoType = enumProfileInfoType.CancelacionCita;
            info2.Value = "101";

            model.ProfileInfo.Add(info2);

            info3.ProfileInfoId = 138;
            info3.ProfileInfoType = enumProfileInfoType.EncuestaSatisfaccion;
            info3.Value = "101";

            model.ProfileInfo.Add(info3);

            model.ProfilePublicId = "2C1D2510";


            SaludGuruProfile.Manager.Controller.Profile.UpsertProfileDetailInfo(model);
        }
        [TestMethod]
        public void UpsertProfileSmallImage()
        {
            SaludGuruProfile.Manager.Controller.Profile.UpsertProfileSmallImage
                (new ProfileModel()
                    {
                        ProfilePublicId = "38E35666",
                    },
                    @"D:\Proyectos\Github\SaludGuru\SaludGuru.Profile\Profile.Test\profile_2822_small.png"
                );
        }

        [TestMethod]
        public void UpsertProfileLargeImage()
        {
            SaludGuruProfile.Manager.Controller.Profile.UpsertProfileLargeImage
                (new ProfileModel()
                {
                    ProfilePublicId = "38E35666",
                },
                    @"D:\Proyectos\Github\SaludGuru\SaludGuru.Profile\Profile.Test\profile_2822_large.png"
                );
        }

        [TestMethod]
        public void UpsertProfileGeneralImage()
        {
            Dictionary<string, int?> img = new Dictionary<string, int?>();
            img.Add(@"D:\Proyectos\Github\SaludGuru\SaludGuru.Profile\Profile.Test\img_2822_General.JPG", null);

            SaludGuruProfile.Manager.Controller.Profile.UpsertProfileGeneralImage
                (new ProfileModel()
                        {
                            ProfilePublicId = "38E35666",
                        },
                img);
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
