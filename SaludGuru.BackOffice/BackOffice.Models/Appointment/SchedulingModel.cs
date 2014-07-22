using BackOffice.Models.General;
using MedicalCalendar.Manager.Models.Appointment;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Appointment
{
    public class SchedulingModel
    {
        public ProfileModel CurrentProfile { get; set; }

        public DateTime CurrentStartDate { get; set; }

        public DateTime CurrentEndDate { get; set; }

        public enumAppointmentType AppointmentType { get; set; }

        public string defaultView
        {
            get
            {
                switch (AppointmentType)
                {
                    case enumAppointmentType.Day:
                        return "agendaDay";
                    case enumAppointmentType.Week:
                        return "agendaWeek";
                    case enumAppointmentType.List:
                        return "basicDay";
                    case enumAppointmentType.Month:
                        return "month";
                    case enumAppointmentType.Detail:
                        return "basicDay";
                    default:
                        return string.Empty;
                }
            }
        }

        public bool RenderScripts { get; set; }

        public AppointmentModel CurrentAppointment { get; set; }

        public string ReturnUrl { get; set; }
    }
}
