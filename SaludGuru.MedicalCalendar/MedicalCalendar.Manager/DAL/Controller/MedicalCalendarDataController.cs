﻿using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.Appointment;
using MedicalCalendar.Manager.Models.General;
using MedicalCalendar.Manager.Models.Patient;
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

        public List<ScheduleBusyModel> GetScheduleBusy(string ProfilePublicId, string OfficePublicId, DateTime? StartDate, DateTime? EndDate, int? CategoryId)
        {
            return DataFactory.GetScheduleBusy(ProfilePublicId, OfficePublicId, StartDate, EndDate, CategoryId);
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

        public void PatientInfoDelete(int PatientInfoId)
        {
            DataFactory.PatientInfoDelete(PatientInfoId);
        }

        public Models.Patient.PatientModel PatientGetAllByPublicPatientId(string PatientPublicId)
        {
            return DataFactory.PatientGetAllByPublicPatientId(PatientPublicId);
        }

        public List<PatientModel> PatientSearch(string ProfilePublicId, string SearchCriteria, int PageNumber, int RowCount, out int TotalRows)
        {
            return DataFactory.PatientSearch(ProfilePublicId, SearchCriteria, PageNumber, RowCount, out TotalRows);
        }

        public List<ItemModel> PatientGetOptions()
        {
            return DataFactory.PatientGetOptions();
        }

        public PatientModel PatientGetFromIdentificationNumber(string ProfilePublicId, string IdentificationNumber)
        {
            return DataFactory.PatientGetFromIdentificationNumber(ProfilePublicId, IdentificationNumber);
        }

        public List<PatientModel> MPPatientGetByUserPublicId(string vUserPublicId)
        {
            return DataFactory.MPPatientGetByUserPublicId(vUserPublicId);
        }

        #endregion

        #region Appointment

        public string AppointmentCreate(string OfficePublicId, MedicalCalendar.Manager.Models.enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate)
        {
            return DataFactory.AppointmentCreate(OfficePublicId, Status, StartDate, EndDate);
        }

        public void AppointmentModify(string AppointmentPublicId, string OfficePublicId, MedicalCalendar.Manager.Models.enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate)
        {
            DataFactory.AppointmentModify(AppointmentPublicId, OfficePublicId, Status, StartDate, EndDate);
        }

        public void AppointmentModifyStatus(string AppointmentPublicId, MedicalCalendar.Manager.Models.enumAppointmentStatus Status)
        {
            DataFactory.AppointmentModifyStatus(AppointmentPublicId, Status);
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

        public List<AppointmentModel> AppointmentGetByPatient(string ProfilePublicId)
        {
            return DataFactory.AppointmentGetByPatient(ProfilePublicId);
        }

        public List<AppointmentModel> AppointmentGetByOfficeIdBasicInfo(string OfficePublicId, DateTime StartDateTime, DateTime EndDateTime)
        {
            return DataFactory.AppointmentGetByOfficeIdBasicInfo(OfficePublicId, StartDateTime, EndDateTime);
        }

        public List<AppointmentModel> AppointmentGetByOfficeIdPatientInfo(string OfficePublicId, DateTime StartDateTime, DateTime EndDateTime)
        {
            return DataFactory.AppointmentGetByOfficeIdPatientInfo(OfficePublicId, StartDateTime, EndDateTime);
        }

        public AppointmentModel AppointmentGetByIdBasicInfo(string AppointmentPublicId)
        {
            return DataFactory.AppointmentGetByIdBasicInfo(AppointmentPublicId);
        }

        public AppointmentModel AppointmentGetByIdPatientInfo(string AppointmentPublicId)
        {
            return DataFactory.AppointmentGetByIdPatientInfo(AppointmentPublicId);
        }

        public List<AppointmentMonthModel> AppointmentGetByOfficeIdMonth(string OfficePublicId, DateTime StartDateTime)
        {
            return DataFactory.AppointmentGetByOfficeIdMonth(OfficePublicId, StartDateTime);
        }

        public string GetAppointmentByOldId(string OldAppointmentId)
        {
            return DataFactory.GetAppointmentByOldId(OldAppointmentId);
        }

        public bool AppointmentValidateDuplicate(string OfficePublicId, string AppointmentPublicId, DateTime StartDate, DateTime EndDate)
        {
            return DataFactory.AppointmentValidateDuplicate(OfficePublicId, AppointmentPublicId, StartDate, EndDate);
        }

        #region Marketplace

        public List<AppointmentModel> MPAppointmentGetByOfficeIdBasicInfo(string OfficePublicId, DateTime StartDateTime, DateTime EndDateTime)
        {
            return DataFactory.MPAppointmentGetByOfficeIdBasicInfo(OfficePublicId, StartDateTime, EndDateTime);
        }
        #endregion

        #endregion
    }
}
