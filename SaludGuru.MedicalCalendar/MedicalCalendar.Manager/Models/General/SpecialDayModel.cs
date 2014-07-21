using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Models.General
{
    public class SpecialDayModel
    {
        public enumSpecialDayType SpecialDayType { get; set; }

        public DateTime SpecialDay { get; set; }

        public int Year { get { return SpecialDay.Year; } }

        public int Month { get { return SpecialDay.Month; } }

        public int Day { get { return SpecialDay.Day; } }
    }
}
