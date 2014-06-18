using Message.Interfaces;
using Message.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Controller
{
    public class MessageDataController : IMessageData
    {
        #region Instancia Singleton
        private static IMessageData oInstance;

        public static IMessageData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new MessageDataController();
                return oInstance;
            }
        }

        private IMessageData DataFactory;
        #endregion

        public MessageDataController()
        {
            MessageDataFactory factory = new MessageDataFactory();
            DataFactory = factory.GetDataInstance();
        }

        /// <summary>
        /// Funcion que obtiene todos los mensajes con sus respectivos parametros 
        /// </summary>
        /// <returns>Lista de mensajes</returns>
        public List<Message.Models.MessageQueueModel> GetQueueMessage()
        {
            return this.DataFactory.GetQueueMessage();
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
            return this.DataFactory.CreateQueueProcess(MessageQueueId, IsSuccess, ProcessInfo, MessageType, Agent, BodyMessage, IdAdrress);
        }

        /// <summary>
        /// Funcion que valida si la dirección de sms a la que se va a enviar existe, de no ser así, la crea.
        /// </summary>
        /// <param name="address">Dirección a validar</param>
        /// <param name="agent">Medio de envio(Inalambria, Infobip,...)</param>
        /// <returns>Lista de direcciones</returns>
        public List<AddressModel> UpsertAddress(string Address, string AggentWay)
        {
            return this.DataFactory.UpsertAddress(Address, AggentWay);
        }

        /// <summary>
        /// Función que envía el mensaje a la tabla resend por sino se pudo enviar.
        /// </summary>
        /// <param name="MessageToSend">Mensaje que se va a enviar a la cola</param>     
        public void AddToResendMsj(int QueueProcessId)
        {
            this.DataFactory.AddToResendMsj(QueueProcessId);
        }


        public List<int> GetAllMessageToResend()
        {
            throw new NotImplementedException();
        }
    }
}
