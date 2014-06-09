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
        /// 
        /// </summary>
        int IdLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string LogAction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int IsSuccessfull { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
