using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Auth.Web.Controllers
{
    public partial class BaseController : Controller
    {
        #region public static properties

        /// <summary>
        /// Current Area Name
        /// </summary>
        public static string AreaName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AreaName"].ToString().Trim();
            }
        }
        #endregion

        #region public properties
        /// <summary>
        /// Return url
        /// </summary>
        public Uri ReturnUrl
        {
            get
            {
                if (Session[Auth.Models.Constants.C_SessionReturnUrl] == null)
                    return null;
                else
                    return new Uri(Session[Auth.Models.Constants.C_SessionReturnUrl].ToString());
            }
            set
            {
                Session[Auth.Models.Constants.C_SessionReturnUrl] = value;
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// get app name for return url to use correct login params
        /// </summary>
        /// <param name="OriginUrl">sended return url</param>
        /// <returns></returns>
        public string GetAppNameByDomain(Uri OriginUrl)
        {
            string oRetorno = string.Empty;

            string strDomain = OriginUrl.GetLeftPart(UriPartial.Authority).ToLower().TrimEnd('/');

            XDocument xDoc = XDocument.Parse(SettingsManager.SettingsController.SettingsInstance.ModulesParams
                [Auth.Models.Constants.C_SettingsModuleName][Auth.Models.Constants.C_AppConfig].Value);

            xDoc.Descendants("applications").Descendants("key").All(app =>
            {
                if (app.Value.Split(',').Any(x => x.ToLower().TrimEnd('/') == strDomain) &&
                    string.IsNullOrEmpty(oRetorno))
                {
                    oRetorno = app.Attribute("name").Value;
                }
                return true;
            });

            return oRetorno;
        }

        /// <summary>
        /// get return url 
        /// </summary>
        /// <param name="ReturnUrl">return url to convert</param>
        /// <returns></returns>
        public Uri GetReturnUrl(string ReturnUrl)
        {
            Uri oRetorno = null;
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                //get url from previus page send
                oRetorno = Request.UrlReferrer;
            }
            else
            {
                //get url from request
                oRetorno = new Uri(ReturnUrl);
            }

            return oRetorno;
        }

        #region Login Methods
        public Auth.Models.User LoginUser(Auth.Models.User OriginalInfo)
        {
            Auth.Models.User oRetorno = null;

            //validate user login
            if (Auth.UserManager.LoginManager.UserIsLoggedIn)
            {
                Auth.UserManager.LoginManager.Logout();
            }

            //upsert user
            oRetorno = new Auth.Models.User();

            oRetorno.UserPublicId = Auth.DAL.Controller.AuthDataController.Instance.UpsertUser
                (OriginalInfo.Name,
                OriginalInfo.LastName,
                OriginalInfo.Birthday,
                OriginalInfo.Gender,
                OriginalInfo.UserLogins.First().ProviderId,
                OriginalInfo.UserLogins.First().LoginType);

            //get temp user info
            oRetorno = Auth.DAL.Controller.AuthDataController.Instance.GetUser(oRetorno.UserPublicId);

            //insert or update extradata
            OriginalInfo.ExtraData.All(nd =>
            {
                if (!oRetorno.ExtraData.Any(od => od.InfoType == nd.InfoType))
                {
                    //create extradata
                    Auth.DAL.Controller.AuthDataController.Instance.InsertUserInfo
                        (oRetorno.UserPublicId,
                        nd.InfoType,
                        nd.Value);
                }
                else if (oRetorno.ExtraData.Any(od => od.InfoType == nd.InfoType && od.Value != nd.Value))
                {
                    //update extradata
                    Auth.DAL.Controller.AuthDataController.Instance.UpdateUserInfo
                        (oRetorno.ExtraData.Where(od => od.InfoType == nd.InfoType).First().UserInfoId,
                        nd.Value);
                }

                return true;
            });

            //get final user info
            oRetorno = Auth.DAL.Controller.AuthDataController.Instance.GetUser(oRetorno.UserPublicId);

            //save session
            Session[Auth.Models.Constants.C_SessionUserInfo] = oRetorno;

            return oRetorno;
        }
        #endregion

        #endregion
    }
}