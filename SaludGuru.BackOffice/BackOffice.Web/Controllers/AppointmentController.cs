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
        public virtual ActionResult Day()
        {
            return View();
        }

        public virtual ActionResult AppointmentList(string PublicProfileId)
        {
            return View();
        }
    }
}