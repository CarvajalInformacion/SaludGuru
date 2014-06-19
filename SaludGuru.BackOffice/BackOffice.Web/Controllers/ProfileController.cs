using BackOffice.Models.Profile;
using SaludGuruProfile.Manager.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BackOffice.Web.Controllers
{
    public partial class ProfileController : BaseController
    {
        public virtual ActionResult Search()
        {
            return View();
        }

        public virtual ActionResult Create()
        {
            ProfileUpSertModel Model = new ProfileUpSertModel()
            {
                ProfileOptions = SaludGuruProfile.Manager.Controller.Profile.GetProfileOptions(),
                Profile = null,
            };


            return View(Model);
        }
    }
}