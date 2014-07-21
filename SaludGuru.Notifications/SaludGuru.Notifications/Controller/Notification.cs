using SaludGuru.Notifications.DAL.Controller;
using SaludGuru.Notifications.Models;
using SessionController.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.Notifications.Controller
{
    public static class Notification
    {
        public static int NotificationCreate(NotificationModel NotificationToCreate)
        {
            return DAL.Controller.NotificationDataController.Instance.NotificationCreate
                (NotificationToCreate.PublicUserId,
                NotificationToCreate.UserFrom.UserPublicId,
                NotificationToCreate.Status,
                NotificationToCreate.NotificationType,
                NotificationToCreate.Title,
                NotificationToCreate.Body);
        }

        public static void UpdateStatus(enumNotificationStatus NewStatus, int NotificationId)
        {
            NotificationDataController.Instance.UpdateStatus(NewStatus, NotificationId);
        }

        public static List<NotificationModel> Notifiation_GetByUserAndStatus(string PublicUserId, enumNotificationStatus? Status)
        {
            //get notifications info
            List<NotificationModel> oReturn = NotificationDataController.Instance.NotifiationGetByUserAndStatus(PublicUserId, Status);

            if (oReturn != null)
            {
                //get user info
                string arrayToConsult = string.Join(",", oReturn.Select(x => x.UserFrom.UserPublicId).ToList());

                List<User> userList = Auth.Client.Controller.Client.GetUserList(arrayToConsult);

                oReturn.All(n =>
                {
                    User CurrentUser = userList.Where(x => x.UserPublicId == n.UserFrom.UserPublicId).FirstOrDefault();

                    if (CurrentUser != null)
                    {
                        n.UserFrom = CurrentUser;
                    }
                    return true;
                });
            }
            return oReturn;
        }
    }
}
