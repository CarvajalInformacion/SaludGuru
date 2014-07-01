using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaludGuruProfile.Manager.Models.Office
{
    public class ScheduleAvailableModel
    {
        public int ScheduleAvailableId { get; set; }
        public DayOfWeek Day { get; set; }
        public string DayName
        {
            get
            {
                string oReturn = string.Empty;
                switch (Day)
                {
                    case DayOfWeek.Monday:
                        oReturn = "Lunes";
                        break;
                    case DayOfWeek.Tuesday:
                        oReturn = "Martes";
                        break;
                    case DayOfWeek.Wednesday:
                        oReturn = "Miercoles";
                        break;
                    case DayOfWeek.Thursday:
                        oReturn = "Jueves";
                        break;
                    case DayOfWeek.Friday:
                        oReturn = "Viernes";
                        break;
                    case DayOfWeek.Saturday:
                        oReturn = "Sabado";
                        break;
                    case DayOfWeek.Sunday:
                        oReturn = "Domingo";
                        break;
                }
                return oReturn;
            }
        }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
