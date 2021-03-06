﻿using System.Web;
using System.Web.Optimization;

namespace BackOffice.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            if (BackOffice.Web.Controllers.BaseController.AreaName == BackOffice.Web.Controllers.BaseController.AreaName)
            {
                #region JQery

                bundles.Add(new ScriptBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/bundles/jquery").Include(
                            "~/Areas/Web/Scripts/jquery-{version}.js",
                            "~/Areas/Web/Scripts/jquery-ui-{version}.js"));

                bundles.Add(new ScriptBundle("~/" + BackOffice.Models.General.Constants.C_WebAreaName + "/bundles/jqueryval").Include(
                            "~/Areas/Web/Scripts/jquery.validate*"));

                #endregion

                #region Kendo

                bundles.Add(new ScriptBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/bundles/kendo").Include(
                             "~/Areas/Web/Scripts/kendo/2014.1.318/kendo.web.min.js"));

                #endregion

                #region moment

                bundles.Add(new ScriptBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/bundles/moment").Include(
                        "~/Areas/Web/Scripts/moment/moment.js"));

                #endregion

                #region fullcalendar

                bundles.Add(new ScriptBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/bundles/fullcalendar").Include(
                        "~/Areas/Web/Scripts/fullcalendar/fullcalendar.js",
                        "~/Areas/Web/Scripts/fullcalendar/gcal.js"));

                #endregion

                #region ptTimeSelect

                bundles.Add(new ScriptBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/bundles/pttimeselect").Include(
                        "~/Areas/Web/Scripts/ptTimeSelect/jquery.ptTimeSelect.js"));

                #endregion

                #region Modernizr
                bundles.Add(new ScriptBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/bundles/modernizr").Include(
                            "~/Areas/Web/Scripts/modernizr-*"));
                #endregion

                #region Bootstrap
                bundles.Add(new ScriptBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/bundles/bootstrap").Include(
                          "~/Areas/Web/Scripts/bootstrap.js",
                          "~/Areas/Web/Scripts/respond.js"));
                #endregion

                #region SiteScripts
                bundles.Add(new ScriptBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/sitescripts").IncludeDirectory(
                          "~/Areas/Web/Scripts/Site",
                          "*.js",
                          true));
                #endregion

                #region Styles

                #region /web/styles

                bundles.Add(new StyleBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/content/css").IncludeDirectory(
                          "~/Areas/Web/Content/Styles",
                          "*.css",
                          true));

                #endregion

                #region jquery

                bundles.Add(new StyleBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/content/jquery/css").IncludeDirectory(
                          "~/Areas/Web/Content/jquery",
                          "*.css",
                          true));

                #endregion

                #region kendo

                bundles.Add(new StyleBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/content/kendo/css").Include(
                          "~/Areas/Web/Content/kendo/2014.1.318/kendo.common.min.css",
                          "~/Areas/Web/Content/kendo/2014.1.318/kendo.default.min.css"));

                #endregion

                #region ptTimeSelect

                bundles.Add(new StyleBundle("~/" + BackOffice.Web.Controllers.BaseController.AreaName + "/content/pttimeselect/css").Include(
                          "~/Areas/Web/Content/ptTimeSelect/jquery.ptTimeSelect.css"));

                #endregion

                #endregion
            }
            else if (BackOffice.Web.Controllers.BaseController.AreaName == BackOffice.Models.General.Constants.C_MobileAreaName)
            {
            }

            //allow bundles in debug mode
            bundles.IgnoreList.Clear();
        }
    }
}
