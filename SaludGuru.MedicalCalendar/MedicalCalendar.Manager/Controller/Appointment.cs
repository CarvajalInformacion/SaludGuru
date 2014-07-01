using MedicalCalendar.Manager.DAL.Controller;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Controller
{
    public class Appointment
    {
        public static List<SpecialDayModel> GetSpecialDays
            (int CountryId, string ProfilePublicId, string OfficePublicId, DateTime StartDate, DateTime EndDate)
        {
            List<SpecialDayModel> oReturn = new List<SpecialDayModel>();

            //get special holiday
            oReturn.AddRange(Holiday.GetByCountry(CountryId).Select(x =>
                new SpecialDayModel()
                {
                    SpecialDayType = Models.enumSpecialDayType.Holiday,
                    SpecialDay = x.Day,
                }).ToList());

            //get not available days

            return oReturn;
        }

        public List<AppointmentModel> AppointmentList(string ProfilePublicId, int PageNumber, int RowCount, out int TotalRow)
        {
            List<AppointmentModel> oReturn = MedicalCalendarDataController.Instance.AppointmentList(ProfilePublicId, PageNumber, RowCount, out TotalRow);

            return oReturn;
        }
    }
}
