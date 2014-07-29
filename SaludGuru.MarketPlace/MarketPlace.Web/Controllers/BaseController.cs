using MarketPlace.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
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
                return System.Configuration.ConfigurationManager.AppSettings[MarketPlace.Models.General.Constants.C_AppSetting_AreaName].ToString().Trim();
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

        private static Dictionary<int, string> oEnabledCities;
        public static Dictionary<int, string> EnabledCities
        {
            get
            {
                if (oEnabledCities == null)
                {
                    oEnabledCities = MarketPlace.Models.General.InternalSettings.Instance
                        [MarketPlace.Models.General.Constants.C_Settings_Cities].
                        Value.
                        Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                        ToDictionary
                            (k => Convert.ToInt32(k.Split(',')[0]),
                            v => v.Split(',')[1].Trim());
                }
                return oEnabledCities;
            }
        }

        private CookieModel oCurrentCookie;
        public CookieModel CurrentCookie
        {
            get
            {
                if (oCurrentCookie == null)
                {
                    oCurrentCookie = GetCookie();
                }
                return oCurrentCookie;
            }
        }

        #endregion

        #region Cookie methods

        //save cookie over request
        public void SetCookie(CookieModel CookieToUpdate)
        {
            string strCookieKey = MarketPlace.Models.General.Constants.C_Cookie_CookieKey;
            string strCookieValue = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(CookieToUpdate);

            if (Request.Cookies.AllKeys.Any(x => x == strCookieKey))
            {
                Request.Cookies.Remove(strCookieKey);
            }
            this.ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie(strCookieKey, strCookieValue));
        }

        public CookieModel GetCookie()
        {
            CookieModel oReturn;

            string strCookieKey = MarketPlace.Models.General.Constants.C_Cookie_CookieKey;

            if (!Request.Cookies.AllKeys.Any(x => x == strCookieKey))
            {
                oReturn = new CookieModel()
                {
                    CurrentCity = EnabledCities.FirstOrDefault().Key,
                };

                SetCookie(oReturn);
            }
            else
            {
                oReturn = (CookieModel)(new System.Web.Script.Serialization.JavaScriptSerializer()).
                                    Deserialize(Request.Cookies[strCookieKey].Value, typeof(CookieModel));
            }

            return oReturn;
        }

        #endregion

        #region Session methods

        public void LogOut()
        {
            SessionController.SessionManager.Logout();
            Response.Redirect(Url.Action(MVC.Home.ActionNames.Index, MVC.Home.Name));
        }

        #endregion
    }
}