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

        public string title { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public string className { get; set; }

        public bool allDay { get { return false; } }

        public string img { get; set; }

        public string name { get; set; }

        public AppointmentModel RelatedAppointment { get; set; }
    }
}
