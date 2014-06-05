using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.Interfaces
{
    public interface ILog
    {
        void SaveLog(ILogModel NewLog);
    }
}
