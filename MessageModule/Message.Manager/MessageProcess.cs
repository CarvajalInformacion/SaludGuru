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
        /// <summary>
        /// Variable para el acceso a funciones DAL
        /// </summary>
        private MessageDataController _controller = new MessageDataController();

        /// <summary>
        /// Función que inicia el proceso
        /// </summary>
        public void StartProcess()
        {
            //read queue
            List<MessageQueueModel> messageQueueList = new List<MessageQueueModel>();
            messageQueueList = _controller.GetQueueMessage();

            if (messageQueueList.Count > 0)
            {
                foreach (MessageQueueModel item in messageQueueList)
                {
                    this.ProcessMesage(item);
                }
            }
        }

        /// <summary>
        /// Funcion que se encarga de llamar los metodos que desarrllan el proceso de envío
        /// </summary>
        /// <param name="QueueItemToProcess">Mensaje para enviar, tomado de la cola</param>
        /// <returns>Modelo con info el mensaje enviado</returns>
        private Message.Models.MessageModel ProcessMesage(Message.Models.MessageQueueModel QueueItemToProcess)
        {
            Message.Models.MessageModel oMsjReturn = new MessageModel();
            /********validate config****************/
            Message.Models.AgentModel oAgentConfig = new AgentModel()
            {
                QueueItemToProcess = QueueItemToProcess,
                MessageConfig = GetMessageConfig(QueueItemToProcess.MessageType),
            };
            oAgentConfig.AgentConfig = GetAgentConfig(oAgentConfig.MessageConfig["Agent"]);

            /*************invoque message agent**********/
            Message.Interfaces.IAgent Agent = GetAgentInstance(oAgentConfig.AgentConfig["Assemblie"]);

            oMsjReturn = Agent.SendMessage(oAgentConfig);

            /*trace log message*/



            //oMsjReturn.ConfigBody = SettingsManager.SettingsController.SettingsInstance.ModulesParams["Message"]["Message_MailConfirmation_Body"].Value;


            return oMsjReturn;
        }


        private Dictionary<string, string> GetMessageConfig(string MessageType)
        {
            XDocument xDocMessType = XDocument.Parse(SettingsManager.SettingsController.SettingsInstance.ModulesParams["Message"]["Message_Params_" + MessageType].Value);
            Dictionary<string, string> MessageConfig = xDocMessType.Descendants("Message").Descendants("key").ToDictionary(k => k.Attribute("name").Value, v => v.Value);
            return MessageConfig;
        }

        /// <summary>
        /// Funcion 
        /// </summary>
        /// <param name="QueueItemToProcess"></param>
        /// <returns></returns>
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
    }
}
