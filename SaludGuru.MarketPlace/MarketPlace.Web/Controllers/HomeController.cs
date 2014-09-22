using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaludGuruProfile.Manager.Models.Profile;
using MarketPlace.Models.General;

namespace MarketPlace.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ActionResult Index()
        {
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Home)));
            }

            ProfileModel Model = SaludGuruProfile.Manager.Controller.Profile.GetFeaturedProfile(1);

            //Seo model
            string oCityName = base.CurrentCookie != null && EnabledCities.ContainsKey(base.CurrentCookie.CurrentCity) ? EnabledCities[base.CurrentCookie.CurrentCity] : DefaultCityName;

            SEOModel oSeoModel = new SEOModel()
            {
                Title = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Title].Value.Replace("{CityName}", oCityName),
                Description = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Description].Value.Replace("{CityName}", oCityName),
                Keywords = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Keywords].Value.Replace("{CityName}", oCityName),

                CityName = oCityName,

                IsNoIndex = false,
                IsNoFollow = false,
            };
            ViewBag.SeoModel = oSeoModel;

            return View(Model);
        }

        public virtual ActionResult LoginDialog(string Date, string OfficePublicId)
        {
            Object urlReturn = Request["UrlReturn"].ToString();

            urlReturn = urlReturn + "&OfficePublicId=" + OfficePublicId + "&Date=" + Date;
            return View(urlReturn);
        }

        public virtual ActionResult ChangeCity(int NewCityId)
        {
            //get return url
            string oldCityName = MarketPlace.Web.Controllers.BaseController.EnabledCities[base.CurrentCookie.CurrentCity];

            string ReturnUrl = Request.UrlReferrer.ToString();

            if (MarketPlace.Web.Controllers.BaseController.EnabledCities.ContainsKey(NewCityId))
            {
                base.SetCookie(new MarketPlace.Models.General.CookieModel()
                {
                    CurrentCity = NewCityId,
                });

                ReturnUrl = ReturnUrl.Replace(oldCityName,
                    MarketPlace.Web.Controllers.BaseController.RemoveAccent(MarketPlace.Web.Controllers.BaseController.EnabledCities[NewCityId]));

                ReturnUrl = ReturnUrl.Replace(
                    MarketPlace.Web.Controllers.BaseController.RemoveAccent(oldCityName),
                    MarketPlace.Web.Controllers.BaseController.RemoveAccent(MarketPlace.Web.Controllers.BaseController.EnabledCities[NewCityId]));
            }

            return Redirect(ReturnUrl);
        }

        public virtual ActionResult ChangeMobileVersion()
        {
            if (MarketPlace.Models.General.SessionModel.MobileSessionInfo != null)
            {
                MarketPlace.Models.General.SessionModel.MobileSessionInfo.ViewFullVersion = true;
            }
            else
            {
                MarketPlace.Models.General.SessionModel.MobileSessionInfo = new SessionController.Models.Mobile.MobileModel()
                {
                    IsMobileDevice = true,
                    ViewFullVersion = true,
                };
            }

            string ReturnUrl = Request.UrlReferrer.ToString().ToLower().Replace
                (MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Url_MP_Mobile].Value,
                MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Url_MP_Desktop].Value);

            return Redirect(ReturnUrl);
        }

        public virtual ActionResult LegalTerms()
        {
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_LegalTerms)));
            }

            //Seo model
            string oCityName = base.CurrentCookie != null && EnabledCities.ContainsKey(base.CurrentCookie.CurrentCity) ? EnabledCities[base.CurrentCookie.CurrentCity] : DefaultCityName;

            SEOModel oSeoModel = new SEOModel()
            {
                Title = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Title].Value.Replace("{CityName}", oCityName),
                Description = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Description].Value.Replace("{CityName}", oCityName),
                Keywords = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Keywords].Value.Replace("{CityName}", oCityName),

                CityName = oCityName,

                IsNoIndex = false,
                IsNoFollow = false,
            };
            ViewBag.SeoModel = oSeoModel;


            return View();
        }

        public virtual ActionResult ConditionsAndRestrictions()
        {
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_ConditionsAndRestrictions)));
            }

            //Seo model
            string oCityName = base.CurrentCookie != null && EnabledCities.ContainsKey(base.CurrentCookie.CurrentCity) ? EnabledCities[base.CurrentCookie.CurrentCity] : DefaultCityName;

            SEOModel oSeoModel = new SEOModel()
            {
                Title = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Title].Value.Replace("{CityName}", oCityName),
                Description = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Description].Value.Replace("{CityName}", oCityName),
                Keywords = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Keywords].Value.Replace("{CityName}", oCityName),

                CityName = oCityName,

                IsNoIndex = false,
                IsNoFollow = false,
            };
            ViewBag.SeoModel = oSeoModel;


            return View();
        }

        public virtual ActionResult FAQ()
        {
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_FAQ)));
            }

            //Seo model
            string oCityName = base.CurrentCookie != null && EnabledCities.ContainsKey(base.CurrentCookie.CurrentCity) ? EnabledCities[base.CurrentCookie.CurrentCity] : DefaultCityName;

            SEOModel oSeoModel = new SEOModel()
            {
                Title = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Title].Value.Replace("{CityName}", oCityName),
                Description = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Description].Value.Replace("{CityName}", oCityName),
                Keywords = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Keywords].Value.Replace("{CityName}", oCityName),

                CityName = oCityName,

                IsNoIndex = false,
                IsNoFollow = false,
            };
            ViewBag.SeoModel = oSeoModel;


            return View();
        }

        public virtual ActionResult Contact()
        {
            if (!ControllerContext.RouteData.Values.
                    Any(x => x.Key == MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect) ? false :
                        Convert.ToBoolean(ControllerContext.RouteData.Values[MarketPlace.Models.General.Constants.C_RouteValue_IsRedirect]))
            {
                //redirect route 301
                Response.RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Contact)));
            }

            //Seo model
            string oCityName = base.CurrentCookie != null && EnabledCities.ContainsKey(base.CurrentCookie.CurrentCity) ? EnabledCities[base.CurrentCookie.CurrentCity] : DefaultCityName;

            SEOModel oSeoModel = new SEOModel()
            {
                Title = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Title].Value.Replace("{CityName}", oCityName),
                Description = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Description].Value.Replace("{CityName}", oCityName),
                Keywords = MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_SEO_Home_Keywords].Value.Replace("{CityName}", oCityName),

                CityName = oCityName,

                IsNoIndex = false,
                IsNoFollow = false,
            };
            ViewBag.SeoModel = oSeoModel;


            return View();
        }

        public virtual ActionResult LogOutUser()
        {
            base.LogOut();

            //get return url
            return Redirect(Request.UrlReferrer.ToString());
        }

        public virtual ActionResult SiteMap()
        {
            List<SiteMapModel> oModel = new List<SiteMapModel>();

            string strDomain = base.CurrentDomainUrl;

            #region Home
            oModel.Add(new SiteMapModel()
            {
                loc = strDomain,
                lastmod = DateTime.Now.ToString("yyyy-MM-dd"),
                priority = "1.0",
            });
            #endregion

            List<SaludGuruProfile.Manager.Models.General.SiteMapsModel> lstSiteMap =
                SaludGuruProfile.Manager.Controller.Profile.GetSiteMaps();

            lstSiteMap.All(smi =>
            {
                SiteMapModel NewSiteMapModel = new SiteMapModel()
                    {
                        CurrentSiteMapItem = smi,
                    };


                if (smi.RelatedProfile != null)
                {
                    NewSiteMapModel.lastmod = DateTime.Now.ToString("yyyy-MM-dd");
                    NewSiteMapModel.priority = "0.8";

                    NewSiteMapModel.loc = strDomain +
                        Server.UrlDecode(Url.RouteUrl(
                            MarketPlace.Models.General.Constants.C_Route_Profile_Default,
                            new
                            {
                                DoctorName = BaseController.RemoveAccent(smi.RelatedProfile.Name.Trim() + " " + smi.RelatedProfile.LastName.Trim()),
                                ProfilePublicId = smi.RelatedProfile.ProfilePublicId,
                                SpecialtyName = BaseController.RemoveAccent(smi.RelatedProfile.DefaultSpecialty.Name),
                            }));
                }
                else if (smi.RelatedCity != null && !string.IsNullOrEmpty(smi.RelatedCity.CityName))
                {
                    if (smi.RelatedInsurance != null)
                    {
                        if (smi.RelatedSpecialty != null)
                        {
                            NewSiteMapModel.lastmod = DateTime.Now.ToString("yyyy-MM-dd");
                            NewSiteMapModel.priority = "0.7";

                            NewSiteMapModel.loc = strDomain +
                                Server.UrlDecode(Url.RouteUrl(
                                    MarketPlace.Models.General.Constants.C_Route_SearchCategory_SpecialtyInsuranceCity,
                                    new
                                    {
                                        SpecialtyName = BaseController.RemoveAccent(smi.RelatedSpecialty.Name.Trim()),
                                        InsuranceName = BaseController.RemoveAccent(smi.RelatedInsurance.Name.Trim()),
                                        CityName = BaseController.RemoveAccent(smi.RelatedCity.CityName.Trim()),
                                    }));
                        }
                        else if (smi.RelatedTreatment != null)
                        {
                            NewSiteMapModel.lastmod = DateTime.Now.ToString("yyyy-MM-dd");
                            NewSiteMapModel.priority = "0.6";

                            NewSiteMapModel.loc = strDomain +
                                Server.UrlDecode(Url.RouteUrl(
                                    MarketPlace.Models.General.Constants.C_Route_SearchCategory_TreatmentInsuranceCity,
                                    new
                                    {
                                        TreatmentName = BaseController.RemoveAccent(smi.RelatedTreatment.Name.Trim()),
                                        InsuranceName = BaseController.RemoveAccent(smi.RelatedInsurance.Name.Trim()),
                                        CityName = BaseController.RemoveAccent(smi.RelatedCity.CityName.Trim()),
                                    }));
                        }
                        else
                        {
                            NewSiteMapModel.lastmod = DateTime.Now.ToString("yyyy-MM-dd");
                            NewSiteMapModel.priority = "0.3";

                            NewSiteMapModel.loc = strDomain +
                                Server.UrlDecode(Url.RouteUrl(
                                    MarketPlace.Models.General.Constants.C_Route_SearchCategory_InsuranceCity,
                                    new
                                    {
                                        InsuranceName = BaseController.RemoveAccent(smi.RelatedInsurance.Name.Trim()),
                                        CityName = BaseController.RemoveAccent(smi.RelatedCity.CityName.Trim()),
                                    }));
                        }
                    }
                    else
                    {
                        if (smi.RelatedSpecialty != null)
                        {
                            NewSiteMapModel.lastmod = DateTime.Now.ToString("yyyy-MM-dd");
                            NewSiteMapModel.priority = "0.5";

                            NewSiteMapModel.loc = strDomain +
                                Server.UrlDecode(Url.RouteUrl(
                                    MarketPlace.Models.General.Constants.C_Route_SearchCategory_SpecialtyCity,
                                    new
                                    {
                                        SpecialtyName = BaseController.RemoveAccent(smi.RelatedSpecialty.Name.Trim()),
                                        CityName = BaseController.RemoveAccent(smi.RelatedCity.CityName.Trim()),
                                    }));
                        }
                        else if (smi.RelatedTreatment != null)
                        {
                            NewSiteMapModel.lastmod = DateTime.Now.ToString("yyyy-MM-dd");
                            NewSiteMapModel.priority = "0.4";

                            NewSiteMapModel.loc = strDomain +
                                Server.UrlDecode(Url.RouteUrl(
                                    MarketPlace.Models.General.Constants.C_Route_SearchCategory_TreatmentCity,
                                    new
                                    {
                                        TreatmentName = BaseController.RemoveAccent(smi.RelatedTreatment.Name.Trim()),
                                        CityName = BaseController.RemoveAccent(smi.RelatedCity.CityName.Trim()),
                                    }));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(NewSiteMapModel.loc))
                {
                    oModel.Add(NewSiteMapModel);
                }

                return true;
            });



            return View(oModel);
        }
    }
}