using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.DAL.Controller
{
    public class MessageClientController : Message.Client.Interfaces.IClientMessageData
    {
        private static Message.Client.Interfaces.IClientMessageData oInstance;
        public static Message.Client.Interfaces.IClientMessageData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new MessageClientController();
                return oInstance;
            }
        }

        public void InsertMessageQueue(string MessageType, DateTime ProgramType, string UserAction)
        {
            MessageClientFactory factory = new MessageClientFactory();
            var CallObj = factory.GetMessageClientInstance();
            CallObj.InsertMessageQueue(MessageType, ProgramType, UserAction);
        }

        public void InsertMessageParameter(int MessageQueueId, string Key, string Value)
        {
            MessageClientFactory factory = new MessageClientFactory();
            var CallObj = factory.GetMessageClientInstance();
            CallObj.InsertMessageParameter(MessageQueueId, Key, Value);
        }
    }
}
