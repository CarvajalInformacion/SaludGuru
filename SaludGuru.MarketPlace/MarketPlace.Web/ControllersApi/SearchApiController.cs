using MarketPlace.Models.Appointment;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.General;
using SaludGuruProfile.Manager.Models.Office;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class SearchApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<AutocompleteModel> AutocompleteSearch
            (string IsAc, string CityId, string SearchParam)
        {
            return SaludGuruProfile.Manager.Controller.Profile.MPProfileSearchAC
                (Convert.ToInt32(CityId.Trim()), SearchParam);
        }

        [HttpPost]
        [HttpGet]
        public string GetSearchUrl
            (string IsGetUrl, string CityId, string SearchParam)
        {
            //get form data

            //string Id = HttpContext.Current.Request["Id"];
            //string ProfileType = HttpContext.Current.Request["ProfileType"];
            //string CategoryType = HttpContext.Current.Request["CategoryType"];
            //string MatchQuery = HttpContext.Current.Request["MatchQuery"];
            //string IsQuery = HttpContext.Current.Request["IsQuery"];

            //if (!string.IsNullOrEmpty(Id) &&
            //    !string.IsNullOrEmpty(ProfileType) &&
            //    !string.IsNullOrEmpty(CategoryType) &&
            //    !string.IsNullOrEmpty(MatchQuery) &&
            //    !string.IsNullOrEmpty(IsQuery))
            //{

            //}
            //else
            //{
            //    List<AutocompleteModel> tmpResults = SaludGuruProfile.Manager.Controller.Profile.MPProfileSearchAC
            //        (Convert.ToInt32(CityId.Trim()), SearchParam);
            //}
            AutocompleteModel oModel = SaludGuruProfile.Manager.Controller.Profile.MPProfileSearchAC
                    (Convert.ToInt32(CityId.Trim()), SearchParam).FirstOrDefault();

            string oReturn = GetUrlFromAcModel(oModel, CityId);

            return oReturn;

        }

        #region PrivateMethods

        private string GetUrlFromAcModel(AutocompleteModel AcModel, string CityId)
        {
            string oReturn = string.Empty;

            if (AcModel == null || Convert.ToInt32(CityId) <= 0)
            {
                oReturn = HttpContext.Current.Server.UrlDecode(Url.Route(MarketPlace.Models.General.Constants.C_Route_Error_NotFound, null));
            }
            else if (AcModel.IsQuery)
            {
                oReturn = HttpContext.Current.Server.UrlDecode(Url.Route(
                       MarketPlace.Models.General.Constants.C_Route_SearchQuery_Default,
                       new
                       {
                           CityName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(
                                MarketPlace.Web.Controllers.BaseController.
                                    EnabledCities[Convert.ToInt32(CityId.Trim())]),
                           Query = MarketPlace.Web.Controllers.BaseController.RemoveAccent(AcModel.MatchQuery)
                       }));
            }
            else
            {
                switch (AcModel.CategoryType.Value)
                {
                    case SaludGuruProfile.Manager.Models.enumCategoryType.Insurance:

                        oReturn = HttpContext.Current.Server.UrlDecode(Url.Route(
                            MarketPlace.Models.General.Constants.C_Route_SearchCategory_InsuranceCity,
                            new
                            {
                                CityName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(
                                    MarketPlace.Web.Controllers.BaseController.
                                        EnabledCities[Convert.ToInt32(CityId.Trim())]),
                                InsuranceName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(AcModel.OriginalTerm)
                            }));

                        break;

                    case SaludGuruProfile.Manager.Models.enumCategoryType.Specialty:

                        oReturn = HttpContext.Current.Server.UrlDecode(Url.Route(
                            MarketPlace.Models.General.Constants.C_Route_SearchCategory_SpecialtyCity,
                            new
                            {
                                CityName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(
                                    MarketPlace.Web.Controllers.BaseController.
                                        EnabledCities[Convert.ToInt32(CityId.Trim())]),
                                SpecialtyName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(AcModel.OriginalTerm)
                            }));

                        break;

                    case SaludGuruProfile.Manager.Models.enumCategoryType.Treatment:

                        oReturn = HttpContext.Current.Server.UrlDecode(Url.Route(
                            MarketPlace.Models.General.Constants.C_Route_SearchCategory_TreatmentCity,
                            new
                            {
                                CityName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(
                                    MarketPlace.Web.Controllers.BaseController.
                                        EnabledCities[Convert.ToInt32(CityId.Trim())]),
                                SpecialtyName = MarketPlace.Web.Controllers.BaseController.RemoveAccent(AcModel.OriginalTerm)
                            }));

                        break;
                }
            }

            return oReturn;

        }

        #endregion
    }
}
