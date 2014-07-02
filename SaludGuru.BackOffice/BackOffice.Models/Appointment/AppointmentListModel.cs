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
        public string AppointmentPublicId { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CreateDate { get; set; }
        public string StatusName { get; set; }
        public string OfficePublicId { get; set; }
        public string OfficeName { get; set; }
    }
}
