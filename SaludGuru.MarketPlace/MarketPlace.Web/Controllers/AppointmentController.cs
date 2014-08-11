using MarketPlace.Models.Appointment;
using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
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
                PatientGroup = MedicalCalendar.Manager.Controller.Patient.MPPatientGetByUserPublicId("17B1EF7E"), //TODO: Ajustar el usuario no quemarlo
                CurrentOffice = OfficePublicId,
                StartDate = Date
            };
            return View(oModel);
        }

        public virtual ActionResult CreateAppointment()
        {

            return View();
        }

        #region Private Functions
        private AppointmentModel GetAppointmetRequest()
        {

            if (!string.IsNullOrEmpty(Request["UpsertAction"])
                && bool.Parse(Request["UpsertAction"]))
            {
                AppointmentModel oReturn = new AppointmentModel()
                {
                    OfficePublicId = Request["SelectedOffice"].ToString(),
                    Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.New,

                    StartDate = DateTime.ParseExact
                        (Request["StartDate"].Replace(" ", "") + "T" + Request["StartTime"].Replace(" ", ""),
                        "dd/MM/yyyyTh:mmtt",
                        System.Globalization.CultureInfo.InvariantCulture),


                };
                return oReturn;
            }
            return null;
        }
        #endregion
    }
}