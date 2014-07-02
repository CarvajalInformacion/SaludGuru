using SaludGuru.Notifications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.Notifications.DAL.Controller
{
    internal class NotificationDataFactory
    {
        public INotificationsData GetNotificationsInstance()
        {
            Type typetoreturn = Type.GetType("SaludGuru.Notifications.DAL.MySQLDAO.Notifications_MySqlDao,SaludGuru.Notifications");
            INotificationsData oRetorno = (INotificationsData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
