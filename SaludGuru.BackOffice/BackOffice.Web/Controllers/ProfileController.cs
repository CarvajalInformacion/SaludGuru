using BackOffice.Models.Profile;
using SaludGuruProfile.Manager.Controller;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BackOffice.Web.Controllers
{
    public partial class ProfileController : BaseController
    {
        #region Profile

        public virtual ActionResult Search()
        {
            return View();
        }

        public virtual ActionResult Create()
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                ProfileOptions = SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions(),
                Profile = null,
            };


            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                ProfileModel ProfileToCreate = GetProfileInfoRequestModel();

                //create profile 
                string oProfilePublicId = SaludGuruProfile.Manager.Controller.Profile.UpsertProfileInfo
                    (ProfileToCreate);

                //return to update view
                return RedirectToAction(MVC.Profile.ActionNames.EditProfile, MVC.Profile.Name, new { ProfilePublicId = oProfilePublicId });
            }
            return View(Model);
        }

        public virtual ActionResult EditProfile(string ProfilePublicId)
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                ProfileOptions = SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions(),
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
            };


            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                ProfileModel ProfileToCreate = GetProfileInfoRequestModel();

                //create profile 
                string oProfilePublicId = SaludGuruProfile.Manager.Controller.Profile.UpsertProfileInfo
                    (ProfileToCreate);

                //get updated profile info
                Model.Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId);
            }
            return View(Model);
        }

        #endregion

        #region Specialty

        public virtual ActionResult Specialty(string ProfilePublicId)
        {
            return View();
        }

        #endregion

        #region Insurance

        public virtual ActionResult Insurance(string ProfilePublicId)
        {
            return View();
        }

        #endregion

        #region private methods

        private ProfileModel GetProfileInfoRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                ProfileModel oReturn = new ProfileModel()
                {
                    ProfilePublicId = string.IsNullOrEmpty(Request["ProfilePublicId"]) ? null : Request["ProfilePublicId"].ToString(),
                    Name = Request["Name"].ToString(),
                    LastName = Request["LastName"].ToString(),
                    ProfileType = (enumProfileType)(int.Parse(Request["ProfileType"].ToString())),
                    ProfileStatus = (enumProfileStatus)(int.Parse(Request["ProfileStatus"].ToString())),
                    ProfileInfo = new List<ProfileInfoModel>() 
                    { 
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.IdentificationType,
                            Value = Request["IdentificationType"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.IdentificationNumber,
                            Value = Request["IdentificationNumber"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.Gender,
                            Value = Request["Gender"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.Email,
                            Value = Request["Email"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.Website,
                            Value = Request["Website"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.FacebookProfile,
                            Value = Request["FacebookProfile"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.IsCertified,
                            Value = (!string.IsNullOrEmpty(Request["IsCertified"]) && Request["IsCertified"].ToString().ToLower() == "on") ? "true" : "false",
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.ProfileText,
                            LargeValue = Request["ProfileText"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.Education,
                            LargeValue = Request["Education"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoType = enumProfileInfoType.Certification,
                            LargeValue = Request["Certification"].ToString(),
                        },
                    }
                };

                return oReturn;
            }
            return null;
        }

        #endregion
    }
}