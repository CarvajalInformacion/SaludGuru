using SaludGuru.Notifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class NotificationApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<NotificationModel> GetNotificationsBySessionUser()
        {
            List<NotificationModel> result = new List<NotificationModel>();
            if (SessionController.SessionManager.Auth_UserLogin != null)
            {
                result = SaludGuru.Notifications.Controller.Notification.Notifiation_GetByUserAndStatus
                (SessionController.SessionManager.Auth_UserLogin.UserPublicId,
                enumNotificationStatus.No_Leida);
            }
            return result;
        }

        [HttpPost]
        [HttpGet]
        public List<NotificationModel> ReadNotifications(string NotificationId)
        {
            SaludGuru.Notifications.Controller.Notification.UpdateStatus(enumNotificationStatus.Leida, Convert.ToInt32(NotificationId));
            return this.GetNotificationsBySessionUser();
        }

    }
}
