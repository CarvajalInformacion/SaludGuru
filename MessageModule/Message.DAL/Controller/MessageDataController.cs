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
        #region Singleton instance
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

        public void CreateMessage()
        {
            this.DataFactory.CreateMessage();
        }

        public void CreateMessageByAddress()
        {
            this.DataFactory.CreateMessageByAddress();
        }

        public void CreateQueueProcess()
        {
            this.DataFactory.CreateQueueProcess();
        }

        public void RemoveResend()
        {
            this.DataFactory.RemoveResend();
        }


        public List<AddressModel> UpsertAddress(string Address, string AggentWay)
        {
            return this.DataFactory.UpsertAddress(Address, AggentWay);
        }
    }
}
