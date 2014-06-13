using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.DAL.Controller
{
    internal class MedicalCalendarDataController : MedicalCalendar.Manager.Interfaces.IMedicalCalendarData
    {
        private static MedicalCalendar.Manager.Interfaces.IMedicalCalendarData oInstance;
        internal static MedicalCalendar.Manager.Interfaces.IMedicalCalendarData Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new MedicalCalendarDataController();
                return oInstance;
            }
        }
    }
}
