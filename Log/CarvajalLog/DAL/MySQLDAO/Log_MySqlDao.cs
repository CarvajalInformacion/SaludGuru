using CarvajalLog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarvajalLog.DAL.MySQLDAO
{
    public class Log_MySqlDao : ILogData
    {
        private ADO.Interfaces.IADO DataInstance;

        public Log_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement("LogConnection");
        }

        /// <summary>
        /// Método que se encarga de almacenar un nuevo registro de log generado en la capa de mensajeria en la bd por medio de un sp.
        /// </summary>
        /// <param name="UserId">Identificación del usuario.</param>
        /// <param name="LogAction">Se registra la acción que generó el log.</param>
        /// <param name="IsSuccessfull">Muestra si la acción fue exitosa o no.</param>
        /// <param name="ErrorMessage">Contiene el mensaje de error del log.</param>
        /// <param name="MessageFrom">Origen del mensaje.</param>
        /// <param name="MessageTo">Destino del mensaje.</param>
        /// <param name="IdMessage">Identificación del mensaje.</param>
        public void SaveLogMessage(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage, string MessageFrom, string MessageTo, int IdMessage)
        {
            List<System.Data.IDbDataParameter> oParams = new List<System.Data.IDbDataParameter>();

            oParams.Add(DataInstance.CreateTypedParameter("LUserId", UserId));
            oParams.Add(DataInstance.CreateTypedParameter("LAction", LogAction));
            oParams.Add(DataInstance.CreateTypedParameter("LIsSuccessfull", IsSuccessfull));
            oParams.Add(DataInstance.CreateTypedParameter("LErrorMessage", ErrorMessage));
            oParams.Add(DataInstance.CreateTypedParameter("LMessageFrom", MessageFrom));
            oParams.Add(DataInstance.CreateTypedParameter("LMessageTo", MessageTo));
            oParams.Add(DataInstance.CreateTypedParameter("LIdMessage", IdMessage));

            ADO.Models.ADOModelRequest Query = new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "M_InsertLogMessage",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = oParams
            };

            ADO.Models.ADOModelResponse QueryResponse = DataInstance.ExecuteQuery(Query);            
        }

        /// <summary>
        /// Método que se encarga de almacenar un nuevo registro de log generado en la capa de autenticación en la bd por medio de un sp.
        /// </summary>
        /// <param name="UserId">Identificación del usuario.</param>
        /// <param name="LogAction">Acción que generó el log.</param>
        /// <param name="IsSuccessfull">Muestra si la acción fue exitosa o no.</param>
        /// <param name="ErrorMessage">Mensaje de error del log.</param>
        /// <param name="LogIn">Identificación del usuario que esta inciando sesion.</param>
        public void SaveLogAuth(int UserId, string LogAction, int IsSuccessfull, string ErrorMessage, int LogIn)
        {
            List<System.Data.IDbDataParameter> oParams = new List<System.Data.IDbDataParameter>();

            oParams.Add(DataInstance.CreateTypedParameter("", UserId));
            oParams.Add(DataInstance.CreateTypedParameter("", LogAction));
            oParams.Add(DataInstance.CreateTypedParameter("", IsSuccessfull));
            oParams.Add(DataInstance.CreateTypedParameter("", ErrorMessage));
            oParams.Add(DataInstance.CreateTypedParameter("", LogIn));

            ADO.Models.ADOModelRequest Query = new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "A_InsertLogAuth",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = oParams
            };

            ADO.Models.ADOModelResponse QueryResponse = DataInstance.ExecuteQuery(Query);
        }
    }
}
