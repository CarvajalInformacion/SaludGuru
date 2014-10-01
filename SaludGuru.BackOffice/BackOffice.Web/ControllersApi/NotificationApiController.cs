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
                enumNotificationStatus.No_Leida).OrderByDescending(x => x.CreateDate).ToList();
            }
            return result;
        }

        [HttpPost]
        [HttpGet]
        public List<NotificationModel> ReadNotifications(string NotificationId)
        {
            List<NotificationModel> notifyList = new List<NotificationModel>();
            SaludGuru.Notifications.Controller.Notification.UpdateStatus(enumNotificationStatus.Leida, Convert.ToInt32(NotificationId));
            notifyList = this.GetNotificationsBySessionUser();
            if (notifyList == null)
            {
                notifyList = new List<NotificationModel>();
            }
            return notifyList;
        }

    }
}
