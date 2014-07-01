using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BackOffice.Web.ControllersApi
{
    public class AppointmentApiController
    {
        [HttpPost]
        [HttpGet]
        public List<BackOffice.Models.Appointment.AppointmentListModel> AppointmentList
            (string ProfilePublicId, int PageNumber, int RowCount)
        {
            int oTotalRowCount;
            List<MedicalCalendar.Manager.Models.Appointment.AppointmentModel> ListAppointment =
                MedicalCalendar.Manager.Controller.Appointment.AppointmentList
                (
                    ProfilePublicId,
                    PageNumber,
                    RowCount,
                    out oTotalRowCount
                );
            if(ListAppointment != null && ListAppointment.Count > 0)
            {
                List<BackOffice.Models.Appointment.AppointmentListModel> oReturn = ListAppointment.
                    Select(x => new BackOffice.Models.Appointment.AppointmentListModel()
                    {
                        SearchAppointmentCount = oTotalRowCount,
                        CreateDate = x.CreateDate.ToString(),
                        Status = x.AppointmentInfo.
                        Where(y => y.AppointmentInfoType == MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category).
                        Select(y => y.Value).
                        DefaultIfEmpty(string.Empty).FirstOrDefault(),                        
                    }).ToList();

                return oReturn;
            }
            else
            {
                return new List<Models.Appointment.AppointmentListModel>();
            }
        }
    }
}