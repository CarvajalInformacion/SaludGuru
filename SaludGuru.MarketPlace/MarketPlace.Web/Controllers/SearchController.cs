using MarketPlace.Models.Profile;
using SaludGuruProfile.Manager.Interfaces;
using SaludGuruProfile.Manager.Models.Profile;
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
                    CurrentSearchSpecialty = SpecialtyName,
                    CurrentSearchTreatment = TreatmentName,
                    CurrentSearchInsurance = InsuranceName,
                    CurrentSearchQuery = Query,
                    CurrentSearchCity = CityName,

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
                        (string.IsNullOrEmpty(oModel.CurrentSearchInsurance) ? null : oModel.CurrentSearchInsurance.Replace("+", " "),
                        string.IsNullOrEmpty(oModel.CurrentSearchSpecialty) ? null : oModel.CurrentSearchSpecialty.Replace("+", " "),
                        string.IsNullOrEmpty(oModel.CurrentSearchTreatment) ? null : oModel.CurrentSearchTreatment.Replace("+", " "));
                }

                //eval redirect
                EvalRedirect
                    (oModel);

                //get city
                oModel.CurrentCityId = BaseController.EnabledCities.
                            Where(x => BaseController.RemoveAccent(x.Value) == BaseController.RemoveAccent(oModel.CurrentSearchCity.Replace("+", " "))).
                            Select(x => x.Key).
                            DefaultIfEmpty(BaseController.DefaultCityId).
                            FirstOrDefault();

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
                        oModel.CurrentCityId,
                        oModel.CurrentSearchQuery.Replace("+", " "),
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
                        oModel.CurrentCityId,
                        null,
                        !string.IsNullOrEmpty(oModel.CurrentSearchInsurance) && oModel.CurrentInsurance != null ? (int?)oModel.CurrentInsurance.CategoryId : null,
                        !string.IsNullOrEmpty(oModel.CurrentSearchSpecialty) && oModel.CurrentSpecialty != null ? (int?)oModel.CurrentSpecialty.CategoryId : null,
                        !string.IsNullOrEmpty(oModel.CurrentSearchTreatment) && oModel.CurrentTreatment != null ? (int?)oModel.CurrentTreatment.CategoryId : null,
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

        #region Private methods

        private void EvalRedirect
            (SearchViewModel ViewModel)
        {
            if (ViewModel.IsQuery)
            {
                if (ViewModel.IsRedirect)
                {
                    if (string.IsNullOrEmpty(ViewModel.CurrentSearchCity))
                    {
                        Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                                MarketPlace.Models.General.Constants.C_Route_SearchQuery_Default,
                                new
                                {
                                    Query = ViewModel.CurrentSearchQuery,
                                    CityName = BaseController.RemoveAccent(BaseController.DefaultCityName),
                                })), true);
                    }
                    else
                    {
                        Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                                MarketPlace.Models.General.Constants.C_Route_SearchQuery_Default,
                                new
                                {
                                    Query = ViewModel.CurrentSearchQuery,
                                    CityName = ViewModel.CurrentSearchCity,
                                })), true);
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ViewModel.CurrentSearchCity))
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
                        ViewModel.CurrentSearchCity.Replace("+", " "));
                }
                else if ((ViewModel.CurrentSpecialty != null &&
                            BaseController.RemoveAccent(ViewModel.CurrentSpecialty.Name) != ViewModel.CurrentSearchSpecialty) ||
                        (ViewModel.CurrentInsurance != null &&
                            BaseController.RemoveAccent(ViewModel.CurrentInsurance.Name) != ViewModel.CurrentSearchInsurance) ||
                        (ViewModel.CurrentTreatment != null &&
                            BaseController.RemoveAccent(ViewModel.CurrentTreatment.Name) != ViewModel.CurrentSearchTreatment))
                {
                    CallCategoryRedirect
                        (ViewModel.CurrentSpecialty != null ? ViewModel.CurrentSpecialty.Name : null,
                        ViewModel.CurrentTreatment != null ? ViewModel.CurrentTreatment.Name : null,
                        ViewModel.CurrentInsurance != null ? ViewModel.CurrentInsurance.Name : null,
                        ViewModel.CurrentSearchCity.Replace("+", " "));
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

        #endregion
    }
}

