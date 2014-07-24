using BackOffice.Models.General;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class AppointmentController : BaseController
    {
        public virtual ActionResult Day(string Date)
        {
            BackOffice.Models.Appointment.SchedulingModel oModel = new Models.Appointment.SchedulingModel();

            //get date
            DateTime dtAux = DateTime.Now;

            if (!string.IsNullOrEmpty(Date))
            {
                dtAux = DateTime.ParseExact(Date, "yyyy-M-d", System.Globalization.CultureInfo.InvariantCulture);
            }

            oModel.CurrentStartDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.Day, 0, 0, 0);
            oModel.CurrentEndDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.AddDays(1).Day, 0, 0, 0);

            //set current appointment type
            oModel.AppointmentType = enumAppointmentType.Day;

            //get schedule config
            oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Office.OfficeGetScheduleSettings(BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId);

            return View(oModel);
        }

        public virtual ActionResult Week(string Date)
        {
            BackOffice.Models.Appointment.SchedulingModel oModel = new Models.Appointment.SchedulingModel();

            //get date
            DateTime dtAux = DateTime.Now;

            if (!string.IsNullOrEmpty(Date))
            {
                dtAux = DateTime.ParseExact(Date, "yyyy-M-d", System.Globalization.CultureInfo.InvariantCulture);
            }

            //get start week date
            int intAuxDayAdd = dtAux.DayOfWeek - DayOfWeek.Monday;
            if (intAuxDayAdd < 0)
                intAuxDayAdd = intAuxDayAdd + 7;

            dtAux = dtAux.AddDays((-1) * intAuxDayAdd);

            oModel.CurrentStartDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.Day, 0, 0, 0);

            dtAux = dtAux.AddDays(7);

            oModel.CurrentEndDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.Day, 0, 0, 0);

            //set current appointment type
            oModel.AppointmentType = enumAppointmentType.Week;

            //get schedule config
            oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Office.OfficeGetScheduleSettings(BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId);

            return View(oModel);
        }

        public virtual ActionResult List(string Date)
        {
            BackOffice.Models.Appointment.SchedulingModel oModel = new Models.Appointment.SchedulingModel();

            //get date
            DateTime dtAux = DateTime.Now;

            if (!string.IsNullOrEmpty(Date))
            {
                dtAux = DateTime.ParseExact(Date, "yyyy-M-d", System.Globalization.CultureInfo.InvariantCulture);
            }

            oModel.CurrentStartDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.Day, 0, 0, 0);
            oModel.CurrentEndDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.AddDays(1).Day, 0, 0, 0);

            //set current appointment type
            oModel.AppointmentType = enumAppointmentType.List;

            //get schedule config
            oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Office.OfficeGetScheduleSettings(BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId);

            return View(oModel);
        }

        public virtual ActionResult Month(string Date)
        {
            BackOffice.Models.Appointment.SchedulingModel oModel = new Models.Appointment.SchedulingModel();

            //get date
            DateTime dtAux = DateTime.Now;

            if (!string.IsNullOrEmpty(Date))
            {
                dtAux = DateTime.ParseExact(Date, "yyyy-M-d", System.Globalization.CultureInfo.InvariantCulture);
            }

            oModel.CurrentStartDate = new DateTime(dtAux.Year, dtAux.Month, 1, 0, 0, 0);
            oModel.CurrentEndDate = new DateTime(dtAux.Year, dtAux.AddMonths(1).Month, 1, 0, 0, 0);

            //set current appointment type
            oModel.AppointmentType = enumAppointmentType.Month;

            //get schedule config
            oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Office.OfficeGetScheduleSettings(BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId);

            return View(oModel);
        }

        public virtual ActionResult Detail(string UpsertAction, string Date, string AppointmentPublicId, string ReturnUrl, string AppointmentPublicIdToDuplicate)
        {
            BackOffice.Models.Appointment.SchedulingModel oModel = new Models.Appointment.SchedulingModel();

            //get return url
            if (string.IsNullOrEmpty(ReturnUrl) && Request.UrlReferrer != null)
            {
                //get url from previus page send
                oModel.ReturnUrl = Request.UrlReferrer.ToString();
            }
            else
            {
                //get url from request
                oModel.ReturnUrl = ReturnUrl;
            }

            if (!string.IsNullOrEmpty(UpsertAction))
            {
                List<PatientModel> PatientToRemove;
                List<PatientModel> PatientNew;
                bool SendNotifications = false;

                if (UpsertAction.Replace(" ", "").ToLower() == "saveappointment")
                {
                    //get request info
                    AppointmentModel AppointmentToUpsert = GetUpsertAppointmentRequestModel(out SendNotifications, out PatientNew, out PatientToRemove);

                    //upsert appointment
                    string NewAppointmentPublicId = MedicalCalendar.Manager.Controller.Appointment.UpsertAppointmentInfo(AppointmentToUpsert, PatientToRemove);

                    if (SendNotifications)
                    {
                        //TODO: send message new patient PatientNew

                        //TODO: send message removed patient
                    }

                    return RedirectToAction(MVC.Appointment.ActionNames.Detail, MVC.Appointment.Name,
                        new { AppointmentPublicId = NewAppointmentPublicId, ReturnUrl = oModel.ReturnUrl });
                }
                else if (UpsertAction.Replace(" ", "").ToLower() == "cancelappointment")
                {
                    //cancel appointment
                }
                else if (UpsertAction.Replace(" ", "").ToLower() == "confirmappointment")
                {
                    //confirm appointment
                }
            }

            if (!string.IsNullOrEmpty(AppointmentPublicId))
            {
                //get appointment edit info
                oModel.CurrentAppointment = new Models.Appointment.ScheduleEventModel(MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById
                    (AppointmentPublicId.Replace(" ", "")));
            }
            else if (!string.IsNullOrEmpty(AppointmentPublicIdToDuplicate))
            {
                //get appointment duplicate info
                AppointmentModel Apmt = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById
                    (AppointmentPublicIdToDuplicate.Replace(" ", ""));

                Apmt.AppointmentPublicId = string.Empty;
                Apmt.Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.New;
                Apmt.AppointmentInfo.All(x =>
                {
                    x.AppointmentInfoId = 0;
                    return true;
                });

                oModel.CurrentAppointment = new Models.Appointment.ScheduleEventModel(Apmt);
            }

            //get date
            DateTime dtAux = DateTime.Now;

            if (!string.IsNullOrEmpty(Date))
            {
                dtAux = DateTime.ParseExact(Date, "yyyy-M-d", System.Globalization.CultureInfo.InvariantCulture);
            }

            else if (oModel.CurrentAppointment != null)
            {
                dtAux = oModel.CurrentAppointment.start;
            }

            oModel.CurrentStartDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.Day, 0, 0, 0);
            oModel.CurrentEndDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.AddDays(1).Day, 0, 0, 0);

            //set current appointment type
            oModel.AppointmentType = enumAppointmentType.Detail;

            //get schedule config
            oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Office.OfficeGetScheduleSettings
                (BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId);


            return View(oModel);
        }

        #region PrivateMethods

        private AppointmentModel GetUpsertAppointmentRequestModel
            (out bool SendNotifications,
            out List<PatientModel> PatientNew,
            out List<PatientModel> PatientToRemove)
        {
            PatientToRemove = new List<PatientModel>();
            PatientNew = new List<PatientModel>();
            SendNotifications = false;

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && Request["UpsertAction"].Replace(" ", "").ToLower() == "saveappointment")
            {
                //get appointment public id
                string oAppointmentPublicId = string.IsNullOrEmpty(Request["AppointmentPublicId"]) ? null : Request["AppointmentPublicId"].ToString();

                AppointmentModel oOriginalAppointment = null;
                if (!string.IsNullOrEmpty(oAppointmentPublicId))
                {
                    //get original appointment info
                    oOriginalAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById(oAppointmentPublicId);
                }

                //get send notifications
                SendNotifications = Convert.ToBoolean(Request["SendNotifications"].ToString());

                //get basic appointment
                AppointmentModel oReturn = new AppointmentModel()
                {
                    AppointmentPublicId = oAppointmentPublicId,
                    OfficePublicId = Request["OfficePublicId"].ToString(),

                    Status = oOriginalAppointment == null ?
                        MedicalCalendar.Manager.Models.enumAppointmentStatus.New :
                        oOriginalAppointment.Status,

                    StartDate = DateTime.ParseExact
                            (Request["StartDate"].Replace(" ", "") + "T" + Request["StartTime"].Replace(" ", ""),
                            "dd/MM/yyyyTh:mmtt",
                            System.Globalization.CultureInfo.InvariantCulture),

                    EndDate = DateTime.ParseExact
                            (Request["StartDate"].Replace(" ", "") + "T" + Request["StartTime"].Replace(" ", ""),
                            "dd/MM/yyyyTh:mmtt",
                            System.Globalization.CultureInfo.InvariantCulture).
                            AddMinutes(Convert.ToInt32(Request["Duration"])),

                    AppointmentInfo = new List<AppointmentInfoModel>(),

                    RelatedPatient = new List<PatientModel>(),
                };

                //get appointment info

                //treatment
                int oTreatmentId = Convert.ToInt32(Request["TreatmentId"].Replace(" ", ""));
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

                //after care
                oReturn.AppointmentInfo.Add
                    (new AppointmentInfoModel()
                    {
                        AppointmentInfoId = oOriginalAppointment == null ? 0 :
                            oOriginalAppointment.AppointmentInfo.
                            Where(x => x.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare).
                            Select(x => x.AppointmentInfoId).
                            DefaultIfEmpty(0).
                            FirstOrDefault(),
                        AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare,
                        LargeValue = Request["AfterCare"],
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
                        LargeValue = Request["BeforeCare"],
                    });


                //get patient to add
                oReturn.RelatedPatient = Request["PatientAppointmentCreate"].Split(',').
                    Where(x => !string.IsNullOrEmpty(x) && x.Replace(" ", "").Length == 8).
                    Select(x => new PatientModel()
                    {
                        PatientPublicId = x.Replace(" ", "")
                    }).ToList();

                //get patient to remove
                PatientToRemove = Request["PatientAppointmentDelete"].Split(',').
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