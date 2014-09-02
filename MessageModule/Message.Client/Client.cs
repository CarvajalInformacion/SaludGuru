using Message.Client.DAL.Controller;
using Message.Client.Interfaces;
using Message.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Client
{
    public class Client : IMessageClient
    {
        #region Variables Globales
        MessageClientController _controller = new MessageClientController();

        private static Message.Client.Interfaces.IMessageClient oInstance;
        public static Message.Client.Interfaces.IMessageClient Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new Client();
                return oInstance;
            }
        }

        #endregion

        #region Métodos de envío de mensajes
        /// <summary>
        /// ´Método para enviar un mensaje
        /// </summary>
        /// <param name="MessageToCreate">Objeto que contiene la información correspondiente al msj</param>
        /// <returns>Objeto de resultados</returns>
        public Models.CreateMessageResponse CreateMessage(Models.CreateMessageRequest MessageToCreate)
        {
            #region Varibles
            int idResult = 0;
            #endregion
            Models.CreateMessageResponse mgResponse = new Models.CreateMessageResponse();
            List<ClientMessageParameter> RelatedParameter = new List<ClientMessageParameter>();

            idResult = this._controller.InsertMessageQueue(MessageToCreate.NewMessage.MessageType, MessageToCreate.NewMessage.ProgramTime, MessageToCreate.NewMessage.UserAction);

            foreach (ClientMessageParameter item in MessageToCreate.NewMessage.RelatedParameter)
            {
                this._controller.InsertMessageParameter(idResult, item.Key, item.Value);
            }
            return mgResponse;
        }

        /// <summary>
        /// Método para enviar varios mensajes
        /// </summary>
        /// <param name="lstMessageToCreate">Lista de mensajes para ser enviados</param>
        /// <returns>lista de resultados</returns>
        public List<Models.CreateMessageResponse> CreateMultipleMessage(List<Models.CreateMessageRequest> lstMessageToCreate)
        {
            #region Variables
            List<Models.CreateMessageResponse> responseList = new List<CreateMessageResponse>();
            #endregion
            foreach (CreateMessageRequest item in lstMessageToCreate)
                responseList.Add(this.CreateMessage(item));

            return responseList;
        } 
        #endregion
    }
}
