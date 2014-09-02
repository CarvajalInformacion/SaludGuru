using MarketPlace.Models.Appointment;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using SaludGuruProfile.Manager.Models;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class AppointmentApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public PatientModel PatientUpsert(string ProfilePublicId)
        {            
            PatientModel modelToValidate = new PatientModel();
            ProfileModel oModelSend = new ProfileModel();
            oModelSend = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId);
            string email = oModelSend.ProfileInfo.Where(x => x.ProfileInfoType == enumProfileInfoType.Email).Select(x => x.Value).FirstOrDefault();
            string mobile = oModelSend.ProfileInfo.Where(x => x.ProfileInfoType == enumProfileInfoType.Mobile).Select(x => x.Value).FirstOrDefault();

            PatientModel patientToCreate = this.GetRequestForNewPatient(email, mobile);
            //Insert the new patient
            string resultPatientPublicId = MedicalCalendar.Manager.Controller.Patient.MPUpsertPatientInfo(patientToCreate, ProfilePublicId, MarketPlace.Models.General.SessionModel.CurrentLoginUser.UserPublicId); //TODO: Ajustar el usuario no quemarlo

            #region Create the notification 
            
            List<PatientModel> PatientSource = new List<PatientModel>();
            PatientModel ItemPatientSource = MedicalCalendar.Manager.Controller.Patient.PatientGetAllByPublicPatientId(resultPatientPublicId);
            PatientSource.Add(ItemPatientSource);
            AppointmentModel appModel = new AppointmentModel();

            //Send Notification reporting a new patient
            MarketPlace.Web.Controllers.BaseController.SendMessage(oModelSend, null, PatientSource, appModel, true); 
            #endregion

            //Compare if patientTemporal Exist
            bool isTemporal = MedicalCalendar.Manager.Controller.Patient.MPPatientTemporalUpsert(resultPatientPublicId, ProfilePublicId, enumPatientState.New, "0");

            patientToCreate.PatientPublicId = resultPatientPublicId;
            return patientToCreate;
        }

        #region  Funciones Privadas
        private PatientModel GetRequestForNewPatient(string email, string mobile)
        {
            PatientModel oReturn = new PatientModel();
            PatientInfoModel oRequestInfo = new PatientInfoModel();

            oReturn.IsProfilePatient = true;
            oReturn.Name = HttpContext.Current.Request["Name"].ToString();
            oReturn.LastName = HttpContext.Current.Request["LastName"].ToString();
            oReturn.PatientInfo = new List<PatientInfoModel>() 
                {
                    new  PatientInfoModel()
                    {
                        PatientInfoId = 0,
                        PatientInfoType = enumPatientInfoType.IdentificationNumber,
                        Value = HttpContext.Current.Request["Identification"].ToString(),
                    },
                    new  PatientInfoModel()
                    {
                        PatientInfoId = 0,
                        PatientInfoType = enumPatientInfoType.Birthday,
                        Value = HttpContext.Current.Request["Birthday"].ToString(),
                    },
                     new  PatientInfoModel()
                    {
                        PatientInfoId = 0,
                        PatientInfoType = enumPatientInfoType.Gender,
                        Value = HttpContext.Current.Request["GenderMale"] == "on" ? "true" : "false",
                    },  
                     new  PatientInfoModel()
                    {
                        PatientInfoId = 0,
                        PatientInfoType = enumPatientInfoType.Email,
                        Value = email,
                    }, 
                     new  PatientInfoModel()
                    {
                        PatientInfoId = 0,
                        PatientInfoType = enumPatientInfoType.Mobile,
                        Value = mobile,
                    }, 
                };
            return oReturn;
        }
        #endregion
    }
}