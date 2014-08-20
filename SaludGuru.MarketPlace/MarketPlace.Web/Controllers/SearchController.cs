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

                //get city
                oModel.CurrentCityId = BaseController.EnabledCities.
                            Where(x => BaseController.RemoveAccent(x.Value) == BaseController.RemoveAccent(CityName.Replace("+", " "))).
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
                    oModel.CurrentQuery = Query.Replace("+", " ");

                    oModel.CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileSearch
                        (true,
                        oModel.CurrentCityId,
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
                        oModel.CurrentCityId,
                        null,
                        !string.IsNullOrEmpty(InsuranceName) && oModel.CurrentInsurance != null ? (int?)oModel.CurrentInsurance.CategoryId : null,
                        !string.IsNullOrEmpty(SpecialtyName) && oModel.CurrentSpecialty != null ? (int?)oModel.CurrentSpecialty.CategoryId : null,
                        !string.IsNullOrEmpty(TreatmentName) && oModel.CurrentTreatment != null ? (int?)oModel.CurrentTreatment.CategoryId : null,
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

        public virtual JsonResult GetSearchUrl
            (string IsGetUrl, string CityId, string SearchParam)
        {
            string oReturn = string.Empty;
            try
            {
                var oModelAux = SaludGuruProfile.Manager.Controller.Profile.MPProfileSearchAC
                        (Convert.ToInt32(CityId.Trim()), SearchParam);

                AutocompleteModel oModel;
                if (oModelAux != null && oModelAux.Count > 0)
                {
                    oModel = oModelAux.FirstOrDefault();
                }
                else
                {
                    oModel = new AutocompleteModel()
                    {
                        IsQuery = true,
                        MatchQuery = SearchParam
                    };
                }

                if (oModel.IsQuery)
                {
                    oModel.MatchQuery = SearchParam;
                }

                oReturn = GetUrlFromAcModel(oModel, CityId);
            }
            catch
            {
                oReturn = Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound));
            }

            return Json(new { Url = oReturn });
        }

        #region Private methods

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
                    if (string.IsNullOrEmpty(vCityName))
                    {
                        Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                                MarketPlace.Models.General.Constants.C_Route_SearchQuery_Default,
                                new
                                {
                                    Query = vQuery,
                                    CityName = BaseController.RemoveAccent(BaseController.DefaultCityName),
                                })), true);
                    }
                    else
                    {
                        Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                                MarketPlace.Models.General.Constants.C_Route_SearchQuery_Default,
                                new
                                {
                                    Query = vQuery,
                                    CityName = vCityName,
                                })), true);
                    }
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

        private string GetUrlFromAcModel(AutocompleteModel AcModel, string CityId)
        {
            string oReturn = string.Empty;

            if (AcModel == null || Convert.ToInt32(CityId) <= 0)
            {
                oReturn = Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound, null);
            }
            else if (AcModel.IsQuery)
            {
                oReturn = Url.RouteUrl(
                    MarketPlace.Models.General.Constants.C_Route_SearchQuery_Default,
                    new
                    {
                        Query = MarketPlace.Web.Controllers.BaseController.RemoveAccent(AcModel.MatchQuery),
                        CityName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(
                                    MarketPlace.Web.Controllers.BaseController.
                                        EnabledCities[Convert.ToInt32(CityId.Trim())]),
                    });
            }
            else
            {
                switch (AcModel.CategoryType.Value)
                {
                    case SaludGuruProfile.Manager.Models.enumCategoryType.Insurance:

                        oReturn = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_SearchCategory_InsuranceCity,
                            new
                            {
                                CityName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(
                                    MarketPlace.Web.Controllers.BaseController.
                                        EnabledCities[Convert.ToInt32(CityId.Trim())]),
                                InsuranceName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(AcModel.OriginalTerm)
                            });

                        break;

                    case SaludGuruProfile.Manager.Models.enumCategoryType.Specialty:

                        oReturn = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_SearchCategory_SpecialtyCity,
                            new
                            {
                                CityName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(
                                    MarketPlace.Web.Controllers.BaseController.
                                        EnabledCities[Convert.ToInt32(CityId.Trim())]),
                                SpecialtyName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(AcModel.OriginalTerm)
                            });

                        break;

                    case SaludGuruProfile.Manager.Models.enumCategoryType.Treatment:

                        oReturn = Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_SearchCategory_TreatmentCity,
                            new
                            {
                                CityName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(
                                    MarketPlace.Web.Controllers.BaseController.
                                        EnabledCities[Convert.ToInt32(CityId.Trim())]),
                                SpecialtyName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(AcModel.OriginalTerm)
                            });

                        break;
                    default:
                        oReturn = Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound, null);
                        break;
                }
            }

            return Server.UrlDecode(oReturn);

        }

        #endregion
    }
}

