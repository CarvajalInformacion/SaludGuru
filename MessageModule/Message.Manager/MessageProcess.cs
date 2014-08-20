using Message.Interfaces;
using Message.Models;
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
                        }                        
                    }
                }
            }
        }

        public void AddResend(int messageQueueId)
        {

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
            //validate config
            Message.Models.AgentModel oAgentConfig = new AgentModel()
            {
                QueueItemToProcess = QueueItemToProcess,
                MessageConfig = this.GetMessageConfig(QueueItemToProcess),
            };
            oAgentConfig.AgentConfig = GetAgentConfig(oAgentConfig.MessageConfig["Agent"]);

            //invoque message agent
            Message.Interfaces.IAgent Agent = this.GetAgentInstance(oAgentConfig.AgentConfig["Assemblie"]);

            oMsjReturn = Agent.SendMessage(oAgentConfig);

            if (oMsjReturn == null)
            {
                //TODO: VERIFICAR SI HAY UN RESEND Y HACER LA OPERACION
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
                //guru
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
                    xDocMessBody = xDocMessBody.Replace("{ProfileUrl}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileUrl").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
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
                    xDocMessBody = xDocMessBody.Replace("{ConfirmCancelLink}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentPublicId").Select(x => !string.IsNullOrEmpty(x.Value) ? "https:/www.saludguru.com.co/ExternalAppointment/Index?AppointmentPublicId=" + x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{BeforeCare}", infoMessage.MessageParameters.Where(x => x.Key == "BeforeCare").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    break;
                case "Sms_ReminderAppointment":
                    xDocMessBody = xDocMessBody.Replace("{ProfileName}", infoMessage.MessageParameters.Where(x => x.Key == "ProfileName").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{AppointmentDate}", infoMessage.MessageParameters.Where(x => x.Key == "AppointmentDate").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{Hour}", infoMessage.MessageParameters.Where(x => x.Key == "Hour").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
                    xDocMessBody = xDocMessBody.Replace("{ConfirmCancelLink}", infoMessage.MessageParameters.Where(x => x.Key == "ConfirmCancelLink").Select(x => !string.IsNullOrEmpty(x.Value) ? x.Value : string.Empty).FirstOrDefault());
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

        #endregion
    }
}
