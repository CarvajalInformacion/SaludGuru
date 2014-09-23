using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketPlace.Web.Controllers.Filters
{
    public class MobileActionFilter : System.Web.Mvc.FilterAttribute, System.Web.Mvc.IActionFilter
    {
        public void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        {
            //validate session variable
            if (MarketPlace.Models.General.SessionModel.MobileSessionInfo == null)
            {
                MarketPlace.Models.General.SessionModel.MobileSessionInfo = new SessionController.Models.Mobile.MobileModel()
                {
                    IsMobileDevice = EvalMobile(),
                    ViewFullVersion = false,
                };
            }

            //eval redirect
            if (MarketPlace.Web.Controllers.BaseController.AreaName == MarketPlace.Models.General.Constants.C_WebAreaName &&
                MarketPlace.Models.General.SessionModel.MobileSessionInfo.IsMobileDevice &&
                !MarketPlace.Models.General.SessionModel.MobileSessionInfo.ViewFullVersion &&
                !(filterContext.RouteData.Values["controller"] == "Home" && filterContext.RouteData.Values["action"] == "ChangeMobileVersion"))
            {
                filterContext.HttpContext.Response.Redirect(filterContext.HttpContext.Request.Url.ToString().ToLower().Replace
                    (MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Url_MP_Desktop].Value,
                    MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Url_MP_Mobile].Value));
            }
        }

        public void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {

        }

        private bool EvalMobile()
        {
            HttpContext HttpCurrentContext = HttpContext.Current;

            if (HttpCurrentContext.Request.Browser.IsMobileDevice)
            {
                return true;
            }
            //TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            else if (HttpCurrentContext.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            //TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            else if (HttpCurrentContext.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                HttpCurrentContext.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            //FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            else if (HttpCurrentContext.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                string CurrentDevice = HttpCurrentContext.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Replace(" ", "");
                string EnabledDevices = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Mobile_Devices].Value;

                if (EnabledDevices.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Any(x => CurrentDevice.Contains(x.ToLower().Replace(" ", ""))))
                {
                    return true;
                }
            }

            return false;
        }
    }
}