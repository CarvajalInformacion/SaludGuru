using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.Interfaces
{
    public interface IClientMessageData
    {
        int InsertMessageQueue(string MessageType, DateTime ProgramType, string UserAction);
        void InsertMessageParameter(int MessageQueueId, string Key, string Value);
    }
}
