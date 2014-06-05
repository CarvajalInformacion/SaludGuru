using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.DAL.Controller
{
    public class LogDataController : CarvajalLog.Interfaces.ILogData
    {
        private static LogDataController oInstance;
        public static LogDataController Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new LogDataController();
                return oInstance;
            }
        }

        public void SaveLogMessage(string logMsj)
        {
            LogDataFactory factory = new LogDataFactory();
            var CallObj = factory.GetLogInstance();
            CallObj.SaveLogMessage(logMsj);
        }

        public void SaveLogAuth(string user)
        {
            LogDataFactory factory = new LogDataFactory();
            var CallObj = factory.GetLogInstance();
            CallObj.SaveLogMessage(user);
        }
    }
}
