using MedicalCalendar.Manager.Models.Appointment;
using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Appointment
{
    public class ExternalAppointmentViewModel
    {
        public AppointmentModel CurrentAppointment { get; set; }

        public ProfileModel CurrentProfile { get; set; }
    }
}
