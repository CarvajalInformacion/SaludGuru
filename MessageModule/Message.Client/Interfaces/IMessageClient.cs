using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.Interfaces
{
    public interface IMessageClient
    {
        public Message.Client.Models.CreateMessageResponse CreateMessage(Message.Client.Models.CreateMessageRequest MessageToCreate);
        public List<Message.Client.Models.CreateMessageResponse> CreateMultipleMessage(List<Message.Client.Models.CreateMessageRequest> lstMessageToCreate);
    }
}
