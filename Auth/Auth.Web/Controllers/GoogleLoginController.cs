using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class GoogleLoginController : BaseController
    {
        /// <summary>
        /// Return url stored in session not sendig to google
        /// </summary>
        private Uri ReturnUrl
        {
            get
            {
                if (Session["GoogleReturnUrl"] == null)
                    return null;
                else
                    return new Uri(Session["GoogleReturnUrl"].ToString());
            }
            set
            {
                Session["GoogleReturnUrl"] = value;
            }
        }

        public virtual ActionResult Login(string UrlRetorno)
        {
            //get return url
            ReturnUrl = base.GetReturnUrl(UrlRetorno);
            //get current application name
            string oAppName = base.GetAppNameByDomain(ReturnUrl);

            //get fb client 
            DotNetOpenAuth.ApplicationBlock.GoogleClient GMClient = GetGMClient(oAppName);

            //validate autentication
            DotNetOpenAuth.OAuth2.IAuthorizationState authorization = GMClient.ProcessUserAuthorization();

            if (authorization == null)
            {
                //user is not login
                GMClient.RequestUserAuthorization(scope: new[] { 
                            DotNetOpenAuth.ApplicationBlock.GoogleClient.Scopes.PlusMe,
                            DotNetOpenAuth.ApplicationBlock.GoogleClient.Scopes.UserInfo.Email,
                            DotNetOpenAuth.ApplicationBlock.GoogleClient.Scopes.UserInfo.Profile},
                            returnTo: new Uri
                                (Request.Url.GetLeftPart(UriPartial.Authority).ToLower().TrimEnd('/') +
                                Url.Action(MVC.GoogleLogin.ActionNames.LoginCallBack)));
            }
            else
            {
                RedirectToAction(MVC.GoogleLogin.ActionNames.LoginCallBack);
            }

            return View();
        }

        public virtual ActionResult LoginCallBack()
        {
            //get current application name
            string oAppName = base.GetAppNameByDomain(ReturnUrl);

            //get fb client 
            DotNetOpenAuth.ApplicationBlock.GoogleClient GMClient = GetGMClient(oAppName);

            //validate autentication
            DotNetOpenAuth.OAuth2.IAuthorizationState authorization = GMClient.ProcessUserAuthorization();

            if (authorization != null)
            {
                DotNetOpenAuth.ApplicationBlock.IOAuth2Graph oauth2Graph = GMClient.GetGraph(authState: authorization);
            }

            return View();
        }

        #region private methods
        //create facebook instance
        DotNetOpenAuth.ApplicationBlock.GoogleClient GetGMClient(string AppName)
        {
            DotNetOpenAuth.ApplicationBlock.GoogleClient client = new DotNetOpenAuth.ApplicationBlock.GoogleClient();

            //appid
            client.ClientIdentifier = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Models.Constants.C_SettingsModuleName]
                                    [Auth.Models.Constants.C_GM_AppId.Replace("{AppName}", AppName)].Value;
            //secret key
            client.ClientCredentialApplicator = DotNetOpenAuth.OAuth2.ClientCredentialApplicator.PostParameter
                                (SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Models.Constants.C_SettingsModuleName]
                                    [Auth.Models.Constants.C_GM_AppSecret.Replace("{AppName}", AppName)].Value);

            return client;
        }
        #endregion
    }
}