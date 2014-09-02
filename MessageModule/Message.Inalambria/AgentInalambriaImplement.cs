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
           
            foreach (AddressModel item in MessageToSend.AddressToSend)
            {               
                ServiceSendSoapClient sendSMS = new ServiceSendSoapClient("ServiceSendSoap12");
                sent = sendSMS.SendWithUser(MessageToSend.AgentConfig["UsrSMSService"], MessageToSend.AgentConfig["PswSMSService"], item.Address, MessageToSend.MessageConfig["Body"], null, "1").Status;

                modelToreturn.RelatedAddress = new List<AddressModel>();
                modelToreturn.RelatedAddress.Add(item);

                if (sent)
                {
                    modelToreturn.isSuccess = true;
                    modelToreturn.MessageResult = "Message added correctly to the system queue";                 
                }
                else
                {                    
                    modelToreturn.isSuccess = false;
                    modelToreturn.MessageResult = "The message could not be sent to the queue system";
                }               
            }

            modelToreturn.Agent = MessageToSend.AddressToSend.FirstOrDefault().Agent;
            modelToreturn.BodyMessage = MessageToSend.MessageConfig["Body"];
            modelToreturn.MessageType = MessageToSend.MessageConfig["Agent"];
            modelToreturn.TimeSent = DateTime.Now;
            return modelToreturn;
        }       
        #endregion


        public void AddResend(int MessageProcessId)
        {
            throw new NotImplementedException();
        }
    }
}
