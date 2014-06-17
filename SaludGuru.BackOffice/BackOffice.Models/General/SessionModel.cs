using SessionController.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.General
{
    public static class SessionModel
    {
        public static User CurrentLoginUser { get { return SessionController.SessionManager.Auth_UserLogin; } }

        public static bool UserIsLoggedIn { get { return (CurrentLoginUser != null); } }

        public static string LoginUserEmail
        {
            get
            {
                return CurrentLoginUser.ExtraData.Where
                    (ed => ed.InfoType == SessionController.Models.Auth.enumUserInfoType.Email).
                    Select(ed => ed.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public static List<SessionController.Models.Profile.Autorization.AutorizationModel> UserAutorization
        {
            get
            {
                return SessionController.SessionManager.Profile_UserAutorization;
            }
            set
            {
                SessionController.SessionManager.Profile_UserAutorization = value;
            }
        }

        public static bool UserIsAutorized { get { return (UserAutorization != null && UserAutorization.Count > 0); } }
    }
}
