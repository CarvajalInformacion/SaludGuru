using BackOffice.Models.Patient;
using MedicalCalendar.Manager.Models.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Appointment
{
    public class ScheduleEventMonthModel
    {
        #region FullCalendar Properties

        public string title
        {
            get
            {
                //get appointment title
                string AppointmentTitleMonth = BackOffice.Models.General.InternalSettings.Instance
                    [BackOffice.Models.General.Constants.C_Settings_Appointment_TitleTemplate_Month].Value;

                foreach (MedicalCalendar.Manager.Models.enumAppointmentStatus AppointmentStatus in (MedicalCalendar.Manager.Models.enumAppointmentStatus[])Enum.GetValues(typeof(MedicalCalendar.Manager.Models.enumAppointmentStatus)))
                {
                    if (CurrentAppointment.StatusDescription.ContainsKey(AppointmentStatus))
                    {
                        AppointmentTitleMonth = AppointmentTitleMonth.
                            Replace("{Count_" + ((int)AppointmentStatus).ToString() + "}",
                            CurrentAppointment.StatusDescription[AppointmentStatus].ToString());
                    }
                    else
                    {
                        AppointmentTitleMonth = AppointmentTitleMonth.
                            Replace("{Count_" + ((int)AppointmentStatus).ToString() + "}",
                            "0");
                    }
                }

                return AppointmentTitleMonth;
            }
        }

        public DateTime start { get { return new DateTime(CurrentAppointment.StartDate.Year, CurrentAppointment.StartDate.Month, CurrentAppointment.StartDate.Day, 0, 0, 0); } }

        public DateTime end { get { return new DateTime(CurrentAppointment.StartDate.Year, CurrentAppointment.StartDate.Month, CurrentAppointment.StartDate.Day, 23, 59, 59); } }

        public bool allDay { get { return false; } }

        #endregion

        #region Related Appointment

        private AppointmentMonthModel CurrentAppointment { get; set; }

        #endregion

        public ScheduleEventMonthModel(AppointmentMonthModel vCurrentAppointment)
        {
            CurrentAppointment = vCurrentAppointment;
        }

    }
}
