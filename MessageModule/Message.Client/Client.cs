using Message.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client
{
    public class Client : IMessageClient
    {
        private static Message.Client.Interfaces.IMessageClient oInstance;
        public static Message.Client.Interfaces.IMessageClient Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new Client();
                return oInstance;
            }
        }

        public Models.CreateMessageResponse CreateMessage(Models.CreateMessageRequest MessageToCreate)
        {
            throw new NotImplementedException();
        }

        public List<Models.CreateMessageResponse> CreateMultipleMessage(List<Models.CreateMessageRequest> lstMessageToCreate)
        {
            throw new NotImplementedException();
        }
    }
}
