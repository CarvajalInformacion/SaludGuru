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

        /// <summary>
        /// get special days for an specific office or profile for two months
        /// </summary>
        /// <param name="CountryId"></param>
        /// <param name="ProfilePublicId"></param>
        /// <param name="OfficePublicId"></param>
        /// <param name="Year"></param>
        /// <param name="Month"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        public List<MedicalCalendar.Manager.Models.General.SpecialDayModel> GetSpecialDayList
            (int CountryId, string ProfilePublicId, string OfficePublicId, string Date)
        {
            DateTime StartDate = DateTime.Parse(Date);

            return MedicalCalendar.Manager.Controller.Appointment.GetSpecialDays
                (CountryId,
                ProfilePublicId,
                OfficePublicId,
                StartDate,
                StartDate.AddMonths(1));
        }
    }
}
