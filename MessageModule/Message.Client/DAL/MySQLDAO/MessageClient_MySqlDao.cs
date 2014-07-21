using Message.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client.DAL.MySQLDAO
{
    public class MessageClient_MySqlDao : IClientMessageData
    {
        private ADO.Interfaces.IADO DataInstance;

        /// <summary>
        /// Instancia de la conexión
        /// </summary>
        public MessageClient_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement("MsgConnection");
        }

        /// <summary>
        /// Método que permite guardar mensajes en la cola
        /// </summary>
        /// <param name="MessageType">Tipo de Mensaje</param>
        /// <param name="ProgramType">Programacion</param>
        /// <param name="UserAction">Usuario quien envía</param>
        public int InsertMessageQueue(string MessageType, DateTime ProgramType, string UserAction)
        {
            #region Variables
		    int idResult = 0;
	        #endregion

            List<System.Data.IDbDataParameter> oParams = new List<System.Data.IDbDataParameter>();

            oParams.Add(DataInstance.CreateTypedParameter("PMessageType", MessageType));
            oParams.Add(DataInstance.CreateTypedParameter("PProgramTime", ProgramType));
            oParams.Add(DataInstance.CreateTypedParameter("PUserAction", UserAction));

            ADO.Models.ADOModelRequest Query = new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "Mg_Insert_B_MessageQueue",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = oParams
            };

            ADO.Models.ADOModelResponse QueryResponse = DataInstance.ExecuteQuery(Query);
            return idResult = Convert.ToInt32(QueryResponse.ScalarResult);
        }

        /// <summary>
        /// Función que permite insertar información en la tabla de parametros
        /// </summary>
        /// <param name="MessageQueueId">Id de la cola</param>
        /// <param name="Key">Llave de lo que se va a envíar</param>
        /// <param name="Value">Valor de la llave</param>
        public void InsertMessageParameter(int MessageQueueId, string Key, string Value)
        {
            List<System.Data.IDbDataParameter> oParams = new List<System.Data.IDbDataParameter>();

            oParams.Add(DataInstance.CreateTypedParameter("PMessageQueueId", MessageQueueId));
            oParams.Add(DataInstance.CreateTypedParameter("PKey", Key));
            oParams.Add(DataInstance.CreateTypedParameter("PValue", Value));

            ADO.Models.ADOModelRequest Query = new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.NonQuery,
                CommandText = "Mg_Insert_B_MessageParameter",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = oParams
            };

            ADO.Models.ADOModelResponse QueryResponse = DataInstance.ExecuteQuery(Query);
        }
    }
}
