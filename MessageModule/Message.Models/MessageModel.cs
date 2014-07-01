using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Models
{
    public class MessageModel
    {
        public int IdMessage { get; set; }
        public string MessageType { get; set; }
        public string Agent { get; set; }
        public DateTime TimeSent { get; set; }
        public string BodyMessage { get; set; }
        public List<AddressModel> RelatedAddress { get; set; }

    }
}
