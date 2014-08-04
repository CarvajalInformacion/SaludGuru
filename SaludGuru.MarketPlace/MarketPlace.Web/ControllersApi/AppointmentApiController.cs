using MarketPlace.Models.Appointment;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Patient;
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
            PatientModel patientToCreate = this.GetRequestForNewPatient();
            string result = MedicalCalendar.Manager.Controller.Patient.MPUpsertPatientInfo(patientToCreate, ProfilePublicId, "17B1EF7E"); //TODO: Ajustar el usuario no quemarlo

            patientToCreate.PatientPublicId = result;
            return patientToCreate;
        }

        #region  Funciones Privadas
        private PatientModel GetRequestForNewPatient()
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
                };
            return oReturn;
        }
        #endregion
    }
}