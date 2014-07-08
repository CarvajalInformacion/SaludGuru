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

        public static string CurrentControllerName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }

        public static string CurrentActionName
        {
            get
            {
                return System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
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

        #region Menu Methods

        public static List<MenuPrincipalModel> GetPrincipalMenu()
        {
            List<MenuPrincipalModel> oRetorno = new List<MenuPrincipalModel>();

            enumMenuPrincipal CurrentSelected = SelectedPrincipalMenu();

            //add default dasahboard
            oRetorno.Add(new MenuPrincipalModel()
            {
                Menu = enumMenuPrincipal.Dashboard,
                EditPermision = enumEditPermision.Read,
                IsSelected = (enumMenuPrincipal.Dashboard == CurrentSelected),
                RelatedMenu = GetSecundaryMenu(enumMenuPrincipal.Dashboard, enumEditPermision.Read),
            });

            //get all permited modules
            BackOffice.Models.General.InternalSettings.Instance[
                BackOffice.Models.General.Constants.C_Settings_PrincipalMenu.
                Replace("{{RoleId}}", ((int)SessionModel.CurrentUserAutorization.Role).ToString())].Value.
                Split(';').
                All(pm =>
                {
                    string[] pmDesc = pm.Replace(" ", "").Split(',');
                    if (pmDesc.Length == 2)
                    {
                        enumMenuPrincipal oCurrent = (enumMenuPrincipal)Enum.Parse(typeof(enumMenuPrincipal), pmDesc[0]);
                        enumEditPermision oPermision = (enumEditPermision)Enum.Parse(typeof(enumEditPermision), pmDesc[1]);

                        oRetorno.Add(new MenuPrincipalModel()
                        {
                            Menu = oCurrent,
                            EditPermision = oPermision,
                            IsSelected = (CurrentSelected == oCurrent),
                            RelatedMenu = GetSecundaryMenu(oCurrent, oPermision),
                        });
                    }
                    return true;
                });

            return oRetorno;
        }

        public static List<MenuProfileModel> GetProfileMenu()
        {
            List<MenuProfileModel> oRetorno = new List<MenuProfileModel>();

            enumMenuProfile CurrentSelected = SelectedProfileMenu();

            enumEditPermision CurrentPermision = GetPrincipalMenu().Where(x => x.IsSelected == true).Select(x => x.EditPermision).DefaultIfEmpty(enumEditPermision.Read).FirstOrDefault();

            foreach (enumMenuProfile CurrentMenu in (enumMenuProfile[])Enum.GetValues(typeof(enumMenuProfile)))
            {
                oRetorno.Add(new MenuProfileModel()
                {
                    Menu = CurrentMenu,
                    EditPermision = CurrentPermision,
                    IsSelected = (CurrentMenu == CurrentSelected),
                });
            }

            return oRetorno;
        }

        public static List<MenuOfficeModel> GetOfficeMenu()
        {
            List<MenuOfficeModel> oRetorno = new List<MenuOfficeModel>();
            return oRetorno;
        }

        private static List<MenuSecundaryModel> GetSecundaryMenu(enumMenuPrincipal MenuToEval, enumEditPermision EditPermisions)
        {
            List<MenuSecundaryModel> oReturn = new List<MenuSecundaryModel>();

            enumMenuSecundary CurrentSelected = SelectedSecundaryMenu();

            BackOffice.Models.General.InternalSettings.Instance[
                BackOffice.Models.General.Constants.C_Settings_SecundaryMenu.
                Replace("{{MenuPrincipal}}", MenuToEval.ToString())].Value.
                Split(',').
                Where(x => !string.IsNullOrEmpty(x)).
                All(pm =>
                {
                    enumMenuSecundary oCurrent = (enumMenuSecundary)Enum.Parse(typeof(enumMenuSecundary), pm.Replace(" ", ""));

                    oReturn.Add(new MenuSecundaryModel()
                    {
                        Menu = oCurrent,
                        EditPermision = EditPermisions,
                        IsSelected = (CurrentSelected == oCurrent)
                    });

                    return true;
                });
            return oReturn;
        }

        private static enumMenuPrincipal SelectedPrincipalMenu()
        {
            enumMenuPrincipal oReturn = enumMenuPrincipal.Dashboard;

            if (MVC.Home.ActionNames.Dashboard == CurrentActionName &&
                MVC.Home.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Dashboard;
            }
            else if (MVC.Insurance.Name == CurrentControllerName ||
                        (MVC.Profile.Name == CurrentControllerName &&
                        MVC.Profile.ActionNames.ProfileSearch == CurrentActionName) ||
                    MVC.Specialty.Name == CurrentControllerName ||
                    MVC.Treatment.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Administrator;
            }
            else if (MVC.Profile.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Profile;
            }
            else if (MVC.Appointment.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Appointment;
            }
            else if (MVC.Patient.Name == CurrentControllerName)
            {
                oReturn = enumMenuPrincipal.Patient;
            }

            return oReturn;
        }

        private static enumMenuSecundary SelectedSecundaryMenu()
        {
            enumMenuSecundary oReturn = enumMenuSecundary.None;

            if (MVC.Insurance.Name == CurrentControllerName)
            {
                oReturn = enumMenuSecundary.Ad_Insurance;
            }
            else if (MVC.Profile.Name == CurrentControllerName)
            {
                oReturn = enumMenuSecundary.Ad_Profile;
            }
            else if (MVC.Specialty.Name == CurrentControllerName)
            {
                oReturn = enumMenuSecundary.Ad_Specialty;
            }
            else if (MVC.Treatment.Name == CurrentControllerName)
            {
                oReturn = enumMenuSecundary.Ad_Treatment;
            }


            else if (MVC.Appointment.Name == CurrentControllerName &&
                    MVC.Appointment.ActionNames.Day == CurrentActionName)
            {
                oReturn = enumMenuSecundary.Ap_Day;
            }
            else if (MVC.Appointment.Name == CurrentControllerName &&
                    MVC.Appointment.ActionNames.Week == CurrentActionName)
            {
                oReturn = enumMenuSecundary.Ap_Week;
            }
            else if (MVC.Appointment.Name == CurrentControllerName &&
                    MVC.Appointment.ActionNames.Month == CurrentActionName)
            {
                oReturn = enumMenuSecundary.Ap_Month;
            }
            else if (MVC.Appointment.Name == CurrentControllerName &&
                    MVC.Appointment.ActionNames.List == CurrentActionName)
            {
                oReturn = enumMenuSecundary.Ap_List;
            }

            return oReturn;
        }

        private static enumMenuProfile SelectedProfileMenu()
        {
            enumMenuProfile oReturn = enumMenuProfile.ProfileInfo;

            if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.ProfileEdit == CurrentActionName)
            {
                oReturn = enumMenuProfile.ProfileInfo;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.ProfileEditImage == CurrentActionName)
            {
                oReturn = enumMenuProfile.ProfileImages;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.AutorizationProfileList == CurrentActionName)
            {
                oReturn = enumMenuProfile.Autorization;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                (MVC.Profile.ActionNames.OfficeList == CurrentActionName ||
                MVC.Profile.ActionNames.OfficeUpsert == CurrentActionName ||
                MVC.Profile.ActionNames.OfficeTreatmentList == CurrentActionName ||
                MVC.Profile.ActionNames.OfficeTreatmentUpsert == CurrentActionName ||
                MVC.Profile.ActionNames.OfficeScheduleAvailableList == CurrentActionName))
            {
                oReturn = enumMenuProfile.Office;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.SpecialtyProfileList == CurrentActionName)
            {
                oReturn = enumMenuProfile.Specialty;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.InsuranceProfileList == CurrentActionName)
            {
                oReturn = enumMenuProfile.Insurance;
            }
            else if (MVC.Profile.Name == CurrentControllerName &&
                MVC.Profile.ActionNames.TreatmentProfileList == CurrentActionName)
            {
                oReturn = enumMenuProfile.Treatment;
            }
            return oReturn;
        }

        #endregion
    }
}