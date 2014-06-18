using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Profile.Manager.Controller;
using BackOffice.Models.General;

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

                if (BackOffice.Models.General.SessionModel.UserIsAutorized)
                {
                    //set first profile
                    base.ChangeCurrentProfile(BackOffice.Models.General.SessionModel.UserAutorization.FirstOrDefault().ProfilePublicId);

                    //redirect to dashboard
                    return RedirectToAction(MVC.Home.ActionNames.Dashboard);
                }
                else
                {
                    //user is not autorized
                }
            }
            return View();
        }

        public virtual ActionResult Dashboard()
        {
            return View();
        }

        public virtual ActionResult ChangeAutorizationProfile(string ProfilePublicId)
        {
            base.ChangeCurrentProfile(ProfilePublicId);
            return RedirectToAction(MVC.Home.ActionNames.Dashboard, MVC.Home.Name);
        }

        public virtual ActionResult LogOutUser()
        {
            base.LogOut();
            return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
        }
    }
}