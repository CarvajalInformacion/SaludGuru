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
                IsNoFollow = !ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow]),
                IsNoIndex = !ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex]),
                IsRedirect = !ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]),
            };

            return View(oModel);

            ////valida si los parametros son correctos
            //if (string.IsNullOrEmpty(DoctorName) &&
            //    string.IsNullOrEmpty(ProfilePublicId) &&
            //    string.IsNullOrEmpty(SpecialtyName))
            //{
            //    //permanent redirecto to home
            //    return RedirectToRoute(MarketPlace.Models.General.Constants.C_Route_Default);
            //}

            //try
            //{
            //    //get model
            //    ProfileViewModel oModel = new ProfileViewModel()
            //    {
            //        CurrentProfile = SaludGuruProfile.Manager.Controller.Profile.MPProfileGetFull(ProfilePublicId),
            //        IsNoFollow = !ControllerContext.RouteData.Values.
            //            Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow) ? false :
            //                Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoFollow]),
            //        IsNoIndex = !ControllerContext.RouteData.Values.
            //            Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex) ? false :
            //                Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsNoIndex]),
            //        IsRedirect = !ControllerContext.RouteData.Values.
            //            Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
            //                Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]),
            //    };


            //    //eval redirect
            //    EvalRedirect(oModel, DoctorName, ProfilePublicId, SpecialtyName);




            //    return View(oModel);
            //}
            //catch
            //{
            //    return RedirectToRoute(MarketPlace.Models.General.Constants.C_Route_Error_NotFound);
            //}
        }

        private void EvalRedirect
            (ProfileViewModel ViewModel,
            string DoctorName,
            string ProfilePublicId,
            string SpecialtyName)
        {

            string str = BaseController.RemoveAccent(ViewModel.CurrentProfile.Name + " " + ViewModel.CurrentProfile.LastName);

            if (ViewModel.CurrentProfile != null &&
                !string.IsNullOrEmpty(ViewModel.CurrentProfile.ProfilePublicId))
            {

                if (ViewModel.IsRedirect)
                {
                    RedirectToRoute(
                        MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                        new
                        {
                            DoctorName = BaseController.RemoveAccent(ViewModel.CurrentProfile.Name + " " + ViewModel.CurrentProfile.LastName),
                            ProfilePublicId = ViewModel.CurrentProfile.ProfilePublicId,
                            SpecialtyName = BaseController.RemoveAccent(ViewModel.CurrentProfile.DefaultSpecialty.Name),
                        });
                }

            }
            else
            {
                RedirectToRoute(MarketPlace.Models.General.Constants.C_Route_Error_NotFound);
            }
        }
    }
}