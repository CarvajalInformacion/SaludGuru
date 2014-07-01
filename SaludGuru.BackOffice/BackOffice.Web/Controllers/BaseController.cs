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

        #region _Layout

        #region Header

        public virtual PartialViewResult _L_Header()
        {
            HeaderModel Model = new HeaderModel();

            //get autorized menus
            Model.Menu = GetMenuPermisions(SessionModel.CurrentUserAutorization.Role, null);

            return PartialView(Model);
        }

        private List<MenuModel> GetMenuPermisions
            (SessionController.Models.Profile.enumRole RoleToEval,
            BackOffice.Models.General.enumPrincipalMenu? SelectedMenu)
        {
            List<MenuModel> oRetorno = new List<MenuModel>();

            //add default dasahboard
            oRetorno.Add(new MenuModel()
            {
                PrincipalMenu = enumPrincipalMenu.Dashboard,
                EditPermision = enumEditPermision.Read,
                IsSelected = (SelectedMenu == null)
            });

            //get all permited modules
            BackOffice.Models.General.InternalSettings.Instance[
                BackOffice.Models.General.Constants.C_Settings_PrincipalMenu.
                Replace("{{RoleId}}", ((int)RoleToEval).ToString())].Value.
                Split(';').
                All(pm =>
                {
                    string[] pmDesc = pm.Split(',');
                    if (pmDesc.Length == 2)
                    {
                        enumPrincipalMenu CurrentPm = (enumPrincipalMenu)Enum.Parse(typeof(enumPrincipalMenu), pmDesc[0].Replace(" ", ""));
                        enumEditPermision CurrentEp = (enumEditPermision)Enum.Parse(typeof(enumEditPermision), pmDesc[1].Replace(" ", ""));
                        oRetorno.Add(new MenuModel()
                        {
                            PrincipalMenu = CurrentPm,
                            EditPermision = CurrentEp,
                            IsSelected = (SelectedMenu != null && SelectedMenu.Value == CurrentPm)
                        });
                    }
                    return true;
                });

            return oRetorno;
        }

        #endregion

        #endregion

    }
}