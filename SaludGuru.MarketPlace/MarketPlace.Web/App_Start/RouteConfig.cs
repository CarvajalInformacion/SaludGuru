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

            #region Standar controller acction

            routes.MapRoute(
                name: "Controller_Appointment",
                url: "Appointment/{action}",
                defaults: new
                {
                    controller = "Appointment",
                }
            ).DataTokens = rvdByArea;

            routes.MapRoute(
                name: "Controller_Error",
                url: "Error/{action}",
                defaults: new
                {
                    controller = "Error",
                }
            ).DataTokens = rvdByArea;

            routes.MapRoute(
                name: "Controller_Home",
                url: "Home/{action}",
                defaults: new
                {
                    controller = "Home",
                }
            ).DataTokens = rvdByArea;

            routes.MapRoute(
                name: "Controller_Profile",
                url: "Profile/{action}",
                defaults: new
                {
                    controller = "Profile",
                }
            ).DataTokens = rvdByArea;

            routes.MapRoute(
                name: "Controller_Search",
                url: "Search/{action}",
                defaults: new
                {
                    controller = "Search",
                }
            ).DataTokens = rvdByArea;

            routes.MapRoute(
                name: "Controller_UserProfile",
                url: "UserProfile/{action}",
                defaults: new
                {
                    controller = "UserProfile",
                }
            ).DataTokens = rvdByArea;

            routes.MapRoute(
                name: "Controller_Agenda",
                url: "Agenda/{action}",
                defaults: new
                {
                    controller = "Agenda",
                }
            ).DataTokens = rvdByArea;
            #endregion

            #region Home

            //home
            routes.MapRoute(
                            name: "Home",
                            url: "",
                            defaults: new
                            {
                                controller = "Home",
                                action = "Index"
                            }
                        ).DataTokens = rvdByArea;

            #endregion

            #region NotFound

            //contenido+no+encontrado
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
                    IsNoIndex = true,
                    IsCanonical = true,
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

            #region Search query

            //Redirect /Professional/Query?query={Query}
            routes.MapRoute(
               name: "R_SearchQuery_V1",
               url: "Professional/Query",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,
                   CityName = string.Empty,

                   IsRedirect = true,
                   IsQuery = true,
               }).DataTokens = rvdByArea;

            //Redirect doctores-{CityName}/{Query}
            routes.MapRoute(
               name: "R_SearchQuery_V2",
               url: "doctores/{Query}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,
                   CityName = string.Empty,

                   IsRedirect = true,
                   IsQuery = true,
               }).DataTokens = rvdByArea;

            //doctores
            routes.MapRoute(
               name: "SearchQuery_All",
               url: "doctores",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsRedirect = true,
                   IsQuery = true,
               }).DataTokens = rvdByArea;

            //doctores-{CityName}/{Query}
            routes.MapRoute(
               name: "SearchQuery_CityAll",
               url: "doctores-{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,

                   Query = string.Empty,

                   IsQuery = true,
               }).DataTokens = rvdByArea;

            //doctores-{CityName}/{Query}
            routes.MapRoute(
               name: "SearchQuery_Default",
               url: "doctores-{CityName}/{Query}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,

                   IsQuery = true,
               }).DataTokens = rvdByArea;

            #endregion

            #region Search Category

            #region Redirect V1

            //Redirect /todos/todos/todos
            routes.MapRoute(
               name: "R_SearchCategory_V1_1",
               url: "todos/todos/todos",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect {SpecialityName}/seguros+medicos-todos
            routes.MapRoute(
               name: "R_SearchCategory_V1_2",
               url: "{SpecialityName}/seguros+medicos-todos",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect {SpecialityName}/todos/todos
            routes.MapRoute(
               name: "R_SearchCategory_V1_3",
               url: "{SpecialityName}/todos/todos",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect todas-las-especialidades/salud/{ciudad}
            routes.MapRoute(
               name: "R_SearchCategory_V1_4",
               url: "todas-las-especialidades/salud/{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect todos/todos/{CityName}
            routes.MapRoute(
               name: "R_SearchCategory_V1_5",
               url: "todos/todos/{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect todas+las+especialidades/{InsuranceName}/todos
            routes.MapRoute(
               name: "R_SearchCategory_V1_6",
               url: "todas+las+especialidades/{InsuranceName}/todos",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect todos/{InsuranceName}/{CityName}
            routes.MapRoute(
               name: "R_SearchCategory_V1_7",
               url: "todos/{InsuranceName}/{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect {SpecialityName}/todos/{CityName}
            routes.MapRoute(
               name: "R_SearchCategory_V1_8",
               url: "{SpecialityName}/todos/{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect {SpecialityName}/{InsuranceName}/todos
            routes.MapRoute(
               name: "R_SearchCategory_V1_9",
               url: "{SpecialityName}/{InsuranceName}/todos",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   TreatmentName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            #endregion

            #region Redirect V2

            //Redirect todas+las+especialidades
            routes.MapRoute(
               name: "R_SearchCategory_V2_1",
               url: "todas+las+especialidades",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect todas+las+especialidades/salud canonical a todas+las+especialidades
            routes.MapRoute(
               name: "R_SearchCategory_V2_2",
               url: "todas+las+especialidades/salud",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect todas+las+especialidades/{InsuranceName}
            routes.MapRoute(
               name: "R_SearchCategory_V2_3",
               url: "todas+las+especialidades/{InsuranceName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect {SpecialityName}/{InsuranceName}
            routes.MapRoute(
               name: "R_SearchCategory_V2_4",
               url: "{SpecialityName}/{InsuranceName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   TreatmentName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            //Redirect {SpecialityName}
            routes.MapRoute(
               name: "R_SearchCategory_V2_5",
               url: "{SpecialityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,
                   CityName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
                   IsRedirect = true
               }).DataTokens = rvdByArea;

            #endregion

            //todas+las+especialidades/salud/{CityName}
            routes.MapRoute(
               name: "SearchCategory_City",
               url: "todas+las+especialidades/salud/{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
               }).DataTokens = rvdByArea;

            //todas+las+especialidades/{InsuranceName}/{CityName}
            routes.MapRoute(
               name: "SearchCategory_InsuranceCity",
               url: "todas+las+especialidades/{InsuranceName}/{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   TreatmentName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
               }).DataTokens = rvdByArea;

            //{SpecialityName}/seguros+medicos-{CityName}
            routes.MapRoute(
               name: "SearchCategory_SpecialtyCity",
               url: "{SpecialityName}/seguros+medicos-{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   TreatmentName = string.Empty,
                   InsuranceName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
               }).DataTokens = rvdByArea;

            //tratamientos+medicos/{TreatmentName}/{CityName}
            routes.MapRoute(
               name: "SearchCategory_TreatmentCity",
               url: "tratamientos+medicos/{TreatmentName}/{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,
                   InsuranceName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
               }).DataTokens = rvdByArea;

            //tratamientos+medicos/{TreatmentName}/{InsuranceName}/{CityName}
            routes.MapRoute(
               name: "SearchCategory_TreatmentInsuranceCity",
               url: "tratamientos+medicos/{TreatmentName}/{InsuranceName}/{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   SpecialtyName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
               }).DataTokens = rvdByArea;

            //{SpecialityName}/{InsuranceName}/{CityName}
            routes.MapRoute(
               name: "SearchCategory_SpecialtyInsuranceCity",
               url: "{SpecialityName}/{InsuranceName}/{CityName}",
               defaults: new
               {
                   controller = "Search",
                   action = "Index",

                   TreatmentName = string.Empty,

                   Query = string.Empty,

                   IsQuery = false,
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
