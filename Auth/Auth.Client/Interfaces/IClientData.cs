using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Client.Interfaces
{
    interface IClientData
    {
        List<SessionController.Models.Auth.User> GetUserList(string UserPublicIdList);
        List<SessionController.Models.Auth.User> GetUserListByEmailList(string UserEmailList);
    }
}
