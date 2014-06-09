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
        /// 
        /// </summary>
        public string IdUsuario { get; set; }

        #endregion
        #region << Metodos implementados de Log General >>

        /// <summary>
        /// 
        /// </summary>
        public int IdLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LogAction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IsSuccessfull { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }

        #endregion
    }
}
