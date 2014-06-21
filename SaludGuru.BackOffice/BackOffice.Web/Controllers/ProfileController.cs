using BackOffice.Models.Office;
using BackOffice.Models.Profile;
using SaludGuruProfile.Manager.Controller;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.Office;
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

        public virtual ActionResult ProfileSearch()
        {
            return View();
        }

        public virtual ActionResult ProfileCreate()
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
                return RedirectToAction(MVC.Profile.ActionNames.ProfileEdit, MVC.Profile.Name, new { ProfilePublicId = oProfilePublicId });
            }
            return View(Model);
        }

        public virtual ActionResult ProfileEdit(string ProfilePublicId)
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

        public virtual ActionResult ProfileEditImage(string ProfilePublicId)
        {

            return View();
        }

        #endregion

        #region Office

        public virtual ActionResult OfficeList(string ProfilePublicId)
        {
            OfficeUpsertModel Model = new OfficeUpsertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
            };

            return View(Model);
        }

        public virtual ActionResult OfficeUpsert(string ProfilePublicId, string OfficePublicId)
        {
            OfficeUpsertModel Model = new OfficeUpsertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                CurrentOffice = string.IsNullOrEmpty(OfficePublicId) ? null : SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(OfficePublicId),
                CitiesToSel = SaludGuruProfile.Manager.Controller.City.CityGetAll(),
            };

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                OfficeModel ProfileToCreate = GetProfileOfficeInfoRequestModel();

                //create profile 
                string oOfficePublicId = SaludGuruProfile.Manager.Controller.Office.UpsertOfficeInfo
                    (Model.Profile.ProfilePublicId, ProfileToCreate);

                //redirect to update page
                if (string.IsNullOrEmpty(ProfileToCreate.OfficePublicId))
                {
                    return RedirectToAction(MVC.Profile.ActionNames.OfficeUpsert, MVC.Profile.Name, new { ProfilePublicId = Model.Profile.ProfilePublicId, OfficePublicId = oOfficePublicId });
                }
                else
                {
                    Model.CurrentOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(oOfficePublicId);
                }
            }
            return View(Model);
        }

        #endregion

        #region Specialty

        public virtual ActionResult SpecialtyProfileList(string ProfilePublicId)
        {
            ProfileSpecialtyModel Model = new ProfileSpecialtyModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                SpecialtyToSelect = SaludGuruProfile.Manager.Controller.Specialty.CategoryGetAllAdmin(string.Empty),
            };
            return View(Model);
        }

        //public JsonResult AutoCompleteSpecialty(string term)
        //{
        //    List<SaludGuruProfile.Manager.Models.General.SpecialtyModel> Model = ;

        //    var result = (from s in Model
        //                  where s.Name.Contains(term)
        //                  select s).ToList();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Insurance

        public virtual ActionResult InsuranceProfileList(string ProfilePublicId)
        {
            ProfileInsuranceModel Model = new ProfileInsuranceModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                InsuranceToSelect = SaludGuruProfile.Manager.Controller.Insurance.CategoryGetAllAdmin(string.Empty),
            };
            return View(Model);
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
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_IdentificationType"])?0:int.Parse(Request["CatId_IdentificationType"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.IdentificationType,
                            Value = Request["IdentificationType"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_IdentificationNumber"])?0:int.Parse(Request["CatId_IdentificationNumber"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.IdentificationNumber,
                            Value = Request["IdentificationNumber"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_Gender"])?0:int.Parse(Request["CatId_Gender"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.Gender,
                            Value = Request["Gender"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_Email"])?0:int.Parse(Request["CatId_Email"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.Email,
                            Value = Request["Email"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_Website"])?0:int.Parse(Request["CatId_Website"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.Website,
                            Value = Request["Website"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_FacebookProfile"])?0:int.Parse(Request["CatId_FacebookProfile"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.FacebookProfile,
                            Value = Request["FacebookProfile"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_IsCertified"])?0:int.Parse(Request["CatId_IsCertified"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.IsCertified,
                            Value = (!string.IsNullOrEmpty(Request["IsCertified"]) && Request["IsCertified"].ToString().ToLower() == "on") ? "true" : "false",
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_ProfileText"])?0:int.Parse(Request["CatId_ProfileText"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.ProfileText,
                            LargeValue = Request["ProfileText"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_Education"])?0:int.Parse(Request["CatId_Education"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.Education,
                            LargeValue = Request["Education"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_Certification"])?0:int.Parse(Request["CatId_Certification"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.Certification,
                            LargeValue = Request["Certification"].ToString(),
                        },
                    }
                };

                return oReturn;
            }
            return null;
        }

        private OfficeModel GetProfileOfficeInfoRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                OfficeModel oReturn = new OfficeModel()
                {
                    OfficePublicId = Request["OfficePublicId"],
                    Name = Request["Name"].ToString(),
                    City = new SaludGuruProfile.Manager.Models.General.CityModel()
                    {
                        CityId = int.Parse(Request["City"].ToString()),
                    },
                    IsDefault = (!string.IsNullOrEmpty(Request["IsDefault"]) && Request["IsDefault"].ToString().ToLower() == "on") ? true : false,
                    OfficeInfo = new List<OfficeInfoModel>() 
                    { 
                        new OfficeInfoModel()
                        {
                            OfficeInfoId = string.IsNullOrEmpty(Request["CatId_Address"])?0:int.Parse(Request["CatId_Address"].ToString().Trim()),
                            OfficeInfoType = enumOfficeInfoType.Address,
                            Value = Request["Address"].ToString(),
                        },
                        new OfficeInfoModel()
                        {
                            OfficeInfoId = string.IsNullOrEmpty(Request["CatId_Telephone"])?0:int.Parse(Request["CatId_Telephone"].ToString().Trim()),
                            OfficeInfoType = enumOfficeInfoType.Telephone,
                            Value = Request["Telephone"].ToString(),
                        },
                        new OfficeInfoModel()
                        {
                            OfficeInfoId = string.IsNullOrEmpty(Request["CatId_Geolocation"])?0:int.Parse(Request["CatId_Geolocation"].ToString().Trim()),
                            OfficeInfoType = enumOfficeInfoType.Geolocation,
                            Value = Request["Geolocation"].ToString(),
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