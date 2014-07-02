using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.Notifications.DAL.Controller
{
    internal class NotificationDataController : SaludGuru.Notifications.Interfaces.INotificationsData
    {
        #region singleton instance
        private static SaludGuru.Notifications.Interfaces.INotificationsData oInstance;
        internal static SaludGuru.Notifications.Interfaces.INotificationsData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new NotificationDataController();
                return oInstance;
            }
        }

        private SaludGuru.Notifications.Interfaces.INotificationsData DataFactory;
        #endregion

        #region constructor
        public NotificationDataController()
        {
            NotificationDataFactory factory = new NotificationDataFactory();
            DataFactory = factory.GetNotificationsInstance();
        }

        #endregion

        public int NotificationCreate(string vPublicUserId, string vPublicUserFrom, int vStatus, string title, string body)
        {
            return DataFactory.NotificationCreate(vPublicUserId, vPublicUserFrom, vStatus, title, body);
        }

        public List<Models.NotificationModel> Notifiation_GetByUserAndStatus(string vPublicUserId, int vStatus)
        {
            return DataFactory.Notifiation_GetByUserAndStatus(vPublicUserId, vStatus);
        }

        public void UpdateStatus(int vStatus, int vNotificationId)
        {
            DataFactory.UpdateStatus(vStatus, vNotificationId);
        }
    }
}
