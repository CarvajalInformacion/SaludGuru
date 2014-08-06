using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MarketPlace.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            #region Route value dictionary by area
            RouteValueDictionary rvdByArea = new RouteValueDictionary();
            rvdByArea.Add("UseNamespaceFallback", false);
            rvdByArea.Add("area", MarketPlace.Web.Controllers.BaseController.AreaName);
            rvdByArea.Add("Namespaces", new string[] { "MarketPlace.Web.Areas." + MarketPlace.Web.Controllers.BaseController.AreaName + ".*" });
            #endregion

            //ignore route
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region NotFound

            //Redirect doctor/{DoctorName}/{SpecialtyName}/{ProfilePublicId}
            routes.MapRoute(
                name: "Error_NotFound",
                url: "contenido+no+encontrado",
                defaults: new
                {
                    controller = "Error",
                    action = "NotFound",

                    IsNoFollow = true,
                    IsNoIndex = true
                }).DataTokens = rvdByArea;


            #endregion

            #region Profile

            //Redirect doctor/{DoctorName}/{SpecialtyName}/{ProfilePublicId}
            routes.MapRoute(
                name: "R_Profile_V1",
               url: "doctor/{DoctorName}/{SpecialtyName}/{ProfilePublicId}",
               defaults: new
               {
                   controller = "Profile",
                   action = "Index",

                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect doctor/ redirect 301 home
            routes.MapRoute(
                name: "R_Profile_Home",
                url: "doctor",
                defaults: new
                {
                    controller = "Profile",
                    action = "Index",

                    DoctorName = string.Empty,
                    ProfilePublicId = string.Empty,
                    SpecialtyName = string.Empty,
                }).DataTokens = rvdByArea;

            //doctor/{DoctorName}-{ProfilePublicId}
            routes.MapRoute(
                name: "Profile_NoFollow",
                url: "doctor/{DoctorName}-{ProfilePublicId}",
                defaults: new
                {
                    controller = "Profile",
                    action = "Index",

                    SpecialtyName = string.Empty,

                    IsNoFollow = true,
                    IsNoIndex = true
                }).DataTokens = rvdByArea;

            //doctor/{DoctorName}-{DoctorId}/{Specialty}
            routes.MapRoute(
                name: "Profile_Default",
                url: "doctor/{DoctorName}-{ProfilePublicId}/{SpecialtyName}",
                defaults: new
                {
                    controller = "Profile",
                    action = "Index",
                }).DataTokens = rvdByArea;
            #endregion

            //default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                }
            ).DataTokens = rvdByArea;
        }
    }
}
