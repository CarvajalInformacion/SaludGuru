using BackOffice.Models.Appointment;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class AppointmentApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<ScheduleEventModel> GetAppoinmentSessionUser(string OfficePublicId, string StartDateTime, string EndDateTime)
        {
            List<AppointmentModel> lstAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByOfficeId
                (OfficePublicId,
                DateTime.ParseExact(StartDateTime.Replace(" ", ""), "yyyy-M-dTH:m", System.Globalization.CultureInfo.InvariantCulture),
                DateTime.ParseExact(EndDateTime.Replace(" ", ""), "yyyy-M-dTH:m", System.Globalization.CultureInfo.InvariantCulture));

            if (lstAppointment != null)
                return lstAppointment.Select(x => new ScheduleEventModel(x)).ToList();
            else
                return new List<ScheduleEventModel>();
        }

        [HttpPost]
        [HttpGet]
        public string UpsertAppointment(string UpsertAction)
        {
            List<PatientModel> PatientToRemove;

            AppointmentModel AppointmentToUpsert = GetAppointmentInfoRequestModel(out PatientToRemove);

            string AppointmentPublicId = MedicalCalendar.Manager.Controller.Appointment.UpsertAppointmentInfo(AppointmentToUpsert, PatientToRemove);

            return AppointmentPublicId;
        }

        #region Private Methods

        private AppointmentModel GetAppointmentInfoRequestModel(out List<PatientModel> PatientToRemove)
        {
            PatientToRemove = new List<PatientModel>();

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["UpsertAction"])
                && bool.Parse(HttpContext.Current.Request["UpsertAction"]))
            {
                AppointmentModel oReturn = new AppointmentModel()
                {
                    AppointmentPublicId = string.IsNullOrEmpty(HttpContext.Current.Request["AppointmentPublicId"]) ? null : HttpContext.Current.Request["AppointmentPublicId"].ToString(),
                    OfficePublicId = HttpContext.Current.Request["OfficePublicId"].ToString(),
                    Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.New,
                    StartDate = GetRequestDateTime(HttpContext.Current.Request["StartDate"], HttpContext.Current.Request["StartTime"]),
                    EndDate = GetRequestDateTime(HttpContext.Current.Request["StartDate"], HttpContext.Current.Request["StartTime"]).AddMinutes(int.Parse(HttpContext.Current.Request["Duration"])),

                    AppointmentInfo = new List<AppointmentInfoModel>() 
                    { 
                        //treatment
                        new AppointmentInfoModel()
                        {
                            AppointmentInfoId = string.IsNullOrEmpty(HttpContext.Current.Request["CatId_TreatmentId"])?0:int.Parse(HttpContext.Current.Request["CatId_TreatmentId"].ToString().Trim()),
                            AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category,
                            Value = HttpContext.Current.Request["TreatmentId"],
                        },
                        //After Care
                        new AppointmentInfoModel()
                        {
                            AppointmentInfoId = string.IsNullOrEmpty(HttpContext.Current.Request["CatId_AfterCare"])?0:int.Parse(HttpContext.Current.Request["CatId_AfterCare"].ToString().Trim()),
                            AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare,
                            LargeValue = HttpContext.Current.Request["AfterCare"],
                        },
                        new AppointmentInfoModel()
                        {
                            AppointmentInfoId = string.IsNullOrEmpty(HttpContext.Current.Request["CatId_BeforeCare"])?0:int.Parse(HttpContext.Current.Request["CatId_BeforeCare"].ToString().Trim()),
                            AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.BeforeCare,
                            LargeValue = HttpContext.Current.Request["BeforeCare"],
                        },
                    },

                    RelatedPatient = HttpContext.Current.Request["PatientAppointmentCreate"].Split(',').
                        Where(x => !string.IsNullOrEmpty(x) && x.Replace(" ", "").Length > 5).
                        Select(x => new PatientModel()
                        {
                            PatientPublicId = x.Replace(" ", "")
                        }).ToList(),
                };

                PatientToRemove = HttpContext.Current.Request["PatientAppointmentDelete"].Split(',').
                        Where(x => !string.IsNullOrEmpty(x) && x.Replace(" ", "").Length > 5).
                        Select(x => new PatientModel()
                        {
                            PatientPublicId = x.Replace(" ", "")
                        }).ToList();


                return oReturn;
            }
            return null;
        }

        private DateTime GetRequestDateTime(string Date, string Time)
        {
            DateTime oRetorno = DateTime.ParseExact
                (Date.Replace(" ", "") + "T" + Time.Replace(" ", ""),
                "dd/MM/yyyyTh:mmtt",
                System.Globalization.CultureInfo.InvariantCulture);

            return oRetorno;
        }

        #endregion
    }
}
