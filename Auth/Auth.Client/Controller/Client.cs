using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Client.Controller
{
    public static class Client
    {
        public static List<SessionController.Models.Auth.User> GetUserList(string UserPublicIdList)
        {
            return DAL.Controller.ClientDataController.Instance.GetUserList(UserPublicIdList);
        }

        public static List<SessionController.Models.Auth.User> GetUserListByEmailList(string UserEmailList)
        {
            return DAL.Controller.ClientDataController.Instance.GetUserListByEmailList(UserEmailList);
        }
    }
}
