using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Interfaces
{
    public interface IAuthData
    {
        Auth.Models.User UpsertUser(Auth.Models.User UserLoginInfo);
    }
}
