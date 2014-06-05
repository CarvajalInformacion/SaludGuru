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
        public ILogData GetLogInstance()
        {
            Type typetoreturn = Type.GetType("CarvajalLog.DAL.MySQLDAO.Log_MySqlDao,CarvajalLog.DAL.MySQLDAO");
            ILogData oRetorno = (ILogData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
