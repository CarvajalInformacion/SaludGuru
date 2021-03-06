﻿using Message.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Interfaces
{
    public interface IAgent
    {
        Message.Models.MessageModel SendMessage(Message.Models.AgentModel MessageToSend);

        void AddResend(int MessageProcessId);
    }
}
