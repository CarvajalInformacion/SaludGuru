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


        #endregion
    }
}