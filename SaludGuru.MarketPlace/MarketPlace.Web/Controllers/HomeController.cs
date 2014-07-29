using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Index()
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