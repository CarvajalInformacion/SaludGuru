using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class SpecialtyController : BaseController
    {
        public virtual ActionResult Search()
        {
            return View();
        }
    }
}