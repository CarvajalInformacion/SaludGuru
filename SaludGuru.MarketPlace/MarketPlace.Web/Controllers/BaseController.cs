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
                        [MarketPlace.Models.General.Constants.C_Settings_City_Cities].
                        Value.
                        Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).
                        ToDictionary
                            (k => Convert.ToInt32(k.Split(',')[0]),
                            v => v.Split(',')[1].Trim());
                }
                return oEnabledCities;
            }
        }

        public static int DefaultCityId
        {
            get { return Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_City_Default].Value); }
        }

        public static string DefaultCityName
        {
            get { return EnabledCities[Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_City_Default].Value)]; }
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
            try
            {
                string strCookieKey = MarketPlace.Models.General.Constants.C_Cookie_CookieKey;
                string strCookieValue = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(CookieToUpdate);

                if (Request.Cookies.AllKeys.Any(x => x == strCookieKey))
                {
                    Request.Cookies.Remove(strCookieKey);
                }
                this.ControllerContext.HttpContext.Response.Cookies.Add(new HttpCookie(strCookieKey, strCookieValue));
            }
            catch { }
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

        #region Url Methods

        private static Dictionary<char, char> oReplaceChar;
        public static Dictionary<char, char> ReplaceChar
        {
            get
            {
                if (oReplaceChar == null)
                {
                    oReplaceChar = new Dictionary<char, char>();
                    MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Url_Invalid_Char].Value.
                        Split(';').All(ch =>
                        {
                            char okey = char.Parse(ch.Split(',')[0]);
                            char oval = char.Parse(ch.Split(',')[1].Replace("\\0", "\0"));
                            oReplaceChar[okey] = oval;
                            return true;
                        });
                }

                return oReplaceChar;
            }
        }

        public static string RemoveAccent(string strToEval)
        {
            string oReturn = strToEval.Trim().ToLower();

            ReplaceChar.All(rc =>
            {
                oReturn = oReturn.Replace(rc.Key, rc.Value);
                return true;
            });

            oReturn = string.Join("+", oReturn.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            return oReturn;
        }

        #endregion
    }
}