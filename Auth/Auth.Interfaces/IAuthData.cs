using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Interfaces
{
    public interface IAuthData
    {
        string UpsertUser(string Name, string LastName, DateTime? Birthday, bool? Gender, string ProviderId, Auth.Models.enumLoginType LoginTypeId);
        Auth.Models.User GetUser(string UserPublicId);
        void InsertUserInfo(string UserPublicId, Auth.Models.enumUserInfoType InfoTypeId, string Value);
        void UpdateUserInfo(int UserInfoId, string Value);
    }
}
