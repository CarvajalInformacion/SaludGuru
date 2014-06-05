using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.Interfaces
{
    public interface ILogData
    {
        void SaveLogMessage(string logMsj);

        void SaveLogAuth(string user);
    }
}
