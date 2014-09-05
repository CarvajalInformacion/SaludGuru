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
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Home)));
            }

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
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_LegalTerms)));
            }

            return View();
        }

        public virtual ActionResult ConditionsAndRestrictions()
        {
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_ConditionsAndRestrictions)));
            }

            return View();
        }

        public virtual ActionResult FAQ()
        {
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_FAQ)));
            }

            return View();
        }

        public virtual ActionResult Contact()
        {
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Contact)));
            }

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