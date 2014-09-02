using SaludGuru.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.Notifications.Interfaces
{
    interface INotificationsData
    {
        int NotificationCreate(string PublicUserId, string PublicUserFrom, enumNotificationStatus NotificationStatus, enumNotificationType NotificationType, string Title, string Body);

        void UpdateStatus(enumNotificationStatus Status, int NotificationId);

        List<NotificationModel> NotifiationGetByUserAndStatus(string PublicUserId, enumNotificationStatus? Status);
    }
}
