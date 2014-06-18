using BackOffice.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Web.Controllers
{
    public partial class BaseController : Controller
    {
        #region public static properties

        /// <summary>
        /// Current Area Name
        /// </summary>
        public static string AreaName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AreaName"].ToString().Trim();
            }
        }

        #endregion

        #region Session methods

        public void LogOut()
        {
            SessionController.SessionManager.Logout();
            Response.Redirect(Url.Action(MVC.Home.ActionNames.Index, MVC.Home.Name));
        }

        public void ChangeCurrentProfile(string ProfilePublicId)
        {
            SessionModel.UserAutorization.All(x =>
            {
                if (string.IsNullOrEmpty(ProfilePublicId) && x.Role ==
                    SessionController.Models.Profile.enumRole.SystemAdministrator)
                {
                    x.Selected = true;
                }
                else if (x.ProfilePublicId == ProfilePublicId)
                {
                    x.Selected = true;
                }
                else
                {
                    x.Selected = false;
                }

                return true;
            });
        }
        #endregion
    }
}