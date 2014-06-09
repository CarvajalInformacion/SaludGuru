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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LogAction"></param>
        /// <param name="IsSuccessfull"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="SaveLogMessage"></param>
        /// <param name="MessageFrom"></param>
        /// <param name="MessageTo"></param>
        /// <param name="IdMessage"></param>
        public void SaveLogMessage(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage, string MessageFrom, string MessageTo, int IdMessage)
        {
            LogDataFactory factory = new LogDataFactory();
            var CallObj = factory.GetLogInstance();
            CallObj.SaveLogMessage(UserId, LogAction, IsSuccessfull, ErrorMessage, MessageFrom, MessageTo, IdMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LogAction"></param>
        /// <param name="IsSuccessfull"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="Message"></param>
        /// <param name="LogIn"></param>
        public void SaveLogAuth(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage, int LogIn)
        {
            LogDataFactory factory = new LogDataFactory();
            var CallObj = factory.GetLogInstance();
            CallObj.SaveLogAuth(UserId, LogAction, IsSuccessfull, ErrorMessage, LogIn);
        }
    }
}
