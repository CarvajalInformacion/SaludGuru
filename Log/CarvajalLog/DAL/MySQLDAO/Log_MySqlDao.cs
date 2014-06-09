using CarvajalLog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.DAL.MySQLDAO
{
    public class Log_MySqlDao : ILogData
    {
        /// <summary>
        /// This method saves a new Messaging log with the default parameters
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
            CarvajalLog.DAL.DBModel.LogConnection logEntities = new DBModel.LogConnection();
            try
            {
                var log = logEntities.SP_InsertLogMessage(UserId, LogAction, IsSuccessfull, ErrorMessage, MessageFrom, MessageTo, IdMessage);
            }
            catch { }
        }

        /// <summary>
        /// This method saves a new Authentication log with the default parameters
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LogAction"></param>
        /// <param name="IsSuccessfull"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="Message"></param>
        /// <param name="LogIn"></param>
        public void SaveLogAuth(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage, int LogIn)
        {
            CarvajalLog.DAL.DBModel.LogConnection logEntities = new DBModel.LogConnection();
            try
            {
                var log = logEntities.SP_InsertLogAuth(UserId, LogAction, IsSuccessfull, ErrorMessage, LogIn);
            }
            catch { }
        }
    }
}
