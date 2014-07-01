using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.Interfaces
{
    public interface IMessageClient
    {
        Message.Client.Models.CreateMessageResponse CreateMessage(Message.Client.Models.CreateMessageRequest MessageToCreate);
        List<Message.Client.Models.CreateMessageResponse> CreateMultipleMessage(List<Message.Client.Models.CreateMessageRequest> lstMessageToCreate);
    }
}
