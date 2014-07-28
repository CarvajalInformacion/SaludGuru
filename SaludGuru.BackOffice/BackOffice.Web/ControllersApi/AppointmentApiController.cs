using BackOffice.Models.Appointment;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using SaludGuruProfile.Manager.Models.Office;
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
        public List<ScheduleEventModel> GetAppoinmentByOffice(string OfficePublicId, string StartDateTime, string EndDateTime)
        {
            OfficeModel oCurrentOffice = SaludGuruProfile.Manager.Controller.Office.OfficeGetFullAdmin(OfficePublicId);

            List<AppointmentModel> lstAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByOfficeId
                (OfficePublicId,
                DateTime.ParseExact(StartDateTime.Replace(" ", ""), "yyyy-M-dTH:m", System.Globalization.CultureInfo.InvariantCulture),
                DateTime.ParseExact(EndDateTime.Replace(" ", ""), "yyyy-M-dTH:m", System.Globalization.CultureInfo.InvariantCulture));

            if (lstAppointment != null)
                return lstAppointment.Select(x => new ScheduleEventModel(x, oCurrentOffice)).ToList();
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
                };

                //cancel appointment
                DoCancelAppointment(AppointmentToUpsert, SendNotifications);
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

        [HttpPost]
        [HttpGet]
        public string BlockAppointment(string BlockAppointment)
        {
            //get request object
            bool CancelAllAppointment = false, SendNotificationAllAppointment = false;
            AppointmentModel BlockAppmt = GetBlockAppointmentRequestModel(out CancelAllAppointment, out SendNotificationAllAppointment);

            //upsert block appointment
            string AppointmentPublicId = MedicalCalendar.Manager.Controller.Appointment.UpsertAppointmentInfo(BlockAppmt, new List<PatientModel>());

            if (CancelAllAppointment)
            {
                //get all appointments to masive cancel
                List<AppointmentModel> lstAppmtToCancel = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByOfficeId
                    (BlockAppmt.OfficePublicId, BlockAppmt.StartDate, BlockAppmt.EndDate);

                lstAppmtToCancel.
                    Where(x => x.Status != MedicalCalendar.Manager.Models.enumAppointmentStatus.Canceled &&
                            x.Status != MedicalCalendar.Manager.Models.enumAppointmentStatus.BlockCalendar).
                    All(AppmtToCancel =>
                {
                    DoCancelAppointment(AppmtToCancel, SendNotificationAllAppointment);

                    return true;
                });
            }

            //return block appointment public id
            return AppointmentPublicId;
        }

        [HttpPost]
        [HttpGet]
        public List<ScheduleEventMonthModel> GetAppoinmentByOfficeMonth(string OfficePublicId, string StartDate)
        {
            List<AppointmentMonthModel> lstAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetByOfficeIdMonth
                (OfficePublicId,
                DateTime.ParseExact(StartDate.Replace(" ", ""), "yyyy-M-d", System.Globalization.CultureInfo.InvariantCulture));

            if (lstAppointment != null)
                return lstAppointment.Select(x => new ScheduleEventMonthModel(x)).ToList();
            else
                return new List<ScheduleEventMonthModel>();
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
                            LargeValue = oCurrentProfile.RelatedOffice.
                                Where(x => x.OfficePublicId == oReturn.OfficePublicId).
                                Select(x => x.RelatedTreatment.Where(y => y.CategoryId == oTreatmentId).
                                    Select(y => y.TreatmentOfficeInfo.
                                        Where(z => z.OfficeCategoryInfoType == SaludGuruProfile.Manager.Models.enumOfficeCategoryInfoType.AfterCare).
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
                                Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.BeforeCare).
                                Select(x => x.AppointmentInfoId).
                                DefaultIfEmpty(0).
                                FirstOrDefault(),
                            AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.BeforeCare,
                            LargeValue = oCurrentProfile.RelatedOffice.
                                Where(x => x.OfficePublicId == oReturn.OfficePublicId).
                                Select(x => x.RelatedTreatment.Where(y => y.CategoryId == oTreatmentId).
                                    Select(y => y.TreatmentOfficeInfo.
                                        Where(z => z.OfficeCategoryInfoType == SaludGuruProfile.Manager.Models.enumOfficeCategoryInfoType.BeforeCare).
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

        private AppointmentModel GetBlockAppointmentRequestModel(out bool CancelAllAppointment, out bool SendNotificationAllAppointment)
        {
            CancelAllAppointment = false;
            SendNotificationAllAppointment = false;

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["BlockAppointment"])
                && bool.Parse(HttpContext.Current.Request["BlockAppointment"]))
            {
                //get send notifications
                SendNotificationAllAppointment = !string.IsNullOrEmpty(HttpContext.Current.Request["SendNotificationAllAppointment"]);

                //get cancel all appointment
                CancelAllAppointment = !string.IsNullOrEmpty(HttpContext.Current.Request["CancelAllAppointment"]);


                //get basic appointment
                AppointmentModel oReturn = new AppointmentModel()
                {
                    AppointmentPublicId = string.IsNullOrEmpty(HttpContext.Current.Request["BlockAppointmentPublicId"]) ? null :
                                HttpContext.Current.Request["BlockAppointmentPublicId"].ToString(),

                    OfficePublicId = HttpContext.Current.Request["BlockOfficePublicId"].ToString(),

                    Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.BlockCalendar,

                    StartDate = DateTime.ParseExact
                            (HttpContext.Current.Request["BlockDate"].Replace(" ", "") + "T" + HttpContext.Current.Request["BlockStartTime"].Replace(" ", ""),
                            "dd/MM/yyyyTh:mmtt",
                            System.Globalization.CultureInfo.InvariantCulture),

                    EndDate = DateTime.ParseExact
                            (HttpContext.Current.Request["BlockDate"].Replace(" ", "") + "T" + HttpContext.Current.Request["BlockEndTime"].Replace(" ", ""),
                            "dd/MM/yyyyTh:mmtt",
                            System.Globalization.CultureInfo.InvariantCulture),

                    AppointmentInfo = new List<AppointmentInfoModel>(),

                    RelatedPatient = new List<PatientModel>(),
                };

                return oReturn;
            }
            return null;
        }

        private void DoCancelAppointment(AppointmentModel AppointmentToUpsert, bool SendNotifications)
        {
            AppointmentToUpsert.Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.Canceled;

            //update appointment status
            MedicalCalendar.Manager.Controller.Appointment.UpdateAppointmentStatus(AppointmentToUpsert);

            if (SendNotifications)
            {
                //TODO: send message new patient PatientNew

                //TODO: send message removed patient
            }
        }

        #endregion
    }
}
