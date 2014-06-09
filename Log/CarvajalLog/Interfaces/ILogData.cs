using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.Interfaces
{
    public interface ILogData
    {
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
        void SaveLogMessage(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage, string MessageFrom, string MessageTo, int IdMessage);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="LogAction"></param>
        /// <param name="IsSuccessfull"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="Message"></param>
        /// <param name="LogIn"></param>
        void SaveLogAuth(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage,int LogIn);
    }
}
