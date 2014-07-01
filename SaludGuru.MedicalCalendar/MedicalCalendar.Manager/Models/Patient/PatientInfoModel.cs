using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Models.Patient
{
    public class PatientInfoModel
    {
        public int PatientInfoId { get; set; }

        public enumPatientInfoType PatientInfoType { get; set; }

        public string Value { get; set; }

        public string LargeValue { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
