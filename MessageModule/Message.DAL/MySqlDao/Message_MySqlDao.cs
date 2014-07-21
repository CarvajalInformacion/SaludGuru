using Message.Interfaces;
using Message.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Message.DAL.MySqlDao
{
    public class Message_MySqlDao : IMessageData
    {
        private ADO.Interfaces.IADO DataInstance;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Message_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement("MsgConnection");
        }

        /// <summary>
        /// Funcion que obtiene todos los mensajes con sus respectivos parametros 
        /// </summary>
        /// <returns>Lista de mensajes</returns>
        public List<MessageQueueModel> GetQueueMessage()
        {
            #region Variables
            List<MessageQueueModel> modelList = null;
            #endregion
            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Mg_GetAll_B_MessageQueue",
                CommandType = System.Data.CommandType.StoredProcedure
            });

            if (response.DataTableResult != null &&
                response.DataTableResult.Rows.Count > 0)
            {
                modelList = new List<MessageQueueModel>();
                modelList = (from l in response.DataTableResult.AsEnumerable()
                             where !l.IsNull("MessageQueueId")
                             group l by
                             new
                             {
                                 MessageQueueId = l.Field<int>("MessageQueueId"),
                                 MessageType = l.Field<string>("MessageType"),
                                 ProgramTime = l.Field<DateTime>("ProgramTime"),
                                 UserAction = l.Field<string>("UserAction"),
                                 CreateDate = l.Field<DateTime>("CreateDate"),
                             } into lig
                             select new MessageQueueModel()
                             {
                                 MessageQueueId = lig.Key.MessageQueueId,
                                 MessageType = lig.Key.MessageType,
                                 ProgramTime = lig.Key.ProgramTime,
                                 UserAction = lig.Key.UserAction,
                                 CreateDate = lig.Key.CreateDate,
                                 MessageParameters = (from ui in response.DataTableResult.AsEnumerable()
                                                      where !ui.IsNull("MessageParameterId") && ui.Field<int>("MessageQueueId") == lig.Key.MessageQueueId
                                                      group ui by
                                                      new
                                                      {
                                                          MessageParameterId = ui.Field<int>("MessageParameterId"),
                                                          Key = ui.Field<string>("ItemKey"),
                                                          Value = ui.Field<string>("Value"),
                                                          CreateDate = ui.Field<DateTime>("CreateDate"),

                                                      } into mgpa
                                                      select new QueueParameterModel
                                                      {
                                                          MessageParameterId = mgpa.Key.MessageParameterId,
                                                          Key = mgpa.Key.Key,
                                                          Value = mgpa.Key.Value,
                                                          CreateDate = mgpa.Key.CreateDate,
                                                      }).ToList(),
                             }).ToList();
            }
            return modelList;
        }

        /// <summary>
        /// Método que valida si la direccion se encuentra dentro de la lista negra para su Medio de envío
        /// </summary>
        /// <param name="Address">Direccion a validar</param>
        /// <param name="AgentWay">Medio de envio (MailMan, Inhalambria, )</param>
        /// <returns>Si esta en la lista negra ó no</returns>
        public List<AddressModel> UpsertAddress(string Address, string AgentWay)
        {
            List<string> addressList = new List<string>();
            List<System.Data.IDbDataParameter> oParams = new List<System.Data.IDbDataParameter>();

            oParams.Add(DataInstance.CreateTypedParameter("PAddress", Address));
            oParams.Add(DataInstance.CreateTypedParameter("PAgent", AgentWay));

            ADO.Models.ADOModelRequest Query = new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Mg_UpSert_P_Address",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = oParams
            };

            List<AddressModel> oReturn = new List<AddressModel>();
            ADO.Models.ADOModelResponse QueryResponse = DataInstance.ExecuteQuery(Query);

            if (QueryResponse.DataTableResult != null &&
                QueryResponse.DataTableResult.Rows.Count > 0)
            {
                oReturn = (from ui in QueryResponse.DataTableResult.AsEnumerable()
                           where !ui.IsNull("Address")
                           group ui by
                           new
                           {
                               AddressId = ui.Field<int>("AddressId"),
                               Address = ui.Field<string>("Address"),
                               IsBlackList = ui.Field<Int64>("IsBlackList"),
                               Agent = ui.Field<string>("Agent"),
                           } into uig
                           select new AddressModel()
                             {
                                 AddressId = uig.Key.AddressId,
                                 Address = uig.Key.Address,
                                 IsBlackList = (int)uig.Key.IsBlackList,
                                 Agent = uig.Key.Agent,
                             }).ToList();
            }
            return oReturn;
        }

        /// <summary>
        /// Función que se encarga de actualizar las tablas de procesos
        /// </summary>
        /// <param name="MessageQueueId">Id del mensaje de la cola</param>        
        /// <param name="ProcessInfo">Informacion general del proceso</param>
        /// <param name="MessageType">Tipo de mensaje que se va a envíar</param>
        /// <param name="Agent">Medio que se va a utilizar para el envio</param>
        /// <param name="BodyMessage">El mensaje</param>
        /// <param name="IdAdrress">Identificación de la Dirección de destino</param>
        /// <returns>Si el proceso se realizó correctamente</returns>
        public bool CreateQueueProcess(int MessageQueueId, bool IsSuccess, string ProcessInfo, string MessageType, string Agent, string BodyMessage, int IdAdrress)
        {
            #region Parametros
            int idResult = 0;

            List<System.Data.IDbDataParameter> oParams = new List<System.Data.IDbDataParameter>();

            oParams.Add(DataInstance.CreateTypedParameter("PMessageQueueId", MessageQueueId));
            oParams.Add(DataInstance.CreateTypedParameter("PIsSuccess", IsSuccess));
            oParams.Add(DataInstance.CreateTypedParameter("PProcessInfo", ProcessInfo));
            oParams.Add(DataInstance.CreateTypedParameter("PMessageType", MessageType));
            oParams.Add(DataInstance.CreateTypedParameter("PAgent", Agent));
            oParams.Add(DataInstance.CreateTypedParameter("PBodyMessage", BodyMessage));
            oParams.Add(DataInstance.CreateTypedParameter("PIdAddress", IdAdrress));

            #endregion
            ADO.Models.ADOModelRequest Query = new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Mg_P_CreateProcessQueue",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = oParams
            };

            ADO.Models.ADOModelResponse QueryResponse = DataInstance.ExecuteQuery(Query);
            idResult = Convert.ToInt32(QueryResponse.NonQueryResult);

            if (idResult != 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Función que envía el mensaje a la tabla resend por sino se pudo enviar.
        /// </summary>
        /// <param name="MessageToSend">Mensaje que se va a enviar a la cola</param>     
        public void AddToResendMsj(int QueueProcessId)
        {
            #region Parametros
            List<System.Data.IDbDataParameter> oParams = new List<System.Data.IDbDataParameter>();
            oParams.Add(DataInstance.CreateTypedParameter("PIdQueueProcess", QueueProcessId));
            #endregion

            ADO.Models.ADOModelRequest Query = new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Mg_Insert_P_Resend",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = oParams
            };
        }

        /// <summary>
        /// Función que obtiene todos los mensajes para ser reenviados
        /// </summary>
        /// <returns>Lista de Mensajes</returns>
        public List<int> GetAllMessageToResend()
        {
           List<int> idReturnList = new List<int>();
            List<System.Data.IDbDataParameter> oParams = new List<System.Data.IDbDataParameter>();

            ADO.Models.ADOModelRequest Query = new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.DataTable,
                CommandText = "Mg_P_GetAllFormResend",
                CommandType = System.Data.CommandType.StoredProcedure
            };
            
            ADO.Models.ADOModelResponse QueryResponse = DataInstance.ExecuteQuery(Query);
            
            if (QueryResponse.DataTableResult != null &&
                QueryResponse.DataTableResult.Rows.Count > 0)
            {
                idReturnList = (from ui in QueryResponse.DataTableResult.AsEnumerable()
                                where !ui.IsNull("QueueProcessId")
                           group ui by
                           new
                           {
                               QueueProcessId = ui.Field<int>("QueueProcessId"),                              
                           } into uig
                           select new int()).ToList();
            }
            return idReturnList;
        }
    }
}
