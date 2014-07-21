using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Models.Appointment
{
    public class AppointmentModel
    {
        public string AppointmentPublicId { get; set; }

        public enumAppointmentStatus Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<AppointmentInfoModel> AppointmentInfo { get; set; }

        public List<PatientModel> RelatedPatient { get; set; }

        public string OfficePublicId { get; set; }

        public string OfficeName { get; set; }

    }
}
