using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Controller
{
    public class Patient
    {
        public static PatientModel PatientGetAllByPublicPatientId(string PatientPublicId)
        {
            return DAL.Controller.MedicalCalendarDataController.Instance.PatientGetAllByPublicPatientId(PatientPublicId);
        }
    }
}
