using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Models
{
    public class QueueParameterModel
    {
        public int MessageParameterId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
