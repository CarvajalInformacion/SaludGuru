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
        /// Este método almacena un nuevo log messaging con los parametros por defecto
        /// </summary>
        /// <param name="UserId">Identificación del usuario</param>
        /// <param name="LogAction">Acción registrada para generar el log.</param>
        /// <param name="IsSuccessfull">Muestra si el proceso se generó de forma exitosa o no.</param>
        /// <param name="ErrorMessage">Mensaje de error del log.</param>
        /// <param name="MessageFrom">Origen del mensaje.</param>
        /// <param name="MessageTo">Destino del mensaje.</param>
        /// <param name="IdMessage">Identificación del mensaje.</param>
        public void SaveLogMessage(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage, string MessageFrom, string MessageTo, int IdMessage)
        {
            LogDataFactory factory = new LogDataFactory();
            var CallObj = factory.GetLogInstance();
            CallObj.SaveLogMessage(UserId, LogAction, IsSuccessfull, ErrorMessage, MessageFrom, MessageTo, IdMessage);
        }

        /// <summary>        
        /// Este metodo almacena un nuevo log authentication con los parametros por defecto.
        /// </summary>
        /// <param name="UserId">Identificación del usuario.</param>
        /// <param name="LogAction">Acción registrada para generar el log.</param>
        /// <param name="IsSuccessfull">Muestra si el proceso se generó de forma exitosa o no.</param>
        /// <param name="ErrorMessage">Mensaje de error del log.</param>
        /// <param name="LogIn">Usuario que se encuentra logeado para registrar el log.</param>
        public void SaveLogAuth(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage, int LogIn)
        {
            LogDataFactory factory = new LogDataFactory();
            var CallObj = factory.GetLogInstance();
            CallObj.SaveLogAuth(UserId, LogAction, IsSuccessfull, ErrorMessage, LogIn);
        }
    }
}
