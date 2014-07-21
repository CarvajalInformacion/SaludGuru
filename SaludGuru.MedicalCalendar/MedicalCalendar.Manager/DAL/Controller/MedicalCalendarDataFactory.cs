using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.DAL.Controller
{
    internal class MedicalCalendarDataFactory
    {
        public MedicalCalendar.Manager.Interfaces.IMedicalCalendarData GetMedicalCalendarInstance()
        {
            Type typetoreturn = Type.GetType("MedicalCalendar.Manager.DAL.MySQLDAO.MedicalCalendar_MySqlDao,MedicalCalendar.Manager");
            MedicalCalendar.Manager.Interfaces.IMedicalCalendarData oRetorno = (MedicalCalendar.Manager.Interfaces.IMedicalCalendarData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
