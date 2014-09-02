using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Models.Appointment
{
    public class AppointmentMonthModel
    {
        public DateTime StartDate { get; set; }

        public Dictionary<enumAppointmentStatus, int> StatusDescription { get; set; }

        public string OfficePublicId { get; set; }

        public string OfficeName { get; set; }

    }
}
