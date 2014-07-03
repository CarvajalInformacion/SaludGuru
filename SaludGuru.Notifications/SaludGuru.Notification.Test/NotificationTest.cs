using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SaludGuru.Notification.Test
{
    [TestClass]
    public class NotificationTest
    {
        [TestMethod]
        public void NotificationCreateTest()
        {
            int oProfile = SaludGuru.Notifications.Controller.Notification.NotificationCreate("12345678", "", 1301 , "Test", "Método de prueba");
            Assert.AreEqual(oProfile > 0, true);
        }

        [TestMethod]
        public void NotificationGetByUserNameAndStatusTest()
        {
            List<SaludGuru.Notifications.Models.NotificationModel> models = new List<Notifications.Models.NotificationModel>();
            models = SaludGuru.Notifications.Controller.Notification.Notifiation_GetByUserAndStatus("17B1EF7E", null);
            //Assert.AreEqual(oProfile > 0, true);
        }
    }
}
