using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.DAL.Controller
{
    internal class ProfileDataFactory
    {
        public Profile.Manager.Interfaces.IProfileData GetLogInstance()
        {
            Type typetoreturn = Type.GetType("Profile.Manager.DAL.MySQLDAO.Profile_MySqlDao,Profile.Manager");
            Profile.Manager.Interfaces.IProfileData oRetorno = (Profile.Manager.Interfaces.IProfileData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
