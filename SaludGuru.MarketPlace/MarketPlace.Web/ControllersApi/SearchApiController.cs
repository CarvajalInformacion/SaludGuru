using MarketPlace.Models.Appointment;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class SearchApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public void GetSearchUrl
            (string CityId, string SearchParam)
        {



        }
    }
}
