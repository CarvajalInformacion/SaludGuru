using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Manager.DAL.Controller
{
    internal class ProfileDataController : Profile.Manager.Interfaces.IProfileData
    {
        private static Profile.Manager.Interfaces.IProfileData oInstance;
        internal static Profile.Manager.Interfaces.IProfileData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ProfileDataController();
                return oInstance;
            }
        }

        #region Autorization
        #endregion

    }
}
