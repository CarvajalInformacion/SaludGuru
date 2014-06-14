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

        public void CreateMessage()
        {
            throw new NotImplementedException();
        }

        public void CreateMessageByAddress()
        {
            throw new NotImplementedException();
        }

        public void CreateQueueProcess()
        {
            throw new NotImplementedException();
        }

        public void RemoveResend()
        {
            throw new NotImplementedException();
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
                               Address = ui.Field<string>("Address"),
                               IsBlackList = ui.Field<Int64>("IsBlackList"),
                               Agent = ui.Field<string>("Agent"),
                           } into uig
                           select new AddressModel()
                             {
                                 Address = uig.Key.Address,
                                 IsBlackList = (int)uig.Key.IsBlackList,
                                 Agent = uig.Key.Agent,
                             }).ToList();
            }
            return oReturn;
        }
    }
}
