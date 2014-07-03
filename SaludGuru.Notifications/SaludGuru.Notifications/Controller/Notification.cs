using SaludGuru.Notifications.DAL.Controller;
using SaludGuru.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.Notifications.Controller
{
    public static class Notification
    {
        public static int NotificationCreate(string vPublicUserId, string vPublicUserFrom, int vStatus, string title, string body)
        {
            return DAL.Controller.NotificationDataController.Instance.NotificationCreate(vPublicUserId, vPublicUserFrom, vStatus, title, body);
        }

        public static List<NotificationModel> Notifiation_GetByUserAndStatus(string vPublicUserId, int? vStatus)
        {
            List<NotificationModel> oReturn = NotificationDataController.Instance.Notifiation_GetByUserAndStatus(vPublicUserId, vStatus);

            return oReturn;
        }

        public static void UpdateStatus(int vStatus, int vNotificationId)
        {
            NotificationDataController.Instance.UpdateStatus(vStatus, vNotificationId);
        }
    }
}
