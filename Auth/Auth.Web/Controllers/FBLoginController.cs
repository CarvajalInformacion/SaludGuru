using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class FBLoginController : BaseController
    {
        public virtual ActionResult Login(string UrlRetorno)
        {
            //get return url
            Uri oReturnUrl = base.GetReturnUrl(UrlRetorno);
            //get current application name
            string oAppName = base.GetAppNameByDomain(oReturnUrl);

            //get fb client 
            DotNetOpenAuth.ApplicationBlock.FacebookClient FBClient = GetFBClient(oAppName);

            //validate autentication
            DotNetOpenAuth.OAuth2.IAuthorizationState authorization = FBClient.ProcessUserAuthorization();

            if (authorization == null)
            {
                //user is not login
                FBClient.RequestUserAuthorization(scope: new[] { 
                            DotNetOpenAuth.ApplicationBlock.FacebookClient.Scopes.UserAboutMe,
                            DotNetOpenAuth.ApplicationBlock.FacebookClient.Scopes.Email, 
                            DotNetOpenAuth.ApplicationBlock.FacebookClient.Scopes.UserBirthday});
            }
            else
            {
                DotNetOpenAuth.ApplicationBlock.IOAuth2Graph oauth2Graph = FBClient.GetGraph(
                        authorization, 
                        new[] { 
                            DotNetOpenAuth.ApplicationBlock.FacebookGraph.Fields.Defaults, 
                            DotNetOpenAuth.ApplicationBlock.FacebookGraph.Fields.Email, 
                            DotNetOpenAuth.ApplicationBlock.FacebookGraph.Fields.Picture, 
                            DotNetOpenAuth.ApplicationBlock.FacebookGraph.Fields.Birthday });
            }

            return View();
        }

        #region private methods
        //create facebook instance
        DotNetOpenAuth.ApplicationBlock.FacebookClient GetFBClient(string AppName)
        {
            DotNetOpenAuth.ApplicationBlock.FacebookClient client = new DotNetOpenAuth.ApplicationBlock.FacebookClient();

            //appid
            client.ClientIdentifier = SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Models.Constants.C_SettingsModuleName]
                                    [Auth.Models.Constants.C_FB_AppId.Replace("{AppName}", AppName)].Value;
            //secret key
            client.ClientCredentialApplicator = DotNetOpenAuth.OAuth2.ClientCredentialApplicator.PostParameter
                                (SettingsManager.SettingsController.SettingsInstance.ModulesParams
                                    [Auth.Models.Constants.C_SettingsModuleName]
                                    [Auth.Models.Constants.C_FB_AppSecret.Replace("{AppName}", AppName)].Value);

            return client;
        }
        #endregion
    }
}

