using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog
{
    public class LogController : CarvajalLog.Interfaces.ILog
    {
        private CarvajalLog.DAL.Controller.LogDataController oLogDAL;
        public CarvajalLog.DAL.Controller.LogDataController LogDAL
        {
            get
            {
                if (oLogDAL == null)
                    oLogDAL = new DAL.Controller.LogDataController();
                return oLogDAL;
            }
        }

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
        /// Metodo que se encarga de distribuir el almacenamiento de los log teniendo en cuenta el tipo de modelo entrante
        /// </summary>
        /// <param name="NewLog">Parametro con la información y el tipo del nuevo log.</param>
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

            // Continúa con la implementación de los registros de log de los demas modulos.
        }

        /// <summary>
        /// Metodo que se encarga de guardar un registro con la información del modulo de mensajería.
        /// </summary>
        /// <param name="messageLog">Parametro que contiene la información del log de la capa de mensajería.</param>
        private void SaveMessageLog(Models.MessageLogModel messageLog)
        {            
            LogDAL.SaveLogMessage(messageLog.UserId, messageLog.LogAction, messageLog.IsSuccessfull, messageLog.ErrorMessage, messageLog.MessageFrom, messageLog.MessageTo, messageLog.IdMessage);
        }

        /// <summary>
        /// Metodo que se encarga de guardar un registro con la información del modulo de autenticación. 
        /// </summary>
        /// <param name="authLog">Parametro que contiene la información del log de la capa de autenticación.</param>
        private void SaveLogAuth(CarvajalLog.Models.AuthLogModel authLog)
        {
            LogDAL.SaveLogAuth(authLog.UserId, authLog.LogAction, authLog.IsSuccessfull, authLog.ErrorMessage, authLog.UserId);
        }
    }
}
