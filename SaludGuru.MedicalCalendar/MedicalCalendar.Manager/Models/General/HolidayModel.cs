using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Models.General
{
    public class HolidayModel
    {
        public int HolidayId { get; set; }

        public int CountryId { get; set; }

        public DateTime Day { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
