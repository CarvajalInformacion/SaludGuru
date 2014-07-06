using MedicalCalendar.Manager.Models.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Appointment
{
    public class ScheduleEventModel
    {
        public string id { get; set; }

        public string AppointmentImage { get; set; }

        public string AppointmentName { get; set; }

        public string start { get; set; }

        public string end { get; set; }

        public AppointmentModel RelatedAppointment { get; set; }
    }
}
