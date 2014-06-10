using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.Models
{
    public class MessageLogModel : CarvajalLog.Interfaces.ILogModel
    {
        #region << Metodos propios de LogMessage >>
        /// <summary>
        /// Método que contiene la información perteneciente al origen del mensaje.
        /// </summary>
        public string MessageFrom { get; set; }

        /// <summary>
        /// Método que contiene la información perteneciente al destinatario del mensaje.
        /// </summary>
        public string MessageTo { get; set; }

        /// <summary>
        /// Método que contiene la información de identificación del mensaje.
        /// </summary>
        public int IdMessage { get; set; }

        #endregion
        #region << Metodos implementados de Log General >>
        /// <summary>
        /// Método que contiene la identificación del log que se almacena.
        /// </summary>
        public int IdLog { get; set; }

        /// <summary>
        /// Método que contiene información del usuario que esta registrando la tarea.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Método que contiene la acción del log.
        /// </summary>
        public string LogAction { get; set; }

        /// <summary>
        /// Método que contiene el resultado de la acción ejecutada (exitoso o no).
        /// </summary>
        public int IsSuccessfull { get; set; }

        /// <summary>
        /// Método que contiene el mensaje de error correspondiente al log.
        /// </summary>
        public string ErrorMessage { get; set; }

        #endregion
    }
}
