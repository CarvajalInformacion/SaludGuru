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

        public bool RenderScripts { get; set; }

    }
}
