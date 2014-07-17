using BackOffice.Models.Appointment;
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
            List<PatientModel> PatientNew;
            bool SendNotifications = false;

            //get request info
            AppointmentModel AppointmentToUpsert = GetUpsertAppointmentRequestModel(out SendNotifications, out PatientNew, out PatientToRemove);

            //upsert appointment
            string AppointmentPublicId = MedicalCalendar.Manager.Controller.Appointment.UpsertAppointmentInfo(AppointmentToUpsert, PatientToRemove);

            if (SendNotifications)
            {
                //TODO: send message new patient PatientNew

                //TODO: send message removed patient
            }

            //return appointment id
            return AppointmentPublicId;
        }

        [HttpPost]
        [HttpGet]
        public void CancelAppointmentStatus(string CancelAppointment)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["CancelAppointment"])
                && bool.Parse(HttpContext.Current.Request["CancelAppointment"]))
            {
                //get request info
                bool SendNotifications = false;

                //get send notifications
                SendNotifications = Convert.ToBoolean(HttpContext.Current.Request["SendNotifications"].ToString());

                AppointmentModel AppointmentToUpsert = new AppointmentModel()
                {
                    AppointmentPublicId = HttpContext.Current.Request["AppointmentPublicId"].ToString(),
                    Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.Canceled,
                };

                //update appointment status
                MedicalCalendar.Manager.Controller.Appointment.UpdateAppointmentStatus(AppointmentToUpsert);

                if (SendNotifications)
                {
                    //TODO: send message new patient PatientNew

                    //TODO: send message removed patient
                }
            }
        }

        [HttpPost]
        [HttpGet]
        public void ConfirmAttendanceAppointmentStatus(string ConfirmAttendance)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["ConfirmAttendance"])
                && bool.Parse(HttpContext.Current.Request["ConfirmAttendance"]))
            {
                //get request info

                //get new status
                MedicalCalendar.Manager.Models.enumAppointmentStatus oStatus =
                    (MedicalCalendar.Manager.Models.enumAppointmentStatus)Convert.ToInt32(HttpContext.Current.Request["NewStatus"]);

                //get send reminded appointment
                bool SendRemindedFuture = !string.IsNullOrEmpty(HttpContext.Current.Request["SendRemindedFuture"]);

                DateTime RemindedDate;
                if (oStatus == MedicalCalendar.Manager.Models.enumAppointmentStatus.Attendance &&
                    SendRemindedFuture)
                {
                    //get time to send reminded appointment
                    RemindedDate = DateTime.ParseExact(HttpContext.Current.Request["RemindedDate"].Replace(" ", ""),
                                    "dd/MM/yyyy",
                                    System.Globalization.CultureInfo.InvariantCulture);
                }
                AppointmentModel AppointmentToUpsert = new AppointmentModel()
                {
                    AppointmentPublicId = HttpContext.Current.Request["R_AppointmentPublicId"].ToString(),
                    Status = oStatus,
                };

                //update appointment status
                MedicalCalendar.Manager.Controller.Appointment.UpdateAppointmentStatus(AppointmentToUpsert);

                if (oStatus == MedicalCalendar.Manager.Models.enumAppointmentStatus.Attendance &&
                    SendRemindedFuture)
                {
                    //TODO: program remember mesaje RemindedDate
                }
            }
        }

        #region Private Methods

        private AppointmentModel GetUpsertAppointmentRequestModel
            (out bool SendNotifications,
            out List<PatientModel> PatientNew,
            out List<PatientModel> PatientToRemove)
        {
            PatientToRemove = new List<PatientModel>();
            PatientNew = new List<PatientModel>();
            SendNotifications = false;

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["UpsertAction"])
                && bool.Parse(HttpContext.Current.Request["UpsertAction"]))
            {
                //get office defaults info
                ProfileModel oCurrentProfile = SaludGuruProfile.Manager.Controller.Office.OfficeGetScheduleSettings
                     (BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId);

                //get appointment public id
                string oAppointmentPublicId = string.IsNullOrEmpty(HttpContext.Current.Request["AppointmentPublicId"]) ? null :
                                HttpContext.Current.Request["AppointmentPublicId"].ToString();

                AppointmentModel oOriginalAppointment = null;
                if (!string.IsNullOrEmpty(oAppointmentPublicId))
                {
                    //get original appointment info
                    oOriginalAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById(oAppointmentPublicId);
                }

                //get send notifications
                SendNotifications = Convert.ToBoolean(HttpContext.Current.Request["SendNotifications"].ToString());

                //get basic appointment
                AppointmentModel oReturn = new AppointmentModel()
                {
                    AppointmentPublicId = oAppointmentPublicId,
                    OfficePublicId = HttpContext.Current.Request["OfficePublicId"].ToString(),

                    Status = oOriginalAppointment == null ?
                        MedicalCalendar.Manager.Models.enumAppointmentStatus.New :
                        oOriginalAppointment.Status,

                    StartDate = DateTime.ParseExact
                            (HttpContext.Current.Request["StartDate"].Replace(" ", "") + "T" + HttpContext.Current.Request["StartTime"].Replace(" ", ""),
                            "dd/MM/yyyyTh:mmtt",
                            System.Globalization.CultureInfo.InvariantCulture),

                    EndDate = DateTime.ParseExact
                            (HttpContext.Current.Request["StartDate"].Replace(" ", "") + "T" + HttpContext.Current.Request["StartTime"].Replace(" ", ""),
                            "dd/MM/yyyyTh:mmtt",
                            System.Globalization.CultureInfo.InvariantCulture).
                            AddMinutes(Convert.ToInt32(HttpContext.Current.Request["Duration"])),

                    AppointmentInfo = new List<AppointmentInfoModel>(),

                    RelatedPatient = new List<PatientModel>(),
                };

                //get appointment info

                //treatment
                int oTreatmentId = Convert.ToInt32(HttpContext.Current.Request["TreatmentId"].Replace(" ", ""));
                oReturn.AppointmentInfo.Add
                    (new AppointmentInfoModel()
                    {
                        AppointmentInfoId = oOriginalAppointment == null ? 0 :
                            oOriginalAppointment.AppointmentInfo.
                            Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category).
                            Select(x => x.AppointmentInfoId).
                            DefaultIfEmpty(0).
                            FirstOrDefault(),

                        AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category,

                        Value = oTreatmentId.ToString(),
                    });

                if (oOriginalAppointment == null ||
                    (oOriginalAppointment != null &&
                        oOriginalAppointment.AppointmentInfo.
                        Any(x => x.Value.Replace(" ", "") != oTreatmentId.ToString() &&
                            x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category)))
                {
                    //after care
                    oReturn.AppointmentInfo.Add
                        (new AppointmentInfoModel()
                        {
                            AppointmentInfoId = oOriginalAppointment == null ? 0 : oOriginalAppointment.AppointmentInfo.
                                Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare).
                                Select(x => x.AppointmentInfoId).
                                DefaultIfEmpty(0).
                                FirstOrDefault(),
                            AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare,
                            Value = oCurrentProfile.RelatedOffice.
                                Where(x => x.OfficePublicId == oReturn.OfficePublicId).
                                Select(x => x.RelatedTreatment.Where(y => y.CategoryId == oTreatmentId).
                                    Select(y => y.TreatmentOfficeInfo.Where(z => z.OfficeCategoryInfoType == SaludGuruProfile.Manager.Models.enumOfficeCategoryInfoType.AfterCare).
                                        Select(z => z.LargeValue).
                                        FirstOrDefault()).
                                    FirstOrDefault()).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault(),
                        });

                    //before care
                    oReturn.AppointmentInfo.Add
                        (new AppointmentInfoModel()
                        {
                            AppointmentInfoId = oOriginalAppointment == null ? 0 : oOriginalAppointment.AppointmentInfo.
                                Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare).
                                Select(x => x.AppointmentInfoId).
                                DefaultIfEmpty(0).
                                FirstOrDefault(),
                            AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.BeforeCare,
                            Value = oCurrentProfile.RelatedOffice.
                                Where(x => x.OfficePublicId == oReturn.OfficePublicId).
                                Select(x => x.RelatedTreatment.Where(y => y.CategoryId == oTreatmentId).
                                    Select(y => y.TreatmentOfficeInfo.Where(z => z.OfficeCategoryInfoType == SaludGuruProfile.Manager.Models.enumOfficeCategoryInfoType.BeforeCare).
                                        Select(z => z.LargeValue).
                                        FirstOrDefault()).
                                    FirstOrDefault()).
                                DefaultIfEmpty(string.Empty).
                                FirstOrDefault(),
                        });
                }

                //get patient to add
                oReturn.RelatedPatient = HttpContext.Current.Request["PatientAppointmentCreate"].Split(',').
                    Where(x => !string.IsNullOrEmpty(x) && x.Replace(" ", "").Length == 8).
                    Select(x => new PatientModel()
                    {
                        PatientPublicId = x.Replace(" ", "")
                    }).ToList();

                //get patient to remove
                PatientToRemove = HttpContext.Current.Request["PatientAppointmentDelete"].Split(',').
                        Where(x => !string.IsNullOrEmpty(x) && x.Replace(" ", "").Length == 8).
                        Select(x => new PatientModel()
                        {
                            PatientPublicId = x.Replace(" ", "")
                        }).ToList();

                if (oOriginalAppointment != null)
                {
                    //get new patient
                    PatientNew = oReturn.RelatedPatient.
                        Where(x => !oOriginalAppointment.RelatedPatient.Any(y => y.PatientPublicId == x.PatientPublicId)).ToList();
                }

                return oReturn;
            }
            return null;
        }

        #endregion
    }
}
