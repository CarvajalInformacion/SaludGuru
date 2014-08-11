using MarketPlace.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class SearchController : BaseController
    {
        public virtual ActionResult Index
            (string SpecialtyName,
            string TreatmentName,
            string InsuranceName,
            string CityName,
            string Query)
        {
            try
            {

                //get basic model
                SearchViewModel oModel = new SearchViewModel()
                {
                    IsNoFollow = !ControllerContext.RouteData.Values.
                        Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow) ? false :
                            Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow]),
                    IsNoIndex = !ControllerContext.RouteData.Values.
                        Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex) ? false :
                            Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex]),
                    IsRedirect = !ControllerContext.RouteData.Values.
                        Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                            Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]),
                    IsCanonical = !ControllerContext.RouteData.Values.
                        Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsCanonical) ? false :
                            Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsCanonical]),
                    IsQuery = !ControllerContext.RouteData.Values.
                        Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsQuery) ? false :
                            Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsQuery]),
                };

                //eval redirect
                EvalRedirect
                    (oModel,
                    SpecialtyName,
                    TreatmentName,
                    InsuranceName,
                    CityName,
                    Query);

                //get page number
                oModel.CurrentPage = Convert.ToInt32(Request["PageNumber"]);
                if (oModel.CurrentPage < 0)
                    oModel.CurrentPage = 0;

                ////get profiles to show
                //if (oModel.IsQuery)
                //{
                //    oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileSearch
                //        (true,
                //        BaseController.EnabledCities.
                //            Where(x => BaseController.RemoveAccent(x.Value) == BaseController.RemoveAccent(CityName.Replace("+", " "))).
                //            Select(x => x.Key).
                //            DefaultIfEmpty(BaseController.DefaultCityId).
                //            FirstOrDefault(),
                //        Query.Replace("+", " "),
                //        null,
                //        null,
                //        null,
                //        oModel.CurrentRowCount,
                //        oModel.CurrentPage);
                //}
                //else
                //{


                //    //oModel.AutocompleteResults = SaludGuruProfile.Manager.Controller.Profile.MPProfileSearchAC(Query);
                //}

                return View(oModel);
            }
            catch
            {
                return RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound)));
            }
        }

        private void EvalRedirect
            (SearchViewModel ViewModel,
            string vSpecialtyName,
            string vTreatmentName,
            string vInsuranceName,
            string vCityName,
            string vQuery)
        {
            if (ViewModel.IsQuery)
            {
                if (ViewModel.IsRedirect)
                {
                    Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_SearchQuery_Default,
                            new
                            {
                                Query = vQuery,
                            })));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(vCityName))
                {
                    CallCategoryRedirect
                        (vSpecialtyName.Replace("+", " "),
                        vTreatmentName.Replace("+", " "),
                        vInsuranceName.Replace("+", " "),
                        BaseController.DefaultCityName);
                }
                else if (ViewModel.IsRedirect)
                {
                    CallCategoryRedirect
                        (vSpecialtyName.Replace("+", " "),
                        vTreatmentName.Replace("+", " "),
                        vInsuranceName.Replace("+", " "),
                        vCityName.Replace("+", " "));
                }
            }
        }

        private void CallCategoryRedirect
            (string vSpecialtyName,
            string vTreatmentName,
            string vInsuranceName,
            string vCityName)
        {
            if (string.IsNullOrEmpty(vSpecialtyName) &&
                string.IsNullOrEmpty(vTreatmentName) &&
                string.IsNullOrEmpty(vInsuranceName))
            {
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                        MarketPlace.Models.General.Constants.C_Route_SearchCategory_City,
                        new
                        {
                            CityName = BaseController.RemoveAccent(vCityName),
                        })));
            }
            else if (string.IsNullOrEmpty(vSpecialtyName) &&
                    string.IsNullOrEmpty(vTreatmentName) &&
                    !string.IsNullOrEmpty(vInsuranceName))
            {
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                        MarketPlace.Models.General.Constants.C_Route_SearchCategory_InsuranceCity,
                        new
                        {
                            CityName = BaseController.RemoveAccent(vCityName),
                            InsuranceName = BaseController.RemoveAccent(vInsuranceName),
                        })));
            }
            else if (!string.IsNullOrEmpty(vSpecialtyName) &&
                    string.IsNullOrEmpty(vTreatmentName) &&
                    string.IsNullOrEmpty(vInsuranceName))
            {
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                        MarketPlace.Models.General.Constants.C_Route_SearchCategory_SpecialtyCity,
                        new
                        {
                            CityName = BaseController.RemoveAccent(vCityName),
                            SpecialtyName = BaseController.RemoveAccent(vSpecialtyName),
                        })));
            }
            else if (!string.IsNullOrEmpty(vSpecialtyName) &&
                    string.IsNullOrEmpty(vTreatmentName) &&
                    !string.IsNullOrEmpty(vInsuranceName))
            {
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                        MarketPlace.Models.General.Constants.C_Route_SearchCategory_SpecialtyInsuranceCity,
                        new
                        {
                            CityName = BaseController.RemoveAccent(vCityName),
                            SpecialtyName = BaseController.RemoveAccent(vSpecialtyName),
                            InsuranceName = BaseController.RemoveAccent(vInsuranceName),
                        })));
            }
            else if (string.IsNullOrEmpty(vSpecialtyName) &&
                    !string.IsNullOrEmpty(vTreatmentName) &&
                    string.IsNullOrEmpty(vInsuranceName))
            {
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                        MarketPlace.Models.General.Constants.C_Route_SearchCategory_TreatmentCity,
                        new
                        {
                            CityName = BaseController.RemoveAccent(vCityName),
                            TreatmentName = BaseController.RemoveAccent(vTreatmentName),
                        })));
            }
            else if (string.IsNullOrEmpty(vSpecialtyName) &&
                    !string.IsNullOrEmpty(vTreatmentName) &&
                    !string.IsNullOrEmpty(vInsuranceName))
            {
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                        MarketPlace.Models.General.Constants.C_Route_SearchCategory_TreatmentInsuranceCity,
                        new
                        {
                            CityName = BaseController.RemoveAccent(vCityName),
                            TreatmentName = BaseController.RemoveAccent(vTreatmentName),
                            InsuranceName = BaseController.RemoveAccent(vInsuranceName),
                        })));
            }
        }
    }
}

