using Auth.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Client.DAL.Controller
{
    internal class ClientDataController : IClientData
    {
        #region singleton instance
        private static Auth.Client.Interfaces.IClientData oInstance;
        internal static Auth.Client.Interfaces.IClientData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new ClientDataController();
                return oInstance;
            }
        }

        private Auth.Client.Interfaces.IClientData DataFactory;
        #endregion

        public ClientDataController()
        {
            ClientDataFactory factory = new ClientDataFactory();
            DataFactory = factory.GetClientInstance();
        }

        public List<SessionController.Models.Auth.User> GetUserList(string UserPublicIdList)
        {
            return DataFactory.GetUserList(UserPublicIdList);
        }
    }
}
