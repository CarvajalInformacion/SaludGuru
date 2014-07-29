using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class UserProfileController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult FamilyGroup()
        {
            return View();
        }

        public virtual ActionResult ProfileList()
        {
            return View();
        }

        public virtual ActionResult Treatment()
        {
            return View();
        }

        public virtual ActionResult Notification()
        {
            return View();
        }
    }
}