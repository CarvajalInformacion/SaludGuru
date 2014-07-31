using MarketPlace.Models.Appointment;
using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class AppointmentController : BaseController
    {
        public virtual ActionResult Index(string ProfilePublicId, string OfficePublicId, string Date)
        {
            AppointmentViewModel oModel = new AppointmentViewModel()
            {
                CurrentDate = string.IsNullOrEmpty(Date) ? DateTime.Now : DateTime.ParseExact(Date.Replace(" ", ""), "yyyy-M-dTH:m", System.Globalization.CultureInfo.InvariantCulture),
                CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId),
                PatientGroup = MedicalCalendar.Manager.Controller.Patient.MPPatientGetByUserPublicId(MarketPlace.Models.General.SessionModel.CurrentLoginUser.UserPublicId)
            };

            if (!string.IsNullOrEmpty(Request["UserPublicId"])
                && bool.Parse(Request["UserPublicId"]))
            {
                this.GetRequestForNewPatient(ProfilePublicId, OfficePublicId, Date);
            }

            return View(oModel);
        }

        private PatientModel GetRequestForNewPatient(string ProfilePublicId, string OfficePublicId, string Date)
        {
            return null;
        }
    }
}