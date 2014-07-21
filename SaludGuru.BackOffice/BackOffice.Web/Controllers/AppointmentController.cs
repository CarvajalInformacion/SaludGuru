using BackOffice.Models.General;
using MedicalCalendar.Manager.Models.Appointment;
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

        public virtual ActionResult Detail(string Date, string AppointmentPublicId)
        {
            BackOffice.Models.Appointment.SchedulingModel oModel = new Models.Appointment.SchedulingModel();

            //get appointment info
            if (!string.IsNullOrEmpty(AppointmentPublicId))
            {
                oModel.CurrentAppointment = MedicalCalendar.Manager.Controller.Appointment.AppointmentGetById
                    (AppointmentPublicId.Replace(" ", ""));
            }

            //get date
            DateTime dtAux = DateTime.Now;

            if (!string.IsNullOrEmpty(Date))
            {
                dtAux = DateTime.ParseExact(Date, "yyyy-M-d", System.Globalization.CultureInfo.InvariantCulture);
            }

            else if (oModel.CurrentAppointment != null)
            {
                dtAux = oModel.CurrentAppointment.StartDate;
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
    }
}