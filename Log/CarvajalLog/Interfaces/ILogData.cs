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
        /// Método de interfaz que se encarga de guardar un nuevo registro de log de la capa de mensajes.
        /// </summary>
        /// <param name="UserId">Identificación del usuario</param>
        /// <param name="LogAction">Acción en la que se genera el log.</param>
        /// <param name="IsSuccessfull">Determina si fue exitoso o no la acción ejecutada.</param>
        /// <param name="ErrorMessage">Mensaje que contiene el error.</param>
        /// <param name="MessageFrom">Origen del mensaje.</param>
        /// <param name="MessageTo">Destino del mensaje.</param>
        /// <param name="IdMessage">Identificación del mensaje.</param>
        void SaveLogMessage(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage, string MessageFrom, string MessageTo, int IdMessage);

        /// <summary>
        /// Método de interfaz que se encarga de guardar un nuevo registro de log de la capa de autenticación.
        /// </summary>
        /// <param name="UserId">Identificación del usuario.</param>
        /// <param name="LogAction">Acción en la que se genera el log.</param>
        /// <param name="IsSuccessfull">Determina si fué exitoso o no la acción ejecutada.</param>
        /// <param name="ErrorMessage">Mensaje que contiene el error.</param>
        /// <param name="LogIn">Usuario que esta realizando el login en el sistema.</param>
        void SaveLogAuth(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage,int LogIn);

        //Declarar las nuevas interfaces  
    }
}
