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
        int NotificationCreate(string vPublicUserId, string vPublicUserFrom, int vStatus, string title, string body);

        List<NotificationModel> Notifiation_GetByUserAndStatus(string vPublicUserId, int vStatus);

        void UpdateStatus(int vStatus, int vNotificationId);
    }
}
