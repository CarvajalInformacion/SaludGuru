using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaludGuruProfile.Manager.Models.Profile;

namespace MarketPlace.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Index()
        {
            ProfileModel Model = SaludGuruProfile.Manager.Controller.Profile.GetFeaturedProfile(1);

            return View(Model);
        }

        public virtual ActionResult ChangeCity(int NewCityId)
        {
            //get return url
            string oldCityName = MarketPlace.Web.Controllers.BaseController.EnabledCities[base.CurrentCookie.CurrentCity];

            string ReturnUrl = Request.UrlReferrer.ToString();

            if (MarketPlace.Web.Controllers.BaseController.EnabledCities.ContainsKey(NewCityId))
            {
                base.SetCookie(new MarketPlace.Models.General.CookieModel()
                {
                    CurrentCity = NewCityId,
                });

                ReturnUrl = ReturnUrl.Replace(oldCityName,
                    MarketPlace.Web.Controllers.BaseController.RemoveAccent(MarketPlace.Web.Controllers.BaseController.EnabledCities[NewCityId]));

                ReturnUrl = ReturnUrl.Replace(
                    MarketPlace.Web.Controllers.BaseController.RemoveAccent(oldCityName),
                    MarketPlace.Web.Controllers.BaseController.RemoveAccent(MarketPlace.Web.Controllers.BaseController.EnabledCities[NewCityId]));
            }

            return Redirect(ReturnUrl);
        }

        public virtual ActionResult LegalTerms()
        {
            return View();
        }

        public virtual ActionResult ConditionsAndRestrictions()
        {
            return View();
        }

        public virtual ActionResult FAQ()
        {
            return View();
        }

        public virtual ActionResult Contact()
        {
            return View();
        }

        public virtual ActionResult LogOutUser()
        {
            base.LogOut();

            //get return url
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}