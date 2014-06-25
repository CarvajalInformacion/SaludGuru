using MedicalCalendar.Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalCalendar.Manager.Interfaces
{
    internal interface IMedicalCalendarData
    {
        //patinet
        string PatientCreate(string Name, string LastName, string ProfilePublicId, string UserPublicId);
        void PatientModify(string PatientPublicId, string Name, string LastName);
        void PatientDelete(string PatientPublicId);

        //patient info
        int PatientInfoCreate(string PatientPublicId, enumPatientInfoType PatientInfoType, string Value, string LargeValue);
        void PatientInfoModify(int PatientInfoId, string Value, string LargeValue);

        //appointment
        string AppointmentCreate(string OfficePublicId, enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate);
        void AppointmentModify(string AppointmentPublicId, enumAppointmentStatus Status, DateTime StartDate, DateTime EndDate);

        //appointment info
        int AppointmentInfoCreate(string AppointmentPublicId, enumAppointmentInfoType AppointmentInfoType, string Value, string LargeValue);
        void AppointmentInfoModify(int AppointmentInfoId, string Value, string LargeValue);

        //appointment patient
        void AppointmentPatientCreate(string AppointmentPublicId, string PatientPublicId);
        void AppointmentPatientDelete(string AppointmentPublicId, string PatientPublicId);
    }
}
