using Message.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Controller
{
    internal class MessageDataFactory
    {
        public IMessageData GetDataInstance()
        {
            Type typetoreturn = Type.GetType("Message.DAL.MySqlDao.Message_MySqlDao,Message.DAL");
            IMessageData oRetorno = (IMessageData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
