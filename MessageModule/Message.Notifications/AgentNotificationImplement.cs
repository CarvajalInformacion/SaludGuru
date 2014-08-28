using Message.Interfaces;
using Message.Models;
using MessageModule.Controller;
using SaludGuru.Notifications.Controller;
using SaludGuru.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Notifications
{
    public class AgentNotificationImplement : IAgent
    {
        /// <summary>
        /// Variable para el acceso a funciones DAL
        /// </summary>
        private MessageDataController _controller = new MessageDataController();

        public Models.MessageModel SendMessage(Models.AgentModel MessageToSend)
        {
            List<AddressModel> addresList = new List<AddressModel>();
            NotificationModel messageToCreate = new NotificationModel();
            MessageModel oReturn = new MessageModel();

            oReturn.RelatedAddress = new List<AddressModel>();
            foreach (AddressModel item in MessageToSend.AddressToSend)
            {
                List<Message.Models.QueueParameterModel> mess = MessageToSend.QueueItemToProcess.MessageParameters.Where(x => x.Key == "TO").ToList();

                messageToCreate.Body = MessageToSend.MessageConfig["Body"];
                messageToCreate.NotificationType = (enumNotificationType)int.Parse(MessageToSend.MessageConfig["NotificationType"]);
                messageToCreate.Status = ((enumNotificationStatus)Enum.Parse(typeof(enumNotificationStatus), MessageToSend.MessageConfig["Status"], true));
                messageToCreate.PublicUserId = item.Address;
                messageToCreate.LastModify = DateTime.Now;
                messageToCreate.Title = MessageToSend.MessageConfig["Title"];
                
                //Instance the User from OAuth
                SessionController.Models.Auth.User user = new SessionController.Models.Auth.User();

                user.UserId = Convert.ToInt32(MessageToSend.QueueItemToProcess.UserAction);
                user.UserPublicId = MessageToSend.QueueItemToProcess.MessageParameters.Where(x => x.Key == "From").Select(x => x.Value).FirstOrDefault();
                messageToCreate.UserFrom = new SessionController.Models.Auth.User();
                messageToCreate.UserFrom = user;

                int idResult = Notification.NotificationCreate(messageToCreate);
                if (idResult != null)
                {
                    oReturn.MessageResult = "Message added correctly to the system queue";
                    oReturn.isSuccess = true;
                }
                else
                {
                    oReturn.MessageResult = "The message could not be sent to the queue system" + DateTime.Now;
                    oReturn.isSuccess = false;                    
                }            
                oReturn.RelatedAddress.Add(item);
            }

            oReturn.Agent = MessageToSend.AddressToSend.FirstOrDefault().Agent;
            oReturn.BodyMessage = MessageToSend.MessageConfig["Body"];
            oReturn.MessageType = MessageToSend.MessageConfig["Title"];
            oReturn.TimeSent = DateTime.Now;
            
            return oReturn;
        }

        public void AddResend(int MessageProcessId)
        {
            throw new NotImplementedException();
        }
    }
}
