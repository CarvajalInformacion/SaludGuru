using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketPlace.Web.Controllers
{
    public partial class AgendaController : BaseController
    {
        public virtual ActionResult CancelAppointment(string id)
        {
            string NewAppointmentPublicId = MedicalCalendar.Manager.Controller.Appointment.GetAppointmentByOldId(id);

            if (!string.IsNullOrEmpty(NewAppointmentPublicId) && NewAppointmentPublicId.Trim().Length >= 8)
            {
                return RedirectPermanent(MarketPlace.Models.General.Constants.C_Settings_UrlBackOffice + "/ExternalAppointment/Index?AppointmentPublicId=" + NewAppointmentPublicId);
            }
            else
            {
                return RedirectPermanent(Server.UrlDecode(Url.RouteUrl(MarketPlace.Models.General.Constants.C_Route_Error_NotFound)));
            }
        }
    }
}