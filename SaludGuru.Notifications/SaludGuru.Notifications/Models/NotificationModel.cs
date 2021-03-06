﻿using SessionController.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuru.Notifications.Models
{
    public class NotificationModel
    {
        public int NotificationId { get; set; }
        public string PublicUserId { get; set; }
        public User UserFrom { get; set; }
        public enumNotificationStatus Status { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime LastModify { get; set; }
        public DateTime CreateDate { get; set; }
        public enumNotificationType NotificationType { get; set; }
    }
}
