using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.UserManager
{
    public class LoginManager
    {
        public static Auth.Models.User LoggedInUser
        {
            get
            {
                if (System.Web.HttpContext.Current.Session[Auth.Models.Constants.C_SessionUserInfo] != null)
                    return (Auth.Models.User)System.Web.HttpContext.Current.Session[Auth.Models.Constants.C_SessionUserInfo];
                return null;
            }
        }
        public static bool UserIsLoggedIn
        {
            get
            {
                if (LoggedInUser == null)
                    return false;
                else
                    return true;
            }
        }
        public static void Logout()
        {
            System.Web.HttpContext.Current.Session.Abandon();
        }
    }
}
