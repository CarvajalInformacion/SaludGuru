using MarketPlace.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class SearchController : BaseController
    {
        public virtual ActionResult Index
            (string SpecialityName,
            string TreatmentName,
            string InsuranceName,
            string CityName,
            string Query,
            string Page,
            string Order)
        {
            SearchViewModel oModel = new SearchViewModel();






            return View(oModel);
        }
    }
}

