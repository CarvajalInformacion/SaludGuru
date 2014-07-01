using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.DAL.Controller
{
    internal class ProfileDataFactory
    {
        public SaludGuruProfile.Manager.Interfaces.IProfileData GetProfileDataInstance()
        {
            Type typetoreturn = Type.GetType("SaludGuruProfile.Manager.DAL.MySQLDAO.Profile_MySqlDao,SaludGuruProfile.Manager");
            SaludGuruProfile.Manager.Interfaces.IProfileData oRetorno = (SaludGuruProfile.Manager.Interfaces.IProfileData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
