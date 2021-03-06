﻿using BackOffice.Models.Appointment;
using BackOffice.Models.Office;
using BackOffice.Models.Profile;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using SaludGuru.Notifications.Models;
using SaludGuruProfile.Manager.Controller;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using SessionController.Models.Auth;
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
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
            };

            return View(Model);
        }

        public virtual ActionResult ProfileUpsertSmall(string ProfilePublicId, HttpPostedFileBase UploadFile)
        {
            if (UploadFile != null && UploadFile.ContentLength > 0)
            {
                //get request profile info id
                int ProfileInfoId_Public = Convert.ToInt32(Request["ProfileInfoId_ImageProfileSmall"]);
                int ProfileInfoId_Original = Convert.ToInt32(Request["ProfileInfoId_ImageProfileSmallOriginal"]);

                //eval folder destination
                string oFolder = BackOffice.Models.General.InternalSettings.Instance
                    [BackOffice.Models.General.Constants.C_Settings_ProfileImage_TempDirectory].Value.TrimEnd('\\') + "\\";

                if (!System.IO.Directory.Exists(oFolder))
                    System.IO.Directory.CreateDirectory(oFolder);

                //get file name
                string oImageName = enumImageType.ProfileSmall.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") +
                    UploadFile.FileName.Substring(UploadFile.FileName.LastIndexOf("."));

                //save file into server
                UploadFile.SaveAs(oFolder + oImageName);

                //save image into s3 and database
                SaludGuruProfile.Manager.Controller.Profile.UpsertProfileSmallImage
                    (new ProfileModel()
                    {
                        ProfilePublicId = ProfilePublicId,
                        ProfileInfo = new List<ProfileInfoModel>()
                        {
                            new ProfileInfoModel()
                            {
                                ProfileInfoId = ProfileInfoId_Public,
                                ProfileInfoType = enumProfileInfoType.ImageProfileSmall
                            },
                            new ProfileInfoModel()
                            {
                                ProfileInfoId = ProfileInfoId_Original,
                                ProfileInfoType = enumProfileInfoType.ImageProfileSmallOriginal
                            }
                        }
                    },
                    oFolder + oImageName);
            }

            return RedirectToAction(MVC.Profile.ActionNames.ProfileEditImage, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }

        public virtual ActionResult ProfileUpsertLarge(string ProfilePublicId, HttpPostedFileBase UploadFile)
        {
            if (UploadFile != null && UploadFile.ContentLength > 0)
            {
                //get request profile info id
                int ProfileInfoId_Public = Convert.ToInt32(Request["ProfileInfoId_ImageProfileLarge"]);
                int ProfileInfoId_Original = Convert.ToInt32(Request["ProfileInfoId_ImageProfileLargeOriginal"]);

                //eval folder destination
                string oFolder = BackOffice.Models.General.InternalSettings.Instance
                    [BackOffice.Models.General.Constants.C_Settings_ProfileImage_TempDirectory].Value.TrimEnd('\\') + "\\";

                if (!System.IO.Directory.Exists(oFolder))
                    System.IO.Directory.CreateDirectory(oFolder);

                //get file name
                string oImageName = enumImageType.ProfileLarge.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") +
                    UploadFile.FileName.Substring(UploadFile.FileName.LastIndexOf("."));

                //save file into server
                UploadFile.SaveAs(oFolder + oImageName);

                //save image into s3 and database
                SaludGuruProfile.Manager.Controller.Profile.UpsertProfileLargeImage
                    (new ProfileModel()
                    {
                        ProfilePublicId = ProfilePublicId,
                        ProfileInfo = new List<ProfileInfoModel>()
                        {
                            new ProfileInfoModel()
                            {
                                ProfileInfoId = ProfileInfoId_Public,
                                ProfileInfoType = enumProfileInfoType.ImageProfileLarge
                            },
                            new ProfileInfoModel()
                            {
                                ProfileInfoId = ProfileInfoId_Original,
                                ProfileInfoType = enumProfileInfoType.ImageProfileLargeOriginal
                            }
                        }
                    },
                    oFolder + oImageName);
            }

            return RedirectToAction(MVC.Profile.ActionNames.ProfileEditImage, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }

        public virtual ActionResult ProfileUpsertGeneral(string ProfilePublicId, List<HttpPostedFileBase> lstUploadFile)
        {
            if (lstUploadFile != null)
            {
                //eval folder destination
                string oFolder = BackOffice.Models.General.InternalSettings.Instance
                    [BackOffice.Models.General.Constants.C_Settings_ProfileImage_TempDirectory].Value.TrimEnd('\\') + "\\";

                if (!System.IO.Directory.Exists(oFolder))
                    System.IO.Directory.CreateDirectory(oFolder);

                List<string> lstFilesToProccess = new List<string>();

                lstUploadFile.All(UploadFile =>
                {
                    if (UploadFile != null && UploadFile.ContentLength > 0)
                    {

                        //get file name
                        string oImageName = string.Empty;
                        do
                        {
                            oImageName = enumImageType.ProfileGeneral.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") +
                                UploadFile.FileName.Substring(UploadFile.FileName.LastIndexOf("."));

                        } while (lstFilesToProccess.Any(x => x.IndexOf(oImageName) >= 0));

                        //save file into server
                        UploadFile.SaveAs(oFolder + oImageName);

                        //get save image name
                        lstFilesToProccess.Add(oFolder + oImageName);
                    }
                    return true;
                });


                //save image into s3 and database
                SaludGuruProfile.Manager.Controller.Profile.InsertProfileGeneralImage
                    (new ProfileModel() { ProfilePublicId = ProfilePublicId, }, lstFilesToProccess);
            }

            return RedirectToAction(MVC.Profile.ActionNames.ProfileEditImage, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }

        public virtual ActionResult ProfileDeleteGeneral(string ProfilePublicId)
        {
            if (!string.IsNullOrEmpty(Request["chk_DeleteGeneral"]))
            {
                List<ProfileInfoModel> pinfToDelete = new List<ProfileInfoModel>();

                Request["chk_DeleteGeneral"].
                    Split(',').
                    Where(x => !string.IsNullOrEmpty(x)).
                    All(pinfid =>
                    {
                        pinfToDelete.Add(new ProfileInfoModel()
                        {
                            ProfileInfoId = Convert.ToInt32(pinfid.Replace(" ", "")),
                        });
                        return true;
                    });

                SaludGuruProfile.Manager.Controller.Profile.DeleteProfileDetailInfo(pinfToDelete);
            }
            return RedirectToAction(MVC.Profile.ActionNames.ProfileEditImage, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
        }

        public virtual ActionResult ProfilePreview(string ProfilePublicId)
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
            };

            return View(Model);
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

        public virtual ActionResult OfficeAppointmentUpload(string ProfilePublicId, string OfficePublicId)
        {
            OfficeUpsertModel Model = new OfficeUpsertModel()
            {
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                CurrentOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(OfficePublicId),
            };

            return View(Model);
        }

        [HttpPost]
        public virtual ActionResult OfficeAppointmentUpload(string ProfilePublicId, string OfficePublicId, HttpPostedFileBase ExcelFile)
        {
            string RemoteErrorFilePath = string.Empty;

            if (ExcelFile.ContentLength > 0)
            {
                //get current office
                OfficeModel CurrentOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(OfficePublicId);

                //save file into server
                string Folder = BackOffice.Models.General.InternalSettings.Instance
                    [BackOffice.Models.General.Constants.C_Settings_File_TmpExcelDir].Value.TrimEnd('\\');

                if (!System.IO.Directory.Exists(Folder)) { System.IO.Directory.CreateDirectory(Folder); };

                string FilePath = Folder + "\\" + ProfilePublicId + "_" + OfficePublicId + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
                string ErrorFilePath = FilePath.Replace(".xls", "_log.csv");
                ExcelFile.SaveAs(FilePath);

                //proccess file
                ProccessAppointmentFile(FilePath, ErrorFilePath, ProfilePublicId, CurrentOffice);

                //upload images
                BackOffice.Models.General.GenericFileLoader oLoader = new BackOffice.Models.General.GenericFileLoader()
                {
                    FilesToUpload = new List<string>() { FilePath, ErrorFilePath },
                    RemoteFolder = BackOffice.Models.General.InternalSettings.Instance
                        [BackOffice.Models.General.Constants.C_Settings_File_RemoteExcelDir].
                        Value,
                };

                oLoader.StartUpload();

                System.IO.File.Delete(FilePath);
                System.IO.File.Delete(ErrorFilePath);

                RemoteErrorFilePath = oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == ErrorFilePath).Select(x => x.PublishFile.ToString()).FirstOrDefault();
            }

            return RedirectToAction(MVC.Profile.ActionNames.OfficeAppointmentUpload,
                MVC.Profile.Name,
                new
                {
                    ProfilePublicId = ProfilePublicId,
                    OfficePublicId = OfficePublicId,
                    ErrorFile = RemoteErrorFilePath,
                });
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
            if (Model.Profile.DefaultSpecialty == null)
            {
                Model.Profile.DefaultSpecialty = new SpecialtyModel();
            }
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
                OfficeOptions = SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions(),
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

        #region Comunications

        public virtual ActionResult ProfileMessangerUpsert(string ProfilePublicId)
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                ProfileOptions = SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions(),
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
            };

            //load comunication model
            Model.RelatedComunication = new List<ProfileComunicationModel>();

            ProfileComunicationModel.MessageComunicationTypeEnabled.All(x =>
            {
                Model.RelatedComunication.Add(
                    new ProfileComunicationModel(Model.Profile.ProfileInfo.
                        Where(y => y.ProfileInfoType == x).
                        ToList(),
                        x));

                return true;
            });

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                List<ProfileInfoModel> oProfileInfoToDelete = null;

                ProfileModel oProfileToUpsert = GetReminderRequestModel(Model.RelatedComunication, out oProfileInfoToDelete);

                oProfileInfoToDelete = oProfileToUpsert.ProfileInfo.Where(x => string.IsNullOrEmpty(x.Value)).Select(p => p).ToList();
                SaludGuruProfile.Manager.Controller.Profile.DeleteProfileDetailInfo(oProfileInfoToDelete);

                oProfileToUpsert.ProfileInfo = oProfileToUpsert.ProfileInfo.Where(x => x.Value != string.Empty).ToList();

                //create profile 
                SaludGuruProfile.Manager.Controller.Profile.UpsertProfileDetailInfo(oProfileToUpsert);

                Model = new ProfileUpSertModel()
                {
                    ProfileOptions = SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions(),
                    Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                };

                //load comunication model
                Model.RelatedComunication = new List<ProfileComunicationModel>();

                ProfileComunicationModel.MessageComunicationTypeEnabled.All(x =>
                {
                    Model.RelatedComunication.Add(
                        new ProfileComunicationModel(Model.Profile.ProfileInfo.
                            Where(y => y.ProfileInfoType == x).
                            ToList(),
                            x));

                    return true;
                });
            }
            return View(Model);
        }

        public virtual ActionResult ProfileReminderUpsert(string ProfilePublicId)
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                ProfileOptions = SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions(),
                Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
            };

            //load comunication model
            Model.RelatedComunication = new List<ProfileComunicationModel>();

            ProfileComunicationModel.MessageReminderTypeEnabled.All(x =>
            {
                Model.RelatedComunication.Add(
                    new ProfileComunicationModel(Model.Profile.ProfileInfo.
                        Where(y => y.ProfileInfoType == x).
                        ToList(),
                        x));

                return true;
            });

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                List<ProfileInfoModel> oProfileInfoToDelete = null;

                ProfileModel oProfileToUpsert = GetReminderRequestModel(Model.RelatedComunication, out oProfileInfoToDelete);

                oProfileInfoToDelete = oProfileToUpsert.ProfileInfo.Where(x => string.IsNullOrEmpty(x.Value)).Select(p => p).ToList();
                SaludGuruProfile.Manager.Controller.Profile.DeleteProfileDetailInfo(oProfileInfoToDelete);

                oProfileToUpsert.ProfileInfo = oProfileToUpsert.ProfileInfo.Where(x => x.Value != string.Empty).ToList();

                //create profile 
                SaludGuruProfile.Manager.Controller.Profile.UpsertProfileDetailInfo(oProfileToUpsert);

                Model = new ProfileUpSertModel()
                {
                    ProfileOptions = SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions(),
                    Profile = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId),
                };

                //load comunication model
                Model.RelatedComunication = new List<ProfileComunicationModel>();

                ProfileComunicationModel.MessageReminderTypeEnabled.All(x =>
                {
                    Model.RelatedComunication.Add(
                        new ProfileComunicationModel(Model.Profile.ProfileInfo.
                            Where(y => y.ProfileInfoType == x).
                            ToList(),
                            x));

                    return true;
                });
            }
            return View(Model);
        }

        #endregion

        #region RelatedProfile

        public virtual ActionResult RelatedProfileSearch(string ProfilePublicId)
        {
            ProfileRelatedModel oReturn = new ProfileRelatedModel();
            ProfileModel Model = new ProfileModel();
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                          && bool.Parse(Request["UpsertAction"]))
            {
                string ProfilePublicIdChild = Request["divGridProfile-txtSearch-id"];
                Model = SaludGuruProfile.Manager.Controller.Profile.GetRelatedProfileAll(ProfilePublicId);
                if (Model.ChildProfile.Where(x => x.ProfilePublicId == ProfilePublicIdChild).Select(x => x).ToList().Count() > 0)
                    return RedirectToAction(MVC.Profile.ActionNames.RelatedProfileSearch, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });

                SaludGuruProfile.Manager.Controller.Profile.RelatedProfileCreate(ProfilePublicId, ProfilePublicIdChild);
            }
            Model = SaludGuruProfile.Manager.Controller.Profile.GetRelatedProfileAll(ProfilePublicId);

            if (Model.ChildProfile.Count() > 0)
            {
                foreach (ProfileModel item in Model.ChildProfile)
                {
                    string img = item.ProfileInfo.Where(x => x.ProfileInfoType == SaludGuruProfile.Manager.Models.enumProfileInfoType.ImageProfileSmall).Select(x => x.Value).FirstOrDefault();

                    if (img == null)
                    {
                        if (item.ProfileInfo.
                           Where(x => x.ProfileInfoType == enumProfileInfoType.Gender).
                           Select(x => x.Value == "true" ? true : false).
                           DefaultIfEmpty(false).
                           FirstOrDefault())
                        {
                            img = BackOffice.Models.General.InternalSettings.Instance
                                [BackOffice.Models.General.Constants.C_Settings_ProfileImage_Man].Value;

                            ProfileInfoModel infoAdd = new ProfileInfoModel();
                            infoAdd.ProfileInfoType = enumProfileInfoType.ImageProfileSmall;
                            infoAdd.Value = img;
                            item.ProfileInfo.Add(infoAdd);
                        }
                        else
                        {
                            img = BackOffice.Models.General.InternalSettings.Instance
                                [BackOffice.Models.General.Constants.C_Settings_ProfileImage_Woman].Value;

                            ProfileInfoModel infoAdd = new ProfileInfoModel();
                            infoAdd.ProfileInfoType = enumProfileInfoType.ImageProfileSmall;
                            infoAdd.Value = img;
                            item.ProfileInfo.Add(infoAdd);
                        }
                    }
                }
            }
            oReturn.PrincipalProfile = Model;
            oReturn.AutoComplitListProfiles = SaludGuruProfile.Manager.Controller.Profile.ProfileSearchToRelate(string.Empty, ProfilePublicId, 0, 20);
            return View(oReturn);
        }

        public virtual ActionResult RelatedProfileDelete(string ProfilePublicId)
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                         && bool.Parse(Request["UpsertAction"]))
            {
                string ProfilePublicIdChild = Request["ProfileRelated"];
                SaludGuruProfile.Manager.Controller.Profile.RelatedProfileDelete(ProfilePublicId, ProfilePublicIdChild);
            }
            return RedirectToAction(MVC.Profile.ActionNames.RelatedProfileSearch, MVC.Profile.Name, new { ProfilePublicId = ProfilePublicId });
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
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_FeaturedProfile"])?0:int.Parse(Request["CatId_FeaturedProfile"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.FeaturedProfile,
                            Value = (!string.IsNullOrEmpty(Request["FeaturedProfile"]) && Request["FeaturedProfile"].ToString().ToLower() == "on") ? "true" : "false",
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_AgendaOnline"])?0:int.Parse(Request["CatId_AgendaOnline"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.AgendaOnline,
                            Value = (!string.IsNullOrEmpty(Request["AgendaOnline"]) && Request["AgendaOnline"].ToString().ToLower() == "on") ? "true" : "false",
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
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_SalesForce"])?0:int.Parse(Request["CatId_SalesForce"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.SalesforceCode,
                            Value = Request["Salesforce"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_Keywords"])?0:int.Parse(Request["CatId_Keywords"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.KeyWords,
                            LargeValue = Request["Keywords"].ToString(),
                        },
                         new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_Mobil"])?0:int.Parse(Request["CatId_Mobil"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.Mobile,
                            Value = Request["Mobil"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_OldProfileId"])?0:int.Parse(Request["CatId_OldProfileId"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.OldProfileId,
                            Value = Request["OldProfileId"].ToString(),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_MPFreeScheduleId"])?0:int.Parse(Request["CatId_MPFreeScheduleId"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.MarketPlaceSlotDuration,
                            Value = Request["MPFreeScheduleText"].ToString(),
                        },
                         new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["CatId_ShortProfileId"])?0:int.Parse(Request["CatId_ShortProfileId"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.ShortProfile,
                            LargeValue = Request["ShortProfileText"].ToString(),
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
                            OfficeInfoId= string.IsNullOrEmpty(Request["CatId_MarketPlaceEnabled"])?0:int.Parse(Request["CatId_MarketPlaceEnabled"].ToString().Trim()),
                            OfficeInfoType = enumOfficeInfoType.MarketPlaceEnabled,
                            Value = (!string.IsNullOrEmpty(Request["MarketPlaceEnabled"]) && Request["MarketPlaceEnabled"].ToString().ToLower() == "on") ? "true" : "false",
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
                        new OfficeInfoModel()
                        {
                            OfficeInfoId = string.IsNullOrEmpty(Request["CatId_SlotMinutes"])?0:int.Parse(Request["CatId_SlotMinutes"].ToString().Trim()),
                            OfficeInfoType = enumOfficeInfoType.SlotMinutes,
                            Value = Request["SlotMinutes"].ToString(),
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
                            CategoryInfoId= string.IsNullOrEmpty(Request["TrInf_MarketPlaceEnabled"])?0:int.Parse(Request["TrInf_MarketPlaceEnabled"].ToString().Trim()),
                            OfficeCategoryInfoType = enumOfficeCategoryInfoType.MarketPlaceEnabled,
                            Value = (!string.IsNullOrEmpty(Request["MarketPlaceEnabled"]) && Request["MarketPlaceEnabled"].ToString().ToLower() == "on") ? "true" : "false",
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

        private ProfileModel GetComunicationRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
               && bool.Parse(Request["UpsertAction"]))
            {
                ProfileModel oReturn = new ProfileModel()
                {
                    ProfilePublicId = string.IsNullOrEmpty(Request["ProfilePublicId"]) ? null : Request["ProfilePublicId"].ToString(),

                    ProfileInfo = new List<ProfileInfoModel>() 
                    { 
                        //Asignacion de cita
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["IsemailAC"])?0:int.Parse(Request["IsemailAC"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.AsignedAppointment,
                            Value = (Request["AC_EMail"] != null ? ((int)enumMessageType.Email).ToString() : string.Empty),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["IsSmsAC"])?0:int.Parse(Request["IsSmsAC"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.AsignedAppointment,
                            Value = (Request["AC_Sms"]!= null ? ((int)enumMessageType.Sms).ToString() : string.Empty),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["IsNotifyAC"])?0:int.Parse(Request["IsNotifyAC"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.AsignedAppointment,                            
                            Value = (Request["AC_NotifyGuru"] != null ? ((int)enumMessageType.GuruNotification).ToString() : string.Empty),
                        },
                        //Cancelacion de cita
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["isEMailCC"])?0:int.Parse(Request["isEMailCC"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.CancelAppointment,
                            Value = (Request["CC_EMail"] != null ? ((int)enumMessageType.Email).ToString() : string.Empty),
                        },                        
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["isSmsCC"])?0:int.Parse(Request["isSmsCC"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.CancelAppointment,
                            Value = (Request["CC_Sms"] != null ? ((int)enumMessageType.Sms).ToString() : string.Empty),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["isNotifyCC"])?0:int.Parse(Request["isNotifyCC"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.CancelAppointment,
                            Value = (Request["CC_GuruNotify"] != null ? ((int)enumMessageType.GuruNotification).ToString() : string.Empty),
                        },
                        //Encuesta Satisfacción
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["isEmailEs"])?0:int.Parse(Request["isEmailEs"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.SatisfactionSurvey,
                            Value = (Request["ES_EMail"] != null ? ((int)enumMessageType.Email).ToString() : string.Empty),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["isSmsEs"])?0:int.Parse(Request["isSmsEs"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.SatisfactionSurvey,
                            Value = (Request["ES_Sms"] != null ? ((int)enumMessageType.Sms).ToString() : string.Empty),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["isNotifyEs"])?0:int.Parse(Request["isNotifyEs"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.SatisfactionSurvey,
                            Value = (Request["ES_GuruNotify"] != null ? ((int)enumMessageType.GuruNotification).ToString() : string.Empty),
                        },
                        //Modificación de cita
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["isEmailMC"])?0:int.Parse(Request["isEmailMC"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.ModifyAppointment,
                            Value = (Request["MC_EMail"] != null ? ((int)enumMessageType.Email).ToString() : string.Empty),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["isSmsMC"])?0:int.Parse(Request["isSmsMC"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.ModifyAppointment,
                            Value = (Request["MC_Sms"] != null ? ((int)enumMessageType.Sms).ToString() : string.Empty),
                        },
                        new ProfileInfoModel()
                        {
                            ProfileInfoId = string.IsNullOrEmpty(Request["isNotifyMC"])?0:int.Parse(Request["isNotifyMC"].ToString().Trim()),
                            ProfileInfoType = enumProfileInfoType.ModifyAppointment,
                            Value = (Request["MC_GuruNotify"] != null ? ((int)enumMessageType.GuruNotification).ToString() : string.Empty),
                       },
                    }
                };
                return oReturn;
            }
            return null;
        }

        private ProfileModel GetReminderRequestModel(List<ProfileComunicationModel> vComunicationInfo, out List<ProfileInfoModel> vProfileInfoToDelete)
        {
            vProfileInfoToDelete = new List<ProfileInfoModel>();

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
               && bool.Parse(Request["UpsertAction"]))
            {
                ProfileModel oReturn = new ProfileModel()
                   {
                       ProfilePublicId = string.IsNullOrEmpty(Request["ProfilePublicId"]) ? null : Request["ProfilePublicId"].ToString(),
                       ProfileInfo = new List<ProfileInfoModel>()
                   };

                vComunicationInfo.All(cit =>
                {
                    int Duration = Convert.ToInt32(Request["ComunicationType_" + ((int)cit.ComunicationType).ToString()]);

                    ((enumMessageType[])Enum.GetValues(typeof(enumMessageType))).All(mst =>
                    {
                        //get actual value
                        ProfileInfoModel CurrentInfo = cit.MessageType.Where(mt => mt.Value.ToString() == ((int)mst).ToString()).FirstOrDefault();
                        if (CurrentInfo != null)
                            CurrentInfo.LargeValue = Duration.ToString();

                        //get request value
                        string RequestKey = "chb_";
                        if (CurrentInfo == null)
                        {
                            //new profile info
                            RequestKey += ((int)cit.ComunicationType).ToString() + "_" + ((int)mst).ToString() + "_";
                        }
                        else
                        {
                            //already profile info
                            RequestKey += ((int)cit.ComunicationType).ToString() + "_" + ((int)mst).ToString() + "_" + CurrentInfo.ProfileInfoId;
                        }

                        bool IsInRequest = Request.Form.AllKeys.Any(x => x.ToLower().Replace(" ", "") == RequestKey.ToLower().Replace(" ", ""));

                        if (IsInRequest && CurrentInfo != null)
                        {
                            //Exist on the DataBase
                            oReturn.ProfileInfo.Add(CurrentInfo);
                        }
                        else if (IsInRequest && CurrentInfo == null)
                        {
                            //nuevo profile info
                            oReturn.ProfileInfo.Add(
                                new ProfileInfoModel
                                {
                                    ProfileInfoId = 0,
                                    ProfileInfoType = cit.ComunicationType,
                                    Value = ((int)mst).ToString(),
                                    LargeValue = Duration.ToString()
                                }
                                );
                        }
                        else if (!IsInRequest && CurrentInfo != null)
                        {
                            //borrar profile info
                            oReturn.ProfileInfo.Add(
                                new ProfileInfoModel
                                {
                                    ProfileInfoId = CurrentInfo.ProfileInfoId
                                }
                                );
                        }

                        return true;
                    });

                    return true;
                });

                return oReturn;
            }
            return null;
        }

        private void ProccessAppointmentFile(string FilePath, string ErrorFilePath, string ProfilePublicId, OfficeModel CurrentOffice)
        {
            //get excel rows
            LinqToExcel.ExcelQueryFactory XlsInfo = new LinqToExcel.ExcelQueryFactory(FilePath);

            List<ExcelAppointmentModel> oAptToProcess =
                (from x in XlsInfo.Worksheet<ExcelAppointmentModel>(0)
                 select x).ToList();

            //get profile info
            List<InsuranceModel> olstInsurance = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(string.Empty);

            List<ExcelAppointmentResultModel> oAptToProcessResult = new List<ExcelAppointmentResultModel>();

            //process appointment
            oAptToProcess.Where(apmt => !string.IsNullOrEmpty(apmt.Identificacion)).All(apmt =>
            {
                try
                {
                    //get patient info
                    PatientModel CurrentPatient = MedicalCalendar.Manager.Controller.Patient.PatientGetFromIdentificationNumber
                        (ProfilePublicId, apmt.Identificacion);

                    if (CurrentPatient == null || string.IsNullOrEmpty(CurrentPatient.PatientPublicId))
                    {
                        #region Create Patient
                        CurrentPatient = new PatientModel()
                        {
                            Name = apmt.Nombre,
                            LastName = apmt.Apellido,
                            PatientInfo = new List<MedicalCalendar.Manager.Models.Patient.PatientInfoModel>()
                                {
                                    new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.IdentificationNumber,
                                        Value = apmt.Identificacion,
                                    },
                                    new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.Email,
                                        Value = apmt.Correo,
                                    },
                                    new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.Telephone,
                                        Value = string.Empty,
                                    },
                                     new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.Mobile,
                                        Value = apmt.Celular,
                                    },                        
                                    new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.Birthday,
                                        Value = string.Empty,
                                    },
                                    new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.Gender,
                                        Value = "true",
                                    },
                                    new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.Insurance,
                                        Value = olstInsurance.Where(x=>x.Name == apmt.Seguro).Select(x=>x.CategoryId.ToString()).DefaultIfEmpty(string.Empty).FirstOrDefault(),
                                    }, 
                                     new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.MedicalPlan,
                                        Value = string.Empty,
                                    },
                                    new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.Responsable,
                                        Value = string.Empty,
                                    },
                                    new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.SendEmail,
                                        Value = "true",
                                    },
                                    new PatientInfoModel()
                                    {
                                        PatientInfoType = enumPatientInfoType.SendSMS,
                                        Value = "true",
                                    }
                                },
                        };

                        CurrentPatient.PatientPublicId = MedicalCalendar.Manager.Controller.Patient.UpsertPatientInfo
                            (CurrentPatient, ProfilePublicId, null);

                        #endregion
                    }

                    //create appointment
                    #region Create Appointment
                    AppointmentModel CurrentAppointment = new AppointmentModel()
                    {
                        OfficePublicId = CurrentOffice.OfficePublicId,
                        Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.New,
                        StartDate = new DateTime(apmt.Ano, apmt.Mes, apmt.Dia, apmt.Hora, apmt.Minutos, 0),
                        EndDate = new DateTime(apmt.Ano, apmt.Mes, apmt.Dia, apmt.Hora, apmt.Minutos, 0).AddMinutes(apmt.Duracion),

                        AppointmentInfo = new List<AppointmentInfoModel>()
                            {
                                new AppointmentInfoModel()
                                {
                                    AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category,
                                    Value = CurrentOffice.RelatedTreatment.Select(x=>x.CategoryId.ToString()).FirstOrDefault(),
                                },
                                new AppointmentInfoModel()
                                {
                                    AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare,
                                    LargeValue = string.Empty,
                                },
                                new AppointmentInfoModel()
                                {
                                    AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.BeforeCare,
                                    LargeValue = string.Empty,
                                },
                                new AppointmentInfoModel()
                                {
                                    AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.AppointmentNote,
                                    LargeValue = string.Empty,
                                },
                            },

                        RelatedPatient = new List<PatientModel>()
                            {
                                CurrentPatient,
                            },
                    };

                    CurrentAppointment.AppointmentPublicId = MedicalCalendar.Manager.Controller.Appointment.UpsertAppointmentInfo
                        (CurrentAppointment, new List<PatientModel>());

                    #endregion

                    oAptToProcessResult.Add(new ExcelAppointmentResultModel()
                    {
                        AptModel = apmt,
                        Success = true,
                        Error = "Se ha creado la cita '" + CurrentAppointment.AppointmentPublicId + "'",
                    });

                    #region Messenger

                    ProfileModel oSource = new ProfileModel();
                    oSource = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId);

                    List<PatientModel> oTargetList = new List<PatientModel>();
                    oTargetList.Add(CurrentPatient);
                    //Send de Signed App
                    BackOffice.Web.Controllers.BaseController.SendMessage(oSource, enumProfileInfoType.AsignedAppointment, oTargetList, CurrentAppointment, false);
                    //Send de Reminder App
                    BackOffice.Web.Controllers.BaseController.SendMessage(oSource, enumProfileInfoType.ReminderAppointment, oTargetList, CurrentAppointment, false);

                    #endregion
                }
                catch (Exception err)
                {
                    oAptToProcessResult.Add(new ExcelAppointmentResultModel()
                    {
                        AptModel = apmt,
                        Success = false,
                        Error = "Error :: " + err.Message + " :: " +
                                    err.StackTrace +
                                    (err.InnerException == null ? string.Empty :
                                    " :: " + err.InnerException.Message + " :: " +
                                    err.InnerException.StackTrace),
                    });
                }
                return true;
            });

            //save log file
            #region Error log file
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(ErrorFilePath))
                {
                    string strSep = ";";

                    sw.WriteLine
                            ("\"Dia\"" + strSep +
                            "\"Mes\"" + strSep +
                            "\"Ano\"" + strSep +
                            "\"Hora\"" + strSep +
                            "\"Minutos\"" + strSep +
                            "\"Duracion\"" + strSep +
                            "\"Identificacion\"" + strSep +
                            "\"Nombre\"" + strSep +
                            "\"Apellido\"" + strSep +
                            "\"Celular\"" + strSep +
                            "\"Correo\"" + strSep +
                            "\"Seguro\"" + strSep +

                            "\"Success\"" + strSep +
                            "\"Error\"");

                    oAptToProcessResult.All(lg =>
                    {
                        sw.WriteLine
                            ("\"" + lg.AptModel.Dia + "\"" + strSep +
                            "\"" + lg.AptModel.Mes + "\"" + strSep +
                            "\"" + lg.AptModel.Ano + "\"" + strSep +
                            "\"" + lg.AptModel.Hora + "\"" + strSep +
                            "\"" + lg.AptModel.Minutos + "\"" + strSep +
                            "\"" + lg.AptModel.Duracion + "\"" + strSep +
                            "\"" + lg.AptModel.Identificacion + "\"" + strSep +
                            "\"" + lg.AptModel.Nombre + "\"" + strSep +
                            "\"" + lg.AptModel.Apellido + "\"" + strSep +
                            "\"" + lg.AptModel.Celular + "\"" + strSep +
                            "\"" + lg.AptModel.Correo + "\"" + strSep +
                            "\"" + lg.AptModel.Seguro + "\"" + strSep +

                            "\"" + lg.Success + "\"" + strSep +
                            "\"" + lg.Error + "\"");

                        return true;
                    });

                    sw.Flush();
                    sw.Close();
                }
            }
            catch { }

            #endregion
        }

        #endregion
    }
}