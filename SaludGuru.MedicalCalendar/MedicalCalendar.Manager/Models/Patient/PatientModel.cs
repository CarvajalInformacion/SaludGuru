using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Models.Patient
{
    public class PatientModel
    {
        public string PatientPublicId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime LastModify { get; set; }

        public DateTime CreateDate { get; set; }

        public List<PatientInfoModel> PatientInfo { get; set; }
    }
}
