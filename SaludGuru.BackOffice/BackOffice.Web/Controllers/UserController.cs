using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaludGuruProfile.Manager.Controller;
using BackOffice.Models.General;
using SaludGuru.Notifications.Models;
using SessionController.Models.Auth;

namespace BackOffice.Web.Controllers
{
    public partial class UserController : BaseController
    {
        #region Notifications

        public virtual ActionResult NotificationList()
        {
            List<NotificationModel> oModel = SaludGuru.Notifications.Controller.Notification.Notifiation_GetByUserAndStatus
                (BackOffice.Models.General.SessionModel.CurrentLoginUser.UserPublicId, null);

            if (oModel == null)
                oModel = new List<NotificationModel>();

            return View(oModel);
        }

        #endregion
    }
}