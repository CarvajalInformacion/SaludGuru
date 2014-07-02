using BackOffice.Models.Office;
using BackOffice.Models.Profile;
using SaludGuruProfile.Manager.Controller;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
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

        public virtual ActionResult ProfileSearch(string SearchParam)
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

                //upsert office 
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

        public virtual ActionResult OfficeTreatmentList(string ProfilePublicId, string OfficePublicId)
        {
            OfficeUpsertModel Model = new OfficeUpsertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                CurrentOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(OfficePublicId),
            };

            return View(Model);
        }

        public virtual ActionResult OfficeTreatmentUpsert(string ProfilePublicId, string OfficePublicId, string TreatmentId)
        {
            OfficeUpsertModel Model = new OfficeUpsertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                CurrentOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(OfficePublicId),
                TreatmentToSel = SaludGuruProfile.Manager.Controller.Treatment.GetAllAdmin(string.Empty)
            };

            //get current treatment
            if (!string.IsNullOrEmpty(TreatmentId))
            {
                Model.CurrentTreatmentOffice = Model.CurrentOffice.RelatedTreatment.Where
                    (x => x.CategoryId.ToString() == TreatmentId).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                    && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                TreatmentOfficeModel TreatmentOfficeToCreate = GetTreatmentOfficeRequestModel();

                //upsert treatment office 
                SaludGuruProfile.Manager.Controller.Office.UpsertTreatmentOffice
                    (Model.CurrentOffice.OfficePublicId, TreatmentOfficeToCreate);

                //redirect to update page
                if (string.IsNullOrEmpty(TreatmentId))
                {
                    return RedirectToAction(MVC.Profile.ActionNames.OfficeTreatmentUpsert, MVC.Profile.Name, new
                        {
                            ProfilePublicId = Model.Profile.ProfilePublicId,
                            OfficePublicId = Model.CurrentOffice.OfficePublicId,
                            TreatmentId = TreatmentOfficeToCreate.CategoryId.ToString()
                        });
                }
                else
                {
                    Model.CurrentOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(Model.CurrentOffice.OfficePublicId);
                    Model.CurrentTreatmentOffice = Model.CurrentOffice.RelatedTreatment.Where
                        (x => x.CategoryId.ToString() == TreatmentId).FirstOrDefault();
                }
            }

            return View(Model);
        }

        public virtual ActionResult OfficeScheduleAvailableList(string ProfilePublicId, string OfficePublicId)
        {
            OfficeUpsertModel Model = new OfficeUpsertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                CurrentOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(OfficePublicId),
            };

            return View(Model);
        }

        public virtual ActionResult OfficeScheduleAvailableCreate(string ProfilePublicId, string OfficePublicId)
        {
            ScheduleAvailableModel ScheduleToCreate = GetScheduleAvailableRequestModel();
            SaludGuruProfile.Manager.Controller.Office.ScheduleAvailableCreate(OfficePublicId, ScheduleToCreate);

            return RedirectToAction(MVC.Profile.ActionNames.OfficeScheduleAvailableList, MVC.Profile.Name,
                new { ProfilePublicId = ProfilePublicId, OfficePublicId = OfficePublicId });
        }

        public virtual ActionResult OfficeScheduleAvailableDelete(string ProfilePublicId, string OfficePublicId)
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                int oScheduleAvailableId = int.Parse(Request["ScheduleAvailableId"].ToString().Trim());
                SaludGuruProfile.Manager.Controller.Office.ScheduleAvailableRemove(oScheduleAvailableId);
            }
            return RedirectToAction(MVC.Profile.ActionNames.OfficeScheduleAvailableList, MVC.Profile.Name,
                new { ProfilePublicId = ProfilePublicId, OfficePublicId = OfficePublicId });
        }

        #endregion

        #region Specialty

        public virtual ActionResult SpecialtyProfileList(string ProfilePublicId)
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                SpecialtyToSelect = SaludGuruProfile.Manager.Controller.Specialty.GetAllAdmin(string.Empty),
            };
            return View(Model);
        }

        public virtual ActionResult ProfileSpecialtyUpsert(string ProfilePublicId)
        {
            ProfileModel modelToCreate = new ProfileModel();
            modelToCreate.RelatedSpecialty = new List<SpecialtyModel>();
            SpecialtyModel modelSpecialtyToCreate = new SpecialtyModel();

            modelToCreate.CreateDate = DateTime.Now;

            modelSpecialtyToCreate.CategoryId = Convert.ToInt32(Request["Specialty-id"]);
            modelToCreate.ProfilePublicId = ProfilePublicId;
            modelToCreate.Name = Request["Specialty"];
            bool IsDefaul = (!string.IsNullOrEmpty(Request["IsDefault"]) && Request["IsDefault"].ToString().ToLower() == "on") ? true : false;
            if (IsDefaul)
            {
                modelToCreate.DefaultSpecialty = new SpecialtyModel();
                modelToCreate.DefaultSpecialty.CategoryId = Convert.ToInt32(Request["Specialty-id"]);
                modelToCreate.DefaultSpecialty.Name = Request["Specialty"];
            }

            modelToCreate.RelatedSpecialty.Add(modelSpecialtyToCreate);
            SaludGuruProfile.Manager.Controller.Profile.SpecialtyProfileUpsert(modelToCreate);

            return RedirectToAction(MVC.Profile.ActionNames.SpecialtyProfileList, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }

        public virtual ActionResult ProfileSpecialtyDelete(string ProfilePublicId)
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                int CategoryId = int.Parse(Request["CategoryId"]);
                SaludGuruProfile.Manager.Controller.Profile.SpecialtyProfileDelete(ProfilePublicId, CategoryId);
            }
            return RedirectToAction(MVC.Profile.ActionNames.SpecialtyProfileList, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }

        #endregion

        #region Insurance

        public virtual ActionResult InsuranceProfileList(string ProfilePublicId)
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                InsuranceToSelect = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(string.Empty),
            };
            return View(Model);
        }

        public virtual ActionResult InsuranceProfileUpsert(string ProfilePublicId)
        {
            ProfileModel modelToCreate = new ProfileModel();
            modelToCreate.RelatedInsurance = new List<InsuranceModel>();
            InsuranceModel modelInsutranceToCreate = new InsuranceModel();

            modelToCreate.CreateDate = DateTime.Now;

            modelInsutranceToCreate.CategoryId = Convert.ToInt32(Request["Insurance-id"]);
            modelToCreate.ProfilePublicId = ProfilePublicId;
            modelToCreate.Name = Request["Insurance"];

            modelToCreate.RelatedInsurance.Add(modelInsutranceToCreate);            
            SaludGuruProfile.Manager.Controller.Profile.InsuranceProfileUpsert(modelToCreate);

            return RedirectToAction(MVC.Profile.ActionNames.InsuranceProfileList, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }

        public virtual ActionResult InsuranceProfileDelete(string ProfilePublicId)
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                int CategoryIdId = int.Parse(Request["CategoryId"]);
                SaludGuruProfile.Manager.Controller.Profile.InsuranceProfileDelete(ProfilePublicId, CategoryIdId);
            }
            return RedirectToAction(MVC.Profile.ActionNames.InsuranceProfileList, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }
        #endregion

        #region Treatment

        public virtual ActionResult TreatmentProfileList(string ProfilePublicId)
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                TreatmentToSelect = SaludGuruProfile.Manager.Controller.Treatment.GetAllAdmin(string.Empty),
            };
            return View(Model);
        }

        public virtual ActionResult TreatmentProfileUpsert(string ProfilePublicId)
        {
            ProfileModel modelToCreate = new ProfileModel();
            modelToCreate.RelatedTreatment = new List<TreatmentModel>();
            TreatmentModel modelTreatmentToCreate = new TreatmentModel();

            modelToCreate.CreateDate = DateTime.Now;

            modelTreatmentToCreate.CategoryId = Convert.ToInt32(Request["Treatment-id"]);
            modelToCreate.ProfilePublicId = ProfilePublicId;
            modelToCreate.Name = Request["Treatment"];

            modelToCreate.RelatedTreatment.Add(modelTreatmentToCreate);
            SaludGuruProfile.Manager.Controller.Profile.TreatmentProfileUpsert(modelToCreate);

            return RedirectToAction(MVC.Profile.ActionNames.TreatmentProfileList, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }

        public virtual ActionResult TreatmentProfileDelete(string ProfilePublicId)
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                int CategoryIdId = int.Parse(Request["CategoryId"]);
                SaludGuruProfile.Manager.Controller.Profile.InsuranceProfileDelete(ProfilePublicId, CategoryIdId);
            }
            return RedirectToAction(MVC.Profile.ActionNames.TreatmentProfileList, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }
        #endregion

        #region Autorization

        public virtual ActionResult AutorizationProfileList(string ProfilePublicId)
        {
            OfficeUpsertModel Model = new OfficeUpsertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                CurrentAutorization = SaludGuruProfile.Manager.Controller.Profile.GetProfileAutorization(ProfilePublicId),
            };
            if (Model.CurrentAutorization == null)
                Model.CurrentAutorization = new List<ProfileAutorizationModel>();

            return View(Model);
        }

        public virtual ActionResult ProfileAutorizationUpsert(string ProfilePublicId, SessionController.Models.Profile.enumRole RoleId)
        {
            int result = 0;

            ProfileAutorizationModel modelToCreate = new ProfileAutorizationModel();
            modelToCreate.CreateDate = DateTime.Now;

            //modelToCreate.ProfileRoleId = ProfilePublicId;
            modelToCreate.Role = RoleId;
            modelToCreate.ProfilePublicId = ProfilePublicId;
            modelToCreate.UserEmail = Request["UserEmail"].Trim();

            result = SaludGuruProfile.Manager.Controller.Profile.ProfileAutorizationUpsert(modelToCreate);

            return RedirectToAction(MVC.Profile.ActionNames.AutorizationProfileList, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }

        public virtual ActionResult ProfileAutorizationDelete(string ProfilePublicId)
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                int profileRoleId = int.Parse(Request["ProfileRoleId"].ToString().Trim());
                SaludGuruProfile.Manager.Controller.Profile.DeleteProfileAutorization(profileRoleId);
            }
            return RedirectToAction(MVC.Profile.ActionNames.AutorizationProfileList, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
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

        private TreatmentOfficeModel GetTreatmentOfficeRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                TreatmentOfficeModel oReturn = new TreatmentOfficeModel()
                {
                    CategoryId = int.Parse(Request["Treatment-id"].Trim()),
                    TreatmentOfficeInfo = new List<TreatmentOfficeInfoModel>() 
                    { 
                        new TreatmentOfficeInfoModel()
                        {
                            CategoryInfoId= string.IsNullOrEmpty(Request["TrInf_DurationTime"])?0:int.Parse(Request["TrInf_DurationTime"].ToString().Trim()),
                            OfficeCategoryInfoType = enumOfficeCategoryInfoType.DurationTime,
                            Value = Request["Treatment-duration"].ToString(),
                        },
                        new TreatmentOfficeInfoModel()
                        {
                            CategoryInfoId= string.IsNullOrEmpty(Request["TrInf_IsDefault"])?0:int.Parse(Request["TrInf_IsDefault"].ToString().Trim()),
                            OfficeCategoryInfoType = enumOfficeCategoryInfoType.IsDefault,
                            Value = (!string.IsNullOrEmpty(Request["IsDefault"]) && Request["IsDefault"].ToString().ToLower() == "on") ? "true" : "false",
                        },
                        new TreatmentOfficeInfoModel()
                        {
                            CategoryInfoId= string.IsNullOrEmpty(Request["TrInf_AfterCare"])?0:int.Parse(Request["TrInf_AfterCare"].ToString().Trim()),
                            OfficeCategoryInfoType = enumOfficeCategoryInfoType.AfterCare,
                            LargeValue = Request["AfterCare"].ToString(),
                        },
                        new TreatmentOfficeInfoModel()
                        {
                            CategoryInfoId= string.IsNullOrEmpty(Request["TrInf_BeforeCare"])?0:int.Parse(Request["TrInf_BeforeCare"].ToString().Trim()),
                            OfficeCategoryInfoType = enumOfficeCategoryInfoType.BeforeCare,
                            LargeValue = Request["BeforeCare"].ToString(),
                        },
                    }
                };
                return oReturn;
            }
            return null;
        }

        private ScheduleAvailableModel GetScheduleAvailableRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                ScheduleAvailableModel oReturn = new ScheduleAvailableModel()
                {
                    Day = (DayOfWeek)int.Parse(Request["Day"].ToString()),
                    StartTime = new TimeSpan(GetPersonalizedHour(Request["hStartDate"].ToString()), int.Parse(Request["mStartDate"].ToString()), 0),
                    EndTime = new TimeSpan(GetPersonalizedHour(Request["hEndDate"].ToString()), int.Parse(Request["mEndDate"].ToString()), 0),
                };
                return oReturn;
            }
            return null;
        }

        private int GetPersonalizedHour(string strHourToEval)
        {
            int oReturn = int.Parse(strHourToEval.Split(',')[0].Trim());
            if (strHourToEval.Split(',')[1].ToLower() == "pm" && oReturn <= 12)
                oReturn = oReturn + 12;
            return oReturn;
        }
        #endregion
    }
}