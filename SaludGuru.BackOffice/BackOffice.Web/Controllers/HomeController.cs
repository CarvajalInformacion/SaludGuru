using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Profile.Manager.Controller;

namespace BackOffice.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        //Starup page project
        public virtual ActionResult Index()
        {
            //validate user loggin
            if (BackOffice.Models.General.SessionModel.UserIsLoggedIn)
            {
                //load roles and profiles
                BackOffice.Models.General.SessionModel.UserAutorization =
                    Autorization.GetEmailAutorization(BackOffice.Models.General.SessionModel.LoginUserEmail);

                //set first profile
                string FirstProfilePublicId = string.Empty;

                if (BackOffice.Models.General.SessionModel.UserAutorization != null &&
                    BackOffice.Models.General.SessionModel.UserAutorization.FirstOrDefault().RelatedProfile != null &&
                    BackOffice.Models.General.SessionModel.UserAutorization.FirstOrDefault().RelatedProfile.FirstOrDefault() != null)
                {
                    FirstProfilePublicId = BackOffice.Models.General.SessionModel.UserAutorization.
                        FirstOrDefault().RelatedProfile.FirstOrDefault().ProfilePublicId;
                }
                base.ChangeCurrentProfile(FirstProfilePublicId);


                //load autorization modules
                if (BackOffice.Models.General.SessionModel.UserIsAutorized)
                {
                    //redirect to dashboard
                    return RedirectToAction(MVC.Home.ActionNames.Dashboard);
                }
            }
            return View();
        }

        public virtual ActionResult Dashboard()
        {
            return View();
        }
    }
}