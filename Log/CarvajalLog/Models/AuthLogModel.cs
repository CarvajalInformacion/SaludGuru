using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.Models
{
    public class AuthLogModel : CarvajalLog.Interfaces.ILogModel
    {
        public int IdLog { get; set; }

        public string UserName { get; set; }
    }
}
