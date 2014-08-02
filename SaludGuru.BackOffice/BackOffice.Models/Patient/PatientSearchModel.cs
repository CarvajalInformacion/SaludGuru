using MedicalCalendar.Manager.Models.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Patient
{
    public class PatientSearchModel
    {
        public int SearchPatientCount { get; set; }

        public string PatientPublicId { get { return CurrentPatient.PatientPublicId; } }

        public string Name { get { return CurrentPatient.Name + " " + CurrentPatient.LastName; } }

        public string ProfileImage
        {
            get
            {
                return CurrentPatient.PatientInfo.
                    Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.ProfileImage).
                    Select(y => y.Value).
                    DefaultIfEmpty(BackOffice.Models.General.InternalSettings.Instance
                [BackOffice.Models.General.Constants.C_Settings_PatientImage_Woman].Value).
                    FirstOrDefault();
            }
        }

        public string IdentificationNumber
        {
            get
            {
                return CurrentPatient.PatientInfo.
                    Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.IdentificationNumber).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string Mobile
        {
            get
            {
                return CurrentPatient.PatientInfo.
                    Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.Mobile).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string Telephone
        {
            get
            {
                return CurrentPatient.PatientInfo.
                    Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.Telephone).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public string Email
        {
            get
            {
                return CurrentPatient.PatientInfo.
                    Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.Email).
                    Select(y => y.Value).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault();
            }
        }

        public List<PatientInfoModel> Notes
        {
            get
            {
                return CurrentPatient.PatientInfo.
                    Where(y => y.PatientInfoType == MedicalCalendar.Manager.Models.enumPatientInfoType.DoctorNotes).
                    ToList();
            }
        }

        private PatientModel CurrentPatient { get; set; }

        public PatientSearchModel(PatientModel vCurrentPatient)
        {
            CurrentPatient = vCurrentPatient;
        }

    }
}
