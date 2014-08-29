using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Http;
using System.Web.Routing;


namespace MarketPlace.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas(MarketPlace.Web.Controllers.BaseController.AreaName);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected static List<string> oUrlHttpExceptions;
        protected static List<string> UrlHttpExceptions
        {
            get
            {
                if (oUrlHttpExceptions == null)
                {
                    oUrlHttpExceptions = MarketPlace.Models.General.InternalSettings.Instance
                        [MarketPlace.Models.General.Constants.C_Settings_UrlHttpExceptions].Value.Split(',').ToList();
                }
                return oUrlHttpExceptions;
            }
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            bool InsecureUrl = UrlHttpExceptions.Any
                (x => x.ToLower() == Request.Url.AbsolutePath.ToLower());

            if (Context.Request.IsSecureConnection && InsecureUrl)
            {
                //not security zone
                Response.Redirect(Context.Request.Url.ToString().Replace("https:", "http:"), true);
            }
            else if (!Context.Request.IsSecureConnection && !InsecureUrl)
            {
                //ensure only https navigation
                Response.Redirect(Context.Request.Url.ToString().Replace("http:", "https:"), true);
            }
        }

        #region Enable web api session read

        private const string _WebApiPrefix = "api";
        private static string _WebApiExecutionPath = String.Format("~/{0}", _WebApiPrefix);

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
            }
        }
        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(_WebApiExecutionPath);
        }

        #endregion
    }
}
