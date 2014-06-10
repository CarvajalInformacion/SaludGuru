using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.Interfaces
{
    public interface ILog
    {
        /// <summary>
        /// Método de interfaz de almacenado de un nuevo log.
        /// </summary>
        /// <param name="NewLog">Ingreso de un nuevo Log.</param>
        void SaveLog(ILogModel NewLog);
    }
}
