using BitlyDotNET.Interfaces;
using Message.Interfaces;
using Message.Models;
using Message.Notifications;
using MessageModule.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Message.Manager
{
    public class MessageProcess
    {
        #region Variables Privadas
        /// <summary>
        /// Variable para el acceso a funciones DAL
        /// </summary>
        private MessageDataController _controller = new MessageDataController();
        #endregion

        #region Funciones Publicas

        /// <summary>
        /// Función que inicia el proceso
        /// </summary>
        public void StartProcess()
        {
            //read queue
            List<MessageQueueModel> messageQueueList = new List<MessageQueueModel>();
            messageQueueList = this._controller.GetQueueMessage();
            if (messageQueueList != null)
            {
                if (messageQueueList.Count > 0)
                {
                    foreach (MessageQueueModel item in messageQueueList)
                    {
                        try
                        {
                            this.ProcessMessage(item);
                         }
                        catch (Exception err)
                        {
                            Console.WriteLine(err.Message);
                            Console.WriteLine(err.StackTrace);

                            if (err.InnerException != null)
                            {
                                Console.WriteLine(err.InnerException.Message);
                                Console.WriteLine(err.InnerException.StackTrace);
                            }
                            Console.ReadLine();
                        }
                    }
                }
            }
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

        #region Funciones Privadas

        /// <summary>
        /// Funcion que se encarga de llamar los metodos que desarrollan el proceso de envío
        /// </summary>
        /// <param name="QueueItemToProcess">Mensaje para enviar, tomado de la cola</param>
        /// <returns>Modelo con info del mensaje enviado</returns>
        private Message.Models.MessageModel ProcessMessage(Message.Models.MessageQueueModel QueueItemToProcess)
        {
            MessageModel oMsjReturn = new MessageModel();
            List<AddressModel> addresList = new List<AddressModel>();
            //validate config
            Message.Models.AgentModel oAgentConfig = new AgentModel()
            {
                QueueItemToProcess = QueueItemToProcess,
                MessageConfig = this.GetMessageConfig(QueueItemToProcess),
            };
            oAgentConfig.AgentConfig = GetAgentConfig(oAgentConfig.MessageConfig["Agent"]);

            //invoque message agent
            Message.Interfaces.IAgent Agent = this.GetAgentInstance(oAgentConfig.AgentConfig["Assemblie"]);

            List<QueueParameterModel> mess = oAgentConfig.QueueItemToProcess.MessageParameters.Where(x => x.Key == "TO").ToList();
            
            //Save the address and split the directions
            addresList = this.UpsertAddress(mess.FirstOrDefault().Value, oAgentConfig.MessageConfig["Agent"]);

            oAgentConfig.AddressToSend = new List<AddressModel>();
            oAgentConfig.AddressToSend = addresList.Where(x => !string.IsNullOrEmpty(x.Address)).Select(x => x).ToList();

            if (!string.IsNullOrEmpty(oAgentConfig.QueueItemToProcess.MessageParameters.Where(x => x.Key == "SendToProcessRelatedMsj").Select(x => x.Value).FirstOrDefault()))
            {
                List<MessageQueueModel> messageToInspect = new List<MessageQueueModel>();
                messageToInspect = this._controller.GetQueueMessageToInspect();

                foreach (MessageQueueModel item in messageToInspect)
	            {
                    item.MessageParameters.Where(x => x.Key == "AppointmentPublicId" && x.Value == oAgentConfig.AgentConfig["AppointmentPublicId"]).Select(x => x);
                    if (item != null)
                        this.SendToProcessWhitOutAgent(item, oAgentConfig.AddressToSend.FirstOrDefault().AddressId);                                        
	            }                
            }
            else
            {
                oMsjReturn = Agent.SendMessage(oAgentConfig);
                foreach (var item in oMsjReturn.RelatedAddress)
                {
                    this.sendProcessMessage(QueueItemToProcess.MessageQueueId,
                         oMsjReturn.isSuccess
                         , oMsjReturn.MessageResult
                         , QueueItemToProcess.MessageType
                         , oAgentConfig.MessageConfig["Agent"]
                         , oAgentConfig.MessageConfig["Body"]
                         , item.AddressId);

                    if (!oMsjReturn.isSuccess)
                    {
                        this.AddResend(QueueItemToProcess.MessageQueueId);
                    }
                }
            }           

            /*trace log message*/
            return oMsjReturn;
        }

        /// <summary>
        /// Función que obtiene la configuración desde el XML de acuerdo al tipo de mensaje
        /// </summary>
        /// <param name="MessageType">Tipo de mensaje a consultar</param>
        /// <returns>Llave y valor de la correspondiente configuración</returns>
        private Dictionary<string, string> GetMessageConfig(MessageQueueModel infoMessage)
        {
            XDocument xDocMessType = XDocument.Parse(SettingsManager.SettingsController.SettingsInstance.ModulesParams["Message"]["Message_Params_" + infoMessage.MessageType].Value);
            Dictionary<string, string> MessageConfig = xDocMessType.Descendants("Message").Descendants("key").ToDictionary(k => k.Attribute("name").Value, v => v.Value);
            string xDocMessBody = SettingsManager.SettingsController.SettingsInstance.ModulesParams["Message"]["Message_Body_" + infoMessage.MessageType].Value;

            switch (infoMessage.MessageType)
            {
                case "Email_AsignedAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Email_MPAsignedAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Sms_AsignedAppointment":
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Sms_MPAsignedAppointment":
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;

                //Cancelacion cita
                case "Email_CancelAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Reason}", infoMessage.MessageParameters.Where(x => x.Key == "Reason").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Sms_CancelAppointment":
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    string profileUrl = infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault();
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", ShortURL(profileUrl));
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Email_ModifyAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ModifyAppDetailLink}", infoMessage.MessageParameters.Where(x => x.Key == "ModifyAppDetailLink").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Sms_ModifyAppointment":
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ModifyAppDetailLink}", infoMessage.MessageParameters.Where(x => x.Key == "ReescheduleLink").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Email_ReminderAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ConfirmLink}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentPublicId").Select(x => !string.IsNullOrEmpty(x.Value) ? "https://admin.saludguru.com.co/ExternalAppointment/Confirm?AppointmentPublicId=" + x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{CancelLink}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentPublicId").Select(x => !string.IsNullOrEmpty(x.Value) ? "https://admin.saludguru.com.co/ExternalAppointment/Cancel?AppointmentPublicId=" + x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{BeforeCare}", infoMessage.MessageParameters.Where(x => x.Key == "BeforeCare").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Sms_ReminderAppointment":
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    string urlShort = infoMessage.MessageParameters.Where(x => x.Key == "ConfirmCancelLink").Select(x => !string.IsNullOrEmpty(x.Value) ? "https://admin.saludguru.com.co/ExternalAppointment/Index?AppointmentPublicId=" + x.Value : string.Empty).FirstOrDefault();
                    xDocMessBody = xDocMessBody.Replace("{ConfirmCancelLink}", ShortURL(urlShort));
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Email_ReminderNextAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Sms_ReminderNextAppointment":
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;

                //Notifications
                case "GuruNotification_CancelAppointment":
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Reason}", infoMessage.MessageParameters.Where(x => x.Key == "Reason").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "GuruNotification_AsignedAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "GuruNotification_ModifyAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "GuruNotification_ReminderAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{BeforeCare}", infoMessage.MessageParameters.Where(x => x.Key == "BeforeCare").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "GuruNotification_ReminderNextAppointment":
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                //Market Place
                case "GuruNotification_MPAsignedAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "GuruNotification_MPConfirmedAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "GuruNotification_MPCancelAppointment":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficeAddress}", infoMessage.MessageParameters.Where(x => x.Key == "OfficeAddress").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{OfficePhone}", infoMessage.MessageParameters.Where(x => x.Key == "OfficePhone").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Reason}", infoMessage.MessageParameters.Where(x => x.Key == "Reason").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "GuruNotification_NewPatient":
                    xDocMessBody = xDocMessBody.Replace("{PatientName}", infoMessage.MessageParameters.Where(x => x.Key == "PatientName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());                   
                    break;
            }
            MessageConfig.Add("Body", xDocMessBody);
            return MessageConfig;
        }

        /// <summary>
        /// Función que obtiene la configuración del agente de envio
        /// </summary>
        /// <param name="QueueItemToProcess">Nombre del agente del cual se requiere la configuración</param>
        /// <returns>Llave y valor correspondiente al agente</returns>
        private Dictionary<string, string> GetAgentConfig(string AgentName)
        {
            XDocument xDoc = XDocument.Parse(SettingsManager.SettingsController.SettingsInstance.ModulesParams["Message"][AgentName].Value);
            Dictionary<string, string> AgentConfig = xDoc.Descendants("Agent").Descendants("key").ToDictionary(k => k.Attribute("name").Value, v => v.Value);

            return AgentConfig;
        }

        /// <summary>
        /// Función que levanta una instancia de una fabrica de acuerdo al assembli devuelto en la configuración
        /// </summary>
        /// <param name="AgentType">Tipo de agente de envío</param>
        /// <returns>Instancia de mi interfaz agente</returns>
        private Message.Interfaces.IAgent GetAgentInstance(string AgentType)
        {
            Type typetoreturn = Type.GetType(AgentType.Replace(" ", ""));
            Message.Interfaces.IAgent oRetorno = (Message.Interfaces.IAgent)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }

        /// <summary>
        /// Funcion que utiliza Bitly para acortar la url y la devuelve
        /// </summary>
        /// <param name="link">Url que quiero acortar</param>
        /// <returns>Url Corta</returns>
        static string ShortURL(string link)
        {
            try
            {
                string user = SettingsManager.SettingsController.SettingsInstance.ModulesParams["Message"]["UsrBitly"].Value;
                string key = SettingsManager.SettingsController.SettingsInstance.ModulesParams["Message"]["KeyBitly"].Value;
                IBitlyService service = new BitlyDotNET.Implementations.BitlyService(user, key);
                if (string.IsNullOrEmpty(link))
                {
                    return string.Empty;
                }
                string shortened;
                return service.Shorten(link, out shortened) == StatusCode.OK ? shortened : link;
            }
            catch
            {   // para implementar log de errores
                return link;
            }
        }

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
        /// Funcion que procesa el mensaje
        /// </summary>
        /// <param name="MessageQueueId">Id del mensaje a procesar</param>
        /// <param name="isOk">Si se realizo con exito</param>
        /// <param name="MessageResult">Mensaje que desea guardar en el proceso</param>
        /// <param name="MessageType">Typo de mensaje</param>
        /// <param name="Agent">Agente que se utilizo en el proceso</param>
        /// <param name="Body">Cuerpo del msje</param>
        /// <param name="Address">Direccion a la cual se enrealizo el envio</param>
        /// <returns>Si se guardo correctamente o no</returns>
        private bool sendProcessMessage(int MessageQueueId, bool isOk, string MessageResult, string MessageType, string Agent, string Body, int Address)
        {
            return this._controller.CreateQueueProcess(MessageQueueId, isOk, MessageResult, MessageType, Agent, Body, Address);
        }

        /// <summary>
        /// Funcion que procesa el mensaje sin hacer el envio a travez de algun agente.
        /// </summary>
        /// <param name="MessageQueueId">Id del mensaje que se va a enviar</param>
        /// <returns>Si la operacion fue correcta o no</returns>
        private bool SendToProcessWhitOutAgent(MessageQueueModel MessageQueue, int AddresToSend)
        {
            return this._controller.CreateQueueProcess(MessageQueue.MessageQueueId, true, "The send was cancelled", "CancelAppointment - Clean MsjAppointment", "NoAgent", MessageQueue.MessageType, AddresToSend);            
        }

        #endregion

    }
}
