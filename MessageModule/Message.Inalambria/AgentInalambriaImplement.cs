using Message.DAL.MySqlDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using Message.Inalambria.ServiceInalambria;
using Message.Models;
using MessageModule.Controller;
using Message.Interfaces;

namespace Message.Inalambria
{
    public class AgentInalambriaImplement : IAgent
    {
        #region Funciones y variables Privadas
        /// <summary>
        /// Variable para el acceso a funciones DAL
        /// </summary>
        private MessageDataController _controller = new MessageDataController();

        /// <summary>
        /// Funcion que valida si la dirección de sms a la que se va a enviar existe, de no ser así, la crea.
        /// </summary>
        /// <param name="address">Dirección a validar</param>
        /// <param name="agent">Medio de envio(Inalambria, Infobip,...)</param>
        /// <returns>Lista de direcciones</returns>
        private List<AddressModel> UpsertAddress(string address, string agent)
        {
            List<AddressModel> addressList = new List<AddressModel>();
            return addressList = this._controller.UpsertAddress(address, agent);
        }

        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Función  que contiene la lógica para el envío del mensaje.
        /// </summary>
        /// <param name="MessageToSend">Mensaje que se va a enviar</param>
        /// <returns>Información del mensaje enviado </returns>
        public Models.MessageModel SendMessage(Models.AgentModel MessageToSend)
        {
            #region Variables

            List<AddressModel> addresList = new List<AddressModel>();
            MessageModel modelToreturn = new MessageModel();
            bool sent = false;
            #endregion
            //validar todas las direcciones
            List<QueueParameterModel> mess = MessageToSend.QueueItemToProcess.MessageParameters.Where(x => x.Key == "TO").ToList();
            if (mess.Count() != 0)
                addresList = this.UpsertAddress(mess.FirstOrDefault().Value, MessageToSend.MessageConfig["Agent"]);
            else
                return null;
            
            foreach (AddressModel item in addresList)
            {
                ServiceSendSoapClient sendSMS = new ServiceSendSoapClient("ServiceSendSoap12");
                sent = sendSMS.SendWithUser(MessageToSend.AgentConfig["UsrSMSService"], MessageToSend.AgentConfig["PswSMSService"], item.Address, MessageToSend.MessageConfig["Body"], null, "1").Status;

                modelToreturn.RelatedAddress = new List<AddressModel>();
                modelToreturn.RelatedAddress.Add(item);

                //Actualiza la cola
                if (!this._controller.CreateQueueProcess(MessageToSend.QueueItemToProcess.MessageQueueId, true, "", MessageToSend.QueueItemToProcess.MessageType, MessageToSend.MessageConfig["Agent"].ToString(), MessageToSend.MessageConfig["Body"], item.AddressId))
                {
                    //TODO: Colocar la lógica para los logs;
                }
            }

            modelToreturn.Agent = addresList.FirstOrDefault().Agent;
            modelToreturn.BodyMessage = MessageToSend.MessageConfig["Body"];
            modelToreturn.MessageType = MessageToSend.MessageConfig["Agent"];
            modelToreturn.TimeSent = DateTime.Now;
            return modelToreturn;
        }

        /// <summary>
        /// Funcion que envia el id del mensaje que va a ser enviado
        /// </summary>
        /// <param name="MessageProcessId">id del proceso</param>
        public void AddResend(int MessageProcessId)
        {
            this._controller.AddToResendMsj(MessageProcessId);
        }
        #endregion
    }
}
