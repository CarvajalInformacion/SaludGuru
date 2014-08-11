using MarketPlace.Models.Profile;
using SaludGuruProfile.Manager.Interfaces;
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

                //get current category info
                if (!oModel.IsQuery)
                {
                    oModel.CurrentCategory = SaludGuruProfile.Manager.Controller.Profile.MPCategoryGetAvailableCategory
                        (InsuranceName, SpecialtyName, TreatmentName);
                }

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

                int oTotalRowsAux;
                //get profiles to show
                if (oModel.IsQuery)
                {
                    oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileSearch
                        (true,
                        BaseController.EnabledCities.
                            Where(x => BaseController.RemoveAccent(x.Value) == BaseController.RemoveAccent(CityName.Replace("+", " "))).
                            Select(x => x.Key).
                            DefaultIfEmpty(BaseController.DefaultCityId).
                            FirstOrDefault(),
                        Query.Replace("+", " "),
                        null,
                        null,
                        null,
                        oModel.CurrentRowCount,
                        oModel.CurrentPage,
                        out oTotalRowsAux);
                    oModel.TotalRows = oTotalRowsAux;
                }
                else
                {
                    oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileSearch
                        (false,
                        BaseController.EnabledCities.
                            Where(x => BaseController.RemoveAccent(x.Value) == BaseController.RemoveAccent(CityName.Replace("+", " "))).
                            Select(x => x.Key).
                            DefaultIfEmpty(BaseController.DefaultCityId).
                            FirstOrDefault(),
                        null,
                        oModel.CurrentInsurance != null ? (int?)oModel.CurrentInsurance.CategoryId : null,
                        oModel.CurrentSpecialty != null ? (int?)oModel.CurrentSpecialty.CategoryId : null,
                        oModel.CurrentTreatment != null ? (int?)oModel.CurrentTreatment.CategoryId : null,
                        oModel.CurrentRowCount,
                        oModel.CurrentPage,
                        out oTotalRowsAux);
                    oModel.TotalRows = oTotalRowsAux;
                }

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
                            })), true);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(vCityName))
                {
                    CallCategoryRedirect
                        (ViewModel.CurrentSpecialty != null ? ViewModel.CurrentSpecialty.Name : null,
                        ViewModel.CurrentTreatment != null ? ViewModel.CurrentTreatment.Name : null,
                        ViewModel.CurrentInsurance != null ? ViewModel.CurrentInsurance.Name : null,
                        BaseController.DefaultCityName);
                }
                else if (ViewModel.IsRedirect)
                {
                    CallCategoryRedirect
                        (ViewModel.CurrentSpecialty != null ? ViewModel.CurrentSpecialty.Name : null,
                        ViewModel.CurrentTreatment != null ? ViewModel.CurrentTreatment.Name : null,
                        ViewModel.CurrentInsurance != null ? ViewModel.CurrentInsurance.Name : null,
                        vCityName.Replace("+", " "));
                }
                else if ((ViewModel.CurrentSpecialty != null &&
                            BaseController.RemoveAccent(ViewModel.CurrentSpecialty.Name) != vSpecialtyName) ||
                        (ViewModel.CurrentInsurance != null &&
                            BaseController.RemoveAccent(ViewModel.CurrentInsurance.Name) != vInsuranceName) ||
                        (ViewModel.CurrentTreatment != null &&
                            BaseController.RemoveAccent(ViewModel.CurrentTreatment.Name) != vTreatmentName))
                {
                    CallCategoryRedirect
                        (ViewModel.CurrentSpecialty != null ? ViewModel.CurrentSpecialty.Name : null,
                        ViewModel.CurrentTreatment != null ? ViewModel.CurrentTreatment.Name : null,
                        ViewModel.CurrentInsurance != null ? ViewModel.CurrentInsurance.Name : null,
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
                        })), true);
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
                        })), true);
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
                        })), true);
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
                        })), true);
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
                        })), true);
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
                        })), true);
            }
        }
    }
}

