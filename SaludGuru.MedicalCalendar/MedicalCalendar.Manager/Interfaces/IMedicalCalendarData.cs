﻿using MedicalCalendar.Manager.Models;
using MedicalCalendar.Manager.Models.General;
using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Interfaces
{
    internal interface IMedicalCalendarData
    {
        //calendar
        List<HolidayModel> HolidayGetByCountry(int CountryId);

        //patinet
        string PatientCreate(string Name, string LastName, string ProfilePublicId, string UserPublicId);
        void PatientModify(string PatientPublicId, string Name, string LastName);
        void PatientDelete(string PatientPublicId);

        //patient info
        int PatientInfoCreate(string PatientPublicId, enumPatientInfoType PatientInfoType, string Value, string LargeValue);
        void PatientInfoModify(int PatientInfoId, string Value, string LargeValue);
        void PatientInfoDelete(int PatientInfoId);
        PatientModel PatientGetAllByPublicPatientId(string PatientPublicId);
        List<PatientModel> PatientSearch(string ProfilePublicId, string SearchCriteria, int PageNumber, int RowCount, out int TotalRows);
        List<ItemModel> PatientGetOptions();

        //appointment
        string AppointmentCreate(string OfficePublicId, enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate);
        void AppointmentModify(string AppointmentPublicId, string OfficePublicId, enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate);
        void AppointmentModifyStatus(string AppointmentPublicId, enumAppointmentStatus Status);
        List<Models.Appointment.AppointmentModel> AppointmentList(string PatientPublicId);
        List<Models.Appointment.AppointmentModel> AppointmentGetByOfficeIdBasicInfo(string OfficePublicId, DateTime StartDateTime, DateTime EndDateTime);
        List<Models.Appointment.AppointmentModel> AppointmentGetByOfficeIdPatientInfo(string OfficePublicId, DateTime StartDateTime, DateTime EndDateTime);
        Models.Appointment.AppointmentModel AppointmentGetByIdBasicInfo(string AppointmentPublicId);
        Models.Appointment.AppointmentModel AppointmentGetByIdPatientInfo(string AppointmentPublicId);
        List<MedicalCalendar.Manager.Models.Appointment.AppointmentMonthModel> AppointmentGetByOfficeIdMonth(string OfficePublicId, DateTime StartDateTime);

        //appointment info
        int AppointmentInfoCreate(string AppointmentPublicId, enumAppointmentInfoType AppointmentInfoType, string Value, string LargeValue);
        void AppointmentInfoModify(int AppointmentInfoId, string Value, string LargeValue);

        //appointment patient
        void AppointmentPatientCreate(string AppointmentPublicId, string PatientPublicId);
        void AppointmentPatientDelete(string AppointmentPublicId, string PatientPublicId);
    }
}

