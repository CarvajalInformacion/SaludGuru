using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SaludGuru.Notifications.Models;
using SessionController.Models.Auth;

namespace SaludGuru.Notification.Test
{
    [TestClass]
    public class NotificationTest
    {
        [TestMethod]
        public void NotificationCreateTest()
        {
            NotificationModel oBj = new NotificationModel();

            oBj.Body = "Esto es una prueba";
            oBj.NotificationType = enumNotificationType.CancelAppointment;
            oBj.PublicUserId = "0000000";
            oBj.Status = enumNotificationStatus.Leida;
            oBj.Title = "";
            oBj.UserFrom = new User();

            int oProfile = SaludGuru.Notifications.Controller.Notification.NotificationCreate(oBj);
            Assert.AreEqual(oProfile > 0, true);
        }

        [TestMethod]
        public void NotificationGetByUserNameAndStatusTest()
        {
            List<SaludGuru.Notifications.Models.NotificationModel> models = new List<Notifications.Models.NotificationModel>();
            models = SaludGuru.Notifications.Controller.Notification.Notifiation_GetByUserAndStatus("17B1EF7E", enumNotificationStatus.No_Leida);
            Assert.AreEqual(models.Count > 0, true);
        }
    }
}
