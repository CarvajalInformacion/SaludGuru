using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Models
{
    public class AddressModel
    {
        public int AddressId { get; set; }
        public string Address { get; set; }
        public int IsBlackList { get; set; }
        public string Agent { get; set; }        
    }
}
