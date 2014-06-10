using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auth.Web.Controllers
{
    public partial class GoogleLoginController : BaseController
    {
        public virtual ActionResult Login(string UrlRetorno)
        {
            //get return url
            Uri oReturnUrl = base.GetReturnUrl(UrlRetorno);
            //get current application name
            string oAppName = base.GetAppNameByDomain(oReturnUrl);

            //get fb client 
            DotNetOpenAuth.ApplicationBlock.GoogleClient GMClient = GetGMClient(oAppName);

            //validate autentication
            DotNetOpenAuth.OAuth2.IAuthorizationState authorization = GMClient.ProcessUserAuthorization();

            if (authorization == null)
            {
                //preserve return url before request
                base.ReturnUrl = oReturnUrl;

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
            //get return url
            Uri oReturnUrl = base.ReturnUrl;

            //get current application name
            string oAppName = base.GetAppNameByDomain(base.ReturnUrl);

            //get fb client 
            DotNetOpenAuth.ApplicationBlock.GoogleClient GMClient = GetGMClient(oAppName);

            //validate autentication
            DotNetOpenAuth.OAuth2.IAuthorizationState authorization = GMClient.ProcessUserAuthorization();

            if (authorization != null)
            {
                DotNetOpenAuth.ApplicationBlock.IOAuth2Graph oauth2Graph = GMClient.GetGraph(authState: authorization);

                //create model login
                Auth.Models.User UserToLogin = GetUserToLogin(oauth2Graph);

                //login user
                UserToLogin = base.LoginUser(UserToLogin);

                //return to site
                Response.Redirect(oReturnUrl.ToString());
            }

            return View();
        }

        #region private methods
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

        Auth.Models.User GetUserToLogin(DotNetOpenAuth.ApplicationBlock.IOAuth2Graph SocialUser)
        {
            Auth.Models.User ConvertUser = new Auth.Models.User()
            {
                Name = SocialUser.FirstName,
                LastName = SocialUser.LastName,
                Birthday = SocialUser.BirthdayDT,
                Gender = SocialUser.GenderEnum == DotNetOpenAuth.ApplicationBlock.HumanGender.Female ? (bool?)false :
                            SocialUser.GenderEnum == DotNetOpenAuth.ApplicationBlock.HumanGender.Male ? (bool?)true : null,
                UserLogins = new List<Models.UserProvider>() 
                { 
                    new Models.UserProvider() 
                    { 
                        ProviderId = SocialUser.Id, 
                        LoginType = Models.enumLoginType.Google 
                    }
                },
                ExtraData = new List<Models.UserInfo>()
                {
                    new Models.UserInfo()
                    {
                        InfoType = Models.enumUserInfoType.Email,
                        Value = SocialUser.Email                    
                    },
                    new Models.UserInfo()
                    {
                        InfoType = Models.enumUserInfoType.ImageProfile,
                        Value = SocialUser.AvatarUrl.ToString()                  
                    },
                    new Models.UserInfo()
                    {
                        InfoType = Models.enumUserInfoType.SocialUrl,
                        Value = SocialUser.Link.ToString()
                    }
                },
            };

            return ConvertUser;
        }
        #endregion
    }
}