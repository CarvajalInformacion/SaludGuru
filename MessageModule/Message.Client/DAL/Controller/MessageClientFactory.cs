using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.DAL.Controller
{
    public class MessageClientFactory
    {
        public Message.Client.Interfaces.IClientMessageData GetMessageClientInstance()
        {
            Type typetoreturn = Type.GetType("Message.Client.DAL.MySQLDAO.MessageClient_MySqlDao,Message.Client");
            Message.Client.Interfaces.IClientMessageData oRetorno = (Message.Client.Interfaces.IClientMessageData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
