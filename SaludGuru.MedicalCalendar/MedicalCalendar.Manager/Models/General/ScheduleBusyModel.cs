using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Models.General
{
    public class ScheduleBusyModel
    {
        public int ScheduleAvailableId { get; set; }

        public string OfficePublicId { get; set; }

        public DateTime EvalDate { get; set; }

        public TimeSpan MaxFreeTime { get; set; }

        public int CategoryId { get; set; }
    }
}
