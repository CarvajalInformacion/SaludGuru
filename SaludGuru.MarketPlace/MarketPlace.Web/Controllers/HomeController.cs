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

        public virtual ActionResult LogOutUser()
        {
            base.LogOut();

            //get return url
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}