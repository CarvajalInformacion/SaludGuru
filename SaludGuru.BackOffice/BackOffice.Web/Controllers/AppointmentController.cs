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
            if (!string.IsNullOrEmpty(Date))
            {
                DateTime dtAux = DateTime.ParseExact(Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                oModel.CurrentStartDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.Day, 0, 0, 0);
                oModel.CurrentEndDate = new DateTime(dtAux.Year, dtAux.Month, dtAux.Day, 23, 59, 59);
            }
            else
            {
                oModel.CurrentStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                oModel.CurrentEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            }

            //get schedule config
            oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Office.OfficeGetScheduleSettings(BackOffice.Models.General.SessionModel.CurrentUserAutorization.ProfilePublicId);

            return View(oModel);
        }

        public virtual ActionResult Week()
        {
            return View();
        }

        public virtual ActionResult Month()
        {
            return View();
        }

        public virtual ActionResult List()
        {
            return View();
        }
    }
}