using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        //Starup page project
        public virtual ActionResult Index()
        {
            //validate user loggin
            if (Auth.UserManager.LoginManager.UserIsLoggedIn)
            {
                //load autorization modules

                //validate if user is autorized

                //redirect to dashboard
            }
            return View();
        }

        public virtual ActionResult Dashboard()
        {
            return View();
        }
    }
}