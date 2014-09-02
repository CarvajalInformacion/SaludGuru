using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.Models
{
    public class CreateMessageResponse
    {
        public ClientMessageModel ProcessMessage { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMesage { get; set; }
    }
}
