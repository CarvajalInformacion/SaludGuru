using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog
{
    public class LogController : CarvajalLog.Interfaces.ILog
    {
        CarvajalLog.DAL.Controller.LogDataController LogDAL = new DAL.Controller.LogDataController();

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NewLog"></param>
        public void SaveLog(Interfaces.ILogModel NewLog)
        {
            if (Type.Equals(NewLog.GetType(), typeof(CarvajalLog.Models.AuthLogModel)))
            {                
                SaveLogAuth((CarvajalLog.Models.AuthLogModel)NewLog);
            }
            else if(Type.Equals(NewLog.GetType(), typeof(CarvajalLog.Models.MessageLogModel)))
            {
                SaveMessageLog((CarvajalLog.Models.MessageLogModel)NewLog);
            }

            // Se debe definir los demas metodos para las nuevas interfaces
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageLog"></param>
        private void SaveMessageLog(Models.MessageLogModel messageLog)
        {            
            LogDAL.SaveLogMessage(messageLog.UserId, messageLog.LogAction, messageLog.IsSuccessfull, messageLog.ErrorMessage, messageLog.MessageFrom, messageLog.MessageTo, messageLog.IdMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authLog"></param>
        private void SaveLogAuth(CarvajalLog.Models.AuthLogModel authLog)
        {
            LogDAL.SaveLogAuth(authLog.UserId, authLog.LogAction, authLog.IsSuccessfull, authLog.ErrorMessage, authLog.UserId);
        }
    }
}
