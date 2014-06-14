using Message.DAL.MySqlDao;
using Message.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carvajal.Mail;
using System.Messaging;
using System.Threading.Tasks;
using MessageMailman = Carvajal.Mail.Message;

namespace Message.Mailman
{
    public class AgentMailmanImplement : Message.Interfaces.IAgent
    {
        private Message_MySqlDao _dao = new Message_MySqlDao();
        public Models.MessageModel SendMessage(Models.AgentModel MessageToSend)
        {
            #region Variables

            List<AddressModel> addresList = new List<AddressModel>();
            #endregion
            //validar todas las direcciones
            List<QueueParameterModel> mess = MessageToSend.QueueItemToProcess.MessageParameters.Where(x => x.Key == "TO").ToList();
            addresList = this.UpsertAddress(mess.FirstOrDefault().Value, MessageToSend.MessageConfig["Agent"]);

            foreach (var item in addresList)
            {
                var messageQueue = new MessageQueue();
                var mailMessage = new MessageMailman
                {
                    From = new Address(item.Address),
                    To = new Address(MessageToSend.QueueItemToProcess.MessageParameters.Where(x => x.Key == "TO").ToString()),
                    Subject = "",
                    Body = "",
                    IsBodyHtml = true,
                    RequestId = Guid.NewGuid().ToString().ToUpper()
                };
                messageQueue.Send(mailMessage, MessageQueueTransactionType.Single);
            }



            //SendMail(mailMessage, mailmanPath);


            //var messageQueue = new MessageQueue();
            //var mqMessage = new System.Messaging.Message { Body = mailMessage, Recoverable = true, Formatter = messageQueue.Formatter };
            //messageQueue.Send()

            throw new NotImplementedException();
        }

        private List<AddressModel> UpsertAddress(string address, string agent)
        {
            List<AddressModel> addressList = new List<AddressModel>();
            return addressList = this._dao.UpsertAddress(address, agent);
        }
    }
}
