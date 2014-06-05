using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog
{
    public class LogController : CarvajalLog.Interfaces.ILog
    {
        private static CarvajalLog.Interfaces.ILog oInstance;
        public static CarvajalLog.Interfaces.ILog Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new LogController();
                return oInstance;
            }
        }

        public void SaveLog(Interfaces.ILogModel NewLog)
        {
            if (Type.Equals(NewLog.GetType(), typeof(CarvajalLog.Models.AuthLogModel)))
            {
                SaveLogAuth((CarvajalLog.Models.AuthLogModel)NewLog);
            }
            //throw new NotImplementedException();
        }

        private void SaveLogAuth(CarvajalLog.Models.AuthLogModel NewLog)
        {

        }
    }
}
