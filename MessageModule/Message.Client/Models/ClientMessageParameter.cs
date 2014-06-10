using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.Models
{
    public class ClientMessageParameter : System.Collections.DictionaryBase
    {
        public string MessageParameterId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string CreateDate { get; set; }
    }
}
