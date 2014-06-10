using CarvajalLog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.DAL.Controller
{
    public class LogDataFactory
    {
        /// <summary>
        /// Método que obtiene el tipo de instancia que se va a ejecutar.
        /// </summary>
        /// <returns></returns>
        public ILogData GetLogInstance()
        {
            Type typetoreturn = Type.GetType("CarvajalLog.DAL.MySQLDAO.Log_MySqlDao,CarvajalLog");
            ILogData oRetorno = (ILogData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
