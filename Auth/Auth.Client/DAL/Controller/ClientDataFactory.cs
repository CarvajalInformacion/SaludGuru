using Auth.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Client.DAL.Controller
{
    internal class ClientDataFactory
    {
        public IClientData GetClientInstance()
        {
            Type typetoreturn = Type.GetType("Auth.Client.DAL.MySQLDAO.Client_MySqlDao,Auth.Client");
            IClientData oRetorno = (IClientData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
