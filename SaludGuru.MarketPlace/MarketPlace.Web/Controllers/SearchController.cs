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

                if (oModel.IsQuery)
                {
                    //get query model
                }
                else
                {
                    //get category model

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
                            })));
                }
            }
            else
            {

            }


            //validate profile
            if (ViewModel.CurrentProfile != null && ViewModel.CurrentProfile.ProfilePublicId == ProfilePublicId)
            {
                //eval route to redirect
                if (ViewModel.IsRedirect)
                {
                    Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                            new
                            {
                                DoctorName = BaseController.RemoveAccent(ViewModel.CurrentProfile.Name + " " + ViewModel.CurrentProfile.LastName),
                                ProfilePublicId = ViewModel.CurrentProfile.ProfilePublicId,
                                SpecialtyName = BaseController.RemoveAccent(ViewModel.CurrentProfile.DefaultSpecialty.Name),
                            })));
                }

                //compare doctor names
                if (DoctorName != BaseController.RemoveAccent(ViewModel.CurrentProfile.Name + " " + ViewModel.CurrentProfile.LastName))
                {
                    Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                            new
                            {
                                DoctorName = BaseController.RemoveAccent(ViewModel.CurrentProfile.Name + " " + ViewModel.CurrentProfile.LastName),
                                ProfilePublicId = ViewModel.CurrentProfile.ProfilePublicId,
                                SpecialtyName = BaseController.RemoveAccent(ViewModel.CurrentProfile.DefaultSpecialty.Name),
                            })));
                }

                //validate specialty for canonical
                if (SpecialtyName != BaseController.RemoveAccent(ViewModel.CurrentProfile.DefaultSpecialty.Name))
                {
                    ViewModel.IsNoFollow = true;
                    ViewModel.IsNoIndex = true;
                    ViewModel.IsCanonical = true;
                }
            }
            else if (string.IsNullOrEmpty(ProfilePublicId))
            {
                //redirect to home no profile id
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Default)));
            }
            else
            {
                //eval redirect from old Profesional 
                SaludGuruProfile.Manager.Models.Profile.ProfileModel NewProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetProfilePublicIdFromOldId(ProfilePublicId);

                if (NewProfile != null && !string.IsNullOrEmpty(NewProfile.ProfilePublicId))
                {
                    Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                            new
                            {
                                DoctorName = BaseController.RemoveAccent(NewProfile.Name + " " + NewProfile.LastName),
                                ProfilePublicId = NewProfile.ProfilePublicId,
                                SpecialtyName = BaseController.RemoveAccent(NewProfile.DefaultSpecialty.Name),
                            })));
                }
                else
                {
                    Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound)));
                }
            }
        }

    }
}

