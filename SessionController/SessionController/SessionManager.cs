using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionController
{
    public class SessionManager
    {
        #region Auth
        public static void Logout()
        {
            System.Web.HttpContext.Current.Session.Abandon();
        }

        public static Uri Auth_ReturnUrl
        {
            get
            {
                return (Uri)System.Web.HttpContext.Current.Session[SessionController.Models.Constants.C_Session_Auth_ReturnUrl];
            }
            set
            {
                System.Web.HttpContext.Current.Session[SessionController.Models.Constants.C_Session_Auth_ReturnUrl] = value;
            }
        }

        public static SessionController.Models.Auth.User Auth_UserLogin
        {
            get
            {
                return (SessionController.Models.Auth.User)System.Web.HttpContext.Current.Session[SessionController.Models.Constants.C_Session_Auth_UserLogin];
            }
            set
            {
                System.Web.HttpContext.Current.Session[SessionController.Models.Constants.C_Session_Auth_UserLogin] = value;
            }
        }
        #endregion

        #region Profile

        public static List<SessionController.Models.Profile.Autorization.AutorizationModel> Profile_UserAutorization
        {
            get
            {
                return (List<SessionController.Models.Profile.Autorization.AutorizationModel>)
                    System.Web.HttpContext.Current.Session[SessionController.Models.Constants.C_Session_Profile_UserAutorization];
            }
            set
            {
                System.Web.HttpContext.Current.Session[SessionController.Models.Constants.C_Session_Profile_UserAutorization] = value;
            }
        }

        #endregion
    }
}
