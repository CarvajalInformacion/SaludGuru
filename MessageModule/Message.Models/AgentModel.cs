using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Models
{
    public class AgentModel
    {
        public Message.Models.MessageQueueModel QueueItemToProcess { get; set; }
        public Dictionary<string, string> AgentConfig { get; set; }
        public Dictionary<string, string> MessageConfig { get; set; }
    }
}
