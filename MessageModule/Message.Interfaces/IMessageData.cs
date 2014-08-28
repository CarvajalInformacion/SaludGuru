using Message.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Interfaces
{
    public interface IMessageData
    {
        #region Bag

        /// <summary>
        /// Funcion que obtiene todos los mensajes con sus respectivos parametros 
        /// </summary>
        /// <returns>Lista de mensajes</returns>
        List<MessageQueueModel> GetQueueMessage();

        /// <summary>
        /// Funcion que obtiene todos los mensajes
        /// </summary>
        /// <returns>Lista de mensajes</returns>
        List<MessageQueueModel> GetQueueMessageToInspect();
        #endregion

        #region ProcessMessage

        /// <summary>
        /// Funcion que valida si la dirección de sms a la que se va a enviar existe, de no ser así, la crea.
        /// </summary>
        /// <param name="address">Dirección a validar</param>
        /// <param name="agent">Medio de envio(Inalambria, Infobip,...)</param>
        /// <returns>Lista de direcciones</returns>
        List<AddressModel> UpsertAddress(string Address, string AggentWay);            

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
        bool CreateQueueProcess(int MessageQueueId, bool IsSuccess, string ProcessInfo, string MessageType, string Agent, string BodyMessage, int IdAdrress);
                
        /// <summary>
        /// Función que envía el mensaje a la tabla resend por sino se pudo enviar.
        /// </summary>
        /// <param name="MessageToSend">Mensaje que se va a enviar a la cola</param>       
        void AddToResendMsj(int QueueProcessId);

        /// <summary>
        /// Función que obtiene todos los mensajes para ser reenviados
        /// </summary>
        /// <returns>Lista de Mensajes</returns>
        List<int> GetAllMessageToResend();
        #endregion
    }
}
