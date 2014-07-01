using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.Models
{
    public class AuthLogModel : CarvajalLog.Interfaces.ILogModel
    {
        #region << Metodos propios de Log Auth >>
        
        /// <summary>
        /// Método que retorna información de identificación del usuario.
        /// </summary>
        public string IdUsuario { get; set; }

        #endregion
        #region << Metodos implementados de Log General >>

        /// <summary>
        /// Método que contiene el identificador del log.
        /// </summary>
        public int IdLog { get; set; }

        /// <summary>
        /// Método que contiene la identificación del usuario.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Método que contiene la acción registrada en el log.
        /// </summary>
        public string LogAction { get; set; }

        /// <summary>
        /// Método en el que se muestra si la acción registrada en el log fue exitosa o no.
        /// </summary>
        public int IsSuccessfull { get; set; }

        /// <summary>
        /// Método que contiene el mensaje de error para registrarlo en el log.
        /// </summary>
        public string ErrorMessage { get; set; }

        #endregion
    }
}
