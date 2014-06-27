using MedicalCalendar.Manager.DAL.Controller;
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

        public static List<PatientModel> PatientSearch(string ProfilePublicId, string SearchCriteria, int PageNumber, int RowCount, out int TotalRows)
        {
            List<PatientModel> oReturn = MedicalCalendarDataController.Instance.PatientSearch(ProfilePublicId, SearchCriteria, PageNumber, RowCount, out TotalRows);

            return oReturn;
        }
    }
}
