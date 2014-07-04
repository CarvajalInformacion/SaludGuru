using BackOffice.Models.General;
using BackOffice.Models.Patient;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class PatientController : BaseController
    {
        public virtual ActionResult Search(string PublicProfileId, string SearchParam)
        {
            return View();
        }

        public virtual ActionResult PatientGetAllByPublicPatientId(string PublicPatientId)
        {
            PatientModel modelToReturn = new PatientModel();
            modelToReturn = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(PublicPatientId);
            return View(modelToReturn);
        }

        public virtual ActionResult PatientUpsert(string PatientPublicId)
        {
            string ProfilePublicId = BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId;

            PatientUpSertModel Model = new PatientUpSertModel();
            Model.PatientOptions = MedicalCalendar.Manager.Controller.Patient.GetPatientOptions();
            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                //get request model
                PatientModel PatientToCreate = GetPatientInfoRequestModel();

                //create patient 
                string oProfilePublicId = MedicalCalendar.Manager.Controller.Patient.UpsertPatientInfo(PatientToCreate, ProfilePublicId, null);

                //get updated profile info
                Model.Patient = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(oProfilePublicId);
                return RedirectToAction(MVC.Patient.ActionNames.Search, MVC.Patient.Name, new { PublicProfileId = ProfilePublicId });
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

            //if (Model.Patient == null )
            //    Model.Patient = new PatientModel();

            return View(Model);
        }

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
                        }                   
                    }
                };

                return oReturn;
            }
            return null;
        }

        #region Appointment

        public virtual ActionResult AppointmentList(string PatientPublicId)
        {
            List<AppointmentModel> ListAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentList(PatientPublicId);
            List<BackOffice.Models.Appointment.AppointmentListModel> Model = new List<Models.Appointment.AppointmentListModel>();
            if (ListAppointment != null && ListAppointment.Count > 0)
            {
                Model = ListAppointment.
                    Select(x => new BackOffice.Models.Appointment.AppointmentListModel()
                    {
                        AppointmentPublicId = x.AppointmentPublicId,
                        StatusName = x.Status.ToString(),
                        StartDate = x.StartDate.ToString(),
                        EndDate = x.EndDate.ToString(),
                        CreateDate = x.CreateDate.ToString(),
                        OfficePublicId = x.OfficePublicId,
                        OfficeName = x.OfficeName,
                    }).ToList();
            }
            return View(Model);
        }

        #endregion
    }
}