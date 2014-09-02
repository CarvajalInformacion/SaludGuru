using MedicalCalendar.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Controller
{
    public class Holiday
    {
        private static Dictionary<int, List<HolidayModel>> CurrentHoliday { get; set; }

        public static List<HolidayModel> GetByCountry(int CountryId)
        {
            if (CurrentHoliday == null)
            {
                CurrentHoliday = new Dictionary<int, List<HolidayModel>>();
            }

            if (!CurrentHoliday.ContainsKey(CountryId))
            {
                CurrentHoliday[CountryId] = DAL.Controller.MedicalCalendarDataController.Instance.HolidayGetByCountry(CountryId);
            }
            return CurrentHoliday[CountryId];
        }
    }
}
