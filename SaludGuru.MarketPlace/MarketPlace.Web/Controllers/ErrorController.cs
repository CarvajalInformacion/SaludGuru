using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ErrorController : BaseController
    {
        public virtual ActionResult NotFound()
        {
            ViewBag.NoIndex = true;
            ViewBag.NoFollow = true;

            return View();
        }
    }
}