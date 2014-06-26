using MedicalCalendar.Manager.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.DAL.Controller
{
    internal class MedicalCalendarDataController : MedicalCalendar.Manager.Interfaces.IMedicalCalendarData
    {
        #region singleton instance
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

        private MedicalCalendar.Manager.Interfaces.IMedicalCalendarData DataFactory;
        #endregion

        #region constructor
        MedicalCalendarDataController()
        {
            MedicalCalendarDataFactory factory = new MedicalCalendarDataFactory();
            DataFactory = factory.GetMedicalCalendarInstance();
        }
        #endregion

        #region Calendar

        public List<HolidayModel> HolidayGetByCountry(int CountryId)
        {
            return DataFactory.HolidayGetByCountry(CountryId);
        }

        #endregion

        #region Patient

        public string PatientCreate(string Name, string LastName, string ProfilePublicId, string UserPublicId)
        {
            return DataFactory.PatientCreate(Name, LastName, ProfilePublicId, UserPublicId);
        }

        public void PatientModify(string PatientPublicId, string Name, string LastName)
        {
            DataFactory.PatientModify(PatientPublicId, Name, LastName);
        }

        public void PatientDelete(string PatientPublicId)
        {
            DataFactory.PatientDelete(PatientPublicId);
        }

        public int PatientInfoCreate(string PatientPublicId, MedicalCalendar.Manager.Models.enumPatientInfoType PatientInfoType, string Value, string LargeValue)
        {
            return DataFactory.PatientInfoCreate(PatientPublicId, PatientInfoType, Value, LargeValue);
        }

        public void PatientInfoModify(int PatientInfoId, string Value, string LargeValue)
        {
            DataFactory.PatientInfoModify(PatientInfoId, Value, LargeValue);
        }

        #endregion

        #region Appointment

        public string AppointmentCreate(string OfficePublicId, MedicalCalendar.Manager.Models.enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate)
        {
            return DataFactory.AppointmentCreate(OfficePublicId, Status, StartDate, EndDate);
        }

        public void AppointmentModify(string AppointmentPublicId, MedicalCalendar.Manager.Models.enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate)
        {
            DataFactory.AppointmentCreate(AppointmentPublicId, Status, StartDate, EndDate);
        }

        public int AppointmentInfoCreate(string AppointmentPublicId, MedicalCalendar.Manager.Models.enumAppointmentInfoType AppointmentInfoType, string Value, string LargeValue)
        {
            return DataFactory.AppointmentInfoCreate(AppointmentPublicId, AppointmentInfoType, Value, LargeValue);
        }

        public void AppointmentInfoModify(int AppointmentInfoId, string Value, string LargeValue)
        {
            DataFactory.AppointmentInfoModify(AppointmentInfoId, Value, LargeValue);
        }

        public void AppointmentPatientCreate(string AppointmentPublicId, string PatientPublicId)
        {
            DataFactory.AppointmentPatientCreate(AppointmentPublicId, PatientPublicId);
        }

        public void AppointmentPatientDelete(string AppointmentPublicId, string PatientPublicId)
        {
            DataFactory.AppointmentPatientDelete(AppointmentPublicId, PatientPublicId);
        }

        #endregion
    }
}
