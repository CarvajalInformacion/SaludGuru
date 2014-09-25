using MarketPlace.Models.General;
using MarketPlace.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class ProfileController : BaseController
    {
        public virtual ActionResult Index
            (string DoctorName,
            string ProfilePublicId,
            string SpecialtyName)
        {
            //get model
            ProfileViewModel oModel = new ProfileViewModel()
            {
                CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId),

                IsRedirect = !ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]),
            };

            if (oModel.CurrentProfile != null && oModel.CurrentProfile.RelatedOffice == null)
                oModel.CurrentProfile.RelatedOffice = new List<SaludGuruProfile.Manager.Models.Office.OfficeModel>();

            //Seo model
            SEOModel oSeoModel = new SEOModel()
            {
                Title = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Profile_Title.Replace("{AreaName}", MarketPlace.Web.Controllers.BaseController.AreaName)].Value,
                Description = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Profile_Description.Replace("{AreaName}", MarketPlace.Web.Controllers.BaseController.AreaName)].Value,
                Keywords = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Profile_Keywords.Replace("{AreaName}", MarketPlace.Web.Controllers.BaseController.AreaName)].Value,

                IsNoFollow = !ControllerContext.RouteData.Values.
                                        Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow) ? false :
                                            Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow]),
                IsNoIndex = !ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex]),
            };

            //get is canonical
            bool IsCanonical = !ControllerContext.RouteData.Values.
                Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsCanonical) ? false :
                    Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsCanonical]);

            //eval redirect
            EvalRedirect(oModel, DoctorName, ProfilePublicId, SpecialtyName, oSeoModel, IsCanonical);


            ReplaceSeoModel(oModel, oSeoModel);
            ViewBag.SeoModel = oSeoModel;

            //render profile
            return View(oModel);
        }

        public virtual ActionResult FBProfile(string ProfilePublicId, string LoginRequired)
        {
            try
            {
                bool oLoginRequired = !string.IsNullOrEmpty(LoginRequired) && LoginRequired == "true" ? true : false;

                if (oLoginRequired && !MarketPlace.Models.General.SessionModel.UserIsLoggedIn)
                {
                    return Redirect(MarketPlace.Models.General.InternalSettings.Instance
                             [MarketPlace.Models.General.Constants.C_Settings_Login_FBUrl.Replace("{{AreaName}}", MarketPlace.Web.Controllers.BaseController.AreaName)]
                             .Value.Replace("{{UrlRetorno}}", Request.Url.ToString()));
                }
                else
                {
                    //get model
                    ProfileViewModel oModel = new ProfileViewModel()
                    {
                        CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId),
                        IsRedirect = false,
                    };

                    //Seo model
                    string oCityName = base.CurrentCookie != null && EnabledCities.ContainsKey(base.CurrentCookie.CurrentCity) ? EnabledCities[base.CurrentCookie.CurrentCity] : DefaultCityName;

                    SEOModel oSeoModel = new SEOModel()
                    {
                        Title = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Title.Replace("{AreaName}", MarketPlace.Web.Controllers.BaseController.AreaName)].Value.Replace("{CityName}", oCityName),
                        Description = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Description.Replace("{AreaName}", MarketPlace.Web.Controllers.BaseController.AreaName)].Value.Replace("{CityName}", oCityName),
                        Keywords = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Keywords.Replace("{AreaName}", MarketPlace.Web.Controllers.BaseController.AreaName)].Value.Replace("{CityName}", oCityName),

                        IsNoIndex = false,
                        IsNoFollow = false,
                    };

                    if (oModel.CurrentProfile.DefaultSpecialty != null)
                    {
                        oSeoModel.CanonicalUrl = base.CurrentDomainUrl +
                            Server.UrlDecode(Url.RouteUrl(
                                    MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                                    new
                                    {
                                        DoctorName = BaseController.RemoveAccent(oModel.CurrentProfile.Name.Trim() + " " + oModel.CurrentProfile.LastName.Trim()),
                                        ProfilePublicId = oModel.CurrentProfile.ProfilePublicId,
                                        SpecialtyName = BaseController.RemoveAccent(oModel.CurrentProfile.DefaultSpecialty.Name.Trim()),
                                    }));
                    }

                    ViewBag.SeoModel = oSeoModel;

                    //render profile
                    return View(oModel);
                }
            }
            catch
            {
                return RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound)));
            }
        }

        #region Private methods

        private void EvalRedirect
            (ProfileViewModel ViewModel,
            string DoctorName,
            string ProfilePublicId,
            string SpecialtyName,
            SEOModel CurrentSeoModel,
            bool IsCanonical)
        {
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
                                DoctorName = BaseController.RemoveAccent(ViewModel.CurrentProfile.Name.Trim() + " " + ViewModel.CurrentProfile.LastName.Trim()),
                                ProfilePublicId = ViewModel.CurrentProfile.ProfilePublicId,
                                SpecialtyName = BaseController.RemoveAccent(ViewModel.CurrentProfile.DefaultSpecialty.Name.Trim()),
                            })));
                }

                //compare doctor names
                if (DoctorName != BaseController.RemoveAccent(ViewModel.CurrentProfile.Name.Trim() + " " + ViewModel.CurrentProfile.LastName.Trim()))
                {
                    Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                            new
                            {
                                DoctorName = BaseController.RemoveAccent(ViewModel.CurrentProfile.Name.Trim() + " " + ViewModel.CurrentProfile.LastName.Trim()),
                                ProfilePublicId = ViewModel.CurrentProfile.ProfilePublicId,
                                SpecialtyName = BaseController.RemoveAccent(ViewModel.CurrentProfile.DefaultSpecialty.Name.Trim()),
                            })));
                }

                //validate specialty for canonical
                if (ViewModel.CurrentProfile.DefaultSpecialty != null &&
                    (string.IsNullOrEmpty(SpecialtyName) ||
                    IsCanonical ||
                    SpecialtyName.Trim() != BaseController.RemoveAccent(ViewModel.CurrentProfile.DefaultSpecialty.Name.Trim())))
                {
                    CurrentSeoModel.CanonicalUrl = Server.UrlDecode(Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                            new
                            {
                                DoctorName = BaseController.RemoveAccent(ViewModel.CurrentProfile.Name.Trim() + " " + ViewModel.CurrentProfile.LastName.Trim()),
                                ProfilePublicId = ViewModel.CurrentProfile.ProfilePublicId,
                                SpecialtyName = BaseController.RemoveAccent(ViewModel.CurrentProfile.DefaultSpecialty.Name.Trim()),
                            }));
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
                SaludGuruProfile.Manager.Models.Profile.ProfileModel NewProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetProfilePublicIdFromOldId
                    (ProfilePublicId);

                if (NewProfile != null && !string.IsNullOrEmpty(NewProfile.ProfilePublicId))
                {
                    Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                            new
                            {
                                DoctorName = BaseController.RemoveAccent(NewProfile.Name.Trim() + " " + NewProfile.LastName.Trim()),
                                ProfilePublicId = NewProfile.ProfilePublicId,
                                SpecialtyName = BaseController.RemoveAccent(NewProfile.DefaultSpecialty.Name.Trim()),
                            })));
                }
                else
                {
                    Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound)));
                }
            }
        }

        private void ReplaceSeoModel
           (ProfileViewModel ViewModel, SEOModel CurrentSeoModel)
        {
            string oCityName = EnabledCities[
                    ViewModel.CurrentProfile.RelatedOffice.
                    Where(x => x.IsDefault).
                    Select(x => x.City.CityId).
                    DefaultIfEmpty(DefaultCityId).
                    FirstOrDefault()];

            string oSpecialtyName = ViewModel.CurrentProfile.DefaultSpecialty == null ?
                    string.Empty :
                    ViewModel.CurrentProfile.DefaultSpecialty.Name;


            CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{SpecialtyName}", oSpecialtyName);
            CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{DoctorName}", ViewModel.CurrentProfile.Name + " " + ViewModel.CurrentProfile.LastName);
            CurrentSeoModel.Title = CurrentSeoModel.Title.Replace("{CityName}", oCityName);

            CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{SpecialtyName}", oSpecialtyName);
            CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{DoctorName}", ViewModel.CurrentProfile.Name + " " + ViewModel.CurrentProfile.LastName);
            CurrentSeoModel.Description = CurrentSeoModel.Description.Replace("{CityName}", oCityName);

            CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{SpecialtyName}", oSpecialtyName);
            CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{DoctorName}", ViewModel.CurrentProfile.Name + " " + ViewModel.CurrentProfile.LastName);
            CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.Replace("{CityName}", oCityName);

            CurrentSeoModel.CityName = oCityName;

            CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.TrimEnd(',') + "," +
                ViewModel.CurrentProfile.ProfileInfo.
                Where(x => x.ProfileInfoType == SaludGuruProfile.Manager.Models.enumProfileInfoType.KeyWords).
                Select(x => x.LargeValue).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();

            if (ViewModel.CurrentProfile.DefaultSpecialty != null)
            {
                CurrentSeoModel.Keywords = CurrentSeoModel.Keywords.TrimEnd(',') + "," +
                    ViewModel.CurrentProfile.DefaultSpecialty.SpecialtyInfo.
                    Where(x => x.CategoryInfoType == SaludGuruProfile.Manager.Models.enumCategoryInfoType.Keyword).
                    Select(x => x.LargeValue).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        #endregion
    }
}