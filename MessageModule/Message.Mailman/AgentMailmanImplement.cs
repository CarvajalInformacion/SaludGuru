using Message.DAL.MySqlDao;
using System.Configuration;
using Message.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carvajal.Mail;
using System.Messaging;
using System.Threading.Tasks;
using MessageMailman = Carvajal.Mail.Message;
using MessageModule.Controller;
using Message.Interfaces;
using System.IO;

namespace Message.Mailman
{
    public class AgentMailmanImplement : IAgent
    {
        #region Funciones y variables privadas

        /// <summary>
        /// Variable para el acceso a funciones DAL
        /// </summary>
        private MessageDataController _controller = new MessageDataController();

        /// <summary>
        /// Función  que contiene la lógica para el envío del mensaje.
        /// </summary>
        /// <param name="MessageToSend">Mensaje que se va a enviar</param>
        /// <returns>Información del mensaje enviado </returns>
        public MessageModel SendMessage(AgentModel MessageToSend)
        {
            #region Variables

            List<AddressModel> addresList = new List<AddressModel>();
            MessageModel modelToreturn = new MessageModel();
            #endregion
            //validar todas las direcciones
            List<QueueParameterModel> mess = MessageToSend.QueueItemToProcess.MessageParameters.Where(x => x.Key == "TO").ToList();
            if (mess.Count() == 0)
            {
                return null;
            }

            addresList = this.UpsertAddress(mess.FirstOrDefault().Value, MessageToSend.MessageConfig["Agent"]);

            foreach (AddressModel item in addresList)
            {
                if (string.IsNullOrEmpty(item.Address))
                {
                    var mailMessage = new MessageMailman
                    {
                        From = new Address(MessageToSend.MessageConfig["From"]),
                        Subject = MessageToSend.MessageConfig["Subject"],
                        Body = MessageToSend.MessageConfig["Body"],
                        IsBodyHtml = true,
                        RequestId = Guid.NewGuid().ToString().ToUpper()
                    };
                    //Actualiza la cola
                    this._controller.CreateQueueProcess(MessageToSend.QueueItemToProcess.MessageQueueId, false, "La direccion de correo electronico esta vacia", MessageToSend.QueueItemToProcess.MessageType, MessageToSend.MessageConfig["Agent"].ToString(), mailMessage.Body, item.AddressId);
                }
                else
                {
                    var messageQueue = new MessageQueue();
                    messageQueue.Path = MessageToSend.MessageConfig["MailmanPath"];
                    messageQueue.Formatter = new XmlMessageFormatter(new[] { typeof(MessageMailman) });
                    var mailMessage = new MessageMailman();
                    try
                    {
                        mailMessage.From = new Address(MessageToSend.MessageConfig["From"]);
                        mailMessage.To = new Address(item.Address);
                        mailMessage.Subject = MessageToSend.MessageConfig["Subject"];
                        mailMessage.Body = MessageToSend.MessageConfig["Body"];
                        mailMessage.IsBodyHtml = true;
                        mailMessage.RequestId = Guid.NewGuid().ToString().ToUpper();

                        modelToreturn.RelatedAddress = new List<AddressModel>();
                        modelToreturn.RelatedAddress.Add(item);
                        string FileName = ConfigurationManager.AppSettings["LogFile"].TrimEnd('/') + "/Log_" + DateTime.Now.ToString("yyyyMMddHHss") + ".txt";

                        File.AppendAllText(FileName, "se supone q voy a entre a la cola");

                        //Envia a la cola de Mailman
                        messageQueue.Send(mailMessage, MessageQueueTransactionType.Single);
                        if (!Directory.Exists(ConfigurationManager.AppSettings["LogFile"]))
                            Directory.CreateDirectory(ConfigurationManager.AppSettings["LogFile"]);

                        File.AppendAllText(FileName, "se supone q ya entre a la cola");

                       //Actualiza la cola
                        this._controller.CreateQueueProcess(MessageToSend.QueueItemToProcess.MessageQueueId, true, "Message added to the queue correctly", MessageToSend.QueueItemToProcess.MessageType, MessageToSend.MessageConfig["Agent"].ToString(), mailMessage.Body, item.AddressId);
                    }
                    catch (Exception err)
                    {
                        try
                        {
                            if (!Directory.Exists(ConfigurationManager.AppSettings["LogFile"]))
                                Directory.CreateDirectory(ConfigurationManager.AppSettings["LogFile"]);

                            string FileName = ConfigurationManager.AppSettings["LogFile"].TrimEnd('/') + "/Log_" + DateTime.Now.ToString("yyyyMMddHHss") + ".txt";

                            File.AppendAllText(FileName,
                                err.Message + "::" + err.StackTrace);

                            if (err.InnerException != null)
                                File.AppendAllText(FileName,
                                    err.InnerException.Message + "::" + err.InnerException.StackTrace);
                        }
                        catch { }
                        this._controller.CreateQueueProcess(MessageToSend.QueueItemToProcess.MessageQueueId, false, err.Message, MessageToSend.QueueItemToProcess.MessageType, MessageToSend.MessageConfig["Agent"].ToString(), mailMessage.Body, item.AddressId);
                    }
                }
            }
            modelToreturn.Agent = addresList.FirstOrDefault().Agent;
            modelToreturn.BodyMessage = MessageToSend.MessageConfig["Body"];
            modelToreturn.MessageType = MessageToSend.MessageConfig["Subject"];
            modelToreturn.TimeSent = DateTime.Now;
            return modelToreturn;
        }
        #endregion

        #region Funciones Publicas

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
