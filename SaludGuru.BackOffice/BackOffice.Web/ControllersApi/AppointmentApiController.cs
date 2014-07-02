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
            (string PatientPublicId)
        {
            List<MedicalCalendar.Manager.Models.Appointment.AppointmentModel> ListAppointment =
                MedicalCalendar.Manager.Controller.Appointment.AppointmentList
                (
                    PatientPublicId
                );
            if(ListAppointment != null && ListAppointment.Count > 0)
            {
                List<BackOffice.Models.Appointment.AppointmentListModel> oReturn = ListAppointment.
                    Select(x => new BackOffice.Models.Appointment.AppointmentListModel()
                    {
                        AppointmentPublicId = x.AppointmentPublicId,
                        //Status = x.Status,
                        StartDate = x.StartDate.ToString(),
                        EndDate = x.EndDate.ToString(),
                        CreateDate = x.CreateDate.ToString(),
                        OfficePublicId = x.OfficePublicId,
                        OfficeName = x.OfficeName,      
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