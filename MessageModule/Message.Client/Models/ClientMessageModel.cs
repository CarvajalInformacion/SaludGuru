using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.Models
{
    public class ClientMessageModel
    {
        public int MessageQueueId { get; set; }
        public string MessageType { get; set; }
        public DateTime ProgramTime { get; set; }
        public string UserAction { get; set; }
        public DateTime CreateDate { get; set; }
        public List<ClientMessageParameter> RelatedParameter { get; set; }
    }
}
