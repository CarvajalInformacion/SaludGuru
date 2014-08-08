using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class CalendarController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<MedicalCalendar.Manager.Models.General.SpecialDayModel> GetSpecialDayList
            (int CountryId, string ProfilePublicId)
        {
            return MedicalCalendar.Manager.Controller.Appointment.GetSpecialDays
                (CountryId,
                ProfilePublicId,
                DateTime.Now.Date);
        }
    }
}
