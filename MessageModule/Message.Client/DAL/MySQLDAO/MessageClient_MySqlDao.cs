using Message.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.DAL.MySQLDAO
{
    public class MessageClient_MySqlDao : IClientMessageData
    {
        public void InsertMessageQueue(string MessageType, DateTime ProgramType, string UserAction)
        {
           
        }

        public void InsertMessageParameter(int MessageQueueId, string Key, string Value)
        {
            throw new NotImplementedException();
        }
    }
}
