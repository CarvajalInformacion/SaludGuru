﻿using MarketPlace.Models.Profile;
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
            try
            {
                //get model
                ProfileViewModel oModel = new ProfileViewModel()
                {
                    CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId),
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
                };

                //eval redirect
                EvalRedirect(oModel, DoctorName, ProfilePublicId, SpecialtyName);

                ViewBag.NoIndex = oModel.IsNoIndex;
                ViewBag.NoFollow = oModel.IsNoFollow;

                if (oModel.IsCanonical && oModel.CurrentProfile.DefaultSpecialty != null)
                {
                    ViewBag.Canonical = Request.Url.ToString().Replace(Request.Url.AbsolutePath, "") +
                        Server.UrlDecode(Url.RouteUrl(
                                MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                                new
                                {
                                    DoctorName = BaseController.RemoveAccent(oModel.CurrentProfile.Name + " " + oModel.CurrentProfile.LastName),
                                    ProfilePublicId = oModel.CurrentProfile.ProfilePublicId,
                                    SpecialtyName = BaseController.RemoveAccent(oModel.CurrentProfile.DefaultSpecialty.Name),
                                }));
                }

                //render profile
                return View(oModel);
            }
            catch
            {
                return RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound)));
            }
        }

        public virtual ActionResult ProfilePreview(string ProfilePublicId)
        {
            try
            {
                //get model
                ProfileViewModel oModel = new ProfileViewModel()
                {
                    CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId),
                    IsNoFollow = true,
                    IsNoIndex = true,
                    IsRedirect = false,
                    IsCanonical = true,
                };

                ViewBag.NoIndex = oModel.IsNoIndex;
                ViewBag.NoFollow = oModel.IsNoFollow;

                if (oModel.IsCanonical && oModel.CurrentProfile.DefaultSpecialty != null)
                {
                    ViewBag.Canonical = Request.Url.ToString().Replace(Request.Url.PathAndQuery, "") +
                        Server.UrlDecode(Url.RouteUrl(
                                MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                                new
                                {
                                    DoctorName = BaseController.RemoveAccent(oModel.CurrentProfile.Name + " " + oModel.CurrentProfile.LastName),
                                    ProfilePublicId = oModel.CurrentProfile.ProfilePublicId,
                                    SpecialtyName = BaseController.RemoveAccent(oModel.CurrentProfile.DefaultSpecialty.Name),
                                }));
                }
                //render profile
                return View(MVC.Profile.ActionNames.Index, oModel);
            }
            catch
            {
                return RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound)));
            }
        }

        public virtual ActionResult FBProfile(string ProfilePublicId)
        {
            try
            {
                if (MarketPlace.Models.General.SessionModel.UserIsLoggedIn)
                {

                    //get model
                    ProfileViewModel oModel = new ProfileViewModel()
                    {
                        CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId),
                        IsNoFollow = true,
                        IsNoIndex = true,
                        IsRedirect = false,
                        IsCanonical = true,
                    };

                    ViewBag.NoIndex = oModel.IsNoIndex;
                    ViewBag.NoFollow = oModel.IsNoFollow;

                    if (oModel.IsCanonical && oModel.CurrentProfile.DefaultSpecialty != null)
                    {
                        ViewBag.Canonical = Request.Url.ToString().Replace(Request.Url.PathAndQuery, "") +
                            Server.UrlDecode(Url.RouteUrl(
                                    MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                                    new
                                    {
                                        DoctorName = BaseController.RemoveAccent(oModel.CurrentProfile.Name + " " + oModel.CurrentProfile.LastName),
                                        ProfilePublicId = oModel.CurrentProfile.ProfilePublicId,
                                        SpecialtyName = BaseController.RemoveAccent(oModel.CurrentProfile.DefaultSpecialty.Name),
                                    }));
                    }
                    //render profile
                    return View(oModel);
                }
                else
                {
                    return Redirect(MarketPlace.Models.General.InternalSettings.Instance
                         [MarketPlace.Models.General.Constants.C_Settings_Login_FBUrl.Replace("{{AreaName}}", MarketPlace.Web.Controllers.BaseController.AreaName)]
                         .Value.Replace("{{UrlRetorno}}", Request.Url.ToString()));
                }
            }
            catch
            {
                return RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound)));
            }
        }

        private void EvalRedirect
            (ProfileViewModel ViewModel,
            string DoctorName,
            string ProfilePublicId,
            string SpecialtyName)
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
                if (ViewModel.CurrentProfile.DefaultSpecialty == null ||
                    SpecialtyName != BaseController.RemoveAccent(ViewModel.CurrentProfile.DefaultSpecialty.Name))
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