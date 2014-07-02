using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
