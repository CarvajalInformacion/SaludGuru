﻿using BackOffice.Models.General;
using BackOffice.Models.Patient;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class PatientController : BaseController
    {
        public virtual ActionResult Search(string SearchParam)
        {
            return View();
        }

        public virtual ActionResult PatientUpsert(string PatientPublicId)
        {
            string ProfilePublicId = BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId;

            PatientUpSertModel Model = new PatientUpSertModel();

            Model.PatientOptions = MedicalCalendar.Manager.Controller.Patient.GetPatientOptions();
            Model.Insurance = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(string.Empty);

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                PatientModel PatientToCreate = GetPatientInfoRequestModel();

                //create patient 
                string oProfilePublicId = MedicalCalendar.Manager.Controller.Patient.UpsertPatientInfo(PatientToCreate, ProfilePublicId, null);

                //get updated profile info
                Model.Patient = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(oProfilePublicId);
                //return RedirectToAction(MVC.Patient.ActionNames.Search, MVC.Patient.Name, new { PublicProfileId = ProfilePublicId });
            }
            else
            {
                //get request model
                PatientModel PatientToCreate = GetPatientInfoRequestModel();
                if (PatientToCreate != null)
                {
                    //create patient 
                    string oProfilePublicId = MedicalCalendar.Manager.Controller.Patient.UpsertPatientInfo(PatientToCreate, ProfilePublicId, SessionController.SessionManager.Auth_UserLogin.UserPublicId);
                }
                Model.Patient = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(PatientPublicId);
            }
            return View(Model);
        }

        public virtual ActionResult AppointmentList(string PatientPublicId)
        {
            PatientUpSertModel Model = new PatientUpSertModel()
            {
                Patient = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(PatientPublicId),
            };

            return View(Model);
        }

        public virtual ActionResult PatientDelete(string PatientPublicId)
        {
            MedicalCalendar.Manager.Controller.Patient.PatientDelete(PatientPublicId);

            return RedirectToAction(MVC.Patient.ActionNames.Search, MVC.Patient.Name);
        }

        #region Patient Notes
        public virtual ActionResult PatientNotes(string PatientPublicId)
        {
            PatientUpSertModel Model = new PatientUpSertModel()
            {
                Patient = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(PatientPublicId),
            };

            return View(Model);
        }

        public virtual ActionResult PatientNotesUpsert(string PatientPublicId, string Name, string LastName)
        {
            string ProfilePublicId = BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId;

            PatientUpSertModel Model = new PatientUpSertModel();

            Model.PatientOptions = MedicalCalendar.Manager.Controller.Patient.GetPatientOptions();
            Model.Insurance = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(string.Empty);

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                PatientModel PatientToCreate = GetPatientNotes();
                PatientToCreate.Name = Name;
                PatientToCreate.LastName = LastName;

                //create patient 
                string oProfilePublicId = MedicalCalendar.Manager.Controller.Patient.UpsertPatientInfo(PatientToCreate, ProfilePublicId, null);

                //get updated profile info
                Model.Patient = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(oProfilePublicId);
            }
            return RedirectToAction(MVC.Patient.ActionNames.PatientNotes, MVC.Patient.Name, new { PatientPublicId = PatientPublicId });
        }
                
        #endregion

        #region Private methods

        private PatientModel GetPatientInfoRequestModel()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                 && bool.Parse(Request["UpsertAction"]))
            {
                PatientModel oReturn = new PatientModel()
                {
                    PatientPublicId = Request["PatientPublicId"],
                    Name = Request["Name"].ToString(),
                    LastModify = DateTime.Now,
                    LastName = Request["LastName"].ToString(),

                    PatientInfo = new List<PatientInfoModel>() 
                    { 
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_IdentificationNumber"])?0:int.Parse(Request["CatId_IdentificationNumber"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.IdentificationNumber,
                            Value = Request["IdentificationNumber"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_Email"])?0:int.Parse(Request["CatId_Email"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.Email,
                            Value = Request["Email"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_Telephone"])?0:int.Parse(Request["CatId_Telephone"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.Telephone,
                            Value = Request["Telefono"].ToString(),
                        },
                         new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_Mobile"])?0:int.Parse(Request["CatId_Mobile"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.Mobile,
                            Value = Request["Mobile"].ToString(),
                        },                        
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_Birthday"])?0:int.Parse(Request["CatId_Birthday"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.Birthday,
                            Value = Request["Birthday"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_Gender"])?0:int.Parse(Request["CatId_Gender"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.Gender,
                            Value = Request["Gender"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_Insurance"])? 0 :int.Parse(Request["CatId_Insurance"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.Insurance,
                            Value = Request["Insurance"].ToString(),
                        }, 
                         new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_MedicalPlan"])?0:int.Parse(Request["CatId_MedicalPlan"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.MedicalPlan,
                            Value = Request["MedicalPlan"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_Responsable"])?0:int.Parse(Request["CatId_Responsable"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.Responsable,
                            Value = Request["Responsable"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_SendEmail"])?0:int.Parse(Request["CatId_SendEmail"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.SendEmail,
                            Value = (!string.IsNullOrEmpty(Request["IsSendEmail"]) && Request["IsSendEmail"].ToString().ToLower() == "on") ? "true" : "false",
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_SendSMS"])?0:int.Parse(Request["CatId_SendSMS"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.SendSMS,
                            Value = (!string.IsNullOrEmpty(Request["IsSendSMS"]) && Request["IsSendSMS"].ToString().ToLower() == "on") ? "true" : "false",
                        }                        
                    }
                };
                return oReturn;
            }
            return null;
        }

        private PatientModel GetPatientNotes()
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                 && bool.Parse(Request["UpsertAction"]))
            {
                PatientModel pReturn = new PatientModel()
                {
                    PatientPublicId = Request["PatientPublicId"],
                    LastModify = DateTime.Now,

                    PatientInfo = new List<PatientInfoModel>() 
                    {
                        new PatientInfoModel()
                        {
                            PatientInfoId = string.IsNullOrEmpty(Request["CatId_NewNote"])?0:int.Parse(Request["CatId_NewNote"].ToString().Trim()),
                            PatientInfoType = enumPatientInfoType.DoctorNotes,
                            LargeValue = Request["NewNote"].ToString()
                        }
                    }
                };
                return pReturn;
            }
            return null;
        }

        #endregion
    }
}