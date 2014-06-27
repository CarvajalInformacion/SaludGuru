using BackOffice.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class PatientController : BaseController
    {
        public virtual ActionResult List(string SearchParam)
        {
            return View();
        }
        public virtual ActionResult Upsert(string PatientPublicId)
        {
            return View();
        }
    }
}