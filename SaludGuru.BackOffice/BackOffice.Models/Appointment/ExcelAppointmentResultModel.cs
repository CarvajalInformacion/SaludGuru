using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Appointment
{
    public class ExcelAppointmentResultModel : ExcelAppointmentModel
    {
        public ExcelAppointmentModel AptModel { get; set; }

        public bool Success { get; set; }

        public string Error { get; set; }
    }
}
