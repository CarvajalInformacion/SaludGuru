using MarketPlace.Models.General;
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
                //start seo model
                SEOModel oSeoModel = new SEOModel()
                {
                    Title = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Search_Title].Value,
                    Description = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Search_Description].Value,
                    Keywords = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Search_Keywords].Value,

                    IsNoFollow = !ControllerContext.RouteData.Values.
                        Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow) ? false :
                            Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow]),
                    IsNoIndex = !ControllerContext.RouteData.Values.
                        Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex) ? false :
                            Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex]),
                };


                //get basic model
                SearchViewModel oModel = new SearchViewModel()
                {
                    CurrentSearchSpecialty = SpecialtyName,
                    CurrentSearchTreatment = TreatmentName,
                    CurrentSearchInsurance = InsuranceName,
                    CurrentSearchQuery = Query,
                    CurrentSearchCity = CityName,

                    IsRedirect = !ControllerContext.RouteData.Values.
                        Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                            Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]),
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

                //get city
                oModel.CurrentCityId = BaseController.EnabledCities.
                            Where(x => BaseController.RemoveAccent(x.Value) == BaseController.RemoveAccent(oModel.CurrentSearchCity.Replace("+", " "))).
                            Select(x => x.Key).
                            DefaultIfEmpty(BaseController.DefaultCityId).
                            FirstOrDefault();

                if (base.CurrentCookie != null &&
                    oModel.CurrentCityId != base.CurrentCookie.CurrentCity)
                {
                    base.SetCookie(new MarketPlace.Models.General.CookieModel()
                    {
                        CurrentCity = oModel.CurrentCityId,
                    });
                }

                //eval redirect
                EvalRedirect
                    (oModel);

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
                        null,
                        null,
                        oModel.CurrentRowCount(BaseController.AreaName),
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
                        null,
                        null,
                        oModel.CurrentRowCount(BaseController.AreaName),
                        oModel.CurrentPage,
                        out oTotalRowsAux);
                    oModel.TotalRows = oTotalRowsAux;
                }

                //Seo model
                ReplaceSeoModel(oModel, oSeoModel);
                ViewBag.SeoModel = oSeoModel;

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

        private void ReplaceSeoModel
            (SearchViewModel ViewModel, SEOModel CurrentSeoModel)
        {
            if (ViewModel.IsQuery)
            {
                CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{SpecialtyName}", string.Empty);
                CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{TreatmentName}", string.Empty);
                CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{InsuranceName}", string.Empty);
                CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{CityName}", EnabledCities[ViewModel.CurrentCityId]);

                CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{SpecialtyName}", string.Empty);
                CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{TreatmentName}", string.Empty);
                CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{InsuranceName}", string.Empty);
                CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{CityName}", EnabledCities[ViewModel.CurrentCityId]);

                CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{SpecialtyName}", string.Empty);
                CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{TreatmentName}", string.Empty);
                CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{InsuranceName}", string.Empty);
                CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{CityName}", EnabledCities[ViewModel.CurrentCityId]);

                CurrentSeoModel.CityName = EnabledCities[ViewModel.CurrentCityId];
            }
            else
            {
                CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{SpecialtyName}", ViewModel.CurrentSpecialty != null ? ViewModel.CurrentSpecialty.Name : string.Empty);
                CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{TreatmentName}", ViewModel.CurrentTreatment != null ? ViewModel.CurrentTreatment.Name : string.Empty);
                CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{InsuranceName}", ViewModel.CurrentInsurance != null ? ViewModel.CurrentInsurance.Name : string.Empty);
                CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{CityName}", EnabledCities[ViewModel.CurrentCityId]);

                CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{SpecialtyName}", ViewModel.CurrentSpecialty != null ? ViewModel.CurrentSpecialty.Name : string.Empty);
                CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{TreatmentName}", ViewModel.CurrentTreatment != null ? ViewModel.CurrentTreatment.Name : string.Empty);
                CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{InsuranceName}", ViewModel.CurrentInsurance != null ? ViewModel.CurrentInsurance.Name : string.Empty);
                CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{CityName}", EnabledCities[ViewModel.CurrentCityId]);

                CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{SpecialtyName}", ViewModel.CurrentSpecialty != null ? ViewModel.CurrentSpecialty.Name : string.Empty);
                CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{TreatmentName}", ViewModel.CurrentTreatment != null ? ViewModel.CurrentTreatment.Name : string.Empty);
                CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{InsuranceName}", ViewModel.CurrentInsurance != null ? ViewModel.CurrentInsurance.Name : string.Empty);
                CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{CityName}", EnabledCities[ViewModel.CurrentCityId]);

                CurrentSeoModel.CityName = EnabledCities[ViewModel.CurrentCityId];

                if (ViewModel.CurrentSpecialty != null)
                {
                    CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.TrimEnd(',') + "," +
                        ViewModel.CurrentSpecialty.
                            SpecialtyInfo.
                            Where(x => x.CategoryInfoType == SaludGuruProfile.Manager.Models.enumCategoryInfoType.Keyword).
                            Select(x => x.LargeValue).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();
                }

                if (ViewModel.CurrentTreatment != null)
                {
                    CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.TrimEnd(',') + "," +
                        ViewModel.CurrentTreatment.
                            TreatmentInfo.
                            Where(x => x.CategoryInfoType == SaludGuruProfile.Manager.Models.enumCategoryInfoType.Keyword).
                            Select(x => x.LargeValue).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault();
                }
            }
        }


        #endregion
    }
}

