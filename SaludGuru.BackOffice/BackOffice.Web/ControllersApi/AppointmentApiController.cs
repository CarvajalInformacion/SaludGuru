using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class AppointmentApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public string UpsertAppointment(string UpsertAction)
        {
            //(string selOffice,
            //string AppointmentPublicId,
            //string PatientAppointment,
            //string StartTime,
            //string StartDate,
            //string selTreatment,
            //string Duration,
            //string AfterCare,
            //string BeforeCare)

            //System.Web.HttpContext.Current.Request.Form

            return string.Empty;
        }
    }
}
