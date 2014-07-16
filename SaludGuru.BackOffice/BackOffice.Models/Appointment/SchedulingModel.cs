using BackOffice.Models.General;
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
                        return string.Empty;
                    case enumAppointmentType.Month:
                        return "month";
                    default:
                        return string.Empty;
                }
            }
        }

        public bool RenderScripts { get; set; }

    }
}
