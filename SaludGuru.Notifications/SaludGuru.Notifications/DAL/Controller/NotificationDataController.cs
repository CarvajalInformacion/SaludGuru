using SaludGuru.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.Notifications.DAL.Controller
{
    internal class NotificationDataController : SaludGuru.Notifications.Interfaces.INotificationsData
    {
        #region singleton instance
        private static SaludGuru.Notifications.Interfaces.INotificationsData oInstance;
        internal static SaludGuru.Notifications.Interfaces.INotificationsData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new NotificationDataController();
                return oInstance;
            }
        }

        private SaludGuru.Notifications.Interfaces.INotificationsData DataFactory;
        #endregion

        #region constructor
        public NotificationDataController()
        {
            NotificationDataFactory factory = new NotificationDataFactory();
            DataFactory = factory.GetNotificationsInstance();
        }

        #endregion

        public int NotificationCreate(string PublicUserId, string PublicUserFrom, enumNotificationStatus NotificationStatus, enumNoticaficationType NotificationType, string Title, string Body)
        {
            return DataFactory.NotificationCreate(PublicUserId, PublicUserFrom, NotificationStatus, NotificationType, Title, Body);
        }

        public void UpdateStatus(enumNotificationStatus Status, int NotificationId)
        {
            DataFactory.UpdateStatus(Status, NotificationId);
        }

        public List<Models.NotificationModel> NotifiationGetByUserAndStatus(string PublicUserId, enumNotificationStatus? Status)
        {
            return DataFactory.NotifiationGetByUserAndStatus(PublicUserId, Status);
        }
    }
}
