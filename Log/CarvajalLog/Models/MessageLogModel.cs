using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.Models
{
    public class MessageLogModel : CarvajalLog.Interfaces.ILogModel
    {

        public int IdLog { get; set; }

        public float otracosa { get; set; }
    }
}
