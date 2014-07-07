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
        public List<ScheduleEventModel> GetAppoinmentLoginUser(string OfficePublicId, string StartDateTime, string EndDateTime)
        {
            List<AppointmentModel> lstAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByOfficeId
                (OfficePublicId,
                DateTime.ParseExact(StartDateTime.Replace(" ", ""), "yyyy-M-dTH:m", System.Globalization.CultureInfo.InvariantCulture),
                DateTime.ParseExact(EndDateTime.Replace(" ", ""), "yyyy-M-dTH:m", System.Globalization.CultureInfo.InvariantCulture));

            List<ScheduleEventModel> oReturn = MapAppointment(lstAppointment);

            return oReturn;
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

        private List<ScheduleEventModel> MapAppointment(List<AppointmentModel> lstAppointment)
        {
            List<ScheduleEventModel> oReturn = new List<ScheduleEventModel>();

            if (lstAppointment != null && lstAppointment.Count > 0)
            {

                lstAppointment.All(apmt =>
                {
                    //get appointment title
                    string AppointmentTitle = BackOffice.Models.General.InternalSettings.Instance
                        [BackOffice.Models.General.Constants.C_Settings_AppointmentTitleTemplate.Replace("{{StatusId}}", ((int)apmt.Status).ToString())].Value;

                    if (apmt.RelatedPatient == null || apmt.RelatedPatient.Count == 0)
                    {
                        //no patients over appointment
                        AppointmentTitle = AppointmentTitle.
                            Replace("{AppointmentName}", "Esta cita no tiene pacientes.").
                            Replace("{AppointmentImage}",
                            BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_AppointmentImageEmpty].Value);
                    }
                    else if (apmt.RelatedPatient.Count == 1)
                    {
                        //one patient over appointment
                        AppointmentTitle = AppointmentTitle.
                            Replace("{AppointmentName}", apmt.RelatedPatient.Select(x => x.Name + " " + x.LastName).FirstOrDefault()).
                            Replace("{AppointmentImage}",
                                apmt.RelatedPatient.
                                    Select(x => x.PatientInfo.
                                        Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.ProfileImage).
                                        Select(y => !string.IsNullOrEmpty(y.Value) ? y.Value :
                                                        BackOffice.Models.General.InternalSettings.Instance
                                                            [BackOffice.Models.General.Constants.C_Settings_AppointmentImageEmpty].Value).
                                        DefaultIfEmpty(BackOffice.Models.General.InternalSettings.Instance
                                                            [BackOffice.Models.General.Constants.C_Settings_AppointmentImageEmpty].Value).
                                        FirstOrDefault()).FirstOrDefault());
                    }
                    else if (apmt.RelatedPatient.Count > 1)
                    {
                        //more than one patient
                        AppointmentTitle = AppointmentTitle.
                            Replace("{AppointmentName}", "Esta cita tiene varios pacientes asociados.").
                            Replace("{AppointmentImage}",
                            BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_AppointmentImageGroup].Value);
                    }

                    oReturn.Add(new ScheduleEventModel()
                    {
                        id = apmt.AppointmentPublicId,
                        start = apmt.StartDate,
                        end = apmt.EndDate,
                        title = AppointmentTitle,
                        className = "AppointmentStatus_" + ((int)apmt.Status).ToString(),
                        RelatedAppointment = apmt,
                    });

                    return true;
                });
            }

            return oReturn;
        }

        #endregion
    }
}
