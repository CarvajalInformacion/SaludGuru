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

        private Auth.Interfaces.IAuthData DataFactory;
        #endregion

        #region constructor
        public AuthDataController()
        {
            AuthDataFactory factory = new AuthDataFactory();
            DataFactory = factory.GetDataInstance();
        }
        #endregion

        #region Implemented methods
        public string UpsertUser(string Name, string LastName, DateTime? Birthday, bool? Gender, string ProviderId, SessionController.Models.Auth.enumLoginType LoginTypeId, string Email)
        {
            return DataFactory.UpsertUser(Name, LastName, Birthday, Gender, ProviderId, LoginTypeId, Email);
        }

        public SessionController.Models.Auth.User GetUser(string UserPublicId)
        {
            return DataFactory.GetUser(UserPublicId);
        }

        public void InsertUserInfo(string UserPublicId, SessionController.Models.Auth.enumUserInfoType InfoTypeId, string Value)
        {
            DataFactory.InsertUserInfo(UserPublicId, InfoTypeId, Value);
        }

        public void UpdateUserInfo(int UserInfoId, string Value)
        {
            DataFactory.UpdateUserInfo(UserInfoId, Value);
        }
        #endregion
    }
}
