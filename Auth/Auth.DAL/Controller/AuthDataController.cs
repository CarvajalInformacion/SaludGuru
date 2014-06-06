using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DAL.Controller
{
    public class AuthDataController : Auth.Interfaces.IAuthData
    {
        #region Singleton instance
        private static Auth.Interfaces.IAuthData oInstance;
        public static Auth.Interfaces.IAuthData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new AuthDataController();
                return oInstance;
            }
        }
        #endregion

        #region Implemented methods
        /// <summary>
        /// upsert user and return all new user info
        /// </summary>
        /// <param name="UserLoginInfo">user data to validate</param>
        /// <returns></returns>
        public Auth.Models.User UpsertUser(Auth.Models.User UserLoginInfo)
        {
            AuthDataFactory factory = new AuthDataFactory();
            var CallObj = factory.GetDataInstance();
            return CallObj.UpsertUser(UserLoginInfo);
        }
        #endregion

    }
}
