using System.Web;
using System.Web.Optimization;

namespace MarketPlace.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            if (MarketPlace.Web.Controllers.BaseController.AreaName == MarketPlace.Models.General.Constants.C_WebAreaName)
            {
                #region JQuery

                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/jquery").Include(
                            "~/Areas/Web/Scripts/jquery-{version}.js",
                            "~/Areas/Web/Scripts/jquery-ui-{version}.js"));

                bundles.Add(new ScriptBundle("~/" + MarketPlace.Models.General.Constants.C_WebAreaName + "/bundles/jqueryval").Include(
                            "~/Areas/Web/Scripts/jquery.validate*"));

                #endregion

                #region Kendo

                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/kendo").Include(
                             "~/Areas/Web/Scripts/kendo/2014.1.318/kendo.web.min.js"));
                #endregion

                #region jquery-ui-map

                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/jquerymap").Include(
                             "~/Areas/Web/Scripts/jquery-ui-map/jquery.ui.map.full.min.js"));

                #endregion

                #region Modernizr
                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/modernizr").Include(
                            "~/Areas/Web/Scripts/modernizr-*"));
                #endregion

                #region Bootstrap
                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/bootstrap").Include(
                          "~/Areas/Web/Scripts/bootstrap.js",
                          "~/Areas/Web/Scripts/respond.js"));
                #endregion

                #region PrettyPhoto
                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/prettyphoto").Include(
                          "~/Areas/Web/Scripts/PrettyPhoto/jquery.prettyPhoto.js"));
                #endregion

                #region SiteScripts
                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/sitescripts").IncludeDirectory(
                          "~/Areas/Web/Scripts/Site",
                          "*.js",
                          true));
                #endregion

                #region Styles

                #region /web/styles

                bundles.Add(new StyleBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/content/css").IncludeDirectory(
                          "~/Areas/Web/Content/Styles",
                          "*.css",
                          true));

                #endregion

                #region jquery

                bundles.Add(new StyleBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/content/jquery/css").IncludeDirectory(
                          "~/Areas/Web/Content/jquery",
                          "*.css",
                          true));

                #endregion

                #region kendo

                bundles.Add(new StyleBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/content/kendo/css").Include(
                          "~/Areas/Web/Content/kendo/2014.1.318/kendo.common.min.css",
                          "~/Areas/Web/Content/kendo/2014.1.318/kendo.default.min.css"));

                #endregion

                #region PrettyPhoto

                bundles.Add(new StyleBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/content/prettyphoto/css").Include(
                          "~/Areas/Web/Content/PrettyPhoto/prettyPhoto.css"));

                #endregion

                #endregion
            }
            else if (MarketPlace.Web.Controllers.BaseController.AreaName == MarketPlace.Models.General.Constants.C_MobileAreaName)
            {
                #region jquery-ui-map

                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/jquerymap").Include(
                             "~/Areas/Mobile/Scripts/jquery-ui-map/jquery.ui.map.full.min.js"));

                #endregion

                #region JQuery

                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/jquery").Include(
                            "~/Areas/Mobile/Scripts/jquery-{version}.js",
                            "~/Areas/Mobile/Scripts/jquery-ui-{version}.js"));

                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/jqueryval").Include(
                            "~/Areas/Mobile/Scripts/jquery.validate*"));

                #endregion

                #region JQuery Mobile

                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/jquerymobile").Include(
                           "~/Areas/Mobile/Scripts/jquery.mobile-{version}.js"));

                #endregion

                #region Modernizr
                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/modernizr").Include(
                            "~/Areas/Mobile/Scripts/modernizr-*"));
                #endregion

                #region Bootstrap
                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/bundles/bootstrap").Include(
                          "~/Areas/Mobile/Scripts/bootstrap.js",
                          "~/Areas/Mobile/Scripts/respond.js"));
                #endregion

                #region SiteScripts
                bundles.Add(new ScriptBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/sitescripts").IncludeDirectory(
                          "~/Areas/Mobile/Scripts/Site",
                          "*.js",
                          true));
                #endregion

                #region Styles

                #region /web/styles

                bundles.Add(new StyleBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/content/css").IncludeDirectory(
                          "~/Areas/Mobile/Content/Styles",
                          "*.css",
                          true));

                #endregion

                #region jquery mobile

                bundles.Add(new StyleBundle("~/" + MarketPlace.Web.Controllers.BaseController.AreaName + "/content/jquerymobile/css").IncludeDirectory(
                          "~/Areas/Mobile/Content/jquerymobile",
                          "*.css",
                          true));

                #endregion


                #endregion
            }

            //allow bundles in debug mode
            bundles.IgnoreList.Clear();
        }
    }
}
