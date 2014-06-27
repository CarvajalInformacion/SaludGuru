using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Patient
{
    public class PatientSearchModel
    {
        public int SearchPatientCount { get; set; }

        public string PatientPublicId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Identification { get; set; }
    }
}
