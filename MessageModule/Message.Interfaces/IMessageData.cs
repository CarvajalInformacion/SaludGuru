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

        #endregion

        #region ProcessMessage

        List<AddressModel> UpsertAddress(string Address, string AggentWay);

        void CreateMessage();

        void CreateMessageByAddress();

        void CreateQueueProcess();

        void RemoveResend();      

        #endregion
    }
}
