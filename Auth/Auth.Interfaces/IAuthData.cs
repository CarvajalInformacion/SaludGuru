using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Interfaces
{
    public interface IAuthData
    {
        string UpsertUser(string Name, string LastName, DateTime? Birthday, bool? Gender, string ProviderId, SessionController.Models.Auth.enumLoginType LoginTypeId, string Email);
        SessionController.Models.Auth.User GetUser(string UserPublicId);
        void InsertUserInfo(string UserPublicId, SessionController.Models.Auth.enumUserInfoType InfoTypeId, string Value);
        void UpdateUserInfo(int UserInfoId, string Value);
    }
}
