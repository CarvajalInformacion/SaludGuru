using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.Interfaces
{
    public interface ILogModel
    {
        /// <summary>
        /// Contiene el identificador del log.
        /// </summary>
        int IdLog { get; set; }

        /// <summary>
        /// Contiene el identificacor de un Usuario.
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// Acción registrada por el log.
        /// </summary>
        string LogAction { get; set; }

        /// <summary>
        /// Muestra si el proceso que se estaba ejecutando fue exitoso o no.
        /// </summary>
        int IsSuccessfull { get; set; }

        /// <summary>
        /// Mensaje de error por el cual se generó el log.
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
