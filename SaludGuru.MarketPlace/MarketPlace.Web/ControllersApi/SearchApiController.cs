using MarketPlace.Models.Appointment;
using MarketPlace.Models.Profile;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class SearchApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<AutocompleteViewModel> AutocompleteSearch
            (string IsAc, string CityId, string SearchParam)
        {
            try
            {
                List<AutocompleteModel> AcResult = SaludGuruProfile.Manager.Controller.Profile.MPProfileSearchAC
                    (Convert.ToInt32(CityId.Trim()), SearchParam);

                if (AcResult == null)
                    AcResult = new List<AutocompleteModel>();

                List<AutocompleteViewModel> oReturn = new List<AutocompleteViewModel>();

                AcResult.All(x =>
                {
                    oReturn.Add(new AutocompleteViewModel(x));
                    return true;
                });

                return oReturn;
            }
            catch
            {
                return new List<AutocompleteViewModel>();
            }
        }
    }
}
