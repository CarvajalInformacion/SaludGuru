using Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DAL.MySqlDao
{
    public class Auth_MySqlDao : IAuthData
    {
        /// <summary>
        /// upsert user and return all new user info
        /// </summary>
        /// <param name="UserLoginInfo">user data to validate</param>
        /// <returns></returns>
        public Models.User UpsertUser(Models.User UserLoginInfo)
        {
            throw new NotImplementedException();
        }
    }
}
