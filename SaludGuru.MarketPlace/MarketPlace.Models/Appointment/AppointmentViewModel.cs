using SaludGuruProfile.Manager.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Appointment
{
    public class AppointmentViewModel
    {
        public DateTime CurrentDate { get; set; }

        public ProfileModel CurrentProfile { get; set; }
    }
}
