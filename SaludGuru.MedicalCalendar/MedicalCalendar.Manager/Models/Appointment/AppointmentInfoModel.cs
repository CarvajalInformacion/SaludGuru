using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Models.Appointment
{
    public class AppointmentInfoModel
    {
        public int AppointmentInfoId { get; set; }

        public enumAppointmentInfoType AppointmentInfoType { get; set; }

        public string Value { get; set; }

        public string LargeValue { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
