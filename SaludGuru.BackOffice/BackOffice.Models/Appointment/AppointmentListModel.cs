using MedicalCalendar.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Appointment
{
    public class AppointmentListModel
    {
        public int SearchAppointmentCount { get; set; }

        public string PatientPublicId { get; set; }
        public string Status { get; set; }
        public string CreateDate { get; set; }
    }
}
