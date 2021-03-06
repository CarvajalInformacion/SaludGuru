﻿using BackOffice.Models.Appointment;
using BackOffice.Models.Patient;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class PatientApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<PatientSearchModel> PatientSearch
            (string SearchCriteria, int PageNumber, int RowCount)
        {
            int oTotalCount;
            List<MedicalCalendar.Manager.Models.Patient.PatientModel> SearchPatient =
                MedicalCalendar.Manager.Controller.Patient.PatientSearch
                (BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId,
                SearchCriteria == null ? string.Empty : SearchCriteria,
                PageNumber,
                RowCount,
                out oTotalCount);

            if (SearchPatient != null && SearchPatient.Count > 0)
            {
                List<BackOffice.Models.Patient.PatientSearchModel> oReturn = SearchPatient.
                    Select(x => new BackOffice.Models.Patient.PatientSearchModel(x)
                    {
                        SearchPatientCount = oTotalCount,
                    }).ToList();

                return oReturn;
            }
            else
            {
                return new List<Models.Patient.PatientSearchModel>();
            }
        }

        [HttpPost]
        [HttpGet]
        public PatientSearchModel AddPatientDoctorNote
            (string PatientPublicId)
        {
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["DoctorNote"]))
            {
                MedicalCalendar.Manager.Controller.Patient.UpsertPatientInfoItem
                    (new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = MedicalCalendar.Manager.Models.enumPatientInfoType.DoctorNotes,
                            LargeValue = System.Web.HttpContext.Current.Request["DoctorNote"].Trim(),
                        },
                    PatientPublicId);
            }
            return new Models.Patient.PatientSearchModel(
                MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(PatientPublicId));
        }

        [HttpPost]
        [HttpGet]
        public PatientSearchModel DeletePatientDoctorNote(string PatientPublicId, string PatientInfoId)
        {
            MedicalCalendar.Manager.Controller.Patient.DeletePatientInfoItem
                (new PatientInfoModel()
                {
                    PatientInfoId = Convert.ToInt32(PatientInfoId),
                });

            return new Models.Patient.PatientSearchModel(
                            MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(PatientPublicId));
        }

        [HttpPost]
        [HttpGet]
        public List<ScheduleEventModel> GetAppoinmentByPatient(string PatientPublicId, int Quantity)
        {
            List<AppointmentModel> lstAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByPatient
                (PatientPublicId);

            ProfileModel oCurrentProfile = SaludGuruProfile.Manager.Controller.Office.OfficeGetScheduleSettings
                (BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId);

            if (lstAppointment != null)
            {
                if (Quantity > 0)
                {
                    lstAppointment = lstAppointment.OrderByDescending(x => x.StartDate).Take(Quantity).ToList();
                }

                return lstAppointment.Select(x => new
                    ScheduleEventModel
                        (x,
                        oCurrentProfile.
                        RelatedOffice.
                        Where(y => x.OfficePublicId == x.OfficePublicId).
                        FirstOrDefault())).ToList();
            }
            else
                return new List<ScheduleEventModel>();
        }

        [HttpPost]
        [HttpGet]
        public PatientSearchModel PatientUpsert(string UpsertAction)
        {
            string ProfilePublicId = BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId;

            PatientUpSertModel Model = new PatientUpSertModel();

            Model.PatientOptions = MedicalCalendar.Manager.Controller.Patient.GetPatientOptions();
            PatientModel PatientToCreate = new PatientModel();

            if (Model != null)
            {
                //get request model
                PatientToCreate = GetPatientInfoRequestModel();                

                //create patient 
                string oPatientPublicId = MedicalCalendar.Manager.Controller.Patient.UpsertPatientInfo(PatientToCreate, ProfilePublicId, null);

                PatientToCreate = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(oPatientPublicId);
            }

            return new PatientSearchModel(PatientToCreate);
        }

        #region Private Methods

        private PatientModel GetPatientInfoRequestModel()
        {
            bool isEmail = false;
            bool isSms = false;

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["Mobile"].ToString()))
                isSms = true;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["Email"].ToString()))
                isEmail = true;

            PatientModel oReturn = new PatientModel()
            {
                PatientPublicId = HttpContext.Current.Request["PatientPublicId"],
                Name = HttpContext.Current.Request["Name"].ToString(),
                LastModify = DateTime.Now,
                LastName = HttpContext.Current.Request["LastName"].ToString(),

                PatientInfo = new List<PatientInfoModel>() 
                    { 
                        new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = enumPatientInfoType.IdentificationNumber,
                            Value = HttpContext.Current.Request["IdentificationNumber"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = enumPatientInfoType.Email,
                            Value = HttpContext.Current.Request["Email"].ToString(),
                        },
                         new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = enumPatientInfoType.Mobile,
                            Value = HttpContext.Current.Request["Mobile"].ToString(),
                        },  
                         new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = enumPatientInfoType.Telephone,
                            Value = HttpContext.Current.Request["Telephone"].ToString(),
                        },  
                        new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = enumPatientInfoType.Insurance,
                            Value = HttpContext.Current.Request["Insurance"].ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = enumPatientInfoType.MedicalPlan,
                            Value = HttpContext.Current.Request["MedicalPlan"].ToString(),
                        },                       
                        new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = enumPatientInfoType.SendEmail,
                            Value = isEmail.ToString(),
                        },
                        new PatientInfoModel()
                        {
                            PatientInfoId = 0,
                            PatientInfoType = enumPatientInfoType.SendSMS,
                            Value = isSms.ToString(),
                        }
                    }
            };

            return oReturn;
        }

        #endregion
    }
}